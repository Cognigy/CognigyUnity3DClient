# Warning
This repository is no longer actively maintained. Our Unity3D integration was once created to demonstrate the inter-connectivity between a game and our conversational AI platform. Our integration essentially implements the way how a client can connect to our infrastructure by using the socket.io library and some messaging-protocol on top of this library. Feel free to fork this repository and adjust it - pull requests are welcome!

You can find our SocketClient, here:
https://github.com/Cognigy/socketclient

---
### CognigyUnity3DClient

*Created and tested with Unity 3D 2018.1.3f1*

#### - Important -
The current version (2.0.0) and all the following versions of this client will connect to the COGNIGY.AI version 3.0+, which causes some fundamental changes in the manner the connection is built up. The consequence is that you cannot connect with an older version (below 2.0.0) of this client to COGNIGY.AI 3.0 resources and you cannot connect with versions older than 2.0.0 of this plugin to COGNIGY.AI below version 3.0.

#### Documentation
For a detailed documentation visit [our Docs](https://docs.cognigy.com/docs/integrate-the-unity-3d-client)