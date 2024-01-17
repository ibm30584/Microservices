namespace Core.Application.Models
{
    public static class AppConstants
    {
        public static readonly int PageNumber = 1;
        public static readonly int PageSize = 20;
        public const int CommandMaxExecutionDelayInMilliseconds = 60000;

        public const string ArabicLanguage = "ar";
        public const string EnglishLanguage = "en";
        public static string DefaultLanguage { get; set; } = "ar";
        
    }
}
