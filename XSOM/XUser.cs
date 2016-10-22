using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace o365Auth
{
    public class XUser
    {
        public string Title { get; set; }
        public int Id { get; set; }
        public string LoginName { get; set; }
        public string PrincipalType { get; set; }
        public string Email { get; set; }
        public bool IsShareByEmailGuestUser { get; set; }
        public bool IsSiteAdmin { get; set; }
        public string NameId { get; set; }
        public string NameIdIssuer { get; set; }
        public XContext Context { get; set; }

        public static XUser ParseUser(string resPonseData)
        {
            XElement x = XElement.Parse(resPonseData);
            x = XmlUtil.RemoveAllNamespaces(x);
            XUser user = new XUser
           {
               Title = x.XPathSelectElement("/content/properties/Title").Value,
               Id = int.Parse(x.XPathSelectElement("/content/properties/Id").Value),
               LoginName = x.XPathSelectElement("/content/properties/LoginName").Value,
               PrincipalType = x.XPathSelectElement("/content/properties/PrincipalType").Value,
               Email = x.XPathSelectElement("/content/properties/Email").Value,
               IsShareByEmailGuestUser = bool.Parse(x.XPathSelectElement("/content/properties/IsShareByEmailGuestUser").Value),
               IsSiteAdmin = bool.Parse(x.XPathSelectElement("/content/properties/IsSiteAdmin").Value),
           };

            return user;
        }


        public async Task<List<XGroup>> GetGroups()
        {
            HttpClient client = Context.GetClient();

            var request = new HttpRequestMessage();
            Uri siteUrl = new Uri(Context.SiteUrl);
            Uri getCurrentUser = new Uri(siteUrl, string.Format("/_api/Web/GetUserById({0})/Groups", Id));
            request.Method = HttpMethod.Get;
            request.RequestUri = getCurrentUser;

            List<XGroup> groups = null;

            //TODO: add error handling
            await client.SendAsync(request).ContinueWith((taskwithmsg) =>
            {
                var response = taskwithmsg.Result;

                var responseContentTask = response.Content.ReadAsStringAsync();
                responseContentTask.Wait();
                string resPonseData = responseContentTask.Result;
                groups = XGroup.ParseGroups(resPonseData);
                groups.ForEach(x => x.Context = Context);
                return groups;
            });

            return groups;
        }


        public static List<XUser> ParseUsers(string resPonseData)
        {
            XElement x = XElement.Parse(resPonseData);
            x = XmlUtil.RemoveAllNamespaces(x);

            var entries = x.Descendants("entry");

            List<XUser> users = new List<XUser>();
            foreach (var entry in entries)
            {
                XUser user = new XUser
                {
                    Title = entry.XPathSelectElement("content/properties/Title").Value,
                    Id = int.Parse(entry.XPathSelectElement("content/properties/Id").Value),
                    LoginName = entry.XPathSelectElement("content/properties/LoginName").Value,
                    PrincipalType = entry.XPathSelectElement("content/properties/PrincipalType").Value,
                    Email = entry.XPathSelectElement("content/properties/Email").Value,
                    IsShareByEmailGuestUser = bool.Parse(entry.XPathSelectElement("content/properties/IsShareByEmailGuestUser").Value),
                    IsSiteAdmin = bool.Parse(entry.XPathSelectElement("content/properties/IsSiteAdmin").Value),
                };
                users.Add(user);
            }

            return users;
        }

    }
}
