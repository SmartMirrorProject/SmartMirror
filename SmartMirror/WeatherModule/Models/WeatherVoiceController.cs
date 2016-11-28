using System.Collections.Generic;
using System.Diagnostics;
using Windows.Media.SpeechRecognition;
using SmartMirror.VoiceControlModule;

namespace SmartMirror.WeatherModule.Models
{
    public class WeatherVoiceController : IVoiceController
    {
        public bool IsVoiceControlLoaded { get; set; }
        public bool IsVoiceControlEnabled { get; set; }
        public bool HasCommands { get; private set; }
        public string VoiceControlKey { get; }
        public string GrammarFilePath { get; }
        public SpeechRecognitionGrammarFileConstraint Grammar { get; set; }
        private readonly WeatherModel weatherModel;
        private Queue<SpeechRecognitionResult> CommandsReceived; 

        public WeatherVoiceController(string grammarFilePath, WeatherModel model)
        {
            IsVoiceControlLoaded = false;
            IsVoiceControlEnabled = false;
            VoiceControlKey = "weather";
            GrammarFilePath = grammarFilePath;
            weatherModel = model;
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
                string timeFrame = tags.ContainsKey(WeatherCommands.TAG_TIME) ? tags[WeatherCommands.TAG_TIME][0] : "";
                weatherModel.HandleVoiceCommand(timeFrame);
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