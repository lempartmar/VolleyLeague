﻿namespace VolleyLeague.Shared.Dtos
{
    public class AdminDefinedCodeDto
    {
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
