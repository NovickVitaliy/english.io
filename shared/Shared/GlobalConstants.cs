namespace Shared;

public static class GlobalConstants
{
    public static class ApplicationClaimTypes
    {
        public const string PreferencesConfigured = "PreferencesConfigured";
        public const string ExampleSentencesPerWord = "ExampleSentencesPerWord";
        public const string CountOfWordsForPractice = "CountOfWordsForPractice";
    }

    public static class Languages
    {
        public const string English = "en-US";
        public const string Ukrainian = "uk-UA";

        public static readonly string[] SupportedLanguages = ["ukrainian", "english"];
    }
}
