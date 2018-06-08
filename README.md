# CognigyUnity3DClient

*Created and tested with Unity 3D 2018.1.3f1*

  ## - Important -
  ---
The current version (2.0.0) and all the following versions of this client will connect to the COGNIGY.AI version 3.0+, which causes some fundamental changes in the manner the connection is built up. The consequence is that you cannot connect with an older version (below 2.0.0) of this client to COGNIGY.AI 3.0 resources and you cannot connect with versions older than 2.0.0 of this plugin to COGNIGY.AI below version 3.0.

## About

The Cognigy Unity3D client provides primarily the ability (within Unity3D) to connect to a **Socket Endpoint** created with COGNIGY.AI. You can find informations about creating and configuring a Socket Endpoint in our [Documentation](https://docs.cognigy.com/docs/deploy-a-socket-endpoint). There are in addition components which can be used to easily implement speech to text and text to speech services. Right now the plugin supports:

#### Speech To Text

- Windows ( runs locally but only on Windows 10 systems )

  

#### Text to Speech

- Acapela ( uses the Voice-as-a-Service Cloud API )

- Speechlib ( runs locally and uses the Microsoft Speech API )

## Installation
Download the .unitypackage and drag it into your Unity3D scene. Import at least the **Cognigy** and the **Plugins** folder and all their subfolders. You also need to set the **API Compatability Level** to .NET 2.0 (*Edit/Project Settings/Player/Other Settings*). Or you can just open the **Unity Project** located in the **src** folder.

## Requirements

### Credentials
If you want to use the COGNIGY.AI service within your project you need to have a valid Login for the COGNIGY.AI platform. For the different speech to text and text to speech services you'll need to accquire the necessary credentials from the given provider.

### Socket Endpoint
Within COGNIGY.AI you have to create and configure a **Socket Endpoint**. You can find instructions how to create a Socket Endpoint in our [Documentation](https://docs.cognigy.com/docs/deploy-a-socket-endpoint). After creating a Socket Endpoint and assigning a Flow to the Endpoint (see Documentation), you have to get the following two strings of the Endpoint:
* Endpoint URL
* URL Token
These informations are used within the CognigyUnity3DClient for connecting to the specified Socket Endpoint.

### Cognigy AI Setup
To use the Cognigy AI service you'll have to attach the **CognigyAI component** (*Component/Cognigy/CognigyAI*) to a gameObject. There a two possible ways to achieve that:

1. Click on *Window/Cognigy/COGNIGY.AI* and you'll see an editor window where you can fill all the necessary and optional fields for the Cognigy AI service. Next you have the option to just create the AI options asset or to directly attach the Cognigy AI component to a (in the scene) selected gameObject which places the just filled AI options on the component.

2. Do a right-click in the project view and choose *Create/Cognigy/Cognigy Socket Options* to create the socket options. Now you are able to fill the necessary and optional fields in the inspector view. When you've finished filling the fields you can place the **CognigyAI component** on the gameObject of your choice. Next drag the **Socket Endpoint Options** asset onto the empty *Socket Endpoint Options* field on the Cognigy AI component.

Now the CogngiyAI component is all set up and ready to use.

  

### Speech To Text Setup

If you want to use one of the speech to text services you'll have to attach the Speech To Text component (*Component/Speech To Text*) to a gameObject. There a two possible ways to achieve that:

  

1. Click on *Window/Cognigy/Speech To Text* and you'll see an editor window where you can select the speech to text service provider of your choice. Select a service provider and fill all the given fields. Next you have the option to just create the options asset or to directly attach the Speech To Text component to a (in the scene) selected gameObject which places the just filled options on the component.

  

2. Do a right-click in the project view and choose *Create/Speech To Text/*. Within this menu you have the choice to create an options asset for different providers. Select one of the service provider options and then you are able to fill the necessary fields in the inspector view. When you've finished filling the fields you can place the Speech To Text component on the gameObject of your choice. Next drag the just created options asset onto the empty *Speech To Text Options* field on the Speech To Text component.

  

### Text to Speech Setup

The setup for the text to speech services works similiar to the setup workflow for the speech to text services but with the text to speech menus.

  

## Usage

### Cognigy AI

  

```cs

using  Cognigy;

using  Newtonsoft.Json.Linq;

using  UnityEngine;

  

public  class  CharacterHandler : MonoBehaviour

{

private  CognigyAI cognigyAI;

private  bool initDone;

  

private  void Awake()

{

cognigyAI = GetComponent<CognigyAI>();

cognigyAI.ConnectAIClient(); // Connects the AI client with the server

cognigyAI.OnOutput += OnOutput; // Event that provides the reponse from the Cognigy AI

}

  

private  void Update()

{

if(cognigyAI.HasAI && !initDone)

{

initDone = true; // Now we're able to send and receive message from the Cognigy AI

  

UnityDataForCognigyAI unityData = new  UnityDataForCognigyAI

{

Position = gameObject.transform.position

};

  

cognigyAI.AISendMessage("Hi"); // We can send simple text

cognigyAI.AISendMessage(unityData); // Or data to the Cognigy AI

}

}

  

private  void OnOutput(object sender, OutputEventArgs args)

{

Debug.Log("OUTPUT:\n" + args.FlowOutput.text); // Output object contains the text from the COGNIGY.AI Flow

  

if(args.Output.data != null)

Debug.Log("DATA:\n" + args.Output.data.ToString()); // Output object also contains the data from the COGNIGY.AI Flow sent by e.g. a Say Node (gets sent as Json)

}

}

```

  

### Speech To Text

  

```cs

using  UnityEngine;

  

public  class  SpeechToTextHandler : MonoBehaviour

{

private  SpeechToText speechToText; // Speech to Text component which is attached to this gameObject

  

private  bool streaming;

  

private  void Awake()

{

speechToText = GetComponent<SpeechToText>();

}

  

private  void Start()

{

speechToText.STTResult += OnSTTResult; // Subscribe to the event that provides the STT result

speechToText.ProcessAudioToText(microphoneInput); // Takes a speech as AudioClip which gets processed to text (non streaming service)

}

  

private  void Update()

{

if(Input.GetKeyDown(KeyCode.R))

{

if(streaming)

{

Debug.Log("Streaming STT Off");

speechToText.DisableSpeechToText(); // Disables the speech to text service (streaming service)

streaming = false;

} else

{

Debug.Log("Streaming STT On");

speechToText.EnableSpeechToText(); // Enables the speech to text service (streaming service)

streaming = true;

}

}

}

  

private  void OnSTTResult(object sender, SpeechToTextResultEventArgs args)

{

Debug.Log(args.STTResult); // STT result as text can be accessed by the STTResult property of the EventArgs object

}

}

  

```

  

### Text To Speech

  

```cs

using  UnityEngine;

  

public  class  TextToSpeechHandler : MonoBehaviour

{

private  TextToSpeech textToSpeech; // Text to Speech component which is attached to this gameObject

private  AudioSource audioSource; // AudioSource to play the received TTS audio clip

  

private  void Awake()

{

textToSpeech = GetComponent<TextToSpeech>();

audioSource = GetComponent<AudioSource>();

}

  

private  void Start()

{

textToSpeech.TTSResult += OnTTSResult; // Subscribe to the event that provides the result from the TTS service

textToSpeech.ProcessTextToAudio("Hi"); // Send text to the service to retrieve the TTS audio clip

}

  

private  void OnTTSResult(object sender, TextToSpeechResultEventArgs args)

{

audioSource.PlayOneShot(args.TTSResult); // TTS result can be accessed by the TTSResult property of the EventArgs object

}

  

private  void OnDisable()

{

textToSpeech.TTSResult -= OnTTSResult;

}

}

```