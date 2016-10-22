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
    public class XGroup
    {
        public int Id { get; set; }
        public string LoginName { get; set; }
        public string Title { get; set; }
        public int PrincipalType { get; set; }
        public bool AllowMembersEditMembership { get; set; }
        public bool AllowRequestToJoinLeave { get; set; }
        public bool AutoAcceptRequestToJoinLeave { get; set; }
        public string Description { get; set; }
        public bool OnlyAllowMembersViewMembership { get; set; }
        public string OwnerTitle { get; set; }

        //TODO:
        public XContext Context { get; set; }

        public static List<XGroup> ParseGroups(string resPonseData)
        {
            XElement x = XElement.Parse(resPonseData);
            x = XmlUtil.RemoveAllNamespaces(x);

            var entries = x.Descendants("entry");

            List<XGroup> groups = new List<XGroup>();
            foreach (var entry in entries)
            {
                XGroup group = new XGroup
                {
                    Title = entry.XPathSelectElement("content/properties/Title").Value,
                    Id = int.Parse(entry.XPathSelectElement("content/properties/Id").Value),
                    LoginName = entry.XPathSelectElement("content/properties/LoginName").Value,
                    PrincipalType = int.Parse(entry.XPathSelectElement("content/properties/PrincipalType").Value),
                    AllowMembersEditMembership = bool.Parse(entry.XPathSelectElement("content/properties/AllowMembersEditMembership").Value),
                    AllowRequestToJoinLeave = bool.Parse(entry.XPathSelectElement("content/properties/AllowRequestToJoinLeave").Value),
                    AutoAcceptRequestToJoinLeave = bool.Parse(entry.XPathSelectElement("content/properties/AutoAcceptRequestToJoinLeave").Value),
                    Description = entry.XPathSelectElement("content/properties/Description").Value,
                    OnlyAllowMembersViewMembership = bool.Parse(entry.XPathSelectElement("content/properties/OnlyAllowMembersViewMembership").Value),
                    OwnerTitle = entry.XPathSelectElement("content/properties/OwnerTitle").Value
                };
                groups.Add(group);
            }

            return groups;
        }

        //TODO: get Owners
        // title="Owner" href="Web/SiteGroups/GetById(7)/Owner" />
        //TODO: get Users
        // title="Users" href="Web/SiteGroups/GetById(7)/Users" />


        public async Task<List<XUser>> GetUsers()
        {
            HttpClient client = Context.GetClient();

            var request = new HttpRequestMessage();
            Uri siteUrl = new Uri(Context.SiteUrl);
            Uri getGroupUsersUrl = new Uri(siteUrl, string.Format("/_api/Web/SiteGroups/GetById({0})/Users", Id));
            request.Method = HttpMethod.Get;
            request.RequestUri = getGroupUsersUrl;

            List<XUser> users = null;

            //TODO: add error handling
            await client.SendAsync(request).ContinueWith((taskwithmsg) =>
            {
                var response = taskwithmsg.Result;
                var responseContentTask = response.Content.ReadAsStringAsync();
                responseContentTask.Wait();
                string resPonseData = responseContentTask.Result;
                users = XUser.ParseUsers(resPonseData);
                return users;
            });

            return users;
        }

    }
}
