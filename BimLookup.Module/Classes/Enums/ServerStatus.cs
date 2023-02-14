using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BimLookup.Module.Classes.Enums
{
    public enum ServerStatus
    {
        [ImageName("Actions_Check")]
        OK,
        [ImageName("Actions_Options")]
        Maintenance,
        [ImageName("Actions_Forbid")]
        Down,
        [ImageName("Weather_Umbrella")]
        Unknown
    }
}
