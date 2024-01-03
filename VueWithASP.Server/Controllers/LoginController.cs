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
    public async Task<string> Post()
    {
      string sReturn = "";
      string sDbConnectionString = "";
      string sUserId = "";

      try
      {
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

        // Check user name and pass in DB
        sDbConnectionString = VueWithASP.Server.MyCode.Core.Settings.cSettings.getSetting(cSettings.eSetting.DbConnectionString);
        string commandText = 
            $"SELECT USER_ID  \n" 
          + $"FROM [dbo].[Users] \n"
          + $"WHERE Login_Name ='{sUserName}' \n" 
          + $"AND Password=HASHBYTES('SHA2_512', '{sPassword}'+CAST(Salt AS NVARCHAR(36))) \n"
          + ";"
          ;

        using (var connection = new SqlConnection(sDbConnectionString))
        {
          await connection.OpenAsync();   //vs  connection.Open();
          using (var tran = connection.BeginTransaction())
          {
            using (var command = new SqlCommand(commandText, connection, tran))
            {
              try
              {
                SqlDataReader oSqlDataReader = await command.ExecuteReaderAsync();  // vs also alternatives, command.ExecuteReader();  or await command.ExecuteNonQueryAsync();

                if (oSqlDataReader.Read())
                {
                  sUserId = oSqlDataReader["USER_ID"].ToString();
                }
                await oSqlDataReader.CloseAsync();
              }
              catch (Exception oException)
              {
                await connection.CloseAsync();
                tran.Rollback();
                VueWithASP.Server.MyCode.Core.ErrorHandling.cErrorHandling.LogError(oException);
                throw;
              }
            }
          }
        }
        // ** END -- Check user name and pass in DB

        if (sUserId.Trim().Length > 0)
        {
          string sSessionID = cUtils.RandomString(20);
          sReturn = @"{
            'message': 'Successful login. Please wait...', 
            'success': true,
            'sessionId': '" + sSessionID + "'\n" +
            "            }";
        }
        else
        {
          sReturn = @"{
            'message': 'Invalid user name or password',
            'success': false,
            'sessionId': ''
            }";
        }
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
