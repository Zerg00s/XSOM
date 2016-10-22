using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Data;
using System.Linq;
using O365App;
using Android.Graphics;
using Android.Provider;
using Uri = Android.Net.Uri;
using System.Collections.Generic;
using Android.Content.PM;
using Java.IO;
using o365Auth;

namespace o365App
{
    public static class App
    {
        public static Java.IO.File _file;
        public static Java.IO.File _dir;
        public static Bitmap bitmap;
    }


    [Activity(Label = "LCBO App", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public static EditText txtUrl = null;
        EditText txtLogin = null;
        EditText txtPass = null;
        public static EditText txtToken = null;
        EditText txtCookieFedAuth = null;
        EditText txtCookiertFa = null;
        public static EditText txtFormDigest = null;
        EditText txtRestResponse = null;
        ImageView _imageView;

        public static Bitmap bitmap;


        private void CreateDirectoryForPictures()
        {
            App._dir = new Java.IO.File(
               Android.OS.Environment.GetExternalStoragePublicDirectory(
                      Android.OS.Environment.DirectoryPictures), "CameraAppDemo");
            if (!App._dir.Exists())
            {
                App._dir.Mkdirs();
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            txtUrl = FindViewById<EditText>(Resource.Id.txtUrl);
            txtLogin = FindViewById<EditText>(Resource.Id.txtLogin);
            txtPass = FindViewById<EditText>(Resource.Id.txtPass);

            // Button buttonPhotos = FindViewById<Button>(Resource.Id.btnPhotos);
            // buttonPhotos.Click += (x,y) => { SetContentView(Resource.Layout.Photos); };

            if (IsThereAnAppToTakePictures())
            {
                CreateDirectoryForPictures();

                Button button = FindViewById<Button>(Resource.Id.btnTakePhoto);
                _imageView = FindViewById<ImageView>(Resource.Id.imageView);
                button.Click += TakeAPicture;
            }


        }

        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities =
                 PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }
        private void TakeAPicture(object sender, EventArgs eventArgs)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);

            string photoTitle = string.Format("myPhoto_{0}.jpg", Guid.NewGuid());
            App._file = new Java.IO.File(App._dir, photoTitle);

            intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(App._file));

            StartActivityForResult(intent, 0);

        }

        protected override async void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            if (App._file != null)
            {
                Uri contentUri = Uri.FromFile(App._file);
                mediaScanIntent.SetData(contentUri);
                SendBroadcast(mediaScanIntent);

                // Display in ImageView. We will resize the bitmap to fit the display
                // Loading the full sized image will consume to much memory 
                // and cause the application to crash.

                int height = Resources.DisplayMetrics.HeightPixels;
                int width = _imageView.Height;
                App.bitmap = App._file.Path.LoadAndResizeBitmap(width, height);


                if (App.bitmap != null)
                {
                    _imageView.SetImageBitmap(App.bitmap);
                    App.bitmap = null;
                }

                // Dispose of the Java side bitmap.
                GC.Collect();

                Authenticator authenticator = new Authenticator(txtLogin.Text, txtPass.Text, txtUrl.Text);
                string debug = null;
                await authenticator.InitializeAuth();

                XContext context = await authenticator.GetCookiesAndDigest();

              
                byte[] bytes = System.IO.File.ReadAllBytes(App._file.Path);
                debug = await authenticator.UploadFile(bytes, "Photos", "/teams/LCBO/Photos/", App._file.Name);
               System.Console.WriteLine(debug);
            }
        }


    }
}

