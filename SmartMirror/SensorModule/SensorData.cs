using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMirror.SensorModule
{
    public class SensorData
    {
        public int Temperature { get; set; }
        public int Humidity { get; set; }
        public int LightSensor { get; set; }
        public int LongRangeMotion { get; set; }
        public int GestureMotion { get; set; }
    }
}
