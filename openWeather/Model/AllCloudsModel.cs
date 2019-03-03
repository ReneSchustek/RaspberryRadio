using Newtonsoft.Json;

namespace OpenWeather.Model
{
    public partial class AllCloudsModel
    {
        [JsonProperty("all")]
        public long All { get; set; }
    }
}
