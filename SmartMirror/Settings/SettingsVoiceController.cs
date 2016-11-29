using System.Collections.Generic;
using Windows.Media.SpeechRecognition;
using SmartMirror.VoiceControlModule;

namespace SmartMirror.Settings
{
    class SettingsVoiceController : IVoiceController
    {
        public bool IsVoiceControlLoaded { get; set; }
        public bool IsVoiceControlEnabled { get; set; }
        public bool HasCommands { get; private set; }
        public string VoiceControlKey { get; }
        public string GrammarFilePath { get; }
        public SpeechRecognitionGrammarFileConstraint Grammar { get; set; }
        private readonly SettingsModel settingsModel;
        private readonly Queue<SpeechRecognitionResult> CommandsReceived;

        public SettingsVoiceController(string grammarFilePath, SettingsModel model)
        {
            IsVoiceControlLoaded = false;
            IsVoiceControlEnabled = false;
            VoiceControlKey = "settings";
            GrammarFilePath = grammarFilePath;
            settingsModel = model;
            HasCommands = false;
            CommandsReceived = new Queue<SpeechRecognitionResult>();
        }

        /// <summary>
        /// Receive a voice recognition result and handle logic based on command.
        /// </summary>
        public void ProcessVoiceCommand()
        {
            if (CommandsReceived.Count > 0)
            {
                SpeechRecognitionResult command = CommandsReceived.Dequeue();
                IReadOnlyDictionary<string, IReadOnlyList<string>> tags = command.SemanticInterpretation.Properties;
                string cmd = tags.ContainsKey(SettingsCommands.TAG_CMD) ? tags[SettingsCommands.TAG_CMD][0] : "";
                settingsModel.HandleVoiceCommand(cmd);
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
