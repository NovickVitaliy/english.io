using Refit;

namespace Learning.LearningShared.Services;

public interface ITextToSpeechService
{
    const string ApiUrlKey = "TextToSpeech";
    [Get("/tts")]
    Task<HttpResponseMessage> GetWordAsSpeech(string text, string lang);
}
