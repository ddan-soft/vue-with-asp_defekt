using System.Net.Mail;
using System.Net;
using VueWithASP.Server.MyCode.Core.Settings;

namespace VueWithASP.Server.MyCode.Core.Utils
{
  public static class cEmail
  {
    public static string SendEmail(string psSubject
           , string psMessage
           , System.Collections.Generic.List<String>  poTo_Addresses
           , string psFrom = ""
           , bool pbHTML = false
           , string psCredentialUser = ""
           , string psCredentialPass = ""
           , System.Collections.Generic.List<String>? poAttachments = null
           , System.Collections.Generic.List<String>? poCC_Adressen = null
           , System.Collections.Generic.List<String>? poBCC_Adressen = null
   )
    {
      string sTo_Adressen = "";
      string sCC_Adressen = "";
      string sBCC_Adressen = "";
      string sCredentialUser = "";
      string sCredentialPass = "";
      string sReturn = "";

      try
      {
        SmtpClient oClient = new SmtpClient(cSettings.getSetting (cSettings.eSetting.SmtpServer)) ;
        MailMessage oMailMessage = new MailMessage();

        oClient.EnableSsl = true;
        oClient.Port = 587;

        if (psCredentialUser.Trim().Length == 0)
        {
          sCredentialUser = cSettings.getSetting(cSettings.eSetting.SmtpEmailUser);
          sCredentialPass = cSettings.getSetting(cSettings.eSetting.SmtpEmailPass);
        }
        else
        {
          sCredentialPass = psCredentialPass;
          sCredentialUser = psCredentialUser;
        }

        oClient.Credentials = new NetworkCredential(sCredentialUser, sCredentialPass);
        // Specify the e-mail sender.
        // Create a mailing address that includes a UTF8 character in the display name.
        // MailAddress from = new MailAddress("jane@contoso.com",   "Jane " + (char)0xD8+ " Clayton", System.Text.Encoding.UTF8)

        foreach (string sRecipient in poTo_Addresses)
        {
          oMailMessage.To.Add(new MailAddress(sRecipient));

          sTo_Adressen = sTo_Adressen + ";" + sRecipient;
        }

        if (poCC_Adressen is not null)
        {
          foreach (string sRecipient in poCC_Adressen)
          {
            oMailMessage.CC.Add(new MailAddress(sRecipient));

            sCC_Adressen = sCC_Adressen + ";" + sRecipient;
          }
        }

        if (poBCC_Adressen is not null)
        {
          foreach (string sRecipient in poBCC_Adressen)
          {
            oMailMessage.Bcc.Add(new MailAddress(sRecipient));

            sBCC_Adressen = sBCC_Adressen + ";" + sRecipient;
          }
        }

        if (psFrom.Trim().Length > 0)
          oMailMessage.From = new MailAddress(psFrom);
        else
          oMailMessage.From = new MailAddress(sCredentialUser);

        oMailMessage.Body = psMessage;
        oMailMessage.BodyEncoding = System.Text.Encoding.UTF8;
        oMailMessage.Subject = psSubject;
        oMailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
        oMailMessage.IsBodyHtml = pbHTML;

        if (poAttachments is not null)
        {
          foreach (string psFile in poAttachments)
          {
            Attachment oAttachment;

            oAttachment = new Attachment(psFile);
            oMailMessage.Attachments.Add(oAttachment);
          }
        }

        // oClient.SendAsync(oMailMessage, "Test")
        oClient.Send(oMailMessage);


        // // Set the method that is called back when the send operation ends.

        // client.SendCompleted += new 
        // SendCompletedEventHandler(SendCompletedCallback)
        // // The userState can be any object that allows your callback 

        // // method to identify this send operation.

        // // For this example, the userToken is a string constant.

        // string userState = "test message1"
        // client.SendAsync(Message, userState)
        // Console.WriteLine("Sending message... press c to cancel mail. Press any other key to exit.")
        // string answer = Console.ReadLine()
        // // If the user canceled the send, and mail hasn't been sent yet,

        // // then cancel the pending operation.

        // if (answer.StartsWith("c") && mailSent == false)

        // client.SendAsyncCancel()

        oMailMessage.Dispose();
      }
      catch (Exception oException)
      {
        sReturn = oException.Message;

        ErrorHandling.cErrorHandling .LogError(oException
          , "psSubject = " + psSubject
            + "\npsFrom = " + psFrom
            + "\nstrAdressen = " + sTo_Adressen
            + "\npsMessage = " + psMessage
          , pnEmailPrio : 0 // don't send an email when is an error by email sending
        );
      }

      return sReturn;
    }
  }
}
