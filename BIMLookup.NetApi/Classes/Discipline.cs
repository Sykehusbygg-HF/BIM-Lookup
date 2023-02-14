using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMLookup.NetApi.Classes
{

    public class DisciplineResponse
    {
        public string odatacontext { get; set; }
        public List<Discipline> value { get; set; }
        public static List<Discipline> FromJson(string json)
        {
            if (json == null)
                return null;
            try
            {
                return JsonConvert.DeserializeObject<DisciplineResponse>(json)?.value;
            }
            catch
            {
                return null;
            }
        }
    }
    public class Discipline
    {
        public Guid Oid { get; set; }
        public int MariaDB_ID_Property { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string[] RevitCategoryIds { get; set; }
        public static Discipline Deserialize(string json)
        {
            if (json == null)
                return null;
            return JsonConvert.DeserializeObject<Discipline>(json);
        }
        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
        public static List<Discipline> FromJson(string json)
        {
            if (json == null)
                return null;
            try
            {
                return JsonConvert.DeserializeObject<List<Discipline>>(json);
            }
            catch
            {
                return null;
            }
        }
        public override string ToString()
        {
            return $"{Name}; {Code}";
        }
    }
}
