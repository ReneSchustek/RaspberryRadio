using Newtonsoft.Json;

namespace OpenWeather.Model
{
    public partial class ApiForecastWeatherModel
    {
        [JsonProperty("cod")]
        public long Cod { get; set; }

        [JsonProperty("message")]
        public double Message { get; set; }

        [JsonProperty("cnt")]
        public long Cnt { get; set; }

        [JsonProperty("list")]
        public ForecastListModel[] List { get; set; }

        [JsonProperty("city")]
        public ForecastCityModel City { get; set; }
    }
}
