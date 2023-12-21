using System.Diagnostics.Metrics;

namespace VueWithASP.Server.MyCode.Core.Settings
{
  public class cSettings
  {
    public enum eSetting
    {
      ErrorLogFolder,
      ErrorEmailListPrio1,
      ErrorEmailListPrio2,
      SmtpServer,
      SmtpEmailUser,
      SmtpEmailPass,
      DbPass
    }

    public static string getSetting(eSetting pSetting)
    {
      String? sReturn = "";

      switch (pSetting)
      {
        case eSetting.ErrorLogFolder:
          sReturn = @".\LogFiles";
          break;
        case eSetting.ErrorEmailListPrio1:
          sReturn = @"ddan6709@hotmail.com";
          break;
        case eSetting.ErrorEmailListPrio2:
          sReturn = @"ddan6709@hotmail.com";
          break;
        case eSetting.SmtpServer:
          sReturn = @"smtp-mail.outlook.com";
          break;
        case eSetting.SmtpEmailUser:
          sReturn = @"best.soft.2023@outlook.com";
          break;
        case eSetting.SmtpEmailPass:
          sReturn = Environment.GetEnvironmentVariable("EMAIL_PASS");

          if (sReturn is null) 
          {
            sReturn = "";
          }
          break;
        case eSetting.DbPass:
          sReturn = Environment.GetEnvironmentVariable("DB_PASS");

          if (sReturn is null)
          {
            sReturn = "";
          }
          break;
        //Environment.GetEnvironmentVariable("MY_VAR")
        default:
          sReturn = "";
          break;
      }

      return sReturn;
    }
  }
}
