using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.SpeechRecognition;
using SmartMirror.VoiceControlModule;
using SmartMirror.WeatherModule.Models;

namespace SmartMirror.ClockModule
{
    class ClockVoiceController : IVoiceController
    {
        public bool IsVoiceControlLoaded { get; set; }
        public bool IsVoiceControlEnabled { get; set; }
        public bool HasCommands { get; private set; }
        public string VoiceControlKey { get; }
        public string GrammarFilePath { get; }
        public SpeechRecognitionGrammarFileConstraint Grammar { get; set; }
        private readonly ClockModel clockModel;
        private Queue<SpeechRecognitionResult> CommandsReceived;


        public ClockVoiceController(string grammarFilePath, ClockModel model)
        {
            IsVoiceControlLoaded = false;
            IsVoiceControlEnabled = false;
            VoiceControlKey = "clock";
            GrammarFilePath = grammarFilePath;
            clockModel = model;
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
                string format = tags.ContainsKey(ClockCommands.TAG_FORMAT) ? tags[ClockCommands.TAG_FORMAT][0] : "";
                clockModel.HandleVoiceCommand(format);
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
