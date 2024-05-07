using System.Text.Json.Serialization;

namespace Poedle.PoeWiki.Models
{
    public abstract class BasePoeWikiModel
    {
        [JsonPropertyName("_ID")]
        public uint Id { get; set; }
        public string Name { get; set; } = "";
        [JsonPropertyName("_pageName")]
        public string PageName { get; set; } = "";
    }
}
