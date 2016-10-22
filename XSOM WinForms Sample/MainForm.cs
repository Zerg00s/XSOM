using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using XSOM;

namespace FinFormsSample
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        static string siteUrl = "https://zergoos.sharepoint.com/";
        static string Login = "Zergoos@Zergoos.onmicrosoft.com";
        static string password = "somepass";


        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private async void btnGetRealmInfo_Click(object sender, EventArgs e)
        {
            Authenticator aithenticator = new Authenticator(Login, password, siteUrl);
            await aithenticator.InitializeAuth();
				XContext context = await aithenticator.GetCookiesAndDigest();
            XUser user = await context.GetCurrentUser();
            XList list = await context.GetList("Tests");

            //XListItem listItem = await list.GetItemById(23);
            //string caml = "<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>TestUpdated</Value></Eq></Where>";
            string currentUserCaml = "<Where><Eq><FieldRef Name='Author' LookupId='True' /><Value Type='Integer'><UserID /></Value></Eq></Where>";
            List<XListItem> listItems = await list.GetItems(currentUserCaml);
            //List<XListItem> listItems = await list.GetItems();
            //await listItems[0].Delete();
            //await listItems[0].Recycle();
            //XListItem item = listItems[0];
            //item["Title"] = "new title";
            //await item.Update();

            //Upload a photo:
            byte[] imageBytes = File.ReadAllBytes("photo.jpg");
            string destinationFileName = string.Format("Photo_{0}.jpg", Guid.NewGuid());
            string result = await context.UploadFile(imageBytes, "/Photos/", destinationFileName);
            var file = await context.GetFile("/Photos/", destinationFileName);


        }
    }
}
