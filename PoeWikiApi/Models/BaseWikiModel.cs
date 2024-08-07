﻿using System.Text.Json.Serialization;

namespace PoeWikiApi.Models
{
    public abstract class BaseWikiModel
    {
        [JsonPropertyName("_ID")]
        public uint Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("_pageName")]
        public string PageName { get; set; } = string.Empty;
    }
}
