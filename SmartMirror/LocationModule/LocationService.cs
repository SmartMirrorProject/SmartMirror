namespace SmartMirror.LocationModule
{
    class LocationService
    {
        public Location DefaultLocation { get; }
        public Address HomeAddress { get; set; }
        public Address WorkAddress { get; set; }
        public Address SchoolAddress { get; set; }

        //TODO use something like IPInfo.io to set the default location based on actual location when the mirror starts
        //for now I am going to hard code it into the start up as Orlando, FL, United States, 32817
        public LocationService(string city, string state, string country, string zipcode)
        {
            DefaultLocation = new Location(city, state, country, zipcode);
        }

        //TODO find a way to pull all the location information from a city, some API or something
//        //for now we will just always use Orlando Florida
//        public static LocationModel GetLocationFromCity(string city)
//        {
//            LocationModel location = new LocationModel("Orlando", "Florida", "United States", "32817");
//            return location;
//        }

        //TODO implement this as a member field on the singleton class that can be accessed
        //and potentially mutated.
        public static Location GetDefaultLocation()
        {
            Location location = new Location("Orlando", "Florida", "United States", "32817");
            return location;
        }
    }
}
