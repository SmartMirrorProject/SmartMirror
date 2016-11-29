using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.SpeechRecognition;
using SmartMirror.Settings;
using SmartMirror.VoiceControlModule;

namespace SmartMirror.MainModule
{
    class SmartMirrorVoiceController : IVoiceController
    {
        public bool IsVoiceControlLoaded { get; set; }
        public bool IsVoiceControlEnabled { get; set; }
        public bool HasCommands { get; private set; }
        public string VoiceControlKey { get; }
        public string GrammarFilePath { get; }
        public SpeechRecognitionGrammarFileConstraint Grammar { get; set; }
        private readonly SmartMirrorModel model;
        private readonly Queue<SpeechRecognitionResult> CommandsReceived;

        public SmartMirrorVoiceController(string grammarFilePath, SmartMirrorModel model)
        {
            IsVoiceControlLoaded = false;
            IsVoiceControlEnabled = false;
            VoiceControlKey = "smartMirror";
            GrammarFilePath = grammarFilePath;
            this.model = model;
            HasCommands = false;
            CommandsReceived = new Queue<SpeechRecognitionResult>();
        }

        public void ProcessVoiceCommand()
        {
            if (CommandsReceived.Count > 0)
            {
                SpeechRecognitionResult command = CommandsReceived.Dequeue();
                IReadOnlyDictionary<string, IReadOnlyList<string>> tags = command.SemanticInterpretation.Properties;
                string cmd = tags.ContainsKey(SmartMirrorCommands.TAG_CMD) ? tags[SmartMirrorCommands.TAG_CMD][0] : "";
                string module = tags.ContainsKey(SmartMirrorCommands.TAG_MODULE) ? tags[SmartMirrorCommands.TAG_MODULE][0] : "";
                model.HandleVoiceCommand(cmd, module);
            }
            if (CommandsReceived.Count == 0)
            {
                HasCommands = false;
            }
        }

        public void EnqueueCommand(SpeechRecognitionResult result)
        {
            CommandsReceived.Enqueue(result);
            HasCommands = true;
        }
    }
}
