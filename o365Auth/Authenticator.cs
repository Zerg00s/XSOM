using o365Auth;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace o365Auth
{
    public class Authenticator
    {
        public static string GetUserRealmURL = "https://login.microsoftonline.com/GetUserRealm.srf";
        public static string rst2URL =         "https://login.microsoftonline.com/rst2.srf";
        public const string office365STS =     "https://login.microsoftonline.com/extSTS.srf";
        public const string office365Login =   "https://login.microsoftonline.com/login.srf";

        private string _siteUrl = null;
        public string _certificate = null;
        public string _stsUrl = null;
        private string _login = null;
        private string _pass = null;
        private string _messageId;
        private string _SignatureValue = null;
        private string _X509Certificate = null;
        public string _DigestValue = null;
        private string _NameIdentifier = null;
        private string _AssertionID = null;
        private string _AssertionFullXml = null;
        private string _created = null;
        private string _expired = null;
        private string _Issuer;
        public string _BinarySecurityToken = null;
        public string _RequestedSecurityToken = null;
        private string _SPOIDCRL = null;
        public CookieContainer cc = null;

        //Managed - Vanila SharePoint online
        //Federated - ADFS is used
        public string NameSpaceType = null;

        public bool IsADFS
        {
            get
            {
                return NameSpaceType == "Federated";
            }

        }

        public Authenticator(string login, string pass, string siteUrl)
        {
            _login = login;
            _pass = pass;
            _siteUrl = siteUrl;
            cc = new CookieContainer();
        }

        public async Task<XContext> GetCookiesAndDigest()
        {
            string debugResult = null;

            if (this.IsADFS)
            {
                debugResult = await this.RequestTokenFromAdfsSTS();
                debugResult = await this.GetSharePointToken();
                debugResult = await this.SetCookiesADFS();
                debugResult = await this.GetDigestValueADFS();
            }
            else
            {
                debugResult = await this.GetTokenValue();
                debugResult = await this.GetCookiesVanila();
                debugResult = await this.GetDigest();
            }

            XContext context = new XContext()
            {
                SiteUrl = _siteUrl,
                Cookies = cc,
                DigestValue = _DigestValue
            };

            return context;
        }

        #region ADFS

        public async Task<Authenticator> InitializeAuth()
        {
            //POST https://login.microsoftonline.com/GetUserRealm.srf
            //Content-Type: application/x-www-form-urlencoded
            //Host: login.microsoftonline.com
            //login=dmolodtsov%40jolera.com&xml=1

            HttpClient client = new HttpClient();

            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(GetUserRealmURL);
            request.Method = HttpMethod.Post;

            request.Headers.Add("Accept", "application/json; odata=verbose");
            request.Headers.Host = "login.microsoftonline.com";

            string realmRequest = string.Format("login={0}&xml=1", _login.Replace("@", "%40"));
            request.Content = new StringContent(realmRequest, Encoding.UTF8, "application/x-www-form-urlencoded");

            await client.SendAsync(request).ContinueWith((taskwithmsg) =>
            {
                var response = taskwithmsg.Result;

                var responseContentTask = response.Content.ReadAsStringAsync();
                responseContentTask.Wait();
                string resPonseData = responseContentTask.Result;

                XElement x = XElement.Parse(resPonseData);
                NameSpaceType = x.Descendants().Where(xg => xg.Name.LocalName == "NameSpaceType").First().Value;
                if (NameSpaceType == "Federated")
                {
                    _stsUrl = x.Descendants().Where(xg => xg.Name.LocalName == "STSAuthURL").First().Value;
                    _certificate = x.Descendants().Where(xg => xg.Name.LocalName == "Certificate").First().Value;
                }

            });

            return this;
        }

        private async Task<string> RequestTokenFromAdfsSTS()
        {
            _messageId = Guid.NewGuid().ToString("D");
            _created = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fff0000Z");
            _expired = DateTime.Now.AddMinutes(10).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fff0000Z");

            Uri stsUri = new Uri(_stsUrl);

            HttpClient client = new HttpClient();

            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(_stsUrl);
            request.Method = HttpMethod.Post;

            request.Headers.Host = stsUri.Host;

            string tokenRequest = string.Format(Constants.TokenRequestADFS, _stsUrl, _messageId, _login, _pass, _created, _expired);

            request.Content = new StringContent(tokenRequest, Encoding.UTF8, "application/soap+xml");

            await client.SendAsync(request).ContinueWith((taskwithmsg) =>
            {
                var response = taskwithmsg.Result;

                var responseContentTask = response.Content.ReadAsStringAsync();
                responseContentTask.Wait();
                string resPonseData = responseContentTask.Result;
                XElement x = XElement.Parse(resPonseData);
                _SignatureValue = x.Descendants().Where(xg => xg.Name.LocalName == "SignatureValue").First().Value;
                _X509Certificate = x.Descendants().Where(xg => xg.Name.LocalName == "X509Certificate").First().Value;
                _DigestValue = x.Descendants().Where(xg => xg.Name.LocalName == "DigestValue").First().Value;
                _NameIdentifier = x.Descendants().Where(xg => xg.Name.LocalName == "NameIdentifier").First().Value;
                _AssertionID = x.Descendants().Where(xg => xg.Name.LocalName == "Assertion").First().Attributes("AssertionID").First().Value;
                _Issuer = x.Descendants().Where(xg => xg.Name.LocalName == "Assertion").First().Attributes("Issuer").First().Value;

                _AssertionFullXml = x.Descendants().Where(xg => xg.Name.LocalName == "Assertion").First().ToString(SaveOptions.DisableFormatting);
            });


            return _AssertionFullXml;

        }

        private async Task<string> GetSharePointToken()
        {
            HttpClient client = new HttpClient();
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(rst2URL);
            request.Method = HttpMethod.Post;
            request.Headers.Host = "login.microsoftonline.com";

            string tokenRequest = string.Format(@"<S:Envelope xmlns:S='http://www.w3.org/2003/05/soap-envelope' xmlns:wsse='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd' xmlns:wsp='http://schemas.xmlsoap.org/ws/2004/09/policy' xmlns:wsu='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd' xmlns:wsa='http://www.w3.org/2005/08/addressing' xmlns:wst='http://schemas.xmlsoap.org/ws/2005/02/trust'>
<S:Header>
	<wsa:Action S:mustUnderstand='1'>http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Issue</wsa:Action>
	<wsa:To S:mustUnderstand='1'>https://login.microsoftonline.com/rst2.srf</wsa:To>
	<ps:AuthInfo xmlns:ps='http://schemas.microsoft.com/LiveID/SoapServices/v1' Id='PPAuthInfo'>
		<ps:BinaryVersion>5</ps:BinaryVersion>
		<ps:HostingApp>Managed IDCRL</ps:HostingApp>
	</ps:AuthInfo>
	<wsse:Security>
		{0}
	</wsse:Security>
</S:Header>
<S:Body>
	<wst:RequestSecurityToken xmlns:wst='http://schemas.xmlsoap.org/ws/2005/02/trust' Id='RST0'>
		<wst:RequestType>http://schemas.xmlsoap.org/ws/2005/02/trust/Issue</wst:RequestType>
		<wsp:AppliesTo>
			<wsa:EndpointReference>
				<wsa:Address>sharepoint.com</wsa:Address>
			</wsa:EndpointReference>
		</wsp:AppliesTo>
		<wsp:PolicyReference URI='MBI'/>
	</wst:RequestSecurityToken>
</S:Body>
</S:Envelope>", _AssertionFullXml);

            request.Content = new StringContent(tokenRequest, Encoding.UTF8, "application/soap+xml");

            await client.SendAsync(request).ContinueWith((taskwithmsg) =>
            {
                var response = taskwithmsg.Result;

                var responseContentTask = response.Content.ReadAsStringAsync();
                responseContentTask.Wait();
                string resPonseData = responseContentTask.Result;
                XElement x = XElement.Parse(resPonseData);
                _BinarySecurityToken = x.Descendants().Where(xg => xg.Name.LocalName == "BinarySecurityToken").First().Value;
                return _BinarySecurityToken;
            });

            return _BinarySecurityToken;

        }

        private async Task<string> SetCookiesADFS()
        {

            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = cc;
            HttpClient client = new HttpClient(handler);

            var request = new HttpRequestMessage();
            Uri requestUri = new Uri(_siteUrl + "/_vti_bin/idcrl.svc/");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("BPOSIDCRL", _BinarySecurityToken);
            client.DefaultRequestHeaders.Add("X-IDCRL_ACCEPTED", "t");

            HttpResponseMessage response = await client.GetAsync(requestUri);
            try
            {
                if (string.IsNullOrEmpty(_SPOIDCRL))
                {
                    _SPOIDCRL = cc.GetCookies(requestUri).Cast<Cookie>().First(c => c.Name == "SPOIDCRL").Value;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return _SPOIDCRL;
        }

        private async Task<string> GetDigestValueADFS()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = cc;
            HttpClient client = new HttpClient(handler);
            var request = new HttpRequestMessage();

            request.Headers.Host = new Uri(_siteUrl).Host;
            request.Method = HttpMethod.Post;

            request.RequestUri = new Uri(_siteUrl + "/_vti_bin/sites.asmx");
            request.Headers.Add("X-RequestForceAuthentication", "true");
            request.Headers.Add("X-FORMS_BASED_AUTH_ACCEPTED", "f");
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.Headers.Add("SOAPAction", "http://schemas.microsoft.com/sharepoint/soap/GetUpdatedFormDigestInformation");


            string postData = @"<?xml version='1.0' encoding='utf-8'?>
	<soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>
	  <soap:Body>
	    <GetUpdatedFormDigestInformation xmlns='http://schemas.microsoft.com/sharepoint/soap/' />
	  </soap:Body>
	</soap:Envelope>";

            request.Content = new StringContent(postData, Encoding.UTF8, "text/xml");

            await client.SendAsync(request).ContinueWith((taskwithmsg) =>
            {
                var response = taskwithmsg.Result;

                var responseContentTask = response.Content.ReadAsStringAsync();
                responseContentTask.Wait();
                string resPonseData = responseContentTask.Result;
                XElement x = XElement.Parse(resPonseData);
                _DigestValue = x.Descendants().Where(xg => xg.Name.LocalName == "DigestValue").First().Value;
            });

            return _DigestValue;
        }
        #endregion

        #region Vaila SharePointOnline (NO ADFS)
        private async Task<string> GetTokenValue()
        {

            HttpClient client = new HttpClient();
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(Constants.StsAuthorityUrl);
            request.Method = HttpMethod.Post;


            string tokenRequest = string.Format(Constants.TokenRequestNoADFS, _login, _pass, _siteUrl);
            request.Content = new StringContent(tokenRequest, Encoding.UTF8, "application/soap+xml");

            var result = await client.SendAsync(request).ContinueWith((taskwithmsg) =>
            {
                var response = taskwithmsg.Result;

                var responseContentTask = response.Content.ReadAsStringAsync();
                responseContentTask.Wait();
                string resPonseData = responseContentTask.Result;
                XElement x = XElement.Parse(resPonseData);
                if (x.Descendants().FirstOrDefault(xg => xg.Name.LocalName == "internalerror")!=null)
                {
                    XElement faultXmlWithExplanations = XmlUtil.RemoveAllNamespaces(x);
                    var Reason = faultXmlWithExplanations.XPathSelectElement("/Body/Fault/Reason/Text").Value;
                    var Detail = faultXmlWithExplanations.XPathSelectElement("/Body/Fault/Detail/error/internalerror/text").Value.Trim();
                    string errorMessage = string.Format("Error: {0}. {1}", Reason, Detail);
                    return errorMessage;
                }
                 _RequestedSecurityToken = x.Descendants().Where(xg => xg.Name.LocalName == "RequestedSecurityToken").First().Value;
                return _RequestedSecurityToken;
            });

            return result;
        }

        private async Task<string> GetCookiesVanila()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = cc;
            HttpClient client = new HttpClient(handler);

            var request = new HttpRequestMessage();
            Uri siteUri = new Uri(_siteUrl + "/_forms/default.aspx?wa=wsignin1.0");
            request.RequestUri = siteUri;
            request.Method = HttpMethod.Post;
            request.Headers.Host = siteUri.Host;

            string Location = null;

            try
            {
                request.Content = new StringContent(_RequestedSecurityToken, Encoding.UTF8, "application/soap+xml");
                await client.SendAsync(request).ContinueWith((taskwithmsg) =>
                {
                    var response = taskwithmsg.Result;

                    var responseContentTask = response.Content.ReadAsStringAsync();
                    responseContentTask.Wait();
                    string resPonseData = responseContentTask.Result;
                    if (response.StatusCode == HttpStatusCode.MovedPermanently)
                    {
                        HttpHeaders headers = response.Headers;
                        IEnumerable<string> values;
                        
                        if (headers.TryGetValues("Location", out values))
                        {
                            Location = values.First();
                        }
                    }
                });

                if (string.IsNullOrEmpty(Location) == false)
                {
                    HttpRequestMessage request2 = new HttpRequestMessage();
                    request2.RequestUri = new Uri(Location);
                    request2.Content = new StringContent(_RequestedSecurityToken, Encoding.UTF8, "application/soap+xml");

                    await client.SendAsync(request2).ContinueWith((taskwithmsg) =>
                    {
                        var response = taskwithmsg.Result;
                        var responseContentTask = response.Content.ReadAsStringAsync();
                    });
                }

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

            return "OK";
        }

        private async Task<string> GetDigest()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = cc;
            HttpClient client = new HttpClient(handler);

            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(_siteUrl + "/_api/contextinfo");
            request.Method = HttpMethod.Post;

            await client.SendAsync(request).ContinueWith((taskwithmsg) =>
            {
                var response = taskwithmsg.Result;

                var responseContentTask = response.Content.ReadAsStringAsync();
                responseContentTask.Wait();
                string resPonseData = responseContentTask.Result;
                XElement digestResponse = XElement.Parse(resPonseData);
                _DigestValue = digestResponse.Descendants().Where(x => x.Name.LocalName == "FormDigestValue").First().Value;
            });

            return _DigestValue;    
        }
        #endregion



        


    }
}
