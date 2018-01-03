using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Media.Imaging;

namespace Shannan.StrawMan
{
    public static class WebImageExtension
    {
        public static BitmapImage ToBitmapImage(this string self)
        {
            try
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(self));
                StringBuilder s = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                    s.Append(data[i].ToString("x2"));

                string savePath = Path.Combine(Environment.CurrentDirectory, "cache") + @"\" + s.ToString() + ".png";

                if (File.Exists(savePath) == false)
                {
                    WebRequest request = WebRequest.Create(self);
                    WebResponse response = request.GetResponse();
                    Stream stream = response.GetResponseStream();

                    FileStream fileStream = File.Create(savePath);
                    byte[] buffer = new byte[1024];
                    int numReadByte = 0;
                    while ((numReadByte = stream.Read(buffer, 0, 1024)) != 0)
                    {
                        fileStream.Write(buffer, 0, numReadByte);
                    }
                    fileStream.Close();
                    stream.Close();
                }

                // 打开文件
                FileStream readFileStream = new FileStream(savePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                // 读取文件的 byte[]
                byte[] bytes = new byte[readFileStream.Length];
                readFileStream.Read(bytes, 0, bytes.Length);
                readFileStream.Close();
                // 把 byte[] 转换成 Stream

                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
                bmp.StreamSource = new MemoryStream(bytes);
                bmp.EndInit();

                return bmp;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
