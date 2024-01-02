using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using VueWithASP.Server.MyCode.Core.Settings;
using VueWithASP.Server.MyCode.MySystem;

namespace VueWithASP.Server.MyCode.MySystem
{
  public static class cUsers
  {
    public static async Task<string> GetUsers(JObject oJObjectRequest)
    {
      string sReturn = "";
      string sDbConnectionString = "";
      string sUsers = "";

      try
      {

        // Check user name and pass in DB
        sDbConnectionString = VueWithASP.Server.MyCode.Core.Settings.cSettings.getSetting(cSettings.eSetting.DbConnectionString);
        string commandText =
            $"SELECT USER_ID, Login_Name  \n"
          + $"FROM [dbo].[Users] \n"
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

                while (oSqlDataReader.Read())
                {
                  if (sUsers.Trim().Length > 0)
                  {
                    sUsers += ",";
                  }
                  sUsers += "{\n";
                  sUsers += "'userId': '" + oSqlDataReader["USER_ID"].ToString().Replace("'", "\'") + "',\n";
                  sUsers += "'loginName': '" + oSqlDataReader["Login_Name"].ToString().Replace("'", "\'") + "'\n ";
                  sUsers += "}\n";
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

        sReturn =
@"{
'message': 'Success', 
'success': true,
'users': [" + sUsers + "\n]" +
"}";
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

    }
  }
}


