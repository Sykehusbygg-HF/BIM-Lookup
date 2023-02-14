using BimLookup.Blazor.Server.API.Classes;
using BimLookup.Module.BusinessObjects;
using BimLookup.Module.BusinessObjects.DBViews;
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
using System.Threading.Tasks;

namespace BimLookup.Blazor.Server.API.NoAuth
{
    //TODO: Till Clabbe en call för att hämta alla properties Med data: Pset, Description, Name, Comment, (ev projectName, Phases, Discipline)
    //Query på: Project, Phase och Discipline

    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class MasterkravProjectViewCustomController : ControllerBase
    {
        INonSecuredObjectSpaceFactory objectSpaceFactory;
        public MasterkravProjectViewCustomController(INonSecuredObjectSpaceFactory objectSpaceFactory)
        {
            this.objectSpaceFactory = objectSpaceFactory;
        }
        [HttpGet("ByCodes")]
        [EnableQuery]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(string ProjectCode, string Phase, string DisciplineCode)
        {
            using IObjectSpace newObjectSpace = objectSpaceFactory.CreateNonSecuredObjectSpace<Project>();

            Stopwatch sw = Stopwatch.StartNew();
            sw.Restart();
            List<MasterkravProjectView> response = new List<MasterkravProjectView>();

            //TODO: What if ni indata om Proj or disc
            switch (Phase.ToLower())
            {
                case "skisseprosjekt":
                case "100":
                    response = await newObjectSpace.GetObjectsQuery<MasterkravProjectView>().Where(pi => (pi.ProjectCode == ProjectCode || string.IsNullOrEmpty(ProjectCode)) && (pi.DicsiplineCode == DisciplineCode || string.IsNullOrEmpty(DisciplineCode)) && pi.Skisseprosjekt).OrderBy(x => x.PropertyName).ToListAsync();
                    break;
                case "forprosjekt":
                case "200":
                    response = response = await newObjectSpace.GetObjectsQuery<MasterkravProjectView>().Where(pi => (pi.ProjectCode == ProjectCode || string.IsNullOrEmpty(ProjectCode)) && (pi.DicsiplineCode == DisciplineCode || string.IsNullOrEmpty(DisciplineCode)) && pi.Forprosjekt).OrderBy(x => x.PropertyName).ToListAsync();
                    break;
                case "detaljprosjekt":
                case "300":
                    response = await newObjectSpace.GetObjectsQuery<MasterkravProjectView>().Where(pi => (pi.ProjectCode == ProjectCode || string.IsNullOrEmpty(ProjectCode)) && (pi.DicsiplineCode == DisciplineCode || string.IsNullOrEmpty(DisciplineCode)) && pi.Detaljprosjekt).OrderBy(x => x.PropertyName).ToListAsync();
                    break;
                case "arbeidstegning":
                case "400":
                    response = await newObjectSpace.GetObjectsQuery<MasterkravProjectView>().Where(pi => (pi.ProjectCode == ProjectCode || string.IsNullOrEmpty(ProjectCode)) && (pi.DicsiplineCode == DisciplineCode || string.IsNullOrEmpty(DisciplineCode)) && pi.Arbeidstegning).OrderBy(x => x.PropertyName).ToListAsync();
                    break;
                case "overlevering":
                case "500":
                    response = await newObjectSpace.GetObjectsQuery<MasterkravProjectView>().Where(pi => (pi.ProjectCode == ProjectCode || string.IsNullOrEmpty(ProjectCode)) && (pi.DicsiplineCode == DisciplineCode || string.IsNullOrEmpty(DisciplineCode)) && pi.Overlevering).OrderBy(x => x.PropertyName).ToListAsync();
                    break;
                default:
                    response = await newObjectSpace.GetObjectsQuery<MasterkravProjectView>().Where(pi => (pi.ProjectCode == ProjectCode || string.IsNullOrEmpty(ProjectCode)) && (pi.DicsiplineCode == DisciplineCode || string.IsNullOrEmpty(DisciplineCode))).OrderBy(x => x.PropertyName).ToListAsync();
                    break;
            }
            sw.Stop();
            List<BIMLookup.NetApi.Classes.MasterkravProjectView> items = new List<BIMLookup.NetApi.Classes.MasterkravProjectView>();
            foreach (MasterkravProjectView obj in response)
            {
                items.Add(obj.Cast<BIMLookup.NetApi.Classes.MasterkravProjectView>());
            }
            //string res = JsonConvert.SerializeObject(apiinstlist, Formatting.Indented);
            //string res = JsonConvert.SerializeObject(apiinstlist);
            return Ok(items);
        }
    }
}

