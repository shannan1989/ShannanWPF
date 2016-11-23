using System.Collections.Concurrent;

namespace Shannan.WeChat.Framework
{
    internal enum MemberType
    {
        Common = 1,
        Group = 2,
        OfficialAccount = 3,
    }

    internal class WeixinMember
    {
        public WeixinMember()
        {
            MemberList = new ConcurrentDictionary<string, WeixinMember>();
        }

        public int Uin { get; set; }

        public string UserName { get; set; }

        public string NickName { get; set; }

        public string RemarkName { get; set; }

        public string HeadImgUrl { get; set; }

        public MemberType Type { get; set; }

        public ConcurrentDictionary<string, WeixinMember> MemberList { get; set; }
    }
}
