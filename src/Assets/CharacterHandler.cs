using Cognigy;
using UnityEngine;

// Place this script on the same gameObject which the Cognigy AI component is attached to
public class CharacterHandler : MonoBehaviour
{
    private CognigyAI cognigyAI;                    // reference to the AI component
    private bool initDone;

    private void Awake()
    {
        cognigyAI = GetComponent<CognigyAI>();
        cognigyAI.ConnectAIClient();                // Connects the AI client with the server
        cognigyAI.OnOutput += OnOutput;             // Event that provides the reponse from the Cognigy AI,
    }                                               // the right hand side of the assignment is the method
                                                    // which gets called whenever a response comes in
    private void Update()
    {
        if (cognigyAI.HasAI && !initDone)           // We have to wait until the client is connected to send messages to the AI
        {
            initDone = true;                        // Now we're able to send and receive messages from the Cognigy AI because now the AI
                                                    // is connected to the server (cognigyAI.HasAI == true)

            cognigyAI.AISendMessage("Hi");          // We can send simple text
        }
    }

    private void OnOutput(object sender, OutputEventArgs args)  // This method gets called whenever the AI sends a response
    {
        Debug.Log("OUTPUT:\n" + args.Output.text);                // Output object contains the text from the Cognigy AI 

        if (args.Output.data != null)
            Debug.Log("DATA:\n" + args.Output.data.ToString());     // Output object also contains the data from the Cognigy AI (gets sent as Json)
    }
}