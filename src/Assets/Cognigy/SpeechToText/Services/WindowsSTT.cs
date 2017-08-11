using UnityEngine;
#if UNITY_STANDALONE_WIN
using UnityEngine.Windows.Speech;
#endif

public class WindowsSTT : SpeechToTextService
{
#if UNITY_STANDALONE_WIN
    private WindowsSTTOptions windowsSTTOptions;
    private DictationRecognizer dictationRecognizer;

    public override void Initialize(SpeechToTextOptions speechToTextOptions)
    {
        if (speechToTextOptions.GetType() == typeof(WindowsSTTOptions))
        {
            windowsSTTOptions = speechToTextOptions as WindowsSTTOptions;

            dictationRecognizer = new DictationRecognizer();
            dictationRecognizer.AutoSilenceTimeoutSeconds = windowsSTTOptions.AutoSilenceTimeoutSeconds;

            dictationRecognizer.DictationResult += (result, confidence) =>
            {
                if (confidence <= windowsSTTOptions.ConfidenceLevel)
                    base.OnSTTResult(new SpeechToTextResultEventArgs(result));
                else
                    Debug.LogWarning("Windows STT Result under confidence level");
            };

            dictationRecognizer.DictationError += (string error, int hresult) =>
            {
                ErrorLogger.LogError(error + " " + hresult);
            };
        }
    }

    /// <summary>
    /// Starts the windows speech to text as a streaming service
    /// </summary>
    public override void EnableSpeechToText()
    {
        if (dictationRecognizer.Status != SpeechSystemStatus.Running)
        {
            dictationRecognizer.Start();
        }
    }

    /// <summary>
    /// Stops the the windows speech to text as a streaming service
    /// </summary>
    public override void DisableSpeechToText()
    {
        if (dictationRecognizer.Status == SpeechSystemStatus.Running)
        {
            dictationRecognizer.Stop();
        }
    }
#endif
}
