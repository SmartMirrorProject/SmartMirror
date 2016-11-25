using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;

namespace SmartMirror.TravelTimeModule
{
    class TravelTimeService
    {
        public TravelTimeData FetchTravelTime(string origin, string destination)
        {
            TravelTimeJSON data = GetTravelTimeData(origin, destination);
            TravelTimeData travelTime = new TravelTimeData(DateTime.Now, origin, destination)
            {
                TravelDistance = data.rows[0].elements[0].distance.text,
                TravelTime = data.rows[0].elements[0].duration.text
            };
            return travelTime;
        }

        private TravelTimeJSON GetTravelTimeData(string origin, string dest)
        {
            try
            {
                //TODO implement code to get current location based on IP or data stored in Azure or user entered. Hardcoded to Orlando for now
                //Request the JSON file from OpenWeatherMaps
                origin = origin.Replace(' ', '+');
                dest = dest.Replace(' ', '+');
                string originAndDest = origin + "&destinations=" + dest +
                                       "&departure_time=now&key=AIzaSyAreVG1Otoxg7Ke0rzAzW_ijkQFJFahCyw";
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get,
                    ($"https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins=" + originAndDest));
                HttpClient client = new HttpClient();
                var response = client.SendAsync(request).Result;
                //Parse the object from the JSON and return it
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    var bytes = Encoding.Unicode.GetBytes(result);
                    using (MemoryStream stream = new MemoryStream(bytes))
                    {
                        var serializer = new DataContractJsonSerializer(typeof(TravelTimeJSON));
                        var data = (TravelTimeJSON)serializer.ReadObject(stream);
                        return data;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }
    }

    public class TravelTimeJSON
    {
        public string destination_address { get; set; }
        public string origin_address { get; set; }
        public List<TravelTimeRows> rows { get; set; }
    }

    public class TravelTimeRows
    {
        public List<TravelTimeElement> elements { get; set; }
    }

    public class TravelTimeElement
    {
        public TravelTimeTextValueData distance { get; set; }
        public TravelTimeTextValueData duration { get; set; }
        public TravelTimeTextValueData duration_in_traffic { get; set; }
        public string status { get; set; }
    }

    public class TravelTimeTextValueData
    {
        public string text { get; set; }
        public long value { get; set; }
    }
}
