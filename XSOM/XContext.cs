using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace o365Auth
{
    public class XContext
    {
        public string SiteUrl { get; set; }
        public CookieContainer Cookies { get; set; }
        public string DigestValue { get; set; }

        public async Task<XList> GetList(string Title)
        {
            HttpClient client = GetClient();

            var request = new HttpRequestMessage();
            Uri siteUrl = new Uri(SiteUrl);
            Uri getListUrl = new Uri(siteUrl, string.Format("/_api/web/lists/GetByTitle('{0}')", Title));
            request.Method = HttpMethod.Get;
            request.RequestUri = getListUrl;
            XList list = null;
            await client.SendAsync(request).ContinueWith((taskwithmsg) =>
            {
                var response = taskwithmsg.Result;

                var responseContentTask = response.Content.ReadAsStringAsync();
                responseContentTask.Wait();
                string resPonseData = responseContentTask.Result;
                list = XList.ParseList(resPonseData);
                list.Context = this;
                return list;
            });

            return list;
        }

        public HttpClient GetClient()
        {
            HttpClientHandler httpHandler = new HttpClientHandler();
            httpHandler.CookieContainer = Cookies;

            HttpClient client = new HttpClient(httpHandler);
            client.DefaultRequestHeaders.Add("X-RequestDigest", DigestValue);
            return client;
        }

        public async Task<string> UploadFile(byte[] image, string relativePath, string uploadFileName)
        {
            string endpointUrl = SiteUrl + String.Format(
               "/_api/web/GetFolderByServerRelativeUrl('{0}')/Files/Add(url='{1}', overwrite=true)",
               relativePath,  //  "/Photos/" 
               uploadFileName); // Photo_123.jpg
               //_api/web/GetFolderByServerRelativeUrl('/Photos/')/Files
               //_api/web/lists/getbytitle('Photos')/items?$select=File/Name&$expand=File
               // /_api/Web/GetFileByServerRelativeUrl('/Photos/Photo_eb756332-b62e-4cd9-8084-034fcb3bb446.jpg')
            

            HttpClientHandler httpHandler = new HttpClientHandler();
            httpHandler.CookieContainer = Cookies;

            HttpClient client = new HttpClient(httpHandler);
            client.DefaultRequestHeaders.Add("X-RequestDigest", DigestValue);

            try
            {
                ByteArrayContent imageBytes = new ByteArrayContent(image);
                var result = await client.PostAsync(endpointUrl, imageBytes);
                return result.Content.ToString();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<XFile> GetFile(string serverRelativePath, string fineName)
        {
            var client = this.GetClient();

            var request = new HttpRequestMessage();
            Uri siteUrl = new Uri(this.SiteUrl);

            Uri getListItemByIdUrl = new Uri(siteUrl, string.Format("/_api/Web/GetFileByServerRelativeUrl('{0}{1}')/ListItemAllFields", serverRelativePath, fineName));
            request.Method = HttpMethod.Get;
            request.RequestUri = getListItemByIdUrl;
            XFile file = null;
            var result = await client.SendAsync(request).ContinueWith((taskwithmsg) =>
            {
                var response = taskwithmsg.Result;
                var responseContentTask = response.Content.ReadAsStringAsync();
                responseContentTask.Wait();
                string resPonseData = responseContentTask.Result;

                file = XFile.ParseFile(resPonseData);
                file.ServerRelativeUrl = serverRelativePath;
                file.Name = fineName;
                //TODO: 
                //listItem.List = this;
                return file;
            });

            //_api/Web/GetFileByServerRelativeUrl('/Photos/Photo_8535919a-7252-46f7-a9aa-d01001a4ee47.jpg')
            //_api/Web/GetFileByServerRelativeUrl('/Photos/Photo_8535919a-7252-46f7-a9aa-d01001a4ee47.jpg')/Author
            //_api/Web/GetFileByServerRelativeUrl('/Photos/Photo_8535919a-7252-46f7-a9aa-d01001a4ee47.jpg')/CheckedOutByUser
            //_api/Web/GetFileByServerRelativeUrl('/Photos/Photo_8535919a-7252-46f7-a9aa-d01001a4ee47.jpg')/EffectiveInformationRightsManagementSettings
            //_api/Web/GetFileByServerRelativeUrl('/Photos/Photo_8535919a-7252-46f7-a9aa-d01001a4ee47.jpg')/InformationRightsManagementSettings
            //_api/Web/GetFileByServerRelativeUrl('/Photos/Photo_8535919a-7252-46f7-a9aa-d01001a4ee47.jpg')/ListItemAllFields
            //_api/Web/GetFileByServerRelativeUrl('/Photos/Photo_8535919a-7252-46f7-a9aa-d01001a4ee47.jpg')/LockedByUser
            //_api/Web/GetFileByServerRelativeUrl('/Photos/Photo_8535919a-7252-46f7-a9aa-d01001a4ee47.jpg')/ModifiedBy
            //_api/Web/GetFileByServerRelativeUrl('/Photos/Photo_8535919a-7252-46f7-a9aa-d01001a4ee47.jpg')/Properties
            //_api/Web/GetFileByServerRelativeUrl('/Photos/Photo_8535919a-7252-46f7-a9aa-d01001a4ee47.jpg')/VersionEvents
            //_api/Web/GetFileByServerRelativeUrl('/Photos/Photo_8535919a-7252-46f7-a9aa-d01001a4ee47.jpg')/Versions

            return result;
        }

        public async Task<XUser> GetCurrentUser()
        {
            HttpClient client = GetClient();

            var request = new HttpRequestMessage();
            Uri siteUrl = new Uri(SiteUrl);
            Uri getCurrentUser = new Uri(siteUrl, "/_api/Web/CurrentUser");
            request.Method = HttpMethod.Get;
            request.RequestUri = getCurrentUser;
            XUser user = null;

            //TODO: add error handling
            await client.SendAsync(request).ContinueWith((taskwithmsg) =>
            {
                var response = taskwithmsg.Result;

                var responseContentTask = response.Content.ReadAsStringAsync();
                responseContentTask.Wait();
                string resPonseData = responseContentTask.Result;
                user = XUser.ParseUser(resPonseData);
                user.Context = this;
                return user;
            });

            return user;
        }
     
    }
}
