using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

[Serializable]
public enum AcapelaVoice
{
    laura = 1,
    saul = 2,
    tracy = 3,
    sharon = 3,
    will = 4,
    ryan = 5,
    karen = 6,
    rod = 7,
    micah = 8
}

public class AcapelaTTS : TextToSpeechService
{
    private AcapelaTTSOptions acapelaTTSOptions;

    private const string Preset = "UrlMaker.json";
    private const string Freq = "22k";

    public override void Initialize(TextToSpeechOptions ttsOptions)
    {
        if (ttsOptions.GetType() == typeof(AcapelaTTSOptions))
        {
            acapelaTTSOptions = ttsOptions as AcapelaTTSOptions;

            if (!acapelaTTSOptions.VaasUrl.EndsWith("/"))
                acapelaTTSOptions.VaasUrl += "/";
        }
    }

    public override void ProcessTextToAudio(string input)
    {
        try
        {
            StartCoroutine(GetSpeechFromVaas(input));
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    private IEnumerator GetSpeechFromVaas(string text)
    {
        AcapelaVoice voice = (AcapelaVoice)acapelaTTSOptions.Voice;

        string parameters = "prot_vers=2&cl_login=" + acapelaTTSOptions.Login +
        "&cl_app=" + acapelaTTSOptions.App + "&cl_pwd=" + acapelaTTSOptions.Password +
        "&req_voice=" + voice.ToString() + Freq + "&req_text=" + WWW.EscapeURL(text) + "&req_snd_type=WAV" + "&req_snd_ext=.wav" + "&req_asw_type=INFO";

        byte[] buffer = Encoding.UTF8.GetBytes(parameters);

        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(acapelaTTSOptions.VaasUrl + Preset);
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = buffer.Length;
        request.Accept = "application/json";

        Stream reqStream = request.GetRequestStream();
        reqStream.Write(buffer, 0, buffer.Length);
        reqStream.Close();


        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream respStream = response.GetResponseStream();
                StreamReader respStreamReader;
                using (respStreamReader = new StreamReader(respStream, Encoding.Default))
                {
                    //Get Response Data
                    string respString = respStreamReader.ReadToEnd();
                    if (!string.IsNullOrEmpty(respString))
                    {
                        JObject jobject = JObject.Parse(respString);
                        string sndUrl = (string)jobject["snd_url"];

                        WWW source = new WWW(sndUrl);
                        yield return source;
                        AudioClip audioClip = source.GetAudioClip(false, false, AudioType.WAV);

                        base.OnTTSResult(new TextToSpeechResultEventArgs(audioClip));
                    }
                }

                respStreamReader.Close();
                respStream.Close();
                response.Close();
            }
            else
            {
                request.Abort();
                response.Close();
            }
        }
    }
}
