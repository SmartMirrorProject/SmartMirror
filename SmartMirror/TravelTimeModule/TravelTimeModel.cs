using SmartMirror.Settings;

namespace SmartMirror.TravelTimeModule
{
    public class TravelTimeModel
    {
        private TravelTimeViewModel viewModel;

        public TravelTimeModel(TravelTimeViewModel vm)
        {
            viewModel = vm;
        }

        public void UpdateTravelTime(string origin, string dest)
        {
            TravelTimeData data = TravelTimeService.FetchTravelTime(origin, dest);
            if (null == data)
            {
                return;
            }
            viewModel.Distance = data.TravelDistance;
            viewModel.Time = data.TravelTime;
        }
    }
}
