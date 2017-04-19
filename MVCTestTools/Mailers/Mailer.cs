using NUnit.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using MVCTestTools.Entities;
using MVCTestTools.Contex;
using System.Net.Configuration;
using System.Configuration;
using System.IO;
using System.Net.Mime;
using System.Net.Mail;
using System.Text;
using System.Net;

namespace MVCTestTools.Mailers
{
    public class Mailer
    {

        MVCTestToolsContext db = new MVCTestToolsContext();

        private SmtpSection smtpSettings;

        public Mailer()
        {
            smtpSettings = ConfigurationManager.GetSection("system.net/mailSettings/smtp") as SmtpSection;
        }

        public void SendEmails(TestResult[] result)
        {
            List<List<TestResult>> appTestResult = new List<List<TestResult>>();
            List<Application> apps = new List<Application>();
            foreach (TestResult res in result)
            {
                int testId = int.Parse(Helper.Result.DivideTestResultName(res)[1]);
                Application app = db.Tests.Find(testId).Application;
                if (app != null)
                {
                    if (!apps.Contains(app))
                    {
                        apps.Add(app);
                        appTestResult.Add(new List<TestResult> { res });
                    }
                    else
                    {
                        int id = apps.IndexOf(app);
                        appTestResult[id].Add(res);
                    }
                }
                else continue;
            }
            for (int i = 0; i < apps.Count; i++)
            {
                String xmlString = GenerateXmlDoc(appTestResult[i].ToArray(), apps[i]);
                Send(xmlString, apps[i].Admins.ToList());
            }
        }

        private String GenerateXmlDoc(TestResult[] result, Application app)
        {
            String XmlString =
                @"<?xml version='1.0' encoding='utf-8'?>
                    <Report>
                        <Application>";
            XmlString += @"<AppName Type='String'>" + app.ProjectName + "</AppName>";
            XmlString += @"<AppUrl Type='String'>" + app.Url + "</AppUrl>";
            XmlString += @"<Tests>";
            foreach (var res in result)
            {
                String[] testParams = Helper.Result.DivideTestResultName(res);
                XmlString = XmlString + @"<Test>";
                XmlString = XmlString + @"<Test_ID Type='String'>" + testParams[1] + "</Test_ID>";
                XmlString = XmlString + @"<Name Type='String'>" + testParams[0] + "</Name>";
                XmlString = XmlString + @"<Input_params>";
                for (int i = 1; i < testParams.Length; i++)
                    XmlString = XmlString + @"<Input Type='String'>" + testParams[i] + "</Input";
                XmlString = XmlString + @"</Input_params>";
                XmlString = XmlString + @"<Executed Type='Bool'>" + res.Executed + "</Executed>";
                XmlString = XmlString + @"<IsSuccess Type='Bool'>" + res.IsSuccess + "</IsSuccess>";
                XmlString = XmlString + @"<Message Type='String'>" + res.Message + "</Message>";
                XmlString = XmlString + @"<Execute_time Type='Double'>" + res.Time + "</Execute_time>";
                XmlString = XmlString + @"<Date Type='DateTime'>" + Convert.ToString(DateTime.Now) + "</Date>";
                XmlString = XmlString + @"</Test>";
            }
            XmlString = XmlString + @"</Tests></Application></Report>";
            return XmlString;
        }

        private void Send(String xmlString, List<Admin> admins)
        {
            MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString));

            ContentType contentType = new ContentType();
            contentType.MediaType = MediaTypeNames.Text.Xml;
            contentType.Name = "Failed_Test_Report" + ".xml";

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = GetSmtpClient();
            mail.From = new MailAddress(smtpSettings.From);
            mail.Subject = "Failed Test Auto Reporter";
            if (ConfigurationManager.AppSettings["MailMode"] == "Test")
            {
                mail.To.Add(ConfigurationManager.AppSettings["TestAlias"]);
            }
            else
            {
                 foreach (Entities.Admin admin in admins)
                {
                    mail.To.Add(admin.Email);
                }
            }

            System.Net.Mail.Attachment attachment = new Attachment(memoryStream, contentType);
            attachment.NameEncoding = UTF8Encoding.UTF8;
            attachment.TransferEncoding = TransferEncoding.Base64;
            attachment.ContentDisposition.DispositionType = DispositionTypeNames.Attachment;
            mail.Attachments.Add(attachment);

            try
            {
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                ///
            }
        }

        private SmtpClient GetSmtpClient()
        {
            SmtpClient client = new SmtpClient();

            if (smtpSettings.DeliveryMethod != SmtpDeliveryMethod.Network)
            {
                client.Credentials = CredentialCache.DefaultNetworkCredentials;
            }
            else
            {
                client.Host = smtpSettings.Network.Host;
                client.Port = smtpSettings.Network.Port;
                client.EnableSsl = smtpSettings.Network.EnableSsl;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(smtpSettings.Network.UserName, smtpSettings.Network.Password);
            }

            return client;
        }
    }
}