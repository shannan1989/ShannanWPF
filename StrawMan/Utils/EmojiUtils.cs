using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Xml;

namespace Shannan.StrawMan.Utils
{
    internal static class EmojiUtils
    {
        private static Dictionary<string, BitmapImage> _emojiCodes = new Dictionary<string, BitmapImage>();

        static EmojiUtils()
        {
            Init();
        }

        public static Dictionary<string, BitmapImage> EmojiCodes
        {
            get
            {
                return _emojiCodes;
            }
        }

        private static void Init()
        {
            XmlDocument xmlDoc = new XmlDocument();
            StreamResourceInfo info = Application.GetResourceStream(new Uri("/Resources/Emojis.xml", UriKind.Relative));
            xmlDoc.Load(info.Stream);
            XmlNode root = xmlDoc.SelectSingleNode("array");
            XmlNodeList nodeList = root.ChildNodes;
            //循环列表，获得相应的内容
            foreach (XmlNode xn in nodeList)
            {
                XmlElement xe = xn as XmlElement;
                XmlNodeList subList = xe.ChildNodes;
                foreach (XmlNode xmlNode in subList)
                {
                    if (xmlNode.Name == "array")
                    {
                        XmlElement lastXe = xmlNode as XmlElement;
                        foreach (XmlNode lastNode in lastXe)
                        {
                            if (lastNode.Name == "a")
                            {
                                string key = "[" + lastNode.InnerText + "]";
                                string val = lastNode.Attributes[1].Value.ToString();

                                BitmapImage bitmap = new BitmapImage();
                                bitmap.BeginInit();
                                bitmap.UriSource = new Uri("pack://application:,,,/Images/Emojis/" + val + ".png");
                                bitmap.EndInit();
                                _emojiCodes.Add(key, bitmap);
                            }
                        }
                    }
                }
            }
        }
    }
}
