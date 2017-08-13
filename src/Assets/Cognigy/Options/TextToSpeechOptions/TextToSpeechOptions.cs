using System;
using UnityEngine;


[Serializable]
public class TextToSpeechOptions : ServiceOptions
{
    [SerializeField]
    public int Voice = 0;
    [SerializeField]
    public int Rate;
}
