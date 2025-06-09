using System.Data.SQLite;

namespace M9Studio.ShadowTalk.Client
{
    public class DataBase
    {
        private SQLiteConnection connection;
        public DataBase()
        {
            string connectionString = "Data Source=localdb.sqlite;Version=3;";
            bool isNew = false;
            if (!File.Exists("localdb.sqlite"))
            {
                SQLiteConnection.CreateFile("localdb.sqlite");
                isNew = true;
            }
            connection = new SQLiteConnection(connectionString);
            connection.Open();
            if (isNew)
            {
                string createUsersTable = @"
                    CREATE TABLE IF NOT EXISTS users (
                        serverid INTEGER,
                        id INTEGER PRIMARY KEY,
                        name TEXT,
                        key TEXT,
                        rsa TEXT,
                        newcount INTEGER
                    )";

                string createServersTable = @"
                    CREATE TABLE IF NOT EXISTS servers (
                        serverid INTEGER PRIMARY KEY,
                        servername TEXT,
                        ip TEXT,
                        port INTEGER,
                        rsa TEXT,
                        name TEXT,
                        id INTEGER,
                        k TEXT,
                        publica TEXT,
                        privatea TEXT
                    )";

                string createMessagesTable = @"
                    CREATE TABLE IF NOT EXISTS messages (
                        uuid TEXT PRIMARY KEY,
                        serverid INTEGER,
                        userid INTEGER,
                        date INTEGER,
                        status INTEGER,
                        text TEXT,
                        sender INTEGER
                    )";


                new SQLiteCommand(createUsersTable, connection).ExecuteNonQuery();
                new SQLiteCommand(createServersTable, connection).ExecuteNonQuery();
                new SQLiteCommand(createMessagesTable, connection).ExecuteNonQuery();
            }

        }
        public List<ServerInfo> ServerInfo()
        {
            var servers = new List<ServerInfo>();
            var command = new SQLiteCommand("SELECT * FROM servers", connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                servers.Add(new ServerInfo
                {
                    ServerId = Convert.ToInt32(reader["serverid"]),
                    ServerName = reader["servername"].ToString(),
                    IP = reader["ip"].ToString(),
                    Port = Convert.ToInt32(reader["port"]),
                    RSA = reader["rsa"].ToString(),
                    Name = reader["name"].ToString(),
                    Id = Convert.ToInt32(reader["id"]),
                    K = reader["k"].ToString(),
                    PublicA = reader["publica"].ToString(),
                    PrivateA = reader["privatea"].ToString(),
                    //token = reader["token"].ToString(),
                    //hmac = reader["hmac"].ToString()
                });
            }
            return servers;
        }
        public List<Message> Message(int user, int server)
        {
            var messages = new List<Message>();
            using var command = new SQLiteCommand("SELECT * FROM messages WHERE userid = @userId AND serverid = @serverId", connection);
            command.Parameters.AddWithValue("@userId", user);
            command.Parameters.AddWithValue("@serverId", server);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                messages.Add(new Message
                {
                    UUID = reader["uuid"].ToString(),
                    ServerId = Convert.ToInt32(reader["serverid"]),
                    UserId = Convert.ToInt32(reader["userid"]),
                    Date = Convert.ToInt32(reader["date"]),
                    Status = Convert.ToInt32(reader["status"]),
                    Text = reader["text"].ToString(),
                    Sender = Convert.ToInt32(reader["sender"])
                });
            }
            return messages;
        }
        public List<User> User()
        {
            var users = new List<User>();
            var command = new SQLiteCommand("SELECT * FROM users", connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                users.Add(new User
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Name = reader["name"].ToString(),
                    Key = reader["key"].ToString(),
                    RSA = reader["rsa"].ToString(),
                    ServerId = Convert.ToInt32(reader["serverid"]),
                    NewCount = Convert.ToInt32(reader["newcount"])
                });
            }
            return users;
        }

        public void Send(string request, params object[] param)
        {
            using (var cmd = new SQLiteCommand(request, connection))
            {
                foreach (var p in param)
                {
                    cmd.Parameters.AddWithValue(null, p);
                }
                cmd.ExecuteNonQuery();
            }
        }
    }
}
