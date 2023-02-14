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
    public class NoAuthDisciplineController : ControllerBase
    {
        INonSecuredObjectSpaceFactory objectSpaceFactory;
        public NoAuthDisciplineController(INonSecuredObjectSpaceFactory objectSpaceFactory)
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
            using IObjectSpace newObjectSpace = objectSpaceFactory.CreateNonSecuredObjectSpace<Discipline>();
            List<Discipline> response = new List<Discipline>();
            response = await newObjectSpace.GetObjectsQuery<Discipline>().ToListAsync();
            List<BIMLookup.NetApi.Classes.Discipline> items = new List<BIMLookup.NetApi.Classes.Discipline>();
            foreach (Discipline obj in response)
            {
                items.Add(obj.Cast<BIMLookup.NetApi.Classes.Discipline>());
            }
            return Ok(items);
        }
    }
}

