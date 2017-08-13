using System;
using UnityEngine;

[AddComponentMenu("Speech To Text")]
public class SpeechToText : MonoBehaviour
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

    [SerializeField]
    public SpeechToTextOptions speechToTextOptions;
    private SpeechToTextService speechToTextService;

    private void Awake()
    {
        if (speechToTextOptions != null)
        {
            speechToTextService = gameObject.AddComponent(speechToTextOptions.GetServiceType()) as SpeechToTextService;
            speechToTextService.Initialize(speechToTextOptions);
        }
        else
        {
            ErrorLogger.LogNoSTTOptions();
        }
    }

    /// <summary>
    ///enables speech to text streaming service for listening
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
