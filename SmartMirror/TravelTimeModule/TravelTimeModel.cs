namespace SmartMirror.TravelTimeModule
{
    public class TravelTimeModel
    {
        public TravelTimeModel()
        {
            
        }

        public void UpdateTravelTime(string origin, string dest)
        {
            TravelTimeData data = TravelTimeService.FetchTravelTime(origin, dest);
        }
    }
}
