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
    public class NoAuthProjectController : ControllerBase
    {
        INonSecuredObjectSpaceFactory objectSpaceFactory;
        public NoAuthProjectController(INonSecuredObjectSpaceFactory objectSpaceFactory)
        {
            this.objectSpaceFactory = objectSpaceFactory;
        }
        [HttpGet()]
        [EnableQuery]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            using IObjectSpace newObjectSpace = objectSpaceFactory.CreateNonSecuredObjectSpace<Project>();
                List<Project> response = new List<Project>();
                response = await newObjectSpace.GetObjectsQuery<Project>().ToListAsync();
                List<BIMLookup.NetApi.Classes.Project> items = new List<BIMLookup.NetApi.Classes.Project>();
                foreach (Project obj in response)
                {
                    items.Add(obj.Cast<BIMLookup.NetApi.Classes.Project>());
                }
                return Ok(items);
            
        }
    }
}

