using System;
using UnityEngine;

/// <summary>
/// Creates Text to Speech service depending on inserted options
/// and provides methods for the usage of the specific Text to Speech service
/// </summary>
[RequireComponent(typeof(AudioSource))]
[AddComponentMenu("Text To Speech")]
public class TextToSpeech : ServiceComponent
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

    private TextToSpeechService textToSpeechService;

    private void Awake()
    {
        if (serviceOptions != null && serviceOptions is TextToSpeechOptions)
        {
            textToSpeechService = gameObject.AddComponent(serviceOptions.ServiceImplementation) as TextToSpeechService;
            textToSpeechService.Initialize(serviceOptions as TextToSpeechOptions);
        }
        else
        {
            ErrorLogger.LogNoOptions("Text To Speech", gameObject.name);
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
