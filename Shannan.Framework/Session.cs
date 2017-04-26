using System.Collections.Specialized;
using System.Net;

namespace Shannan.Framework
{
    internal class Session
    {
        private NameValueCollection _param;

        private Session()
        {
            Init();
        }

        static Session()
        {
            Instance = new Session();
        }

        public static Session Instance
        {
            get;
            private set;
        }

        public string BaseUri
        {
            get;
            set;
        }

        public CookieContainer Cookies
        {
            get;
            private set;
        }

        public NameValueCollection Param
        {
            get
            {
                NameValueCollection param = new NameValueCollection();
                param.Add(_param);
                return param;
            }
            private set
            {
                _param = value;
            }
        }

        public void Init()
        {
            Cookies = new CookieContainer();
            _param = new NameValueCollection();
            BaseUri = string.Empty;
        }

        public void AddPersistedParam(string name, string value)
        {
            _param.Add(name, value);
        }

        public void RemovePersistedParam(string name)
        {
            _param.Remove(name);
        }

        public void ClearPersistedParam()
        {
            _param.Clear();
        }
    }
}
