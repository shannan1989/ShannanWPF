using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shannan.WeChat.Framework;
using System;
using System.Collections.Generic;
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
                        WeixinMember current = new WeixinMember();
                        current.UserName = values["User"]["UserName"].ToString();
                        current.NickName = values["User"]["NickName"].ToString();
                        Weixin.Instance.CurrentUser = current;
                        Weixin.Instance.SyncKey = values["SyncKey"] as JObject;

                        ObservableCollection<object> membersSource = new ObservableCollection<object>();
                        foreach (JObject jo in (JArray)values["ContactList"])
                        {
                            WeixinMember member = new WeixinMember();
                            member.UserName = jo["UserName"].ToString();
                            member.NickName = jo["NickName"].ToString();
                            if (member.UserName.Contains("@@"))
                            {
                                member.Type = MemberType.Group;
                            }
                            else if (jo["ContactFlag"].ToString() == "3")
                            {
                                member.Type = MemberType.OfficialAccount;
                            }
                            else
                            {
                                member.Type = MemberType.Common;
                            }

                            Weixin.Instance.Contact.AddOrUpdate(member.UserName, member, (key, oldValue) => member);

                            membersSource.Add(member);
                        }
                        MemberList.Items.Clear();
                        MemberList.ItemsSource = membersSource;

                        thread = new Thread(new ThreadStart(WeChatConstruct));
                        thread.Start();

                        MyAvatar.Source = new BitmapImage(new Uri("https://" + Weixin.Instance.Host + values["User"]["HeadImgUrl"].ToString()));
                        MyNickname.Text = Weixin.Instance.CurrentUser.NickName;
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
            WeChatStatusNotify();
            WeChatGetContact();
            WeChatBatchGetContact();
            WeChatSyncCheck();
        }

        private void WeChatStatusNotify()
        {
            DataRequest request = new DataRequest("https://" + Weixin.Instance.Host + "/cgi-bin/mmwebwx-bin/webwxstatusnotify?lang=zh_CN&pass_ticket=" + Weixin.Instance.PassTicket, null);

            JObject body = new JObject();
            JObject r = new JObject();
            r.Add("Uin", Weixin.Instance.Uin);
            r.Add("Sid", Weixin.Instance.Sid);
            r.Add("Skey", Weixin.Instance.Skey);
            r.Add("DeviceID", Weixin.Instance.DeviceId);
            body.Add("BaseRequest", r);
            body.Add("ClientMsgId", Weixin.GetTimeStamp());
            body.Add("Code", "3");
            body.Add("FromUserName", Weixin.Instance.CurrentUser.UserName);
            body.Add("ToUserName", Weixin.Instance.CurrentUser.UserName);
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
                    }
                    else
                    {
                    }
                }
            };
            request.Start();
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
                            WeixinMember member = new WeixinMember();
                            member.UserName = jo["UserName"].ToString();
                            member.NickName = jo["NickName"].ToString();
                            if (member.UserName.Contains("@@"))
                            {
                                member.Type = MemberType.Group;
                            }
                            else if (jo["ContactFlag"].ToString() == "3")
                            {
                                member.Type = MemberType.OfficialAccount;
                            }
                            else
                            {
                                member.Type = MemberType.Common;
                            }

                            Weixin.Instance.Contact.AddOrUpdate(member.UserName, member, (key, oldValue) => member);

                            membersSource.Add(member);
                        }
                        MemberList.ItemsSource = membersSource;
                    }
                    else
                    {
                        MessageBox.Show(e.Value);
                    }

                    WeChatBatchGetContact();
                }
            };
            request.Start();
        }

        private void WeChatBatchGetContact()
        {
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("lang", "zh_CN");
            parameters.Add("type", "ex");
            parameters.Add("pass_ticket", Weixin.Instance.PassTicket);
            parameters.Add("r", Weixin.GetTimeStamp());
            DataRequest request = new DataRequest("https://" + Weixin.Instance.Host + "/cgi-bin/mmwebwx-bin/webwxbatchgetcontact", parameters);

            JObject body = new JObject();
            JObject r = new JObject();
            r.Add("Uin", Weixin.Instance.Uin);
            r.Add("Sid", Weixin.Instance.Sid);
            r.Add("Skey", Weixin.Instance.Skey);
            r.Add("DeviceID", Weixin.Instance.DeviceId);
            body.Add("BaseRequest", r);

            JArray list = new JArray();
            foreach (KeyValuePair<string, WeixinMember> item in Weixin.Instance.Contact)
            {
                if (item.Value.Type == MemberType.Group)
                {
                    JObject member = new JObject();
                    member.Add("UserName", item.Value.UserName);
                    member.Add("EncryChatRoomId", "");
                    list.Add(member);
                }
            }
            body.Add("List", list);
            body.Add("Count", list.Count);
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

                        DealMessages((JArray)values["AddMsgList"]);
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

        private void DealMessages(JArray messages)
        {
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("MsgList", messages.ToString());
            DataRequest request = new DataRequest("http://c.me.com/ta/index.php", parameters);
            request.Start();

            foreach (JObject msg in messages)
            {
                if (msg["MsgType"].ToString() == "51")
                {
                    continue;
                }

                if (msg["MsgType"].ToString() == "49")//模板消息
                {
                    continue;
                }

                if (msg["MsgType"].ToString() == "3")
                {
                    Image img = new Image();
                    img.Source = new BitmapImage(new Uri("https://" + Weixin.Instance.Host + "/cgi-bin/mmwebwx-bin/webwxgetmsgimg?&MsgID=" + msg["MsgId"] + "&skey=" + Weixin.Instance.Skey));
                    MsgList.Children.Insert(0, img);
                }

                string txt = string.Empty;

                txt += "消息标识：" + msg["MsgId"] + Environment.NewLine;
                WeixinMember fromUser, toUser;
                if (Weixin.Instance.Contact.TryGetValue(msg["FromUserName"].ToString(), out fromUser))
                {
                    txt += "发送方昵称：" + fromUser.NickName + Environment.NewLine;
                }
                txt += "发送方标识：" + msg["FromUserName"] + Environment.NewLine;

                if (Weixin.Instance.Contact.TryGetValue(msg["ToUserName"].ToString(), out toUser))
                {
                    txt += "接收方昵称：" + toUser.NickName + Environment.NewLine;
                }
                txt += "接收方标识：" + msg["ToUserName"] + Environment.NewLine;

                txt += "内容：" + msg["Content"] + Environment.NewLine;

                msg.Remove("MsgId");
                msg.Remove("NewMsgId");
                msg.Remove("Content");
                msg.Remove("ToUserName");
                msg.Remove("FromUserName");

                msg.Remove("RecommendInfo");//推荐朋友或公众号
                msg.Remove("ImgHeight");
                msg.Remove("ImgWidth");

                txt += Environment.NewLine + JsonConvert.SerializeObject(msg);
                TextBlock tb = new TextBlock();
                tb.Text = txt;
                tb.TextWrapping = TextWrapping.Wrap;
                tb.Margin = new Thickness(10);

                MsgList.Children.Insert(0, tb);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Application.Current.Shutdown();
            base.OnClosing(e);
        }
    }
}
