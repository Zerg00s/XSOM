using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Text;

namespace XSOM
{
    public class XFile : Dictionary<string, object>
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public string ServerRelativeUrl { get; set; }
        public string Id { get; set; }
        public string GUID { get; set; }
        public string ContentTypeId { get; set; }
        public string Created { get; set; }
        public string Modified { get; set; }
        public string AuthorId { get; set; }

        /*
        public string CheckInComment { get; set; }
        public string CheckOutType { get; set; }
        public string ContentTag { get; set; }
        public string CustomizedPageStatus { get; set; }
        public string ETag { get; set; }
        public string Exists { get; set; }
        public string IrmEnabled { get; set; }
        public string Length { get; set; }
        public string Level { get; set; }
        public string LinkingUrl { get; set; }
        public string MajorVersion { get; set; }
        public string MinorVersion { get; set; }


        public string TimeLastModified { get; set; }
        public string UIVersion { get; set; }
        public string UIVersionLabel { get; set; }
        */

        // Indexer which takes the name of the property and retrieves it from the PropertyBag  
        public XList List { get; set; }

        internal static XFile ParseFile(string resPonseData)
        {
            XElement x = XElement.Parse(resPonseData);
            x = XmlUtil.RemoveAllNamespaces(x);
            if (x.Name.LocalName == "error")
            {
                if (x.XPathSelectElement("/message") != null)
                {
                    throw new ArgumentException(x.XPathSelectElement("/message").Value);
                }
            }
           

            XFile listItem = new XFile
            {
                Title = x.XPathSelectElement("/content/properties/Title").Value,
                GUID = x.XPathSelectElement("/content/properties/GUID").Value,
                Id = x.XPathSelectElement("/content/properties/Id").Value,
                ContentTypeId = x.XPathSelectElement("/content/properties/ContentTypeId").Value,
                Created = x.XPathSelectElement("/content/properties/Created").Value,
                Modified = x.XPathSelectElement("/content/properties/Modified").Value,
                AuthorId = x.XPathSelectElement("/content/properties/AuthorId").Value,
                
            };

            foreach (var field in x.Descendants().First(x2 => x2.Name.LocalName == "properties").Descendants())
            {
                listItem[field.Name.LocalName] = field.Value;
            }

            return listItem;
        }

        public async Task<string> Delete()
        {
            HttpClient client = List.Context.GetClient();
            client.DefaultRequestHeaders.Add("X-HTTP-Method", "DELETE");
            client.DefaultRequestHeaders.Add("IF-MATCH", "*");
            var request = new HttpRequestMessage();
            Uri siteUrl = new Uri(List.Context.SiteUrl);
            Uri deleteListITemUrl = new Uri(siteUrl, string.Format("/_api/web/lists/GetByTitle('{0}')/items({1})", List.Title, Id));
            request.Method = HttpMethod.Post;
            request.RequestUri = deleteListITemUrl;

            await client.SendAsync(request).ContinueWith((taskwithmsg) =>
            {
                var response = taskwithmsg.Result;
                var responseContentTask = response.Content.ReadAsStringAsync();
                responseContentTask.Wait();
                string resPonseData = responseContentTask.Result;
                return resPonseData;
            });

            return string.Empty;
        }

        public async Task<string> Recycle()
        {
            HttpClient client = List.Context.GetClient();

            var request = new HttpRequestMessage();
            Uri siteUrl = new Uri(List.Context.SiteUrl);
            Uri recycleItemUrl = new Uri(siteUrl, string.Format("/_api/web/lists/GetByTitle('{0}')/items({1})/recycle()", List.Title, Id));
            request.Method = HttpMethod.Post;
            request.RequestUri = recycleItemUrl;

            await client.SendAsync(request).ContinueWith((taskwithmsg) =>
            {
                var response = taskwithmsg.Result;
                var responseContentTask = response.Content.ReadAsStringAsync();
                responseContentTask.Wait();
                string resPonseData = responseContentTask.Result;
                return resPonseData;
            });

            return string.Empty;
        }

        public async Task<string> Update()
        {
            HttpClient client = List.Context.GetClient();
            client.DefaultRequestHeaders.Add("IF-MATCH", "*");
            client.DefaultRequestHeaders.Add("X-HTTP-Method", "MERGE");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var request = new HttpRequestMessage();
            Uri siteUrl = new Uri(List.Context.SiteUrl);
            Uri recycleItemUrl = new Uri(siteUrl, string.Format("/_api/web/lists/GetByTitle('{0}')/items({1})", List.Title, Id));

            var valuesDictionary = this.Keys.Select(fieldName =>
            {
                return string.Format("'{0}':'{1}'", fieldName, this[fieldName]);
            });

            string values = string.Join(",", valuesDictionary);

            StringContent UpdateItemCommand = new StringContent("{ " + values + "}", Encoding.UTF8, "application/json");

            await client.PostAsync(recycleItemUrl, UpdateItemCommand).ContinueWith((taskwithmsg) =>
            {
                var response = taskwithmsg.Result;
                var responseContentTask = response.Content.ReadAsStringAsync();
                responseContentTask.Wait();
                string resPonseData = responseContentTask.Result;
                return resPonseData;
            });

            return string.Empty;
        }
    }
}
