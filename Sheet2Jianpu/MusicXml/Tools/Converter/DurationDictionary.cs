using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicXml.Tools.Converter
{
    public static class DurationDictionary
    {
        private static Dictionary<string, int> durationDic = new Dictionary<string, int>();

        public static int Count { get { return durationDic.Count; } }

        public static KeyValuePair<string, int> ElementAt(int index)
        {
            return durationDic.ElementAt(index);
        }

        public static int Get(string key)
        {
            return durationDic[key];
        }

        public static void Set(int divisions)
        {
            try
            {
                durationDic = new Dictionary<string, int>();
                durationDic.Add("maxima", divisions * 32);
                durationDic.Add("long", divisions * 16);
                durationDic.Add("breve", divisions * 8);
                durationDic.Add("whole", divisions * 4);
                durationDic.Add("half", divisions * 2);
                durationDic.Add("quarter", divisions);
                durationDic.Add("eighth", divisions / 2);
                durationDic.Add("16th", divisions / 4);
                durationDic.Add("32nd", divisions / 8);
                durationDic.Add("64th", divisions / 16);
                durationDic.Add("128th", divisions / 32);
                durationDic.Add("256th", divisions / 64);
                durationDic.Add("512th", divisions / 128);
                durationDic.Add("1024th", divisions / 256);
            }
            catch
            {
                return;
            }
        }
    }
}
