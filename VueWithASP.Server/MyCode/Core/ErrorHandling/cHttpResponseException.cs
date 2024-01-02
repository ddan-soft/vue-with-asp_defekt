namespace VueWithASP.Server.MyCode.Core.ErrorHandling
{
  public class cHttpResponseException : Exception
  {
    public cHttpResponseException(int pStatusCode, object? pValue = null) =>
        (StatusCode, Value) = (pStatusCode, pValue);

    public int StatusCode { get; }

    public object? Value { get; }
  }
}
