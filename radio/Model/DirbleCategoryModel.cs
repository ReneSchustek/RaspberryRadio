using Newtonsoft.Json;

namespace radio.Model
{
    public class DirbleCategoryModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("ancestry")]
        public long? Ancestry { get; set; }
    }
}
