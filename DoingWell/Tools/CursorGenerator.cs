using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Shannan.DoingWell.Tools
{
    internal class CursorGenerator
    {
        public static Cursor Create(Uri uri, byte hotspotx, byte hotspoty)
        {
            Stream s = Application.GetResourceStream(uri).Stream;
            byte[] buffer = new byte[s.Length];

            MemoryStream ms = new MemoryStream();
            ms.WriteByte(0); // always 0
            ms.WriteByte(0);
            ms.WriteByte(2); // change file type to CUR
            ms.WriteByte(0);
            ms.WriteByte(1); // 1 icon in table
            ms.WriteByte(0);

            s.Position = 6; // skip over first 6 bytes in ICO as we just wrote

            s.Read(buffer, 0, 4);
            ms.Write(buffer, 0, 4);

            ms.WriteByte(hotspotx);
            ms.WriteByte(0);

            ms.WriteByte(hotspoty);
            ms.WriteByte(0);

            s.Position += 4; // skip 4 bytes as we just wrote our own

            int remaining = (int)s.Length - 14;

            s.Read(buffer, 0, remaining);
            ms.Write(buffer, 0, remaining);
            ms.Position = 0;

            return new Cursor(ms);
        }
    }
}
