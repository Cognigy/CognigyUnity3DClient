using System;
using UnityEngine;

/// <summary>
/// Creates Speech to Text service depending on inserted options
/// and provides method for the usage of the specific Speech to Text service
/// </summary>
[AddComponentMenu("Speech To Text")]
public class SpeechToText : ServiceComponent
{
    /// <summary>
    /// Provides the result from the selected speech to text service
    /// </summary>
    public event EventHandler<SpeechToTextResultEventArgs> STTResult
    {
        add
        {
            if (speechToTextService != null)
                speechToTextService.STTResult += value;
            else
                ErrorLogger.LogError("Speech To Text service not set");
        }
        remove
        {
            if (speechToTextService != null)
                speechToTextService.STTResult -= value;
            else
                ErrorLogger.LogError("Speech To Text service not set");
        }
    }

    /// <summary>
    /// Reference to the specific Speech to Text service
    /// </summary>
    private SpeechToTextService speechToTextService;

    private void Awake()
    {
        if (serviceOptions != null && serviceOptions is SpeechToTextOptions)
        {
            speechToTextService = gameObject.AddComponent(serviceOptions.ServiceImplementation) as SpeechToTextService;
            speechToTextService.Initialize(serviceOptions as SpeechToTextOptions);
        }
        else
        {
            ErrorLogger.LogNoOptions("Speech To Text", gameObject.name);
        }
    }

    /// <summary>
    ///enables speech to text streaming service for listening to speech input
    /// </summary>
    public void EnableSpeechToText()
    {
        try
        {
            speechToTextService.EnableSpeechToText();
        }
        catch (NotImplementedException)
        {
            Debug.LogError("selected service is a non streaming service. Try to send the speech by calling ProcessAudioToText().");
        }
    }

    /// <summary>
    /// disables the speech to text streaming service
    /// </summary>
    public void DisableSpeechToText()
    {
        try
        {
            speechToTextService.DisableSpeechToText();
        }
        catch (NotImplementedException)
        {
            Debug.LogError("selected service is a non streaming service. Try to send the speech by calling ProcessAudioToText().");
        }
    }

    /// <summary>
    ///Processes the given audioclip to text
    /// </summary>
    /// <param name="audio"></param>
    public void ProcessAudioToText(AudioClip audio)
    {
        try
        {
            speechToTextService.ProcessAudioToText(audio);
        }
        catch (NotImplementedException)
        {
            Debug.LogError("selected service is a streaming service. Try to enable/disable the service.");
        }
    }
}
