namespace SmartMirror.LocationModule
{
    class Address : Location
    {
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }

        public Address(string city, string state, string country, string zip) : base(city, state, country, zip)
        {
        }

        public string FullAddress => StreetAddress1 + " " + StreetAddress2 + ", " + City + ", " + State + " " + ZipCode;
    }
}