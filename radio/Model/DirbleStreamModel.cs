using Newtonsoft.Json;
using System;

namespace radio.Model
{
    public class DirbleStreamModel
    {
        [JsonProperty("stream")]
        public Uri StreamStream { get; set; }

        [JsonProperty("bitrate")]
        public long? Bitrate { get; set; }

        [JsonProperty("content_type")]
        public string ContentType { get; set; }

        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("listeners")]
        public long Listeners { get; set; }
    }
}
