using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace radio.Model
{
    public class DirbleRadioSearchModel
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

        [JsonProperty("created_at")]
        public string Created_At { get; set; }

        [JsonProperty("updated_at")]
        public string Updated_At { get; set; }

        [JsonProperty("streams")]
        public DirbleStreamModel[] Streams { get; set; }

        [JsonProperty("categories")]
        public DirbleCategoryModel[] Categories { get; set; }

        
    }
}
