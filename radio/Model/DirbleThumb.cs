using Newtonsoft.Json;
using System;

namespace radio.Model
{
    public class DirbleThumb
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }
    }
}
