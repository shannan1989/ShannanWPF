using System;

namespace Shannan.WeChat.Framework
{
    internal class DataRequestEventArgs : EventArgs
    {
        public DataRequestEventArgs(DataRequest request, string value = "", Exception e = null)
        {
            Request = request;
            Value = value;
            Exception = e;
        }

        public DataRequest Request
        {
            get;
            private set;
        }

        public string Value
        {
            get;
            private set;
        }

        public Exception Exception
        {
            get;
            private set;
        }
    }
}
