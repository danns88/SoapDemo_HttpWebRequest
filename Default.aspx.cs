using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace SoapDemo
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Invoke();
        }

        public void Invoke()
        {
            var doc = new XmlDocument();
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            XmlTextWriter xtw = null;

            Stream response = AcquireTicket();

            try
            {
                doc.Load(response);
                xtw = new XmlTextWriter(sw);
                xtw.Formatting = Formatting.Indented;
                doc.WriteTo(xtw);
                resp.InnerText = sb.ToString();
            }
            catch (XmlException ex)
            {
                throw new ApplicationException(ex.Message);
            }
            finally
            {
                if (xtw != null)
                    xtw.Close();
            }
        }

        public Stream AcquireTicket()
        {
            string ticketRequest = @"<?xml version=""1.0"" encoding=""utf-16""?>
            <soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
              <soap:Body>
                <AcquireTicket xmlns=""http://CTL.COM.Services.Prime.Issuer.WebServices/PrimeIssuerServices"">
                  <xmlRequest>
                    <Header>
                      <MessageID>acquire ticket</MessageID>
                      <CorrelationID />
                      <SystemID />
                      <RequestorID />
                      <Ticket></Ticket>
                      <CallerRef />
                      <Origin />
                      <Culture />
                    </Header>
                    <Ticket>
                      <hostIP />
                      <applicationName />
                    </Ticket>
                  </xmlRequest>
                </AcquireTicket>
              </soap:Body>
            </soap:Envelope>";
                 
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://localhost/PRIME4API/Issuer/WebServices/PrimeIssuerServices.asmx");
            request.ContentLength = ticketRequest.Length;
            request.ContentType = "text/xml; charset=utf-8";
            request.Method = "POST";
            request.Pipelined = false;
            request.PreAuthenticate = true;
            request.UnsafeAuthenticatedConnectionSharing = true;
            request.Credentials = CredentialCache.DefaultNetworkCredentials;

            Stream stream = null;
            try
            {
                stream = request.GetRequestStream();
                stream.Write(Encoding.UTF8.GetBytes(ticketRequest), 0, ticketRequest.Length);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode != HttpStatusCode.OK)
                    throw new ApplicationException(string.Format("AcquireTicket call response was '{0}'.", response.StatusDescription));
                return response.GetResponseStream();
            }
            finally
            {
                if (stream != null) stream.Close();
            }
        }

        protected void acquireTicket_Click(object sender, EventArgs e)
        {
            Invoke();
        }
    }
}