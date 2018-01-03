using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Shannan.StrawMan
{
    public static class EncryptExtension
    {
        public static string Encrypt(this string data, string key = "leleketang")
        {
            string strValue = string.Empty;

            if (key != string.Empty)
            {
                // convert key to 16 characters for simplicity
                int iLength = key.Length;
                if (iLength < 16) key = key.PadRight(16, 'X');
                else if (iLength > 16) key = key.Substring(0, 16);

                // create encryption keys
                byte[] byteKey = Encoding.UTF8.GetBytes(key.Substring(0, 8));
                byte[] byteVector = Encoding.UTF8.GetBytes(key.Substring(8));

                // convert data to byte array
                byte[] byteData = Encoding.UTF8.GetBytes(data);

                // encrypt
                DESCryptoServiceProvider objDES = new DESCryptoServiceProvider();
                MemoryStream objMemoryStream = new MemoryStream();
                CryptoStream objCryptoStream = new CryptoStream(objMemoryStream, objDES.CreateEncryptor(byteKey, byteVector), CryptoStreamMode.Write);
                objCryptoStream.Write(byteData, 0, byteData.Length);
                objCryptoStream.FlushFinalBlock();

                // convert to string and Base64 encode
                strValue = Convert.ToBase64String(objMemoryStream.ToArray());
            }
            else
            {
                strValue = data;
            }

            return strValue;
        }

        public static string Decrypt(this string data, string key = "leleketang")
        {
            if (data == string.Empty)
            {
                return string.Empty;
            }

            string strValue = string.Empty;

            if (key != string.Empty)
            {
                // convert key to 16 characters for simplicity
                int iLength = key.Length;
                if (iLength < 16) key = key.PadRight(16, 'X');
                else if (iLength > 16) key = key.Substring(0, 16);

                // create encryption keys
                byte[] byteKey = Encoding.UTF8.GetBytes(key.Substring(0, 8));
                byte[] byteVector = Encoding.UTF8.GetBytes(key.Substring(8));

                // convert data to byte array and Base64 decode
                byte[] byteData = new byte[data.Length + 1];
                try
                {
                    byteData = Convert.FromBase64String(data);
                }
                catch
                {
                    // invalid length
                    strValue = data;
                }

                if (strValue == "")
                {
                    try
                    {
                        // decrypt
                        DESCryptoServiceProvider objDES = new DESCryptoServiceProvider();
                        MemoryStream objMemoryStream = new MemoryStream();
                        CryptoStream objCryptoStream = new CryptoStream(objMemoryStream, objDES.CreateDecryptor(byteKey, byteVector), CryptoStreamMode.Write);
                        objCryptoStream.Write(byteData, 0, byteData.Length);
                        objCryptoStream.FlushFinalBlock();

                        // convert to string
                        Encoding objEncoding = Encoding.UTF8;
                        strValue = objEncoding.GetString(objMemoryStream.ToArray());
                    }
                    catch
                    {
                        // decryption error
                        strValue = "";
                    }
                }
            }
            else
            {
                strValue = data;
            }

            return strValue;
        }
    }
}
