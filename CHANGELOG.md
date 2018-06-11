# Changelog
## [Version 2.0.0] (08.06.2018)
### Changed
- AI Client
    - Updated the connection behaviour to be able to connect to COGNIGY.AI 3.0

- AI Options renamed to **SocketEndpointOptions**
    - Reduced content to the new needed fields

- Renamed **RawMessage** to **CognigyInput**
    - Now contains the necessary parameters for the COGNIGY.AI 3.0 socket connection
    - Each message now has a *URLToken*, *userId*, *sessionId* and *source* field

- Cognigy AI Window
    - Displays now the necessary fields
    - Removed flow settings fields since these options are configured in the socket endpoint
    
### Removed
- Since the COGNIGY.AI 3.0 Update causes changes in the connection buildup, some classes are now unnecessary - the following are removed:
    - /UtilityClasses/Connection/InitializationParameters
    - /UtilityClasses/Connection/RequestBodyContent
    - /UtilityClasses/Connection/ResponseBodyContent
    - /UtilityClasses/Exceptions/CognigyRequestException

- /UtilityClasses/ErrorLogger 
- /UtilityClasses/EventArgs/StepEventArgs
- /UtilityClasses/Response/Step

## [Version 1.0.3] (02.10.2017)

### Changed
- Cognigy AI
    - Edited accessors to prevent interference of multiple clients

- AI Client
    - Added some connection options to enable multiple connections

## [Version 1.0.2] (30.08.2017)

### Bugfix
- fixed NullReference which occured at opening a options window (STT, TTS, AI)

## [Version 1.0.1] (17.08.2017)

### Added
- AI Options
    - Added toggle to enable/disable listening to step events

### Changed
- Cognigy AI
    - Improved performance of sending and receiving messages

- UI
    - Minor changes

## [Version 1.0.0] (11.08.2017)

### Added
 - AI
    - Cognigy AI Client

- Speech To Text
    - Windows STT

- Text To Speech
    - Acapela TTS
    - SpeechLib TTS