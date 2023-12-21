using Microsoft.VisualBasic;
using System.Data.Common;
using System.Reflection;
using VueWithASP.Server.MyCode.Core.Settings;
using VueWithASP.Server.MyCode.Core.Utils;

namespace VueWithASP.Server.MyCode.Core.ErrorHandling
{
  public static class cErrorHandling
  {
    static string msPreviousError = "";
    static decimal mdLastErrorTimeInSeconds = 0;

    public static void WriteErrorLog(
          string psText
        , string psFileName = ""
        , bool pbCarriageReturn = true
    )
    {
      string sErrorFilePath = "";
      string sFileName = "ErrorLog";
      string sFolder = "";
      FileInfo oFile;
      StreamWriter oStreamWriter;
      bool bContinue = true;
      string sMessage;

      try
      {
        if (cGlobals.gsErrorInLogFile == false)
        {
          bContinue = false;
          cGlobals.gsErrorInLogFile = true;
        }

        if (bContinue == true & psFileName.Trim().Length > 0)
          sFileName = psFileName;

        if (bContinue == true & sFolder == "")
        {
          // sFolder = Application.StartupPath
          sFolder = cSettings.getSetting(cSettings.eSetting.ErrorLogFolder);
          if (sFolder.Length > 0)
          {
            try
            {
              Directory.CreateDirectory(sFolder);
            }
            catch (UnauthorizedAccessException oException)
            {
              sFolder = "";
            }
            catch (IOException oException)
            {
              sFolder = "";
            }
          }
        }
        if (bContinue == true)
        {
          if (Directory.Exists(sFolder) == false)
          {
            try
            {
              Directory.CreateDirectory(sFolder);
            }
            catch (UnauthorizedAccessException oException)
            {
              sFolder = "";
            }
            catch (IOException oException)
            {
              sFolder = "";
            }
          }
        }

        if (Directory.Exists(sFolder) == true)
        {
          sErrorFilePath = sFolder + @"\" + sFileName + ".txt";

          if (cFilesFunctions.checkFileAccess(sErrorFilePath) == false)
            sErrorFilePath = sFolder + @"\" + sFileName + "_Falls_Datei_Gesperrt.txt";
        }
        else { 
          sFolder = "";
        }

      if (bContinue == true & sFolder == "")
      {
          sFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\"
            + System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
        if (Directory.Exists(sFolder) == false)
        {
          try
          {
            Directory.CreateDirectory(sFolder);
          }
          catch (UnauthorizedAccessException oException)
          {
            sFolder = "";
          }
          catch (IOException oException)
          {
            sFolder = "";
          }
        }
      }

      if (bContinue == true & sFolder == "")
      {
        bContinue = false;
        Console.Write ("It was not possible to write the error log.");
      }

      if (bContinue == true)
      {
        // Wenn die Datei ist großer als 10 MB, verschiebe ich die.
        oFile = new FileInfo(sErrorFilePath);
        if (oFile.Exists == true)
        {
          if (oFile.Length > 10485760)
          {
            oFile.MoveTo(sFolder + @"\" + sFileName + DateTime.Now.ToString ("yyyyMMddhhmm") + ".txt");
            oStreamWriter = oFile.CreateText();
          }
          else
            oStreamWriter = oFile.AppendText();
        }
        else
          oStreamWriter = oFile.CreateText();

        if (pbCarriageReturn == true)
          psText = psText + "\n";

        oStreamWriter.Write(psText);
        oStreamWriter.Close();
      }
    }
    catch (Exception oException)
      {
        if (oException.Message.Contains("Der Prozess kann nicht auf die Datei") == false)
        {
          sMessage = "An unexpected error occured. ";
          sMessage = sMessage + "\nError: " + oException.Message;
          sMessage = sMessage + "\nStack: " + oException.StackTrace;
          Console.Write(sMessage);
        }
      }
    } // End of --- public static void WriteErrorLog

    public static void LogError(
        Exception poException
      , string psVariables = ""
      , bool pbPropagate = true
      , int pnEmailPrio = 1
    )
    {
      string sSource = "";
      string sStackTrace = "";

      if (poException.Source is not null) 
      {
        sSource = poException.Source;
      }

      if (poException.StackTrace is not null)
      {
        sSource = poException.StackTrace;
      }

      LogError2(poException.Message, sSource, sStackTrace, psVariables, pbPropagate, pnEmailPrio);
    } // End of --- public static void LogError

    public static void LogError2(
      string psErrorMessage
      , string psErrSource
      , string psStack = ""
      , string psVariables = ""
      , bool pbPropagate = true
      , int pnEmailPrio = 1
      )
    {
      string sLoggedText = "";
      bool bAlreadyLogged = false;
      bool bLogError = true;

      if (msPreviousError  == psErrorMessage & mdLastErrorTimeInSeconds > 0 
        & System.Convert.ToDecimal(Math.Round(DateTime.Now.TimeOfDay.TotalSeconds, 3)) 
        - mdLastErrorTimeInSeconds < 5)
      { bAlreadyLogged = true; 
      }        

      msPreviousError = psErrorMessage;

      if (bLogError == true)
      {
        sLoggedText = "";
        if (bAlreadyLogged == true)
        {
          sLoggedText = sLoggedText + "\n Zeit:  " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
          sLoggedText = sLoggedText + "\n Stack:  \n" + psStack;
        }
        else
        {
          AssemblyInformationalVersionAttribute infoVersion =
            (AssemblyInformationalVersionAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(
              typeof(AssemblyInformationalVersionAttribute), false).FirstOrDefault()
            ;
          sLoggedText = sLoggedText + "\n ******** " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ******************************";
          sLoggedText = sLoggedText + "\n " + System.Reflection.Assembly.GetEntryAssembly().GetName().Name
            + "; Version: " + infoVersion.InformationalVersion;
          ;
          sLoggedText = sLoggedText + "\n Error message:  " + psErrorMessage;
          sLoggedText = sLoggedText + "\n Source:  " + psErrSource;
          sLoggedText = sLoggedText + "\n Stack\n:  " + psStack;
        }

        if (psVariables.Trim() != "")
        {
          sLoggedText = sLoggedText + "\n --- Variablen --";
          sLoggedText = sLoggedText + "\n" + psVariables;
        }

        sLoggedText = sLoggedText + "\n ------------------------------------------------";
        sLoggedText = sLoggedText + "\n";

        //WriteErrorLog(sLoggedText);
        Console.Write(sLoggedText);

        if (bAlreadyLogged == false & pnEmailPrio > 0)
        {
          //if (lfunEmailAusnahme(psErrorMessage, pnEmailPrio) == false)
          SendErrorMessageByEmail(sLoggedText, pnEmailPrio);

        }
      }

      if (bAlreadyLogged == false )
      {
        mdLastErrorTimeInSeconds = System.Convert.ToDecimal(Math.Round(DateTime.Now.TimeOfDay.TotalSeconds, 3));
      }

      if (pbPropagate == false)
      {
        msPreviousError = "";
      }
    } // End of -- public static void LogError

    private static void SendErrorMessageByEmail(string psMessage, int pnPriority)
    {
      string sEmailAddresses = "";
      System.Collections.Generic .List<String>  oEmailAddresses = new System.Collections.Generic.List<String>();

      try
      {
        if (pnPriority == 1)
          sEmailAddresses = cSettings.getSetting(cSettings.eSetting.ErrorEmailListPrio1);
        else if (pnPriority == 2)
          sEmailAddresses = cSettings.getSetting(cSettings.eSetting.ErrorEmailListPrio2);

        sEmailAddresses = sEmailAddresses.Trim();

        while (sEmailAddresses.Trim().Length > 0)
        {
          string strEmailAdresse = "";

          if (sEmailAddresses.Contains(";") == true)
          {
            if (sEmailAddresses.IndexOf(";") > 1)
            {
              strEmailAdresse = sEmailAddresses.Substring(0, sEmailAddresses.IndexOf(";"));

              if (sEmailAddresses.Trim().Length > sEmailAddresses.IndexOf(";") + 1)
                sEmailAddresses = sEmailAddresses.Substring(sEmailAddresses.IndexOf(";") + 1);
              else
                sEmailAddresses = "";
            }
            else
              sEmailAddresses = "";
          }
          else
          {
            strEmailAdresse = sEmailAddresses;
            sEmailAddresses = "";
          }

          oEmailAddresses.Add(strEmailAdresse);
        }

        foreach (string strEmailAdresse in oEmailAddresses)
        {
          Utils.cEmail.SendEmail("Programm error", psMessage, oEmailAddresses);
        }
      }
      catch (Exception objException)
      {
        LogError (objException, pnEmailPrio: 0);
      }
    }
  }// End --- public static class cErrorHandling
}
