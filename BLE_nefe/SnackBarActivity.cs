using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.Snackbar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLE_nefe
{
    [Activity(Label = "SnackBarActivity")]
    public class SnackBarActivity : AppCompatActivity
    {
        Button default_snackbar_button;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.snackbar_layout);
            default_snackbar_button = FindViewById<Button>(Resource.Id.default_snackbar_action);
            default_snackbar_button.Click += Default_snackbar_button_Click;
        }

        private void Default_snackbar_button_Click(object sender, System.EventArgs e)
        {
            //Create the Snackbar 
            Snackbar default_snackBar = (Snackbar)Snackbar.Make(default_snackbar_button, "Default SnackBar", Snackbar.LengthIndefinite)
                 .SetDuration(1000);
            //Show the snackbar
            default_snackBar.Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}