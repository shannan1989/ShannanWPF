using Newtonsoft.Json.Linq;
using System;

namespace Shannan.Framework
{
    public class DataRequestEventArgs : EventArgs
    {
        public DataRequestEventArgs(DataRequest request, JObject value = null, Exception e = null)
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

        public JObject Value
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
