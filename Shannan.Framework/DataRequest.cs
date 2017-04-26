using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Windows;

namespace Shannan.Framework
{
    public class DataRequest : IDisposable
    {
        protected string _uri;
        protected NameValueCollection _param;
        protected Dictionary<string, FormData> _uploadFiles;
        protected Stream _postStream;

        private HttpWebRequest _request;
        private HttpWebResponse _response;
        private Stream _responseStream;
        private MemoryStream _ms;
        private byte[] _buffer;

        public delegate void DataRequestEventHandler(object sender, DataRequestEventArgs e);

        public event DataRequestEventHandler RaiseCompletedEvent;

        private string Boundary
        {
            get
            {
                return "-----------" + "AaB03x";
            }
        }

        public DataRequestMethod Method
        {
            get;
            set;
        }

        public struct FormData
        {
            public string FileName;
            public Stream Stream;
        }

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
            _uploadFiles = new Dictionary<string, FormData>();
            Method = DataRequestMethod.POST;
        }

        public DataRequest(string uri, string param) : this(uri)
        {
            _param = HttpUtility.ParseQueryString(param);
        }

        public DataRequest(string uri, NameValueCollection param) : this(uri)
        {
            _param = param == null ? new NameValueCollection() : param;
        }

        public DataRequest(string uri, params object[] nameValuePairs) : this(uri, (NameValueCollection)null)
        {
            if (((int)(nameValuePairs.Length / 2)) * 2 != nameValuePairs.Length)
                throw new ArgumentException("参数数量错误");

            for (int i = 0; i < nameValuePairs.Length; i += 2)
            {
                _param.Add(nameValuePairs[i].ToString(), nameValuePairs[i + 1].ToString());
            }
        }

        protected string NormalizeUri(string uri, DataRequestMethod method)
        {
            string s = uri.IndexOf("http://", StringComparison.CurrentCultureIgnoreCase) == -1 && uri.IndexOf("https://", StringComparison.CurrentCultureIgnoreCase) == -1 ? Session.Instance.BaseUri + _uri : _uri;

            if (method == DataRequestMethod.POST)
            {
                return s;
            }
            else
            {
                Uri u = new Uri(s);
                NameValueCollection param = new NameValueCollection();
                param.Add(_param);
                param.Add(HttpUtility.ParseQueryString(u.Query));
                param.Add(Session.Instance.Param);

                StringBuilder sb = new StringBuilder();
                foreach (string name in param.AllKeys)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append("&");
                    }
                    sb.Append(name);
                    sb.Append("=");
                    sb.Append(param.Get(name));
                }

                UriBuilder ub = new UriBuilder(u);
                ub.Query = sb.ToString();

                return ub.Uri.ToString();
            }
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

        public bool Abort()
        {
            Clean();
            return true;
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
                if (_uploadFiles.Count == 0)
                    request.ContentType = "application/x-www-form-urlencoded";
                else
                    request.ContentType = "multipart/form-data; boundary=" + Boundary;
            }
            request.Accept = "text/html, */*";
            request.CookieContainer = Session.Instance.Cookies;

            AssemblyName assemName = Assembly.GetEntryAssembly().GetName();
            request.UserAgent = string.Format("{0}/{1} Webkit/{2}({3} {4}) Resolution/{5}*{6}", assemName.Name.ToLower(), assemName.Version.ToString(3), "1.0", Environment.OSVersion.Platform, Environment.OSVersion.Version.Major + "." + Environment.OSVersion.Version.Minor, SystemParameters.PrimaryScreenWidth, SystemParameters.PrimaryScreenHeight);

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
                    else if (_uploadFiles.Count > 0)
                    {
                        NameValueCollection param = new NameValueCollection();
                        param.Add(_param);
                        param.Add(Session.Instance.Param);

                        foreach (string key in param.AllKeys)
                        {
                            string value = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}\r\n", Boundary, key, param.Get(key));
                            byte[] buffer = Encoding.UTF8.GetBytes(value);
                            ps.Write(buffer, 0, buffer.Length);
                        }

                        foreach (string name in _uploadFiles.Keys)
                        {
                            string value = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: application/octet-stream\r\n\r\n", Boundary, name, _uploadFiles[name].FileName);
                            byte[] header = Encoding.UTF8.GetBytes(value);
                            ps.Write(header, 0, header.Length);

                            _uploadFiles[name].Stream.CopyTo(ps);
                            _uploadFiles[name].Stream.Close();

                            byte[] buffer = Encoding.UTF8.GetBytes("\r\n");
                            ps.Write(buffer, 0, buffer.Length);
                        }

                        byte[] trailer = Encoding.ASCII.GetBytes("--" + Boundary + "--\r\n");
                        ps.Write(trailer, 0, trailer.Length);
                    }
                    else
                    {
                        NameValueCollection param = new NameValueCollection();
                        param.Add(_param);
                        param.Add(Session.Instance.Param);

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

                string contentEncoding = _response.Headers["Content-Encoding"] == null ? string.Empty : _response.Headers["Content-Encoding"].ToLower();
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
                        JObject value = null;
                        Exception e = null;
                        _ms.Seek(0, SeekOrigin.Begin);
                        using (StreamReader reader = new StreamReader(_ms, System.Text.Encoding.UTF8))
                        {
                            value = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
                        }
                        if ((int)value["errcode"] != 0)
                        {
                            e = new Exception((String)value["msg"]);
                            e.Data["errcode"] = (int)value["errcode"];
                            value = null;
                        }
                        else
                            value = (JObject)value["data"];

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

        protected virtual void RaiseEvent(JObject value, Exception e)
        {
            if (RaiseCompletedEvent != null)
            {
                //Application.Current.Dispatcher.BeginInvoke(new Action(delegate
                //{
                //    RaiseCompletedEvent(this, new DataRequestEventArgs(this, value, e));
                //}));
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
