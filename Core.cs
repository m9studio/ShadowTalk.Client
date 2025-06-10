using M9Studio.ShadowTalk.Core;
using Newtonsoft.Json.Linq;
using M9Studio.ShadowTalk.Client.Packet;
using System.Security.Policy;
using M9Studio.SecureStream;
using System.Net;

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
                        u.Panel = new PanelUser(u);
                        u.Server = item;
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

                    continue;
                }
                PacketServerToClientStatusMessages p2 = PacketStruct.TryParse<PacketServerToClientStatusMessages>(packet);
                if (p2 != null)
                {

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
                            DataBase.Send("INSERT INTO users (id, name, serverid, rsa, newcount) VALUES (?, ?, ?, ?, ?)", user.Id, user.Name, user.ServerId, user.RSA, user.NewCount);
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
                        server.Users.Add(p3.UserId, user);
                        DataBase.Send("INSERT INTO users (id, name, serverid, rsa, newcount) VALUES (?, ?, ?, ?, ?)", user.Id, user.Name, user.ServerId, user.RSA, user.NewCount);
                    }

                    form.textBox1_TextChanged(null, null);

                    user.Session = channelManager.Connect(new IPEndPoint(IPAddress.Parse(p3.Ip), p3.Port));
                    sesssionsUsers.Add(user.Session, user);
                    sesssions.Add(user.Session);
                    continue;
                }
                PacketServerToClientAnswerOnConnectP2P p4 = PacketStruct.TryParse<PacketServerToClientAnswerOnConnectP2P>(packet);
                if (p4 != null)
                {
                    if(waitP2P != null)
                    {
                        Task.Run(() =>
                        {
                            SecureSession<IPEndPoint> s = null;
                            for (int i = 0; i < 100; i++)
                            {
                                s = sesssions.FirstOrDefault(s => s.RemoteAddress.Address.Address.ToString() == p4.Ip && s.RemoteAddress.Port == p4.Port, null);
                                if (s != null)
                                {
                                    sesssionsUsers.Add(s, waitP2P);
                                    waitP2P.Session = s;
                                    break;
                                }
                                Thread.Sleep(100);
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
                        //TODO
                    }
                    continue;
                }
            }
            server.Session = null;
        }

        public List<Message> LoadChat(int Server, int User)
        {
            Servers[Server].Users[User].NewCount = 0;
            DataBase.Send("UPDATE users SET newcount = ? WHERE serverid = ? AND id = ?", 0, Server, User);
            return DataBase.Message(User, Server).OrderBy(m => m.Date).ToList();
        }

        public int SendMessage(User user, string text, string? uuid = null)
        {
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
                if (user.Server.Session != null)
                {
                    if (user.Server.Session.Send(new PacketClientToServerSendMessage()
                    {
                        Id = user.Id,
                        Text = user.Key == null ? EncryptWithRSA(text, user.RSA) : EncryptAesBase64(text, user.Key),
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


            return message.Status;
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
