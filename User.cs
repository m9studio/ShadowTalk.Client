using M9Studio.SecureStream;
using System.Net;

namespace M9Studio.ShadowTalk.Client
{
    public class User
    {
        public int ServerId;

        public int Id;
        public string Name;

        //симетричный ключ шифрования, для отправки через сервер
        public string Key;

        public string RSA;



        //сессия
        private SecureSession<IPEndPoint> _Session;
        public SecureSession<IPEndPoint> Session
        {
            get => _Session;
            set
            {
                _Session = value;
                if (Panel != null)
                {
                    if (value != null)
                    {
                        Panel.labelName.ForeColor = Color.Green;
                    }
                    else
                    {
                        Panel.labelName.ForeColor = Color.Black;
                    }
                }
                if (Form != null)
                {
                    if (value != null)
                    {
                        Form.labelName.ForeColor = Color.Green;
                    }
                    else
                    {
                        Form.labelName.ForeColor = Color.Black;
                    }
                }
            }
        }



        private int _newCount = 0;
        public int NewCount {
            get => _newCount;
            set
            {
                if(Form != null)
                {
                    _newCount = 0;
                }
                else
                {
                    _newCount = value;
                }
                if (Panel != null)
                {
                    if(_newCount > 0)
                    {
                        Panel.labelCount.Text = _newCount.ToString();
                    }
                    else
                    {
                        Panel.labelCount.Text = "";
                    }
                }
            }
        }
        public ServerInfo Server;
        public PanelUser Panel;
        public FormMain Form;
    }
}
