using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Exercise1.Commands.FetchData
{
    public class PublicApisResponse
    {
        public PublicApisResponse()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; init; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("entries")]
        public List<Entry> Entries { get; set; }
    }

    public class Entry
    {
        public string API { get; set; }
        public string Description { get; set; }
        public string Auth { get; set; }
        public bool HTTPS { get; set; }
        public string Cors { get; set; }
        public string Link { get; set; }
        public string Category { get; set; }
    }
}

