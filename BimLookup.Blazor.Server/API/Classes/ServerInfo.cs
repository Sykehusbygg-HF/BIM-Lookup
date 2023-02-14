using System;
using System.Collections.Generic;

namespace BimLookup.Blazor.Server.API.Classes
{
    public class ServerInfo
    {
        public int StatusAsInteger { get; set; }
        public string Status { get; set; }
        public DateTime StatusTime { get; set; }
    }
}
