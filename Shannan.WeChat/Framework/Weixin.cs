using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Xml;

namespace Shannan.WeChat.Framework
{
    internal class Weixin
    {
        private Weixin()
        {
            Cookies = new CookieContainer();
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

        public string Host
        {
            get;
            set;
        }

        public CookieContainer Cookies
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

        public string DeviceId
        {
            get;
            set;
        }

        public JObject SyncKey
        {
            get;
            set;
        }

        public string FormatSyncKey()
        {
            List<string> items = new List<string>();
            foreach (JObject item in (JArray)SyncKey["List"])
            {
                items.Add(item["Key"] + "_" + item["Val"]);
            }
            return string.Join("|", items.ToArray());
        }

        public static JObject ParseJs(string js)
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

        public static JObject ParseXml(string xml)
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

        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string t = Convert.ToInt64(ts.TotalMilliseconds).ToString();
            return t;
        }
    }
}
