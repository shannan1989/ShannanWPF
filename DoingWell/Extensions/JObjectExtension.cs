using Newtonsoft.Json.Linq;
using System;

namespace Shannan.DoingWell
{
    public static class JObjectExtension
    {
        public static JToken GetJSONValue(this JObject self, string type, string key = "data")
        {
            try
            {
                foreach (JObject jo in (JArray)self["items"])
                {
                    if (jo["type"].ToString() == type)
                    {
                        return jo[key].Type == JTokenType.Null ? null : jo[key];
                    }
                }
            }
            catch (Exception)
            {
            }
            return null;
        }
    }
}
