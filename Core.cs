using M9Studio.ShadowTalk.Core;
using Newtonsoft.Json.Linq;
using M9Studio.ShadowTalk.Client.Packet;
using M9Studio.SecureStream;
using System.Net;
using Microsoft.VisualBasic.ApplicationServices;
using System.Diagnostics;

namespace M9Studio.ShadowTalk.Client
{
    public partial class Core
    {
        public FormMain form;
        public int Port => udpTransportAdapter._socket.Port;
        public List<User> Users;
        public Dictionary<int, ServerInfo> Servers = new Dictionary<int, ServerInfo>();
        public DataBase DataBase = new DataBase();


        UdpLikeTcpSecureTransportAdapter udpTransportAdapter;
        SecureChannelManager<IPEndPoint> channelManager;



        public Core()
        {
            udpTransportAdapter = new UdpLikeTcpSecureTransportAdapter();
            channelManager = new SecureChannelManager<IPEndPoint>(udpTransportAdapter);
            channelManager.OnConnected += OnConnect;
            channelManager.OnDisconnected += OnDisconnect;

            List<ServerInfo>  servers = DataBase.ServerInfo();
            Users = DataBase.User();
            
            foreach (var item in servers)
            {
                Servers.Add(item.ServerId, item);
                foreach (var u in Users)
                {
                    if(u.ServerId == item.ServerId)
                    {
                        item.Users.Add(u.Id, u);
                        u.Server = item;
                        u.Panel = new PanelUser(u);
                    }
                }
                try
                {
                    Connect(item);
                }
                catch (Exception ex)
                {

                }
            }
        }

        private Dictionary<SecureSession<IPEndPoint>, User> sesssionsUsers = new Dictionary<SecureSession<IPEndPoint>, User>();
        private List<SecureSession<IPEndPoint>> sesssions = new List<SecureSession<IPEndPoint>>();



        private void DaemonSession(User user)
        {
            user.Panel.labelName.ForeColor = Color.Green;
            while (user.Session != null)
            {

                JObject packet = user.Session.ReceiveJObject();
                PacketMessageSend p1 = PacketStruct.TryParse<PacketMessageSend>(packet);
                if (p1 != null)
                {
                    Message msg = new Message()
                    {
                        UUID = p1.UUID,
                        Text = p1.Text,
                        Date = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                        Status = 1,
                        Sender = user.Id,
                        UserId = user.Id,
                        ServerId = user.ServerId,
                    };

                    DataBase.Send("INSERT INTO messages (uuid, serverid, userid, date, status, text, sender) VALUES (?, ?, ?, ?, ?, ?, ?)",
                        msg.UUID,
                        msg.ServerId,
                        msg.UserId,
                        msg.Date,
                        msg.Status,
                        msg.Text,
                        msg.Sender
                        );

                    form.messages.Add( msg );
                    if (user.Form != null)
                    {
                        form.AddMessage(msg);
                    }
                    user.NewCount++;
                }
            }
            user.Panel.labelName.ForeColor = Color.Red;
        }

        private void OnDisconnect(SecureSession<IPEndPoint> session)
        {
            sesssions.Remove(session);
            if (sesssionsUsers.ContainsKey(session))
            {
                User user = sesssionsUsers[session];
                sesssionsUsers.Remove(session);
                user.Session = null;
            }
        }
        private void OnConnect(SecureSession<IPEndPoint> session)
        {
            sesssions.Add(session);
        }
        private SecureSession<IPEndPoint> GetSession(string ip, int port)
        {
            foreach(var item in sesssions)
            {
                if(item.RemoteAddress.Address.ToString() == ip && item.RemoteAddress.Port == port)
                {
                    return item;
                }
            }
            return null;
        }

        public User waitP2P;
        private void Daemon(ServerInfo server)
        {
            while (server.Session.IsLive)
            {
                JObject packet = server.Session.ReceiveJObject();
                PacketServerToClientSendMessages p1 = PacketStruct.TryParse<PacketServerToClientSendMessages>(packet);
                if(p1 != null)
                {
                    string text = null;
                    string encryptedText = p1.Texts[0];

                    User user = server.Users.GetValueOrDefault(p1.Users[0], null);
                    if(user == null)
                    {
                        user = OnLoadUser(server, p1.Users[0]);
                        if(user == null)
                        {
                            continue;
                        }
                        user.NewCount++;
                        DataBase.Send("INSERT INTO users (id, name, serverid, rsa, newcount) VALUES (?, ?, ?, ?, ?)", user.Id, user.Name, user.ServerId, user.RSA, user.NewCount);
                        server.Users.Add(p1.Users[0], user);
                        Users.Add(user);
                        form.textBox1_TextChanged(null, null);
                    }

                    try
                    {
                        text = DecryptWithRSA(encryptedText, server.RSA);
                    }
                    catch (Exception ex) { }

                    if (text == null)
                    {
                        try
                        {
                            text = DecryptAesBase64(encryptedText, user.Key);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }

                    DataBase.Send("INSERT INTO messages (uuid, serverid, userid, date, status, text, sender) VALUES (?, ?, ?, ?, ?, ?, ?)",
                        p1.UUIDs[0],
                        server.ServerId,
                        p1.Users[0],
                        p1.Dates[0],
                        1,
                        text,
                        p1.Users[0]
                        );
                    user.NewCount++;

                    if (user == form.userNow)
                    {
                        Message message = new Message()
                        {
                            UUID = p1.UUIDs[0],
                            ServerId = server.Id,
                            Date = p1.Dates[0],
                            Sender = p1.Users[0],
                            Status = 1,
                            Text = text,
                            UserId = p1.Users[0],
                        };
                        form.messages.Add(message);
                        form.AddMessage(message);
                    }
                    continue;
                }
                PacketServerToClientStatusMessages p2 = PacketStruct.TryParse<PacketServerToClientStatusMessages>(packet);
                if (p2 != null)
                {
                    DataBase.Send("UPDATE messages SET  status = ? WHERE uuid = ?", p2.Checks[0], p2.UUIDs[0]);
                    form.messages.FindAll(m => m.UUID == p2.UUIDs[0]).ForEach(m => m.Status = p2.Checks[0]);
                    form.UpdateMessage(p2.UUIDs[0]);
                    continue;
                }
                PacketServerToClientRequestOnConnectP2P p3 = PacketStruct.TryParse<PacketServerToClientRequestOnConnectP2P>(packet);
                if (p3 != null)
                {
                    User user = server.Users.GetValueOrDefault(p3.UserId, null);
                    if(user == null)
                    {
                        user = SearchUserList.FirstOrDefault(user => user.Id == p3.UserId && user.ServerId == server.ServerId, null);
                        if(user != null)
                        {
                            server.Users.Add(p3.UserId, user);
                            Users.Add(user);
                            DataBase.Send("INSERT INTO users (id, name, serverid, rsa, newcount) VALUES (?, ?, ?, ?, ?)", user.Id, user.Name, user.ServerId, user.RSA, user.NewCount);
                        }
                    }
                    else
                    {
                        if(user.RSA != p3.RSA)
                        {
                            user.RSA = p3.RSA;
                            user.Key = null;
                            DataBase.Send("UPDATE users SET RSA = ?, key = ? WHERE serverid = ? AND id = ?", user.RSA, null, server.ServerId, user.Id);
                        }
                    }
                    if(user == null)
                    {
                        user = new User()
                        {
                            Server = server,
                            Id = p3.UserId,
                            ServerId = server.ServerId,
                            Name = p3.UserName,
                            RSA = p3.RSA,
                            NewCount = 0,
                        };
                        user.Panel = new PanelUser(user);
                        Users.Add(user);
                        server.Users.Add(p3.UserId, user);
                        DataBase.Send("INSERT INTO users (id, name, serverid, rsa, newcount) VALUES (?, ?, ?, ?, ?)", user.Id, user.Name, user.ServerId, user.RSA, user.NewCount);
                    }

                    form.textBox1_TextChanged(null, null);

                    user.Session = channelManager.Connect(new IPEndPoint(IPAddress.Parse(p3.Ip), p3.Port));
                    sesssionsUsers.Add(user.Session, user);
                    sesssions.Add(user.Session);

                    Task.Run(() => { DaemonSession(user); });
                    continue;
                }
                PacketServerToClientAnswerOnConnectP2P p4 = PacketStruct.TryParse<PacketServerToClientAnswerOnConnectP2P>(packet);
                if (p4 != null)
                {
                    if (waitP2P != null)
                    {
                        Task.Run(() =>
                        {
                            SecureSession<IPEndPoint> s = null;
                            for (int i = 0; i < 100; i++)
                            {
                                s = sesssions.FirstOrDefault(s => /*s.RemoteAddress.Address.Address.ToString() == p4.Ip &&*/ s.RemoteAddress.Port == p4.Port, null);
                                if (s != null)
                                {
                                    sesssionsUsers.Add(s, waitP2P);
                                    waitP2P.Session = s;
                                    Task.Run(() => { DaemonSession(waitP2P); });
                                    break;
                                }
                                Thread.Sleep(100);
                            }
                            if(s == null)
                            {
                                newP2P.Remove(waitP2P);
                            }
                            else
                            {
                                newP2P[waitP2P] = 1;
                            }
                            waitP2P = null;
                        });
                    }
                    continue;
                }
                PacketServerToClientAnswerOnSearchUser p5 = PacketStruct.TryParse<PacketServerToClientAnswerOnSearchUser>(packet);
                if (p5 != null)
                {
                    if(SearchUserCount > 0)
                    {
                        SearchUserCount--;
                        for(int i = 0; i < p5.Names.Length; i++)
                        {
                            if(p5.Ids[i] == server.Id)
                            {
                                continue ;
                            }

                            if (server.Users.ContainsKey(p5.Ids[i]))
                            {
                                SearchUserList.Add(server.Users[p5.Ids[i]]);
                            }
                            else
                            {
                                User user = new User()
                                {
                                    RSA = p5.RSAs[i],
                                    Id = p5.Ids[i],
                                    Name = p5.Names[i],
                                    ServerId = server.Id,
                                    Server = server,
                                    NewCount = 0,
                                };
                                user.Panel = new PanelUser(user);
                                SearchUserList.Add(user);
                            }
                        }
                        if (SearchUserCount == 0)
                        {
                            form.SearchUserCallBack(SearchUserList);
                        }
                    }
                    else
                    {
                        if (p5.Ids.Length == 1)
                        {

                            User user = new User()
                            {
                                RSA = p5.RSAs[0],
                                Id = p5.Ids[0],
                                Name = p5.Names[0],
                                ServerId = server.Id,
                                Server = server,
                                NewCount = 0,
                            };
                            user.Panel = new PanelUser(user);
                            if (server.Users.ContainsKey(user.Id))
                            {
                                //юзер перезашел и наш симетричный ключ пропал
                                if (server.Users[user.Id].RSA != user.RSA)
                                {
                                    server.Users[user.Id].RSA = user.RSA;
                                    server.Users[user.Id].Key = null;
                                    DataBase.Send("UPDATE users SET RSA = ?, key = ? WHERE serverid = ? AND id = ?", user.RSA, null, server.ServerId, user.Id);
                                }
                                LoadUser = server.Users[user.Id];
                            }
                            else
                            {
                                LoadUser = user;
                            }
                        }
                        else
                        {
                            //TODO нужна ли ошибка?
                        }
                    }
                    continue;
                }
                PacketServerToClientErrorP2P p6 = PacketStruct.TryParse<PacketServerToClientErrorP2P>(packet);
                if (p6 != null)
                {
                    if (server.Users.ContainsKey(p6.Id) && newP2P.ContainsKey(server.Users[p6.Id])){
                        newP2P.Remove(server.Users[p6.Id]);
                    }
                }
            
            
            }
            server.Session = null;
        }

        public User LoadUser = null;
        public User OnLoadUser(ServerInfo server, int id)
        {
            User user = SearchUserList.Find(user => user.Id == id && user.ServerId == server.ServerId);
            if (user != null)
            {
                return user;
            }
            LoadUser = null;
            server.Session.Send(new PacketClientToServerGetUser()
            {
                Id = id,
            });
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(100);
                if(LoadUser != null)
                {
                    User u = LoadUser;
                    LoadUser = null;
                    return u;
                }
            }
            return server.Users.GetValueOrDefault(id, null);
        }


        public List<Message> LoadChat(int Server, int User)
        {
            Servers[Server].Users[User].NewCount = 0;
            DataBase.Send("UPDATE users SET newcount = ? WHERE serverid = ? AND id = ?", 0, Server, User);
            return DataBase.Message(User, Server).OrderBy(m => m.Date).ToList();
        }


        Dictionary<User, int> newP2P = new Dictionary<User, int>();


        //функция лочит поток вызова, юзать через Task.Run
        public Message SendMessage(User user, string text, string? uuid = null)
        {
            if (!user.Server.Users.ContainsKey(user.Id))
            {
                user.Server.Users.Add(user.Id, user);
                Users.Add(user);
                DataBase.Send("INSERT INTO users (id, name, serverid, rsa, newcount) VALUES (?, ?, ?, ?, ?)", user.Id, user.Name, user.ServerId, user.RSA, user.NewCount);
            }
            Message message = new Message()
            {
                ServerId = user.ServerId,
                UserId = user.Id,
                Text = text,
                Date = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                UUID = uuid == null? Guid.NewGuid().ToString() : uuid,
                Status = 0,
                Sender = user.Server.Id
            };
            bool send = false;
            //p2p пытается установится, поэтому ждем
            if (user.Session == null && newP2P.ContainsKey(user) && newP2P[user] == 0)
            {
                for (int i = 0; i < 250; i++)
                {
                    Thread.Sleep(100);
                    if (!newP2P.ContainsKey(user)) break;
                }
            }
            //отправляем через p2p
            if (user.Session != null)
            {
                if (user.Session.Send(new PacketMessageSend()
                {
                    UUID = message.UUID,
                    Text = text,
                }))
                {
                    send = true;
                    message.Status = 1;
                }
            }
            if (!send)
            {
                //создаем p2p для отправки, если его не было
                if (user.Server.Session != null && user.Session == null)
                {
                    if (user.Server.Session.Send(new PacketClientToServerConnectP2P()
                    {
                        UserId = user.Id,
                    })){
                        waitP2P = user;
                        newP2P.TryAdd(user, 0);
                        //ожидаем p2p
                        for (int i = 0; i < 250; i++)
                        {
                            Thread.Sleep(100);
                            if (newP2P.ContainsKey(user))
                            {
                                if (newP2P[user] == 1)
                                {
                                    if (user.Session.Send(new PacketMessageSend()
                                    {
                                        UUID = message.UUID,
                                        Text = text,
                                    }))
                                    {
                                        send = true;
                                        message.Status = 1;
                                    }
                                    newP2P.Remove(user);
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                else
                {
                    message.Status = -2;
                }
            }

            if (!send)
            {
                //пытаемся отправить через сервер
                if (user.Server.Session != null && user.RSA != null && user.Server.Session.Send(new PacketClientToServerSendMessage()
                {
                    Id = user.Id,
                    Text = user.Key == null || user.Key == "" ? EncryptWithRSA(text, user.RSA) : EncryptAesBase64(text, user.Key),
                    UUID = message.UUID
                }))
                {
                    message.Status = 0;
                }
                else
                {
                    message.Status = -2;
                }
            }

            if(uuid == null)
            {
                DataBase.Send("INSERT INTO messages (uuid, serverid, userid, date, status, text, sender) VALUES (?, ?, ?, ?, ?, ?, ?)",
                    message.UUID,
                    message.ServerId,
                    message.UserId,
                    message.Date,
                    message.Status,
                    text,
                    message.Sender
                    );
            }
            else
            {
                DataBase.Send("UPDATE messages SET status = ?, date = ? WHERE uuid = ?", message.Status, message.Date, message.UUID);
            }


            return message;
        }




        int SearchUserCount = 0;
        List<User> SearchUserList = new List<User>();
        public bool SearchUser(string pattern)
        {
            SearchUserList = new List<User>();
            foreach (var item in Servers)
            {
                if(item.Value.Session != null && item.Value.Session.Send(new PacketClientToServerSearchUser()
                {
                    Name = pattern
                })){
                    SearchUserCount++;
                }
            }
            return SearchUserCount > 0;
        }
    }
}
