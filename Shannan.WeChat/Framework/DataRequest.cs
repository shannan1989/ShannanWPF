using System;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Web;
using System.Windows;

namespace Shannan.WeChat.Framework
{
    internal class DataRequest : IDisposable
    {
        protected string _uri;
        protected NameValueCollection _param;
        protected Stream _postStream;
        protected string _postString;

        private HttpWebRequest _request;
        private HttpWebResponse _response;
        private Stream _responseStream;
        private MemoryStream _ms;
        private byte[] _buffer;

        public DataRequestMethod Method
        {
            get;
            set;
        }

        public delegate void DataRequestEventHandler(object sender, DataRequestEventArgs e);

        public event DataRequestEventHandler RaiseCompletedEvent;

        static DataRequest()
        {
            if (ServicePointManager.DefaultConnectionLimit < 16)
            {
                ServicePointManager.DefaultConnectionLimit = 16;
            }
            ServicePointManager.Expect100Continue = false;
        }

        private DataRequest(string uri)
        {
            _uri = uri;
            _buffer = new byte[1024 * 64];
            _ms = new MemoryStream(1024 * 64);
            Method = DataRequestMethod.POST;
        }

        public DataRequest(string uri, NameValueCollection param) : this(uri)
        {
            _param = param == null ? new NameValueCollection() : param;
        }

        public virtual void SetPostString(string postString)
        {
            if (Method != DataRequestMethod.POST)
            {
                throw new InvalidOperationException("请求方法为GET时无法设置POST串");
            }

            _postString = postString;
        }

        public virtual bool Start()
        {
            _uri = NormalizeUri(_uri, Method);
            _request = CreateRequest();
            if (_request == null)
                return false;
            else if (Method == DataRequestMethod.POST)
                _request.BeginGetRequestStream(RequestCallback, this);
            else
                _request.BeginGetResponse(ResponseCallback, this);

            return true;
        }

        private string NormalizeUri(string uri, DataRequestMethod method)
        {
            string s = _uri;

            if (method == DataRequestMethod.POST)
                return s;
            else
            {
                Uri u = new Uri(s);
                NameValueCollection param = new NameValueCollection();
                param.Add(_param);
                param.Add(HttpUtility.ParseQueryString(u.Query));

                StringBuilder sb = new StringBuilder();
                foreach (string name in param.AllKeys)
                {
                    if (sb.Length > 0)
                        sb.Append("&");
                    sb.Append(name);
                    sb.Append("=");
                    sb.Append(param.Get(name));
                }

                UriBuilder ub = new UriBuilder(u);
                ub.Query = sb.ToString();

                return ub.Uri.ToString();
            }
        }

        private HttpWebRequest CreateRequest()
        {
            HttpWebRequest request = WebRequest.Create(_uri) as HttpWebRequest;
            if (request == null)
                return null;

            request.Method = Method == DataRequestMethod.POST ? "POST" : "GET";
            request.Timeout = 60 * 1000;
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
            request.Headers.Add("Pragma", "no-cache");
            request.Headers.Add("Cache-Control", "no-cache");
            if (Method == DataRequestMethod.POST)
            {
                request.ContentType = "application/x-www-form-urlencoded";
            }
            request.Accept = "text/html, */*";

            request.CookieContainer = Weixin.Instance.Cookies;

            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.116 Safari/537.36";

            request.AllowAutoRedirect = true;
            request.KeepAlive = false;

            return request;
        }

        private void RequestCallback(IAsyncResult asyncResult)
        {
            try
            {
                if (Method == DataRequestMethod.POST)
                {
                    Stream ps = _request.EndGetRequestStream(asyncResult);

                    if (_postStream != null)
                    {
                        _postStream.CopyTo(ps);
                        _postStream.Close();
                        _postStream = null;
                    }
                    else if (_postString != null)
                    {
                        byte[] buffer = Encoding.UTF8.GetBytes(_postString);
                        ps.Write(buffer, 0, buffer.Length);
                    }
                    else
                    {
                        NameValueCollection param = new NameValueCollection();
                        param.Add(_param);

                        if (param.Count > 0)
                        {
                            StringBuilder sb = new StringBuilder();
                            foreach (string key in param.AllKeys)
                            {
                                if (sb.Length > 0)
                                    sb.Append('&');
                                sb.Append(Uri.EscapeDataString(key).Replace("%20", "+"));
                                sb.Append('=');
                                sb.Append(Uri.EscapeDataString(param.Get(key)).Replace("%20", "+"));
                            }
                            byte[] buffer = Encoding.UTF8.GetBytes(sb.ToString());

                            ps.Write(buffer, 0, buffer.Length);
                        }
                    }
                    ps.Close();
                }

                _request.BeginGetResponse(ResponseCallback, this);
            }
            catch (Exception e)
            {
                RaiseEvent(null, e);
                Clean();
            }
        }

        private void ResponseCallback(IAsyncResult asyncResult)
        {
            try
            {
                _response = _request.EndGetResponse(asyncResult) as HttpWebResponse;

                String contentEncoding = _response.Headers["Content-Encoding"] == null ? String.Empty : _response.Headers["Content-Encoding"].ToLower();
                if (contentEncoding.Contains("gzip"))
                    _responseStream = new GZipStream(_response.GetResponseStream(), CompressionMode.Decompress);
                else if (contentEncoding.Contains("deflate"))
                    _responseStream = new DeflateStream(_response.GetResponseStream(), CompressionMode.Decompress);
                else
                    _responseStream = _response.GetResponseStream();

                _responseStream.BeginRead(_buffer, 0, _buffer.Length, ReadCallback, this);
            }
            catch (Exception e)
            {
                RaiseEvent(null, e);
                Clean();
            }
        }

        private void ReadCallback(IAsyncResult asyncResult)
        {
            try
            {
                int read = _responseStream.EndRead(asyncResult);
                if (read == 0)
                {
                    if (RaiseCompletedEvent != null)
                    {
                        string value = string.Empty;
                        Exception e = null;
                        _ms.Seek(0, SeekOrigin.Begin);
                        using (StreamReader reader = new StreamReader(_ms, Encoding.UTF8))
                        {
                            value = reader.ReadToEnd();
                        }

                        RaiseEvent(value, e);
                    }
                    Clean();
                }
                else if (read > 0)
                {
                    _ms.Write(_buffer, 0, read);
                    _responseStream.BeginRead(_buffer, 0, _buffer.Length, ReadCallback, this);
                }
                else
                {
                    RaiseEvent(null, new Exception("未知的网络错误"));
                    Clean();
                }
            }
            catch (Exception e)
            {
                RaiseEvent(null, e);
                Clean();
            }
        }

        private void RaiseEvent(string value, Exception e)
        {
            if (RaiseCompletedEvent != null)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(delegate
                {
                    RaiseCompletedEvent(this, new DataRequestEventArgs(this, value, e));
                }));
            }
        }

        protected void Clean()
        {
            if (_ms != null)
            {
                _ms.Close();
                _ms = null;
            }
            if (_responseStream != null)
            {
                _responseStream.Close();
                _responseStream = null;
            }
            if (_response != null)
            {
                _response.Close();
                _response = null;
            }
            _request = null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Clean();
            }
        }
    }
}
