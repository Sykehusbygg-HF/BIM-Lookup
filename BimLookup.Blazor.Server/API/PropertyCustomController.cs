using BimLookup.Blazor.Server.API.Classes;
using BimLookup.Module.BusinessObjects;
using BimLookup.Module.Classes;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Core;
using DevExpress.Xpo;
using DevExpress.XtraRichEdit.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace BimLookup.Blazor.Server.API
{
    //TODO: Till Clabbe en call för att hämta alla properties Med data: Pset, Description, Name, Comment, (ev projectName, Phases, Discipline)
    //Query på: Project, Phase och Discipline

    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class PropertyCustomController : ControllerBase
    {
        INonSecuredObjectSpaceFactory objectSpaceFactory;
        public PropertyCustomController(INonSecuredObjectSpaceFactory objectSpaceFactory)
        {
            this.objectSpaceFactory = objectSpaceFactory;
        }
        [HttpGet()]
        [EnableQuery]
        [ProducesResponseType(typeof(List<ViewModelProperty>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Authorize]
        //public IEnumerable<string> Get()
        public async Task<IActionResult> Get()
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Restart();
            using IObjectSpace newObjectSpace = objectSpaceFactory.CreateNonSecuredObjectSpace<Property>();
            List<Property> response = await newObjectSpace.GetObjectsQuery<Property>().OrderBy(x => x.Oid).ToListAsync();
            sw.Stop();
            Debug.Print(sw.ElapsedMilliseconds.ToString());
            sw.Restart();
            List<ViewModelProperty> apiinstlist = new List<ViewModelProperty>();

            apiinstlist = response
                           .SelectMany(pi => pi.PropertySets.Select(pset => new ViewModelProperty()
                           {
                               Comment = pi.Comment,
                               Name = pi.Name,
                               Description = pi.Description,
                               Disciplines = pi.GetDisciplines()?.Select(x => x.Name).ToList(),
                               Skisseprosjekt = pi.Skisseprosjekt,
                               Forprosjekt = pi.Forprosjekt,
                               Detaljprosjekt = pi.Detaljprosjekt,
                               Arbeidstegning = pi.Arbeidstegning,
                               Overlevering = pi.Overlevering,
                               ProjectName = pi.Projects.Select(pi => pi.Name).ToList(),
                               //Phases = pi.Phases?.Select(x => x.Name).ToList(),
                               PropertySet = pset.Name
                               
                           })).ToList();

            //foreach (Property pi in response)
            //{
            //    foreach (PropertySet pset in pi.PropertySets)
            //    {
            //        ViewModelProperty mprop = new ViewModelProperty()
            //        {
            //            Comment = pi.Comment,
            //            Name = pi.Name,
            //            Description = pi.Description,
            //            Disciplines = pi.GetDisciplines()?.Select(x => x.Name).ToList(),
            //            Phases = pi.Phases?.Select(x => x.Name).ToList()
            //        };
            //        apiinstlist.Add(mprop);
            //    }
            //}
            sw.Stop();
            Debug.Print(sw.ElapsedMilliseconds.ToString());
            //string res = JsonConvert.SerializeObject(apiinstlist, Formatting.Indented);
            return Ok(apiinstlist);
        }
    }
}
