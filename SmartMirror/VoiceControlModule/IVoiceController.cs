using System.Collections.Generic;
using Windows.Media.SpeechRecognition;

namespace SmartMirror.VoiceControlModule
{
    public interface IVoiceController
    {
        //Interface Properties
        bool IsVoiceControlLoaded { get; set; }
        bool IsVoiceControlEnabled { get; set; }
        //TODO THIS IS NOT THREAD SAFE IN THE IMPLEMENTATIONS!!!!
        bool HasCommands { get; }
        string VoiceControlKey { get; }
        string GrammarFilePath { get; }
        SpeechRecognitionGrammarFileConstraint Grammar { get; set; }

        //Interface Methods
        void ProcessVoiceCommand();
        void EnqueueCommand(SpeechRecognitionResult result);
    }
}