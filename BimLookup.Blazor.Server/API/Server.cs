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
    public class ServerController : ControllerBase
    {
        INonSecuredObjectSpaceFactory objectSpaceFactory;
        public ServerController(INonSecuredObjectSpaceFactory objectSpaceFactory)
        {
            this.objectSpaceFactory = objectSpaceFactory;
        }
        [HttpGet()]
        [EnableQuery]
        [ProducesResponseType(typeof(ServerInfo), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Authorize]
        //public IEnumerable<string> Get()
        public async Task<IActionResult> Get()
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Restart();
            using IObjectSpace newObjectSpace = objectSpaceFactory.CreateNonSecuredObjectSpace<AppSettings>();
            List<AppSettings> response = await newObjectSpace.GetObjectsQuery<AppSettings>().ToListAsync();
            AppSettings settings = response.FirstOrDefault();
            if (settings == null)
                return null;
            sw.Stop();
            Debug.Print(sw.ElapsedMilliseconds.ToString());
            sw.Restart();
            ServerInfo si = new ServerInfo { StatusAsInteger = (int)settings.ServerStatus, StatusTime = settings.ServerStatusDateTime, Status = settings.ServerStatusText };
            sw.Stop();
            Debug.Print(sw.ElapsedMilliseconds.ToString());
            //string res = JsonConvert.SerializeObject(apiinstlist, Formatting.Indented);
            return Ok(si);
        }
    }
}
