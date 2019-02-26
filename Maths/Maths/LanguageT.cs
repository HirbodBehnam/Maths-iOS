using System;
using System.Collections.Generic;
using System.Text;

namespace Maths
{
    public static class LanguageC
    {
        public const string SavedName = "Language";
        public static LanguageE SavedToEnum(int saved) => saved == 0 ? LanguageE.English : LanguageE.Persian;
    }
    public enum LanguageE
    {
        English,Persian
    }
}
