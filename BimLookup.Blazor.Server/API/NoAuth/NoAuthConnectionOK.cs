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
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
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
    public class NoAuthConnectionOK : ControllerBase
    {
        INonSecuredObjectSpaceFactory objectSpaceFactory;
        public NoAuthConnectionOK(INonSecuredObjectSpaceFactory objectSpaceFactory)
        {
            this.objectSpaceFactory = objectSpaceFactory;
        }
        [HttpGet()]
        [EnableQuery]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Get()
        {
            return Ok("true");
        }
    }
}

