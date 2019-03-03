using Newtonsoft.Json;
using System;

namespace OpenWeather.Model
{
    public partial class ApiCurrentUvIndexModel
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lon")]
        public double Lon { get; set; }

        [JsonProperty("date_iso")]
        public DateTimeOffset DateIso { get; set; }

        [JsonProperty("date")]
        public long Date { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }
    }
}
