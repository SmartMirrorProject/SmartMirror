using System;

namespace SmartMirror.TravelTimeModule
{
    class TravelTimeData
    {
        public string TravelTime { get; set; }
        public string TravelDistance { get; set; }
        public DateTime ReadTime { get; set; }

        public TravelTimeData(DateTime readTime)
        {
            ReadTime = readTime;
        }
    }
}
