using System;
using UnityEngine;

public static class ErrorLogger
{
    private const string NoTTSOptions = "No TTS options set. You need to create Text to Speech options to use the Text to Speech component." +
        "Create it by clicking at 'Window/Cognigy/Text to Speech' and place it on the Text to Speech component.";

    private const string NoSTTOptions = "No STT options set. You need to create Speech to Text options to use the Speech to Text component." +
        " Create it by clicking at 'Window/Cognigy/Speech To Text' and place it on your Speech to Text component.";

    public static void LogError(string errorMessage)
    {
        Debug.LogError(errorMessage);
    }

    public static void LogException(Exception exception)
    {
        Debug.LogError("-- " + exception.GetType().ToString() + " --" + "\n" + exception.Message + "\n\n" + exception.StackTrace);
    }

    public static void LogNoTTSOptions()
    {
        Debug.LogError(NoTTSOptions);
    }

    public static void LogNoSTTOptions()
    {
        Debug.LogError(NoSTTOptions);
    }
}
