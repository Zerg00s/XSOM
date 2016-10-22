using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XSOM
{
    public class XList
    {
        public string Title { get; set; }
        public string AllowContentTypes { get; set; }
        public string BaseTemplate { get; set; }
        public string BaseType { get; set; }
        public string ContentTypesEnabled { get; set; }
        public string Created { get; set; }
        public string DefaultContentApprovalWorkflowId { get; set; }
        public string Description { get; set; }
        public string Direction { get; set; }
        public string DocumentTemplateUrl { get; set; }
        public string DraftVersionVisibility { get; set; }
        public string EnableAttachments { get; set; }
        public string EnableFolderCreation { get; set; }
        public string EnableMinorVersions { get; set; }
        public string EnableModeration { get; set; }
        public string EnableVersioning { get; set; }
        public string EntityTypeName { get; set; }
        public string ForceCheckout { get; set; }
        public string HasExternalDataSource { get; set; }
        public string Hidden { get; set; }
        public string Id { get; set; }
        public string ImageUrl { get; set; }
        public string IrmEnabled { get; set; }
        public string IrmExpire { get; set; }
        public string IrmReject { get; set; }
        public string IsApplicationList { get; set; }
        public string IsCatalog { get; set; }
        public string IsPrivate { get; set; }
        public string ItemCount { get; set; }
        public string LastItemDeletedDate { get; set; }
        public string LastItemModifiedDate { get; set; }
        public string ListItemEntityTypeFullName { get; set; }
        public string MultipleDataList { get; set; }
        public string NoCrawl { get; set; }
        public string ParentWebUrl { get; set; }
        public string ServerTemplateCanCreateFolders { get; set; }
        public string TemplateFeatureId { get; set; }


        public XContext Context { get; set; }

        public static XList ParseList(string resPonseData)
        {
            XElement x = XElement.Parse(resPonseData);

            XList list = new XList
            {
                Title = x.Descendants().Last(xg => xg.Name.LocalName == "Title").Value,
                AllowContentTypes = x.Descendants().Where(xg => xg.Name.LocalName == "AllowContentTypes").First().Value,
                BaseTemplate = x.Descendants().Where(xg => xg.Name.LocalName == "BaseTemplate").First().Value,
                BaseType = x.Descendants().Where(xg => xg.Name.LocalName == "BaseType").First().Value,
                ContentTypesEnabled = x.Descendants().Where(xg => xg.Name.LocalName == "ContentTypesEnabled").First().Value,
                Created = x.Descendants().Where(xg => xg.Name.LocalName == "Created").First().Value,
                DefaultContentApprovalWorkflowId = x.Descendants().Where(xg => xg.Name.LocalName == "DefaultContentApprovalWorkflowId").First().Value,
                Description = x.Descendants().Where(xg => xg.Name.LocalName == "Description").First().Value,
                Direction = x.Descendants().Where(xg => xg.Name.LocalName == "Direction").First().Value,
                DocumentTemplateUrl = x.Descendants().Where(xg => xg.Name.LocalName == "DocumentTemplateUrl").First().Value,
                DraftVersionVisibility = x.Descendants().Where(xg => xg.Name.LocalName == "DraftVersionVisibility").First().Value,
                EnableAttachments = x.Descendants().Where(xg => xg.Name.LocalName == "EnableAttachments").First().Value,
                EnableFolderCreation = x.Descendants().Where(xg => xg.Name.LocalName == "EnableFolderCreation").First().Value,
                EnableMinorVersions = x.Descendants().Where(xg => xg.Name.LocalName == "EnableMinorVersions").First().Value,
                EnableModeration = x.Descendants().Where(xg => xg.Name.LocalName == "EnableModeration").First().Value,
                EnableVersioning = x.Descendants().Where(xg => xg.Name.LocalName == "EnableVersioning").First().Value,
                EntityTypeName = x.Descendants().Where(xg => xg.Name.LocalName == "EntityTypeName").First().Value,
                ForceCheckout = x.Descendants().Where(xg => xg.Name.LocalName == "ForceCheckout").First().Value,
                HasExternalDataSource = x.Descendants().Where(xg => xg.Name.LocalName == "HasExternalDataSource").First().Value,
                Hidden = x.Descendants().Where(xg => xg.Name.LocalName == "Hidden").First().Value,
                Id = x.Descendants().Where(xg => xg.Name.LocalName == "Id").First().Value,
                ImageUrl = x.Descendants().Where(xg => xg.Name.LocalName == "ImageUrl").First().Value,
                IrmEnabled = x.Descendants().Where(xg => xg.Name.LocalName == "IrmEnabled").First().Value,
                IrmExpire = x.Descendants().Where(xg => xg.Name.LocalName == "IrmExpire").First().Value,
                IrmReject = x.Descendants().Where(xg => xg.Name.LocalName == "IrmReject").First().Value,
                IsApplicationList = x.Descendants().Where(xg => xg.Name.LocalName == "IsApplicationList").First().Value,
                IsCatalog = x.Descendants().Where(xg => xg.Name.LocalName == "IsCatalog").First().Value,
                IsPrivate = x.Descendants().Where(xg => xg.Name.LocalName == "IsPrivate").First().Value,
                ItemCount = x.Descendants().Where(xg => xg.Name.LocalName == "ItemCount").First().Value,
                LastItemDeletedDate = x.Descendants().Where(xg => xg.Name.LocalName == "LastItemDeletedDate").First().Value,
                LastItemModifiedDate = x.Descendants().Where(xg => xg.Name.LocalName == "LastItemModifiedDate").First().Value,
                ListItemEntityTypeFullName = x.Descendants().Where(xg => xg.Name.LocalName == "ListItemEntityTypeFullName").First().Value,
                MultipleDataList = x.Descendants().Where(xg => xg.Name.LocalName == "MultipleDataList").First().Value,
                NoCrawl = x.Descendants().Where(xg => xg.Name.LocalName == "NoCrawl").First().Value,
                ParentWebUrl = x.Descendants().Where(xg => xg.Name.LocalName == "ParentWebUrl").First().Value,
                ServerTemplateCanCreateFolders = x.Descendants().Where(xg => xg.Name.LocalName == "ServerTemplateCanCreateFolders").First().Value,
                TemplateFeatureId = x.Descendants().Where(xg => xg.Name.LocalName == "TemplateFeatureId").First().Value,
            };
            return list;
        }


        public async Task<XListItem> GetItemById(int Id)
        {
            var client = Context.GetClient();

            var request = new HttpRequestMessage();
            Uri siteUrl = new Uri(Context.SiteUrl);
            Uri getListItemByIdUrl = new Uri(siteUrl, string.Format("/_api/web/lists/GetByTitle('{0}')/items({1})", Title, Id));
            request.Method = HttpMethod.Get;
            request.RequestUri = getListItemByIdUrl;
            XListItem listItem = null;
            var result = await client.SendAsync(request).ContinueWith((taskwithmsg) =>
            {
                var response = taskwithmsg.Result;
                var responseContentTask = response.Content.ReadAsStringAsync();
                responseContentTask.Wait();
                string resPonseData = responseContentTask.Result;

                listItem = XListItem.ParseListItem(resPonseData);
                listItem.List = this;
                return listItem;
            });

            return result;
        }

        public async Task<List<XListItem>> GetItems()
        {
            var client = Context.GetClient();

            var request = new HttpRequestMessage();
            Uri siteUrl = new Uri(Context.SiteUrl);
            Uri getItemsUrl = new Uri(siteUrl, string.Format("/_api/web/lists/GetByTitle('{0}')/items", Title));
            request.Method = HttpMethod.Get;
            request.RequestUri = getItemsUrl;
            List<XListItem> listItems = null;
            await client.SendAsync(request).ContinueWith((taskwithmsg) =>
            {
                var response = taskwithmsg.Result;

                var responseContentTask = response.Content.ReadAsStringAsync();
                responseContentTask.Wait();
                string resPonseData = responseContentTask.Result;

                XElement element = XElement.Parse(resPonseData);
                listItems = element.Descendants().Where(x => x.Name.LocalName == "entry").Select(
                (xmlElement) =>
                {
                    XListItem listItem = XListItem.ParseListItem(xmlElement.ToString());
                    listItem.List = this;
                    return listItem;
                }).ToList();
            });

            return listItems;
        }

        public async Task<List<XListItem>> GetItems(string CAML)
        {
            var client = Context.GetClient();
            var request = new HttpRequestMessage();
            Uri siteUrl = new Uri(Context.SiteUrl);
            Uri getItemsUrl = new Uri(siteUrl, 
                string.Format("/_api/web/lists/GetByTitle('{0}')/GetItems(query=@v1)?@v1={{\"ViewXml\":\"<View><Query>{1}</Query></View>\"}}", 
                Title, CAML));

            request.Method = HttpMethod.Post;
            request.RequestUri = getItemsUrl;
            List<XListItem> listItems = null;
            await client.SendAsync(request).ContinueWith((taskwithmsg) =>
            {
                var response = taskwithmsg.Result;

                var responseContentTask = response.Content.ReadAsStringAsync();
                responseContentTask.Wait();
                string resPonseData = responseContentTask.Result;

                XElement element = XElement.Parse(resPonseData);

                listItems = element.Descendants().Where(x => x.Name.LocalName == "entry").Select(
                    (xmlElement) =>
                    {
                        XListItem listItem = XListItem.ParseListItem(xmlElement.ToString());
                        listItem.List = this;
                        return listItem;
                    }).ToList();

            });

            return listItems;
        }

    }
}
