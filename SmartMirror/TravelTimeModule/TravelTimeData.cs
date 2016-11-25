using System;

namespace SmartMirror.TravelTimeModule
{
    class TravelTimeData
    {
        public string OriginAddress { get; set; }
        public string DestinationAddress { get; set; }
        public string TravelTime { get; set; }
        public string TravelDistance { get; set; }
        public DateTime ReadTime { get; set; }

        public TravelTimeData(DateTime readTime, string origin, string destination)
        {
            OriginAddress = origin;
            DestinationAddress = destination;
            ReadTime = readTime;
        }
    }
}
