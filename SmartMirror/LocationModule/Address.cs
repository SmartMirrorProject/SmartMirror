namespace SmartMirror.LocationModule
{
    class Address : Location
    {
        public string StreetAddress { get; set; }

        public Address(string city, string state, string country, string zip) : base(city, state, country, zip)
        {
        }

        /// <summary>
        /// The address string passed in must be in the format as shown to be parsed correctly.
        /// Street Address, City, State Zip
        /// </summary>
        /// <param name="address"></param>
        public Address(string address)
        {
            string[] data = address.Split(',');
            StreetAddress = data[0].Trim();
            City = data[1].Trim();
            string[] stateZip = data[2].Trim().Split(' ');
            State = stateZip[0].Trim();
            ZipCode = stateZip[1].Trim();
            Country = "US";
        }

        public string FullAddress => StreetAddress + ", " + City + ", " + State + " " + ZipCode;
    }
}