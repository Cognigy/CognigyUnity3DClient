using SpeechLib;
using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class SpeechLibTTS : TextToSpeechService
{
    private SpeechLibTTSOptions windowsTTSOptions;
    private static int fileID = 0;
    private const string fileSuffix = "_temp.wav";

    private SpVoice spVoice;
    private SpFileStream spFileStream;
    SpeechStreamFileMode spFileMode = SpeechStreamFileMode.SSFMCreateForWrite;

    public override void Initialize(TextToSpeechOptions ttsOptions)
    {
        if (ttsOptions.GetType() == typeof(SpeechLibTTSOptions))
        {
            windowsTTSOptions = (SpeechLibTTSOptions)ttsOptions;

            SpObjectTokenCategory tokenCat = new SpObjectTokenCategory();
            tokenCat.SetId(SpeechLib.SpeechStringConstants.SpeechCategoryVoices, false);
            ISpeechObjectTokens tokens = tokenCat.EnumerateTokens(null, null);

            spVoice = new SpVoice();

            spVoice.Voice = tokens.Item(windowsTTSOptions.Voice);
            spVoice.Volume = windowsTTSOptions.Volume;
            spVoice.Rate = windowsTTSOptions.Rate;

            spFileStream = new SpFileStream();
        }
    }

    public override void ProcessTextToAudio(string input)
    {
        StartCoroutine(Speak(input));
    }

    private IEnumerator Speak(string input)
    {
        fileID++;
        string dataPath = Application.dataPath + fileID + fileSuffix;
        spFileStream.Open(dataPath, spFileMode, false);
        spVoice.AudioOutputStream = spFileStream;
        spVoice.Speak(input, SpeechVoiceSpeakFlags.SVSFlagsAsync);
        spVoice.WaitUntilDone(-1);
        spFileStream.Close();

        byte[] data = File.ReadAllBytes(dataPath);

        base.OnTTSResult(new TextToSpeechResultEventArgs(ToAudioClip(data)));
        File.Delete(dataPath);

        yield return null;
    }

    private float[] Convert16BitToFloat(byte[] input)
    {
        int inputSamples = input.Length / 2; // 16 bit input, so 2 bytes per sample
        float[] output = new float[inputSamples];
        int outputIndex = 0;
        for (int n = 0; n < inputSamples; n++)
        {
            short sample = BitConverter.ToInt16(input, n * 2);
            output[outputIndex++] = sample / 32768f;
        }
        return output;
    }

    private AudioClip ToAudioClip(byte[] chunkByteArray)
    {
        float[] finalFloatArray = Convert16BitToFloat(chunkByteArray);

        AudioClip clip = AudioClip.Create(
        "TTS_Sound",
        finalFloatArray.Length,
        1,
        22050,
        false
        );

        clip.SetData(finalFloatArray, 0);
        return clip;
    }
}
