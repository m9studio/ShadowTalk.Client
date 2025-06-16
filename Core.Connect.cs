using M9Studio.SecureStream;
using M9Studio.ShadowTalk.Core;
using Newtonsoft.Json.Linq;
using System.Net.Sockets;
using System.Net;
using System.Globalization;
using System.Numerics;
using System.Text;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;

namespace M9Studio.ShadowTalk.Client
{
    public partial class Core
    {
        public bool NewConnect(ServerInfo server, string Email, string Password)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint address = new IPEndPoint(IPAddress.Any, 0);
            socket.Bind(address);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(server.IP), server.Port);
            server.Socket = socket;

            TcpSecureTransportAdapter adapter = new TcpSecureTransportAdapter(socket);

            server.ChannelManager = new SecureChannelManager<IPEndPoint>(adapter);
            socket.Connect(endPoint);
            server.Session = server.ChannelManager.Connect(endPoint);

            if (server.Session.Send(new PacketClientToServerLogin()
            {
                Email = Email,
                //Password = Password
            }))
            {
                JObject packet1 = server.Session.ReceiveJObject();
                PacketServerToClientLoginError error = PacketStruct.TryParse<PacketServerToClientLoginError>(packet1);
                if (error == null)
                {
                    PacketServerToClientChallengeSRP newSRP = PacketStruct.TryParse<PacketServerToClientChallengeSRP>(packet1);
                    // Шаг 1: подготовка
                    string saltHex = newSRP.Salt;
                    BigInteger B = BigInteger.Parse(newSRP.B, NumberStyles.HexNumber);
                    BigInteger N = SRPConstants.N;
                    BigInteger g = SRPConstants.g;

                    // Шаг 2: хешируем salt + password, получаем x
                    string combined = saltHex + Password;
                    byte[] xHash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(combined));
                    BigInteger x = new BigInteger(xHash, isUnsigned: true, isBigEndian: true);

                    // Шаг 3: генерируем a и A
                    byte[] aBytes = SRPHelper.GenerateRandomBytes(32);
                    BigInteger a = new BigInteger(aBytes, isUnsigned: true, isBigEndian: true);
                    BigInteger A = BigInteger.ModPow(g, a, N);

                    // Шаг 4: считаем u = H(A | B)
                    BigInteger u = SRPHelper.ComputeU(A, B, N);

                    // Шаг 5: считаем S = (B - k * g^x) ^ (a + ux) mod N
                    BigInteger gx = BigInteger.ModPow(g, x, N);
                    BigInteger k = SRPConstants.k;
                    BigInteger baseS = (B - k * gx + N) % N;
                    baseS = (baseS % N + N) % N;
                    BigInteger exp = a + u * x;
                    BigInteger S = BigInteger.ModPow(baseS, exp, N);


                    byte[] K = SRPHelper.Hash(S);

                    // Шаг 6: считаем M1
                    string M1 = SRPHelper.ComputeM1(A, B, K);
                    /*
                    Debug.WriteLine($"Client X = {x}");
                    Debug.WriteLine("CLIENT SALT: " + saltHex);
                    Debug.WriteLine("CLIENT X: " + x.ToString("X"));
                    Debug.WriteLine($"CLIENT A = {A.ToString("X")}");
                    Debug.WriteLine($"CLIENT B = {B.ToString("X")}");
                    Debug.WriteLine($"CLIENT u = {u}");
                    Debug.WriteLine($"CLIENT S = {S.ToString("X")}");
                    Debug.WriteLine($"CLIENT K = {Convert.ToHexString(K)}");
                    Debug.WriteLine($"CLIENT M1 = {M1}");
                    */
                    (string publicKey, string privateKey) = GenRSA();

                    server.RSA = privateKey;
                    server.K = Convert.ToHexString(K);
                    server.Id = newSRP.Id;
                    server.Name = newSRP.Name;
                    server.PrivateA = a.ToString("X");
                    server.PublicA = A.ToString("X");

                    server.Session.Send(new PacketClientToServerResponseSRP()
                    {
                        A = A.ToString("X"),
                        M1 = M1,
                        Port = Port,
                        RSA = publicKey
                    });
                    try
                    {
                        JObject packet2Ignore = server.Session.ReceiveJObject();
                        JObject packet3Ignore = server.Session.ReceiveJObject();
                        DataBase.Send("INSERT INTO servers (serverid, servername, ip, port, id, name, rsa, k, publica, privatea) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)",
                            server.ServerId,
                            server.ServerName,
                            server.IP,
                            server.Port,
                            server.Id,
                            server.Name,
                            server.RSA,
                            server.K,
                            server.PublicA,
                            server.PrivateA);
                        Servers.Add(server.ServerId, server);
                        Task.Run(() => {
                            Daemon(server);
                        });
                        return true;
                    }
                    catch (Exception e)
                    {

                    }
                }
                else
                {

                }
            }
            server.ChannelManager = null;
            server.Session = null;
            server.Socket = null;
            return false;
        }


        private void Connect(ServerInfo server)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint address = new IPEndPoint(IPAddress.Any, 0);
            socket.Bind(address);
            TcpSecureTransportAdapter adapter = new TcpSecureTransportAdapter(socket);

            server.ChannelManager = new SecureChannelManager<IPEndPoint>(adapter);

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(server.IP), server.Port);
            socket.Connect(endPoint);
            SecureSession<IPEndPoint> Session = server.ChannelManager.Connect(endPoint);
            server.Socket = socket;


            Debug.WriteLine("Client: ");
            Debug.WriteLine($"K {server.K}");
            Debug.WriteLine($"token {server.token}");
            Debug.WriteLine($"hmac {server.hmac}");
            Debug.WriteLine($"pri {server.PrivateA}");
            Debug.WriteLine($"pub {server.PublicA}");

            if (Session.Send(new PacketClientToServerReconectSRP
            {
                Token = server.token,
                HMAC = server.hmac,
                A = server.PublicA,
                Port = Port
            }))
            {
                JObject packet = Session.ReceiveJObject();//либо ошибка, либо сообщения
                PacketServerToClientLoginError error = PacketStruct.TryParse<PacketServerToClientLoginError>(packet);
                if (error == null)
                {
                    JObject packet2 = Session.ReceiveJObject();//инфа о сообщениях

                    PacketServerToClientSendMessages messages = PacketStruct.TryParse<PacketServerToClientSendMessages>(packet);
                    PacketServerToClientStatusMessages status = PacketStruct.TryParse<PacketServerToClientStatusMessages>(packet2);

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
                            if (!Session.Send(new PacketClientToServerGetUser()
                            {
                                Id = messages.Users[i]
                            }))
                            {
                                continue;
                            }
                            JObject packet3 = Session.ReceiveJObject();
                            PacketServerToClientAnswerOnSearchUser u = PacketStruct.TryParse<PacketServerToClientAnswerOnSearchUser>(packet3);
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
                    server.Session = Session;
                    Task.Run(() => {
                        Daemon(server);
                    });
                    return;
                }
            }
            server.ChannelManager = null;
            server.Session = null;
            server.Socket = null;
        }
    
    
    
    
    }
}
