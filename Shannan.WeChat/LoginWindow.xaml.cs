using Newtonsoft.Json.Linq;
using Shannan.WeChat.Framework;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml;

namespace Shannan.WeChat
{
    public partial class LoginWindow : Window
    {
        private Thread thread;
        private int _tip;

        public LoginWindow()
        {
            InitializeComponent();
            Loaded += delegate
            {
                GetLoginQrcode();
            };

            ReLoginButton.Click += delegate
            {
                GetLoginQrcode();
            };

            QrcodeHyperlink.Click += delegate
            {
                if (QrcodeHyperlink.NavigateUri != null)
                {
                    System.Diagnostics.Process.Start(QrcodeHyperlink.NavigateUri.OriginalString);
                }
            };
        }

        private void GetLoginQrcode()
        {
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("appid", Weixin.Instance.AppId);
            parameters.Add("redirect_uri", "https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxnewloginpage");
            parameters.Add("fun", "new");
            parameters.Add("lang", "zh_CN");
            parameters.Add("_", GetTimeStamp());
            DataRequest request = new DataRequest("https://login.wx.qq.com/jslogin", parameters);
            request.Method = DataRequestMethod.GET;
            request.RaiseCompletedEvent += (sender, e) =>
            {
                if (e.Exception != null)
                {
                    LoginHint.Text = e.Exception.Message;
                    ReLoginButton.Visibility = Visibility.Visible;
                }
                else
                {
                    JObject values = ParseJs(e.Value);
                    if (values["window.QRLogin.code"].ToString() == "200")
                    {
                        Weixin.Instance.Uuid = values["window.QRLogin.uuid"].ToString();

                        string qrcode = "https://login.weixin.qq.com/qrcode/" + Weixin.Instance.Uuid + "?t=" + GetTimeStamp();
                        LoginQrcode.Source = new BitmapImage(new Uri(qrcode));

                        QrcodeHyperlink.NavigateUri = new Uri(qrcode);

                        LoginHint.Text = "请用微信扫描二维码登录";

                        thread = new Thread(new ThreadStart(LoopCheckLoginStatus));
                        thread.Start();
                    }
                }
            };
            request.Start();
        }

        private void LoopCheckLoginStatus()
        {
            _tip = 1;
            int count = 0;
            while (true)
            {
                CheckLoginStatus();

                count++;
                if (count > 5)
                {
                    break;
                }
            }
        }

        private void CheckLoginStatus()
        {
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("loginicon", "true");
            parameters.Add("uuid", Weixin.Instance.Uuid);
            parameters.Add("tip", _tip.ToString());
            parameters.Add("r", (~int.Parse(GetTimeStamp())).ToString());
            parameters.Add("_", GetTimeStamp());
            DataRequest request = new DataRequest("https://login.weixin.qq.com/cgi-bin/mmwebwx-bin/login", parameters);
            request.Method = DataRequestMethod.GET;
            request.RaiseCompletedEvent += (sender, e) =>
            {
                if (e.Exception != null)
                {
                    LoginHint.Text = e.Exception.Message;
                    CheckLoginStatus();
                }
                else
                {
                    if (_tip == -1)
                    {
                        return;
                    }

                    JObject values = ParseJs(e.Value);
                    if (values["window.code"].ToString() == "200")
                    {
                        _tip = -1;
                        LoginHint.Text = "登录成功";

                        GetLoginParams(values["window.redirect_uri"].ToString());
                    }
                    else if (values["window.code"].ToString() == "201")
                    {
                        _tip = 0;
                        byte[] binaryData = Convert.FromBase64String(values["window.userAvatar"].ToString().Replace("data:img/jpg;base64,", ""));
                        BitmapImage bi = new BitmapImage();
                        //bi.BeginInit();
                        bi.CacheOption = BitmapCacheOption.OnLoad;
                        bi.StreamSource = new MemoryStream(binaryData, 0, binaryData.Length);
                        //bi.EndInit();
                        LoginQrcode.Source = bi;

                        LoginHint.Text = "扫描成功";

                        CheckLoginStatus();
                    }
                    else if (values["window.code"].ToString() == "408")
                    {
                        CheckLoginStatus();
                    }
                }
            };
            request.Start();
        }

        private void GetLoginParams(string url)
        {
            DataRequest request = new DataRequest(url + "&fun=new&version=v2", null);
            request.Method = DataRequestMethod.GET;
            request.RaiseCompletedEvent += (sender, e) =>
            {
                if (e.Exception != null)
                {
                }
                else
                {
                    JObject values = ParseXml(e.Value);
                    Weixin.Instance.Skey = values["skey"].ToString();
                    Weixin.Instance.Sid = values["wxsid"].ToString();
                    Weixin.Instance.Uin = values["wxuin"].ToString();
                    Weixin.Instance.PassTicket = values["pass_ticket"].ToString();

                    DialogResult = true;
                    Close();
                }
            };
            request.Start();
        }

        private JObject ParseXml(string xml)
        {
            JObject value = new JObject();
            XmlDocument xd = new XmlDocument();
            xd.LoadXml(xml);
            foreach (XmlNode node in xd.SelectSingleNode("error").ChildNodes)
            {
                value.Add(node.Name, node.InnerText);
            }
            return value;
        }

        private JObject ParseJs(string js)
        {
            JObject value = new JObject();
            JArray kv_array = new JArray();
            string[] a = js.Replace("\'", "\"").Split(new string[] { "\"" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string i in a)
            {
                if (i.Contains(";") && i.Contains("=") && i.Contains("window."))
                {
                    string[] ttt = i.Split(new string[] { "=", ";" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string t in ttt)
                    {
                        if (t.Trim() != string.Empty)
                        {
                            kv_array.Add(t.Trim());
                        }
                    }
                }
                else if (i == ";")
                {
                }
                else
                {
                    if (i.Trim() != string.Empty)
                    {
                        kv_array.Add(i.Trim());
                    }
                }
            }

            for (int i = 0; i < kv_array.Count;)
            {
                value.Add(kv_array[i].ToString(), kv_array[i + 1]);
                i += 2;
            }

            return value;
        }

        private string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string t = Convert.ToInt64(ts.TotalSeconds).ToString();
            return t;
        }
    }
}
