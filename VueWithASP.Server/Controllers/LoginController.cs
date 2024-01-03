using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text.Json.Nodes;

namespace VueWithASP.Server.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class LoginController : ControllerBase
  {
    private readonly ILogger<LoginController> _logger;


    public LoginController(ILogger<LoginController> logger)
    {
      _logger = logger;
    }

    [HttpGet(Name = "GetLogin")]
    public string Get()
    {
      return "";
    }

    [HttpPost(Name = "PostLogin")]
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    public async Task<string> Post()
    {
      string sReturn = "";
      string sDataFromDatabase = "";
      try
      {

        //add  using System.Data.SqlClient;
        //     using System.Data;
        //Along with the using statements, you need the system assembly reference.
        //To add assembly you can do the following.
        //   install nuget package. Right Click on Project > Manage Nuget Packages > 
        //   Search & install 'System.Data.SqlClient' and make sure it is compatible with the type of project (Core/Standard);

        //string commandText = @"SELECT * FROM [dbo].[APP_OBJECTS] ";

        //using (var connection = new SqlConnection(connStr))
        //{
        //  await connection.OpenAsync();   //vs  connection.Open();
        //  using (var tran = connection.BeginTransaction())
        //  {
        //    using (var command = new SqlCommand(commandText, connection, tran))
        //    {
        //      try
        //      {
        //        //command.Parameters.Add("@SUBMITTEREMAIL", SqlDbType.NVarChar);
        //        //command.Parameters["@SUBMITTEREMAIL"].Value = "me@someDomain.org";

        //        SqlDataReader rdr = await command.ExecuteReaderAsync();  // vs also alternatives, command.ExecuteReader();  or await command.ExecuteNonQueryAsync();

        //        while (rdr.Read())
        //        {
        //          sDataFromDatabase += rdr["Object_Name"].ToString();
        //        }
        //        await rdr.CloseAsync();
        //      }
        //      catch (Exception Ex)
        //      {
        //        await connection.CloseAsync();
        //        string msg = Ex.Message.ToString();
        //        tran.Rollback();
        //        throw;
        //      }
        //    }
        //  }
        //}

        IFormCollection oFormCollection = Request.Form;

        string? sJSON = "";
        string? sUserName = "";
        string? sPassword = "";
        if (!string.IsNullOrEmpty(oFormCollection["json"]))
        {
          sJSON = oFormCollection["json"];
        }

        // Newtonsoft.Json.Linq
        var oData = (JObject?)JsonConvert.DeserializeObject(sJSON);
        sUserName = oData["userName"].Value<string>();
        sPassword = oData["password"].Value<string>();

        Response.Headers.Add("Content-Type", "application/json");

        if (sUserName.ToLower() == "test" && sPassword.ToLower() == "test")
        {
          sReturn = "Successful login";
        }
        else
        {
          sReturn = "Invalid user name or password";
        }
      }
      catch (Exception oException)
      {
        VueWithASP.Server.MyCode.Core.ErrorHandling.cErrorHandling.LogError(oException);
      }
      return sReturn;
    } // END -- public async Task<string> Post()  }
  }
  }
