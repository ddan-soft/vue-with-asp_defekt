
namespace VueWithASP.Server.MyCode.Core.Utils
{
  public static class cUtils
  {
    public static string RandomString(int pintLength)
    {
      const string strChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
      string strReturn = "";
      Random objRandom = new System.Random();

      for (int intIndex = 1; intIndex <= pintLength; intIndex++)
      {
        strReturn = strReturn + strChars.Substring(objRandom.Next(0, strChars.Length), 1);
      }

      return strReturn;
    }
  }
}
