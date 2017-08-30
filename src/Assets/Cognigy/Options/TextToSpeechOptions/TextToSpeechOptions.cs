using System;
using UnityEngine;


[Serializable]
public class TextToSpeechOptions : ServiceOptions
{
    [SerializeField]
    public int Voice = 1;
    [SerializeField]
    public int Rate = 0;
}
