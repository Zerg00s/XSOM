using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using AuthAsyncWp.Resources;
using System.Threading.Tasks;
using Microsoft.Phone.Tasks;
using System.IO;

namespace AuthAsyncWp
{
    public partial class MainPage : PhoneApplicationPage
    {

        static string siteUrl = "https://jolera365.sharepoint.com/teams/LCBO/";
        static string Login = "dmolodtsov@jolera.com";
        static string password = "LitMemGu3";

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            cameraCaptureTask = new CameraCaptureTask();
            cameraCaptureTask.Completed += new EventHandler<PhotoResult>(cameraCaptureTask_Completed);

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        Authenticator a = null;
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            btnPhoto.Visibility = System.Windows.Visibility.Collapsed;
            progressBar.Text = "Connecting to SharePoint...";
            if (a == null)
            {
                a = new Authenticator(Login, password, siteUrl);
            }
            await a.RequestCertificateFromMicrosoft();
            string g = a._certificate;

            g  = await a.RequestTokenFromAdfsSTS();
            

            g = await a.GetSharePointToken();


            g = await a.SetCookies();

            g = await a.GetDigestValue();

            try
            {
               
                cameraCaptureTask.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                
            }
        }

        CameraCaptureTask cameraCaptureTask;

        async void cameraCaptureTask_Completed(object sender, PhotoResult e)
        {
            progressBar.Text = "Uploading the picture...";
            try
            {
                if (e.TaskResult == TaskResult.OK)
                {
                    // MessageBox.Show(e.ChosenPhoto.Length.ToString());
                    System.Windows.Media.Imaging.BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage();
                    bmp.SetSource(e.ChosenPhoto);
                    photoImage.Source = bmp;

                    byte[] imageBytes;
                    using (BinaryReader br = new BinaryReader(e.ChosenPhoto))
                    {
                        e.ChosenPhoto.Position = 0;
                        imageBytes = br.ReadBytes(Convert.ToInt32(e.ChosenPhoto.Length));
                    }

                    await a.UploadFile(imageBytes, "Photos", "/teams/LCBO/Photos/", string.Format("Photo_{0}.jpg", Guid.NewGuid()));
                    btnPhoto.Visibility = System.Windows.Visibility.Visible;
                    progressBar.Text = "Uploaded!";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}