using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicXml.Tools.Converter
{
    public static class JianpuTable
    {
        private static string[,,] table = new string[4, 9, 4]{ { { "1", "!", "+1", "+!" }, { "2", "@", "+2", "+@" }, { "3", "#", "+3", "+#" }, { "4", "$", "+4", "+$" }, { "5", "%", "+5", "+%" }, { "6", "^", "+6", "+^" }, { "7", "&", "+7", "+&" }, { "8", "*", "+8", "+*" }, { "0", ")", "+0", "+)" } },
                                                        { { "q", "Q", "+q", "+Q" }, { "w", "W", "+w", "+W" }, { "e", "E", "+e", "+E" }, { "r", "R", "+r", "+R" }, { "t", "T", "+t", "+T" }, { "y", "Y", "+y", "+Y" }, { "u", "U", "+u", "+U" }, { "o", "O", "+o", "+O" }, { "p", "P", "+p", "+P" } },
                                                        { { "a", "A", "+a", "+A" }, { "s", "S", "+s", "+S" }, { "d", "D", "+d", "+D" }, { "f", "F", "+f", "+F" }, { "g", "G", "+g", "+G" }, { "h", "H", "+h", "+H" }, { "j", "J", "+j", "+J" }, { "i", "I", "+i", "+I" }, { "[", "{", "+[", "+{" } },
                                                        { { "z", "Z", "+z", "+Z" }, { "x", "X", "+x", "+X" }, { "c", "C", "+c", "+C" }, { "v", "V", "+v", "+V" }, { "b", "B", "+b", "+B" }, { "n", "N", "+n", "+N" }, { "m", "M", "+m", "+M" }, { ".", ">", "+.", "+>" }, { "-", "", "-", "" } } };

        public static string Get(int octaveIndex, int stepIndex, int typeIndex)
        {
            return table[octaveIndex, stepIndex, typeIndex];
        }
    }
}
