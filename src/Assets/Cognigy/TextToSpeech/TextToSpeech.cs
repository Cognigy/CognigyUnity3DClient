using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[AddComponentMenu("Text To Speech")]
public class TextToSpeech : MonoBehaviour
{
    /// <summary>
    /// Provides the result from the selected text to speech service
    /// </summary>
    public event EventHandler<TextToSpeechResultEventArgs> TTSResult
    {
        add
        {
            if (textToSpeechService != null)
                textToSpeechService.TTSResult += value;
            else
                ErrorLogger.LogError("Text to Speech service not set");
        }
        remove
        {
            if (textToSpeechService != null)
                textToSpeechService.TTSResult -= value;
            else
                ErrorLogger.LogError("Text to Speech service not set");
        }
    }

    [SerializeField]
    public TextToSpeechOptions textToSpeechOptions;
    private TextToSpeechService textToSpeechService;

    private void Awake()
    {
        if (textToSpeechOptions != null)
        {
            textToSpeechService = gameObject.AddComponent(textToSpeechOptions.GetServiceType()) as TextToSpeechService;
            textToSpeechService.Initialize(textToSpeechOptions);
        }
        else
        {
            ErrorLogger.LogNoTTSOptions();
        }
    }

    /// <summary>
    /// Processes the given input text to an audioclip
    /// </summary>
    public void ProcessTextToAudio(string input)
    {
        if (textToSpeechService != null)
            textToSpeechService.ProcessTextToAudio(input);
        else
            ErrorLogger.LogError("Text to Speech service not set");
    }
}
