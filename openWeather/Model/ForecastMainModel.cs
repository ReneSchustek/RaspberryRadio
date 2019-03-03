using Newtonsoft.Json;

namespace OpenWeather.Model
{
    public partial class ForecastMainModel : CurrentMainModel
    {
        [JsonProperty("sea_level")]
        public double SeaLevel { get; set; }

        [JsonProperty("grnd_level")]
        public double GrndLevel { get; set; }

        [JsonProperty("temp_kf")]
        public double TempKf { get; set; }
    }
}
