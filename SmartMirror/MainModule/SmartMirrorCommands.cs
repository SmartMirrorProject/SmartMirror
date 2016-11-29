using System.Collections.Generic;
using SmartMirror.VoiceControlModule;

namespace SmartMirror.MainModule
{
    class SmartMirrorCommands
    {
        public const string TAG_CMD = "cmd";

        public const string TAG_MODULE = "module";

        //There is no target for the main module
        public const string TARGET = "";

        public const string SENSORS = "SENSORS";

        //If the user asks to show settings it will be handled in 
        //the main commands list because the settings commands
        //will only be active when the settings window is shown.
        public const string SETTINGS = "SETTINGS";

        public const string TRAVEL = "TRAVEL";

        public const string CMD_ON = "ON";

        public const string CMD_OFF = "OFF";

        public const string CMD_SHOW = "SHOW";

        public const string CMD_HIDE = "HIDE";

        public const string CMD_FOCUS = "FOCUS";
    }
}
