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
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace BimLookup.Blazor.Server.API
{
    //TODO: Till Clabbe en call för att hämta alla properties Med data: Pset, Description, Name, Comment, (ev projectName, Phases, Discipline)
    //Query på: Project, Phase och Discipline

    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class PropertyInstanceCustomController : ControllerBase
    {
        INonSecuredObjectSpaceFactory objectSpaceFactory;
        public PropertyInstanceCustomController(INonSecuredObjectSpaceFactory objectSpaceFactory)
        {
            this.objectSpaceFactory = objectSpaceFactory;
        }
        [HttpGet()]
        [EnableQuery]
        [ProducesResponseType(typeof(List<BIMLookup.NetApi.Classes.PropertyInstance>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Authorize]
        //public IEnumerable<string> Get()
        public async Task<IActionResult> Get()
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Restart();
            using IObjectSpace newObjectSpace = objectSpaceFactory.CreateNonSecuredObjectSpace<PropertyInstance>();
            List<PropertyInstance> response = await newObjectSpace.GetObjectsQuery<PropertyInstance>().ToListAsync();
            sw.Stop();
            Debug.Print(sw.ElapsedMilliseconds.ToString());
            sw.Restart();


            sw.Restart();
            List<BIMLookup.NetApi.Classes.PropertyInstance> apiinstlist = new List<BIMLookup.NetApi.Classes.PropertyInstance>();
            foreach (PropertyInstance pi in response)
            {
                apiinstlist.Add(pi.Cast<BIMLookup.NetApi.Classes.PropertyInstance>());
            }
            sw.Stop();
            //Debug.Print(sw.ElapsedMilliseconds.ToString());
            //sw.Restart();
            //List<BIMLookup.NetApi.Classes.PropertyInstance> apiinstlist2 = response.Select(pi => pi.Cast<BIMLookup.NetApi.Classes.PropertyInstance>()).ToList();
            //sw.Stop();
            //Debug.Print(sw.ElapsedMilliseconds.ToString());
            sw.Restart();


            string res = JsonConvert.SerializeObject(apiinstlist, Formatting.Indented);
            sw.Stop();
            Debug.Print(sw.ElapsedMilliseconds.ToString());
            return Ok(res);
        }
#if DEBUG
        [HttpGet("TestPerformanceOnDifferentCalls")]
        [EnableQuery]
        [ProducesResponseType(typeof(List<BIMLookup.NetApi.Classes.PropertyInstance>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(string ProjectCode)
        {
            List<PropertyInstance> response = new List<PropertyInstance>();
            using IObjectSpace newObjectSpacePi = objectSpaceFactory.CreateNonSecuredObjectSpace<PropertyInstance>();
            using IObjectSpace newObjectSpaceProject = objectSpaceFactory.CreateNonSecuredObjectSpace<Project>();
            response = await newObjectSpacePi.GetObjectsQuery<PropertyInstance>().Where(pi => pi.Property.RevitCategories.Any(c => pi.Project.Code == "HSSP")).OrderBy(x => x.Oid).ToListAsync();
            response = await newObjectSpaceProject.GetObjectsQuery<Project>().Where(pi => pi.Code == "HSSP").SelectMany(x => x.Properties).ToListAsync();
            response = await newObjectSpaceProject.GetObjectsQuery<Project>().Where(pi => pi.Code == "HSSP").SelectMany(x => x.Properties).OrderBy(x => x.Oid).ToListAsync();
            return Ok(response);
        }

#endif
        [HttpGet("ByCodes")]
        [EnableQuery]
        [ProducesResponseType(typeof(List<BIMLookup.NetApi.Classes.PropertyInstance>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(string ProjectCode, string Phase, string DisciplineCode)
        {
            //From BimKrav
            //    public enum Phase
            //{
            //    Skisseprosjekt = 100,
            //    Forprosjekt = 200,
            //    Detaljprosjekt = 300,
            //    Arbeidstegning = 400,
            //    Overlevering = 500
            //}

            Stopwatch sw = Stopwatch.StartNew();
            sw.Restart();
            List<PropertyInstance> response = new List<PropertyInstance>();
            using IObjectSpace newObjectSpacePi = objectSpaceFactory.CreateNonSecuredObjectSpace<PropertyInstance>();
            //TODO: What if ni indata om Proj or disc
            switch (Phase.ToLower())
            {
                case "skisseprosjekt":
                case "100":
                    response = await newObjectSpacePi.GetObjectsQuery<PropertyInstance>().Where(pi => pi.Property.RevitCategories.Any(c => c.Disciplines.Any(d => d.Code == DisciplineCode || string.IsNullOrEmpty(DisciplineCode))) && (pi.Project.Code == ProjectCode || string.IsNullOrEmpty(ProjectCode)) && pi.Skisseprosjekt).OrderBy(x => x.Oid).ToListAsync();
                    break;
                case "forprosjekt":
                case "200":
                    response = await newObjectSpacePi.GetObjectsQuery<PropertyInstance>().Where(pi => pi.Property.RevitCategories.Any(c => c.Disciplines.Any(d => d.Code == DisciplineCode || string.IsNullOrEmpty(DisciplineCode))) && (pi.Project.Code == ProjectCode || string.IsNullOrEmpty(ProjectCode)) && pi.Forprosjekt).OrderBy(x => x.Oid).ToListAsync();
                    break;
                case "detaljprosjekt":
                case "300":
                    response = await newObjectSpacePi.GetObjectsQuery<PropertyInstance>().Where(pi => pi.Property.RevitCategories.Any(c => c.Disciplines.Any(d => d.Code == DisciplineCode || string.IsNullOrEmpty(DisciplineCode))) && (pi.Project.Code == ProjectCode || string.IsNullOrEmpty(ProjectCode)) && pi.Detaljprosjekt).OrderBy(x => x.Oid).ToListAsync();
                    break;
                case "arbeidstegning":
                case "400":
                    response = await newObjectSpacePi.GetObjectsQuery<PropertyInstance>().Where(pi => pi.Property.RevitCategories.Any(c => c.Disciplines.Any(d => d.Code == DisciplineCode || string.IsNullOrEmpty(DisciplineCode))) && (pi.Project.Code == ProjectCode || string.IsNullOrEmpty(ProjectCode)) && pi.Arbeidstegning).OrderBy(x => x.Oid).ToListAsync();
                    break;
                case "overlevering":
                case "500":
                    response = await newObjectSpacePi.GetObjectsQuery<PropertyInstance>().Where(pi => pi.Property.RevitCategories.Any(c => c.Disciplines.Any(d => d.Code == DisciplineCode || string.IsNullOrEmpty(DisciplineCode))) && (pi.Project.Code == ProjectCode || string.IsNullOrEmpty(ProjectCode)) && pi.Overlevering).OrderBy(x => x.Oid).ToListAsync();
                    break;
                default:
                    response = await newObjectSpacePi.GetObjectsQuery<PropertyInstance>().Where(pi => pi.Property.RevitCategories.Any(c => c.Disciplines.Any(d => d.Code == DisciplineCode || string.IsNullOrEmpty(DisciplineCode))) && (pi.Project.Code == ProjectCode || string.IsNullOrEmpty(ProjectCode))).OrderBy(x => x.Oid).ToListAsync();
                    break;
            }
            sw.Stop();
            Debug.Print(sw.ElapsedMilliseconds.ToString());
            List<BIMLookup.NetApi.Classes.PropertyInstance> apiinstlist = new List<BIMLookup.NetApi.Classes.PropertyInstance>();
            foreach (PropertyInstance pi in response)
            {
                BIMLookup.NetApi.Classes.PropertyInstance apipi = pi.Cast<BIMLookup.NetApi.Classes.PropertyInstance>();
                apipi.Categories = pi.Property.RevitCategories.Select(x => x.Name).ToList();
                apiinstlist.Add(apipi);
            }
            //string res = JsonConvert.SerializeObject(apiinstlist, Formatting.Indented);
            //string res = JsonConvert.SerializeObject(apiinstlist);
            return Ok(apiinstlist);
        }
    }
}

