using System.Net.Sockets;
using System.Net;
using M9Studio.SecureStream;
using M9Studio.ShadowTalk.Core;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;
using M9Studio.ShadowTalk.Client.Packet;
using Microsoft.VisualBasic.ApplicationServices;

namespace M9Studio.ShadowTalk.Client
{
    partial class Core
    {
        public List<User> Users;
        public Dictionary<int, ServerInfo> Servers = new Dictionary<int, ServerInfo>();
        public DataBase DataBase = new DataBase();

        public Core()
        {
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
                    }
                }
                Connect(item);
            }
        }

        

        private void Daemon(ServerInfo server)
        {
            while (server.Session.IsLive)
            {
                JObject packet = server.Session.ReceiveJObject();
                PacketServerToClientSendMessages p1 = null;
                PacketServerToClientStatusMessages p2 = null;
                PacketServerToClientRequestOnConnectP2P p3 = null;
                PacketServerToClientAnswerOnConnectP2P p4 = null;
                PacketServerToClientAnswerOnSearchUser p5 = null;
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

    }
}
