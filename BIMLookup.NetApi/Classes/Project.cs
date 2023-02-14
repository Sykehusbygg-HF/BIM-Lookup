using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMLookup.NetApi.Classes
{

    public class ProjectResponse
    {
        public string odatacontext { get; set; }
        public List<Project> value { get; set; }
        public static List<Project> FromJson(string json)
        {
            if (json == null)
                return null;
            try
            {
                return JsonConvert.DeserializeObject<ProjectResponse>(json)?.value;
            }
            catch
            {
                return null;
            }
        }
    }

    public class Project
    {
        public Guid Oid { get; set; }
        public int MariaDB_ID_Property { get; set; }
        public string Name { get; set; }
        public bool Existing { get; set; }
        public string Code { get; set; }
        public string HealthCompany { get; set; }
        public string[] PropertyIds { get; set; }
        public static Project Deserialize(string json)
        {
            if (json == null)
                return null;
            return JsonConvert.DeserializeObject<Project>(json);
        }
        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
        public static List<Project> FromJson(string json)
        {
            if (json == null)
                return null;
            try
            {
                return JsonConvert.DeserializeObject<List<Project>>(json);
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
