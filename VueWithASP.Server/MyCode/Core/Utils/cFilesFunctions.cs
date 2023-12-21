namespace VueWithASP.Server.MyCode.Core.Utils
{
  public static class cFilesFunctions
  {
    public static bool checkFileAccess(string psPath)
    {
      bool bReturn = true;
      System.IO.StreamWriter oStreamWriter = null;
      System.IO.FileInfo oFile;

      try
      {
        oFile = new System.IO.FileInfo(psPath);

        try
        {
          if (oFile.Exists == true)
            oStreamWriter = oFile.AppendText();
          else
            oStreamWriter = oFile.CreateText();
        }
        catch (UnauthorizedAccessException oException)
        {
          bReturn = false;
        }

        if (bReturn == true)
          oStreamWriter.Close();
      }
      catch (Exception oException)
      {
        bReturn = false;
        // Avoid endless loop by error log
        Console .Write ( "Error: " + oException.Message + "\n\nStack: \n" + oException.StackTrace  + "\n");
      }
      return bReturn;
    }
  }
}
