using Newtonsoft.Json;
using System;

namespace radio.Model
{
    public class DirbleRadioInCountryModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("image")]
        public DirbleImageModel Image { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }

        [JsonProperty("twitter")]
        public string Twitter { get; set; }

        [JsonProperty("facebook")]
        public string Facebook { get; set; }

        [JsonProperty("total_listeners")]
        public long TotalListeners { get; set; }

        [JsonProperty("categories")]
        public DirbleCategoryModel[] Categories { get; set; }

        [JsonProperty("streams")]
        public DirbleStreamModel[] Streams { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }
    }
}

