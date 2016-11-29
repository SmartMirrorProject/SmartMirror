namespace SmartMirror.LocationModule
{
    public class Location
    {
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set;  }

        public Location()
        {
            
        }

        public Location(string city, string state, string country, string zip)
        {
            City = city;
            State = state;
            Country = country;
            ZipCode = zip;
        }
    }
}