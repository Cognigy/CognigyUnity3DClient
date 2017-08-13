using System;
using UnityEngine;

public static class ErrorLogger
{
    private static Func<string, string, string> CreateNoOptionsMessage = (service, name) => "No " + service + " options on gameObject:\n" + name + "\n" +
    "You need to create " + service + " options to use the component. Create it by clicking at 'Window/Cognigy/" + service + "' and place it on your component";

    public static void LogError(string errorMessage)
    {
        Debug.LogError("-- Error --\n" + errorMessage);
    }

    public static void LogException(Exception exception)
    {
        Debug.LogError("-- " + exception.GetType().ToString() + " --" + "\n" + exception.Message + "\n\n" + exception.StackTrace);
    }

    public static void LogNoOptions(string service, string name)
    {
        Debug.LogError(CreateNoOptionsMessage(service, name));
    }
}
