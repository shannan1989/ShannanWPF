using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shannan.WeChat.Framework;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Shannan.WeChat
{
    public partial class MainWindow : Window
    {
        private Thread thread;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += delegate
            {
                WeChatInit();
            };
        }

        private void WeChatInit()
        {
            NameValueCollection parameters = new NameValueCollection();
            DataRequest request = new DataRequest("https://" + Weixin.Instance.Host + "/cgi-bin/mmwebwx-bin/webwxinit?r=" + (~long.Parse(Weixin.GetTimeStamp())) + "&pass_ticket=" + Weixin.Instance.PassTicket, parameters);

            JObject body = new JObject();
            JObject r = new JObject();
            r.Add("Uin", Weixin.Instance.Uin);
            r.Add("Sid", Weixin.Instance.Sid);
            r.Add("Skey", Weixin.Instance.Skey);
            r.Add("DeviceID", Weixin.Instance.DeviceId);
            body.Add("BaseRequest", r);
            request.SetPostString(JsonConvert.SerializeObject(body));

            request.Method = DataRequestMethod.POST;
            request.RaiseCompletedEvent += (sender, e) =>
            {
                if (e.Exception != null)
                {
                }
                else
                {
                    JObject values = JsonConvert.DeserializeObject(e.Value) as JObject;
                    if (int.Parse(values["BaseResponse"]["Ret"].ToString()) == 0)
                    {
                        string avatar = "https://" + Weixin.Instance.Host + values["User"]["HeadImgUrl"].ToString();
                        MyAvatar.Source = new BitmapImage(new Uri(avatar));
                        MyNickname.Text = values["User"]["NickName"].ToString();

                        Weixin.Instance.SyncKey = values["SyncKey"] as JObject;

                        thread = new Thread(new ThreadStart(WeChatConstruct));
                        thread.Start();
                    }
                    else
                    {
                        MessageBox.Show(e.Value);
                    }
                }
            };
            request.Start();
        }

        private void WeChatConstruct()
        {
            WeChatGetContact();

            WeChatSyncCheck();
        }

        private void WeChatGetContact()
        {
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("lang", "zh_CN");
            parameters.Add("pass_ticket", Weixin.Instance.PassTicket);
            parameters.Add("r", Weixin.GetTimeStamp());
            parameters.Add("skey", Weixin.Instance.Skey);
            parameters.Add("seq", "0");

            DataRequest request = new DataRequest("https://" + Weixin.Instance.Host + "/cgi-bin/mmwebwx-bin/webwxgetcontact", parameters);
            request.Method = DataRequestMethod.GET;
            request.RaiseCompletedEvent += (sender, e) =>
            {
                if (e.Exception != null)
                {
                }
                else
                {
                    JObject values = JsonConvert.DeserializeObject(e.Value) as JObject;
                    if (int.Parse(values["BaseResponse"]["Ret"].ToString()) == 0)
                    {
                        MemberCount.Text = "共有" + values["MemberCount"] + "位好友";

                        ObservableCollection<object> membersSource = new ObservableCollection<object>();
                        foreach (JObject jo in (JArray)values["MemberList"])
                        {
                            membersSource.Add(new
                            {
                                NickName = jo["NickName"].ToString(),
                                Province = jo["Province"].ToString(),
                            });
                        }
                        MemberList.Items.Clear();
                        MemberList.ItemsSource = membersSource;
                    }
                    else
                    {
                        MessageBox.Show(e.Value);
                    }
                }
            };
            request.Start();
        }

        private void WeChatSyncCheck()
        {
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("r", Weixin.GetTimeStamp());
            parameters.Add("skey", Weixin.Instance.Skey);
            parameters.Add("sid", Weixin.Instance.Sid);
            parameters.Add("uin", Weixin.Instance.Uin);
            parameters.Add("deviceid", Weixin.Instance.DeviceId);
            parameters.Add("synckey", Weixin.Instance.FormatSyncKey());
            parameters.Add("_", Weixin.GetTimeStamp());
            DataRequest request = new DataRequest("https://webpush." + Weixin.Instance.Host + "/cgi-bin/mmwebwx-bin/synccheck", parameters);
            request.Method = DataRequestMethod.GET;
            request.RaiseCompletedEvent += (sender, e) =>
            {
                if (e.Exception != null)
                {
                    WeChatSyncCheck();
                }
                else
                {
                    //retcode:
                    //0 正常
                    //1100 失败 / 退出微信
                    //selector:
                    //0 正常
                    //2 新的消息
                    //7 进入 / 离开聊天界面
                    //window.synccheck={retcode:"0",selector:"0"}

                    if (e.Value.Contains("retcode:\"0\""))
                    {
                        if (e.Value.Contains("selector:\"2\""))
                        {
                            WeChatSync();
                        }
                        else
                        {
                            WeChatSyncCheck();
                        }
                    }
                    else
                    {
                        MessageBox.Show(e.Value);
                    }
                }
            };
            request.Start();
        }

        private void WeChatSync()
        {
            DataRequest request = new DataRequest("https://" + Weixin.Instance.Host + "/cgi-bin/mmwebwx-bin/webwxsync?sid=" + Weixin.Instance.Sid + "&skey=" + Weixin.Instance.Skey + "&lang=zh_CN&pass_ticket=" + Weixin.Instance.PassTicket, null);

            JObject body = new JObject();
            JObject r = new JObject();
            r.Add("Uin", Weixin.Instance.Uin);
            r.Add("Sid", Weixin.Instance.Sid);
            r.Add("Skey", Weixin.Instance.Skey);
            r.Add("DeviceID", Weixin.Instance.DeviceId);
            body.Add("BaseRequest", r);
            body.Add("SyncKey", Weixin.Instance.SyncKey);
            body.Add("rr", (~long.Parse(Weixin.GetTimeStamp())).ToString());
            request.SetPostString(JsonConvert.SerializeObject(body));

            request.Method = DataRequestMethod.POST;
            request.RaiseCompletedEvent += (sender, e) =>
            {
                if (e.Exception != null)
                {
                }
                else
                {
                    JObject values = JsonConvert.DeserializeObject(e.Value) as JObject;
                    if (int.Parse(values["BaseResponse"]["Ret"].ToString()) == 0)
                    {
                        Weixin.Instance.SyncKey = values["SyncKey"] as JObject;

                        foreach (JObject msg in (JArray)values["AddMsgList"])
                        {
                            TextBlock tb = new TextBlock();
                            tb.Text = JsonConvert.SerializeObject(msg);
                            tb.TextWrapping = TextWrapping.Wrap;
                            tb.Margin = new Thickness(10);
                            MsgList.Children.Insert(0, tb);
                        }
                    }
                    else
                    {
                        MessageBox.Show(e.Value);
                    }
                }
                WeChatSyncCheck();
            };
            request.Start();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Application.Current.Shutdown();
            base.OnClosing(e);
        }
    }
}
