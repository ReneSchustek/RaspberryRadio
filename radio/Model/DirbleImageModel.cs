using Newtonsoft.Json;
using System;

namespace radio.Model
{
    public class DirbleImageModel
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("thumb")]
        public DirbleThumb Thumb { get; set; }
    }
}
