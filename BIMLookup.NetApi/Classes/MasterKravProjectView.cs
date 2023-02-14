using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMLookup.NetApi.Classes
{
   
        public class MasterkravProjectView 
        {
            
            public long TempKey { get; set; }
            public Guid ID_PropertyInstance { get; set; }

            public Guid ID_Property { get; set; }

            public Guid ID_RevitCategory { get; set; }

            public Guid ID_PSet { get; set; }

            public string ProjectCode { get; set; }

            public string DicsiplineCode { get; set; }

            public string PSetName { get; set; }

            public string PropertyName { get; set; }

            public int TypeInstance { get; set; }

            public string IfcPropertyType { get; set; }

            public string RevitPropertyType { get; set; }

            public string PropertyGroup { get; set; }

            public string RevitElement { get; set; }

            public string PropertyGuid { get; set; }

            public bool Skisseprosjekt { get; set; }

            public bool Forprosjekt { get; set; }

            public bool Detaljprosjekt { get; set; }

            public bool Arbeidstegning { get; set; }

            public bool Overlevering { get; set; }

        public string TypeIntsnceAsString
        {
            get
            {
                if (TypeInstance == 0)
                    return "Type";
                return "Instance";

            }
            set {; }
        }

        public static List<MasterkravProjectView> FromJson(string json)
        {
            if (json == null)
                return null;
            return JsonConvert.DeserializeObject<List<MasterkravProjectView>>(json);
        }
    }
    }

