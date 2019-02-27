using Xamarin.Essentials;

namespace Maths
{
    public static class LanguageC
    {
        /// <summary>
        /// The name that the language is saved
        /// </summary>
        public const string SavedName = "Language";
        /// <summary>
        /// Get the saved language; English is default
        /// </summary>
        /// <returns></returns>
        public static LanguageE SavedLanguage() => SavedToEnum(Preferences.Get(SavedName, 0));
        /// <summary>
        /// Change saved int for language to <see cref="LanguageE"/>
        /// </summary>
        /// <param name="saved">The saved int</param>
        /// <returns></returns>
        public static LanguageE SavedToEnum(int saved) => saved == 0 ? LanguageE.English : LanguageE.Persian;
        /// <summary>
        /// Change <see cref="LanguageE"/> to a int
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static int LanguageEToInt(LanguageE language) => language == LanguageE.English ? 0 : 1;
        /// <summary>
        /// Save language
        /// </summary>
        /// <param name="language">The <see cref="LanguageE"/> to save</param>
        public static void SaveLanguage(LanguageE language) => Preferences.Set(SavedName, LanguageEToInt(language));
    }
    public enum LanguageE
    {
        English,Persian
    }
}
