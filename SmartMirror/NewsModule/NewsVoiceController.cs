using System.Collections.Generic;
using SmartMirror.VoiceControlModule;
using Windows.Media.SpeechRecognition;

namespace SmartMirror.NewsModule
{
    class NewsVoiceController : IVoiceController
    {
        public bool IsVoiceControlLoaded { get; set; }
        public bool IsVoiceControlEnabled { get; set; }
        public bool HasCommands { get; private set; }
        public string VoiceControlKey { get; }
        public string GrammarFilePath { get; }
        public SpeechRecognitionGrammarFileConstraint Grammar { get; set; }
        private readonly NewsModel newsModel;
        private Queue<SpeechRecognitionResult> CommandsReceived;


        public NewsVoiceController(string grammarFilePath, NewsModel model)
        {
            IsVoiceControlLoaded = false;
            IsVoiceControlEnabled = false;
            VoiceControlKey = "news";
            GrammarFilePath = grammarFilePath;
            newsModel = model;
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
                string cmd = tags.ContainsKey(NewsCommands.TAG_CMD) ? tags[NewsCommands.TAG_CMD][0] : "";
                string type = tags.ContainsKey(NewsCommands.TAG_TYPE) ? tags[NewsCommands.TAG_TYPE][0] : "";
                newsModel.HandleVoiceCommand(cmd, type);
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
