using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMirror.VoiceControlModule
{
    interface IVoiceControllableModule
    {
        IVoiceController VoiceController { get; set; }
    }
}
