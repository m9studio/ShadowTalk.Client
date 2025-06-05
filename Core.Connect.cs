using M9Studio.SecureStream;
using M9Studio.ShadowTalk.Core;
using Newtonsoft.Json.Linq;
using System.Net.Sockets;
using System.Net;

namespace M9Studio.ShadowTalk.Client
{
    partial class Core
    {

        public void NewConnect(ServerInfo server, string Email, string Password)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint address = new IPEndPoint(IPAddress.Any, 0);
            socket.Bind(address);
            socket.Listen(10);
            TcpSecureTransportAdapter adapter = new TcpSecureTransportAdapter(socket);

            server.ChannelManager = new SecureChannelManager<IPEndPoint>(adapter);
            server.Session = server.ChannelManager.Connect(new IPEndPoint(IPAddress.Parse(server.IP), server.Port));

            if (server.Session.Send(new PacketClientToServerLogin()
            {
                Email = Email,
                Password = Password
            }))
            {
                JObject packet1 = server.Session.ReceiveJObject();
                PacketServerToClientLoginError error = PacketStruct.Parse<PacketServerToClientLoginError>(packet1);
                if(error == null)
                {

                }
                else
                {
                    //TODO окно с ошибкой
                }
            }
            server.ChannelManager = null;
            server.Session = null;
        }


        private void Connect(ServerInfo server)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint address = new IPEndPoint(IPAddress.Any, 0);
            socket.Bind(address);
            socket.Listen(10);
            TcpSecureTransportAdapter adapter = new TcpSecureTransportAdapter(socket);

            server.ChannelManager = new SecureChannelManager<IPEndPoint>(adapter);
            server.Session = server.ChannelManager.Connect(new IPEndPoint(IPAddress.Parse(server.IP), server.Port));

            var (a, A_hex) = SRPHelper.GenerateA();
            if (server.Session.Send(new PacketClientToServerReconectSRP
            {
                Token = server.token,
                HMAC = server.hmac,
                A = A_hex,
                Port = server.Port
            }))
            {
                JObject packet = server.Session.ReceiveJObject();//либо ошибка, либо сообщения
                PacketServerToClientLoginError error = PacketStruct.Parse<PacketServerToClientLoginError>(packet);
                if (error == null)
                {
                    JObject packet2 = server.Session.ReceiveJObject();//инфа о сообщениях

                    PacketServerToClientSendMessages messages = PacketStruct.Parse<PacketServerToClientSendMessages>(packet);
                    PacketServerToClientStatusMessages status = PacketStruct.Parse<PacketServerToClientStatusMessages>(packet2);

                    for (int i = 0; i < status.UUIDs.Length; i++)
                    {
                        DataBase.Send("UPDATE messages SET status = ? WHERE uuid = ?", status.Checks[i], status.UUIDs[i]);
                    }
                    for (int i = 0; i < messages.Users.Length; i++)
                    {

                        User? user = server.Users.GetValueOrDefault(messages.Users[i], null);
                        if (user == null)
                        {
                            //сообщение от нового юзера
                            if (!server.Session.Send(new PacketClientToServerGetUser()
                            {
                                Id = messages.Users[i]
                            }))
                            {
                                continue;
                            }
                            JObject packet3 = server.Session.ReceiveJObject();
                            PacketServerToClientAnswerOnSearchUser u = PacketStruct.Parse<PacketServerToClientAnswerOnSearchUser>(packet3);
                            if (u == null && u.Ids.Length != 1)
                            {
                                continue;
                            }
                            User newUser = new User()
                            {
                                ServerId = server.ServerId,
                                Id = u.Ids[0],
                                Name = u.Names[0],
                                RSA = u.RSAs[0],
                                Server = server
                            };
                            server.Users.Add(newUser.Id, newUser);
                            user = newUser;

                            DataBase.Send("INSERT INTO users (id, serverid, name, rsa, newcount) VALUES (?, ?, ?, ?, ?)",
                                newUser.Id,
                                newUser.ServerId,
                                newUser.Name,
                                newUser.RSA,
                                0
                                );
                        }
                        user.NewCount++;
                        DataBase.Send("UPDATE users SET newcount = ? WHERE serverid = ? AND id = ?", user.NewCount, user.ServerId, user.Id);


                        string text = null;
                        string encryptedText = messages.Texts[i];
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
                            messages.UUIDs[i],
                            server.Id,
                            messages.Users[i],
                            messages.Dates[i],
                            1,
                            text,
                            messages.Users[i]
                            );
                    }
                    Task.Run(() => {
                        Daemon(server);
                    });
                    return;
                }
            }
            server.ChannelManager = null;
            server.Session = null;
        }
    }
}
