using System.Collections.Generic;

namespace BimLookup.Blazor.Server.API.Classes
{
    public class ViewModelProperty
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public string PropertySet { get; set; }
        public List<string> ProjectName { get; set; }
        //public List<string> Phases { get; set; }= new List<string>();
        public bool Skisseprosjekt { get; set; }
        public bool Forprosjekt { get; set; }
        public bool Detaljprosjekt { get; set; }
        public bool Arbeidstegning { get; set; }
        public bool Overlevering { get; set; }

        public List<string> Disciplines { get; set; } = new List<string>();
    }
}
