using Newtonsoft.Json;

namespace radio.Model
{
    public class DirbleRadioSpecificModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("total_listeners")]
        public long TotalListeners { get; set; }

        [JsonProperty("image")]
        public DirbleImageModel Image { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }

        [JsonProperty("streams")]
        public DirbleStreamModel[] Streams { get; set; }

        [JsonProperty("categories")]
        public DirbleCategoryModel[] Categories { get; set; }

    }
}
