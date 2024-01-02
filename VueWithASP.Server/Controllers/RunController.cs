using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VueWithASP.Server.MyCode.Core.Utils;
using Microsoft.Data.SqlClient;
using System.Data;
using VueWithASP.Server.MyCode.Core.Settings;

namespace VueWithASP.Server.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class RunController : ControllerBase
  {
    private readonly ILogger<RunController> _logger;


    public RunController(ILogger<RunController> logger)
    {
      _logger = logger;
    }

    [HttpGet(Name = "GetRun")]
    public string Get()
    {
      return "";
    }

    [HttpPost(Name = "PostRun")]
    public async Task<string> Post()
    {
      string sReturn = "";

      try
      {
        IFormCollection oFormCollection = Request.Form;

        string? sJSON = "";
        string? sAction = "";
        string? sPassword = "";
        
        if (!string.IsNullOrEmpty(oFormCollection["json"]))
        {
          sJSON = oFormCollection["json"];
        }

        // Newtonsoft.Json.Linq
        var oData = (JObject?)JsonConvert.DeserializeObject(sJSON);
        sAction = oData["action"].Value<string>();

        switch (sAction) {
          case "GetUsers":
            sReturn = await VueWithASP.Server.MyCode.MySystem.cUsers.GetUsers(oData);
            break;
        }

        Response.Headers.Add("Content-Type", "application/json");
      }
      catch (Exception oException)
      {
        VueWithASP.Server.MyCode.Core.ErrorHandling.cErrorHandling.LogError(oException);
        sReturn = @"{
            'message': 'Programm error', 
            'success': false,
            'sessionId': ''\n
            }";
      }
      return sReturn;
    } // END -- public async Task<string> Post()  }
  }
  }
