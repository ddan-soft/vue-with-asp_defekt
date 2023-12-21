using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using VueWithASP.Server.MyCode.Core.Settings;

namespace VueWithASP.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

    public IConfiguration Configuration { get; }
    public string? connStr = String.Empty;


    public WeatherForecastController(
        ILogger<WeatherForecastController> logger
      , IConfiguration configuration
      , IWebHostEnvironment env
      )
    {
      _logger = logger;
      Configuration = configuration;
      if (env.IsDevelopment())
      {
        connStr = Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
      }
      else
      {
        connStr = Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
      }

      if (connStr is not null) {
        connStr = connStr.Replace("{your_password}"
        , VueWithASP.Server.MyCode.Core.Settings.cSettings.getSetting(cSettings.eSetting.DbPass));
      }

    }

    [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>>  Get()
        {
          string sDataFromDatabase = "";

      //add  using System.Data.SqlClient;
      //     using System.Data;
      //Along with the using statements, you need the system assembly reference.
      //To add assembly you can do the following.
      //   install nuget package. Right Click on Project > Manage Nuget Packages > 
      //   Search & install 'System.Data.SqlClient' and make sure it is compatible with the type of project (Core/Standard);

      string commandText = @"SELECT * FROM [dbo].[APP_OBJECTS] ";
      // WHERE SUBMITTEREMAIL = @SUBMITTEREMAIL 
      //ORDER BY CreationDatetime DESC";

      using (var connection = new SqlConnection(connStr))
      {
        await connection.OpenAsync();   //vs  connection.Open();
        using (var tran = connection.BeginTransaction())
        {
          using (var command = new SqlCommand(commandText, connection, tran))
          {
            try
            {
              //command.Parameters.Add("@SUBMITTEREMAIL", SqlDbType.NVarChar);
              //command.Parameters["@SUBMITTEREMAIL"].Value = "me@someDomain.org";

              SqlDataReader rdr = await command.ExecuteReaderAsync();  // vs also alternatives, command.ExecuteReader();  or await command.ExecuteNonQueryAsync();

              while (rdr.Read())
              {
                sDataFromDatabase += rdr["Object_Name"].ToString();
              }
              await rdr.CloseAsync();
            }
            catch (Exception Ex)
            {
              await connection.CloseAsync();
              string msg = Ex.Message.ToString();
              tran.Rollback();
              throw;
            }
          }
        }
      }

      IEnumerable<WeatherForecast> oReturn;

      oReturn = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)] + sDataFromDatabase
      })
            .ToArray();
      return oReturn;
    }


  }
}
