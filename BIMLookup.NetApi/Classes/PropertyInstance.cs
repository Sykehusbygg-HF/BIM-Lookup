using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMLookup.NetApi.Classes
{

    public class PropertyInstanceResponse
    {
        public string odatacontext { get; set; }
        public List<PropertyInstance> value { get; set; }
        public static List<PropertyInstance> FromJson(string json)
        {
            return JsonConvert.DeserializeObject<PropertyInstanceResponse>(json)?.value;
        }
        public static List<PropertyInstance> FromJson2(string json)
        {
            if (json == null)
                return null;
            return JsonConvert.DeserializeObject<List<PropertyInstance>>(json);
        }
        public static List<PropertyInstance> FromJson3(string json)
        {
            if (json == null)
                return null;
            json = json.Substring(1,json.Length - 2);
            return JsonConvert.DeserializeObject<List<PropertyInstance>>(json);
        }

    }
    public class PropertyInstance
    {
        public Guid Oid { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }
        public string Type_InstanceName { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        
        public bool Skisseprosjekt { get; set; }
        public bool Forprosjekt { get; set; }
        public bool Detaljprosjekt { get; set; }
        public bool Arbeidstegning { get; set; }
        public bool Overlevering { get; set; }
        public string PropertyId { get; set; }
        public string OwnerId { get; set; }
        public string PropertyGroupName { get; set; }
        public string Guid { get; set; }
        public string RevitPropertyTypeName { get; set; }
        public string IfcPropertyType { get; set; }
        public List<string> Categories { get; set; }

        public string PropertySetDisplayValue { get; set; }
        public static PropertyInstance Deserialize(string json)
        {
            if (json == null)
                return null;
            return JsonConvert.DeserializeObject<PropertyInstance>(json);
        }
        //public string Serialize()
        //{
        //    return JsonConvert.SerializeObject(this);
        //}
        public override string ToString()
        {
            return $"{Name}";
        }

    }


}
