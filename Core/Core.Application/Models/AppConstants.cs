namespace Core.Application.Models
{
    public static class AppConstants
    {
        public static readonly int PageNumber = 1;
        public static readonly int PageSize = 20;
        public const int CommandMaxExecutionDelayInMilliseconds = 60000;

        public const string DefaultLanguage = "ar";
        public const string ArabicLanguage = "ar";
        public const string EnglishLanguage = "en";


        public static string CurrentLanguage { get; set; } = "ar";
        public static bool CurrentLanguageIsArabic
        {
            get 
            {
                return 0 == string.Compare(CurrentLanguage, "ar", true); 
            }
        }
    }
}
