namespace Shannan.WeChat.Framework
{
    internal class Weixin
    {
        private Weixin()
        {
        }

        static Weixin()
        {
            Instance = new Weixin();
        }

        public static Weixin Instance
        {
            get;
            private set;
        }

        public string AppId
        {
            get
            {
                return "wx782c26e4c19acffb";
            }
        }

        public string Uuid
        {
            get;
            set;
        }

        public string Uin
        {
            get;
            set;
        }

        public string Skey
        {
            get;
            set;
        }

        public string Sid
        {
            get;
            set;
        }

        public string PassTicket
        {
            get;
            set;
        }
    }
}
