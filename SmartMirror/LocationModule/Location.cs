namespace SmartMirror.LocationModule
{
    public class Location
    {
        public string City { get; }
        public string State { get; }
        public string Country { get; }
        public string ZipCode { get; }

        public Location(string city, string state, string country, string zip)
        {
            City = city;
            State = state;
            Country = country;
            ZipCode = zip;
        }
    }
}