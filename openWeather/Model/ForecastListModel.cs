using Newtonsoft.Json;
using System;

namespace OpenWeather.Model
{
    public partial class ForecastListModel
    {
        [JsonProperty("dt")]
        public long Dt { get; set; }

        [JsonProperty("main")]
        public ForecastMainModel Main { get; set; }

        [JsonProperty("weather")]
        public AllWeatherModel[] Weather { get; set; }

        [JsonProperty("clouds")]
        public AllCloudsModel Clouds { get; set; }

        [JsonProperty("wind")]
        public AllWindModel Wind { get; set; }

        [JsonProperty("sys")]
        public ForecastSysModel Sys { get; set; }

        [JsonProperty("dt_txt")]
        public DateTimeOffset DtTxt { get; set; }

        [JsonProperty("rain", NullValueHandling = NullValueHandling.Ignore)]
        public ForecastRainModel Rain { get; set; }
    }
}
