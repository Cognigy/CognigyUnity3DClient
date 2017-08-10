using System;
using UnityEngine;


[Serializable]
public class TextToSpeechOptions : ServiceOptions
{
    [SerializeField]
    public int Voice;
    [SerializeField]
    public int Rate;
}
