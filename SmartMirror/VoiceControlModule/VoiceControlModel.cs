using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Media.SpeechRecognition;
using Windows.Storage;

namespace SmartMirror.VoiceControlModule
{
    class VoiceControlModel
    {
        private static readonly VoiceControlModel _instance = new VoiceControlModel();
        //Constructor is private because this is class is a singleton.
        private VoiceControlModel() { }
        public static VoiceControlModel Instance => _instance;

        private const string TAG_MODULE = "module";
        private const string TAG_TARGET = "target";
        private const string TAG_CMD = "cmd";
        private const string TAG_TIME_FRAME = "timeFrame";

        private static Dictionary<string, IVoiceController> activeModules = new Dictionary<string, IVoiceController>();

        private SpeechRecognizer _recognizer;

        /// <summary>
        /// When the program is started, initialize the SpeechRecognizer
        /// </summary>
        public void InitializeSpeechRecognizer()
        {
            // Initialize recognizer and set it's event handlers
            _recognizer = new SpeechRecognizer();
            _recognizer.StateChanged += RecognizerStateChanged;
            _recognizer.ContinuousRecognitionSession.ResultGenerated += RecognizerResultGenerated;
        }

        public async void LoadModulesAndStartProcessor(List<IVoiceController> modules)
        {
            foreach (IVoiceController module in modules)
            {
                await LoadVoiceControlModule(module);
            }
            StartSpeechRecognizer();
        }

        public async Task<bool> LoadVoiceControlModule(IVoiceController module)
        {
            //If the module is not loaded.
            if (!IsModuleLoaded(module))
            {
                //Create the grammar for the passed in module.
                SpeechRecognitionGrammarFileConstraint grammar = await CreateGrammarFromFile(module.GrammarFilePath,
                    module.VoiceControlKey);
                //Add the grammar file from the passed in module to the speech recognizer
                _recognizer.Constraints.Add(grammar);
                //Store the module into the activeModules Dictionary
                activeModules[module.VoiceControlKey] = module;
                //Set the voice control memeber variables of the module.
                module.IsVoiceControlLoaded = true;
                module.IsVoiceControlEnabled = true;
                module.Grammar = grammar;
                return true;
            }
            return false;
        }


        public bool IsModuleLoaded(IVoiceController module)
        {
            return activeModules.ContainsValue(module);
        }

        private async void StartSpeechRecognizer()
        {
            // Compile the loaded GrammarFiles
            SpeechRecognitionCompilationResult compilationResult = await _recognizer.CompileConstraintsAsync();

            // If successful, display the recognition result.
            if (compilationResult.Status == SpeechRecognitionResultStatus.Success)
            {
                Debug.WriteLine("Result: " + compilationResult.ToString());

                SpeechContinuousRecognitionSession session = _recognizer.ContinuousRecognitionSession;
                try
                {
                    await session.StartAsync();
                }
                catch (Exception e)
                {
                    //TODO this needs to report to the user that something failed.
                    //also potentially write to a log somewhere.               
                    Debug.WriteLine(e.Data);
                }

            }
            else
            {
                //TODO this needs to report to the user that something failed.
                //also potentially write to a log somewhere.
                Debug.WriteLine("Status: " + compilationResult.Status);
            }
        }

        //Handle appropriate voice commands
        private void RecognizerResultGenerated(SpeechContinuousRecognitionSession session,
                                                      SpeechContinuousRecognitionResultGeneratedEventArgs args)
        {
            // Output debug strings
//            Debug.WriteLine(args.Result.Status);
//            Debug.WriteLine(args.Result.Text);

            //This is how we will tell which module grammar the command came from
//            Debug.WriteLine("Grammar File Constraint Tag: " + args.Result.Constraint.Tag);
//
//            Debug.WriteLine(args.Result.Confidence.ToString());
//            Debug.WriteLine("Confidence 0 = High, 1 = Medium, 2 = Low");
//
//            int count = args.Result.SemanticInterpretation.Properties.Count;
//            Debug.WriteLine("Count: " + count);
//            Debug.WriteLine("Tag: " + args.Result.Constraint.Tag);

            //Receive the commands and pass it to the appropriate module to handle
            IVoiceController commandsModule = activeModules[args.Result.Constraint.Tag];
            commandsModule.EnqueueCommand(args.Result);
        }



        // Debug changes to state of the recognizer for test purposes
        private void RecognizerStateChanged(SpeechRecognizer sender, SpeechRecognizerStateChangedEventArgs args)
        {
            //Debug.WriteLine("Speech recognizer state: " + args.State.ToString());
        }

        public async Task<SpeechRecognitionGrammarFileConstraint> CreateGrammarFromFile(string file, string key)
        {
            StorageFile grammarContentFile = await Package.Current.InstalledLocation.GetFileAsync(String.Format(file));
            //Create grammar constraint
            return new SpeechRecognitionGrammarFileConstraint(grammarContentFile, key);
        }



        /// <summary>
        /// When the program is terminated, stop the SpeechRecognizer
        /// </summary>
        public async void UnloadSpeechRecognizer()
        {
            //Stop voice recognition
            await _recognizer.ContinuousRecognitionSession.StopAsync();
        }
    }
}