using Newtonsoft.Json;

namespace OpenWeather.Model
{
    public partial class ApiCurrentWeatherModel
    {
        [JsonProperty("coord")]
        public CurrentCoordModel Coord { get; set; }

        [JsonProperty("weather")]
        public AllWeatherModel[] Weather { get; set; }

        [JsonProperty("base")]
        public string Base { get; set; }

        [JsonProperty("main")]
        public CurrentMainModel Main { get; set; }

        [JsonProperty("visibility")]
        public long Visibility { get; set; }

        [JsonProperty("wind")]
        public AllWindModel Wind { get; set; }

        [JsonProperty("clouds")]
        public AllCloudsModel Clouds { get; set; }

        [JsonProperty("dt")]
        public long Dt { get; set; }

        [JsonProperty("sys")]
        public CurrentSysModel Sys { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("cod")]
        public long Cod { get; set; }
    }
}
