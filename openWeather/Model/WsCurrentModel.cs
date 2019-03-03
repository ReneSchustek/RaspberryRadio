using Newtonsoft.Json;

namespace OpenWeather.Model
{
    public class WsCurrentModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("temp")]
        public double Temp { get; set; }

        [JsonProperty("pressure")]
        public long Pressure { get; set; }

        [JsonProperty("humidity")]
        public long Humidity { get; set; }

        [JsonProperty("speed")]
        public double Speed { get; set; }

        [JsonProperty("deg")]
        public string Deg { get; set; }

        [JsonProperty("sunrise")]
        public string Sunrise { get; set; }

        [JsonProperty("sunset")]
        public string Sunset { get; set; }

        [JsonProperty("uvindex")]
        public double UvIndex { get; set; }

        [JsonProperty("cloudinessdescr")]
        public string CloudinessDescr { get; set; }

        [JsonProperty("cloudinessperc")]
        public long CloudinessPerc { get; set; }

    }
}
