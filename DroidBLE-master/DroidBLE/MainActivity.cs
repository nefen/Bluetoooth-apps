using Android;
using Android.App;
using Android.Bluetooth;
using Android.Bluetooth.LE;
using Android.Content;
using Android.Content.PM;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Widget;
using Plugin.Permissions;
using System;
using System.Collections.Generic;

namespace DroidBLE
{
    [Activity(Label = "DroidBLE", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        protected BluetoothAdapter bleAdapter;
        private LocationManager locManager;
        private readonly string[] requiredPermissions = new string[] {
                                        Manifest.Permission.AccessFineLocation,
                                        Manifest.Permission.Bluetooth,
                                        Manifest.Permission.BluetoothScan,
                                        Manifest.Permission.BluetoothConnect,
                                        Manifest.Permission.BluetoothAdmin,
                                        Manifest.Permission.AccessCoarseLocation,
                                        Manifest.Permission.AccessFineLocation,
                                        Manifest.Permission.AccessBackgroundLocation,
                                    };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.myButton);
            button.Text = "Start Scanning";

            StartActivity(new Intent(BluetoothAdapter.ActionRequestEnable));

            bleAdapter = ((BluetoothManager)GetSystemService(BluetoothService)).Adapter;
            locManager = (LocationManager)GetSystemService(LocationService);



            var newCallback = new BLEScanCallback();
            var oldCallBack = new BLELeScanCallBack();

            var intent = new Intent(this, typeof(BleReceiver));

            var pendingIntent = PendingIntent.GetBroadcast(this, 111, intent, PendingIntentFlags.UpdateCurrent);


            ActivityCompat.RequestPermissions(this, requiredPermissions, 10101);


            var _isScanning = false;

            button.Click += delegate
            {

                if (!IsLocAndBleEnabled())
                    return;

                if (_isScanning || bleAdapter.IsDiscovering)
                {
                    _isScanning = false;
                    button.Text = "Start Scanning";
                    if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                    {
                        bleAdapter.BluetoothLeScanner.StopScan(pendingIntent);
                    }
                    else
                    if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                    {
                        bleAdapter.BluetoothLeScanner.StopScan(newCallback);
                    }
                    else
                    {

#pragma warning disable CS0618 // Type or member is obsolete
                        bleAdapter.StopLeScan(oldCallBack);
#pragma warning restore CS0618 // Type or member is obsolete
                    }
                }
                else
                {

                    if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                    {
                        var filters = new List<ScanFilter>();

                        //todo : set filter uuid
                        //var filter = new ScanFilter.Builder()
                        //                            .SetServiceUuid(ParcelUuid.FromString(""))
                        //                            .Build();
                        //filters.Add(filter);

                        var settings = new ScanSettings.Builder()
                                                       .SetScanMode(Android.Bluetooth.LE.ScanMode.Balanced)
                                                       .Build();



                        bleAdapter.BluetoothLeScanner.StartScan(filters, settings, pendingIntent);


                    }
                    else
                    if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                    {

                        bleAdapter.BluetoothLeScanner.FlushPendingScanResults(newCallback);

                        bleAdapter.BluetoothLeScanner.StartScan(newCallback);

                    }
                    else
                    {

#pragma warning disable CS0618 // Type or member is obsolete
                        bleAdapter.StartLeScan(oldCallBack);
#pragma warning restore CS0618 // Type or member is obsolete
                    }

                    _isScanning = true;
                    button.Text = "Stop Scanning";
                }

            };


        }

        private bool IsLocAndBleEnabled()
        {

            int i = 0;
            foreach (var perm in requiredPermissions)
            {
                ++i;
                //var permissionGranted = Android.Support.V4.Content.ContextCompat.CheckSelfPermission(this, perm) == Permission.Granted;

                if (!(Android.Support.V4.Content.ContextCompat.CheckSelfPermission(this, perm) == Permission.Granted))
                {
                    Toast.MakeText(this, $"{perm}, required", ToastLength.Long).Show();

                    //ActivityCompat.RequestPermissions(this, new string[] { perm }, i);
                    ActivityCompat.RequestPermissions(this, requiredPermissions, i);
                    return false;
                }
            }

            if (!bleAdapter.IsEnabled)
            {
                Intent enableBtIntent = new Intent(BluetoothAdapter.ActionRequestEnable);

                StartActivity(enableBtIntent);
                Toast.MakeText(this, "Kindly enable bluetooth in the app setting, and try this action again later", ToastLength.Long).Show();

                return false;
            }


            if (!locManager.IsProviderEnabled(LocationManager.NetworkProvider))
            {

                StartActivity(new Intent(Android.Provider.Settings.ActionLocationSourceSettings));
                //locManager.RequestSingleUpdate(LocationManager.KeyProviderEnabled, null);
                Toast.MakeText(this, "Kindly enable location in the app setting, and try this action again later", ToastLength.Long).Show();

                return false;
            }

            return true;
        }

        protected override void OnResume()
        {
            base.OnResume();

            var requiredPermissions = new String[] { Manifest.Permission.AccessFineLocation, Manifest.Permission.Bluetooth, Manifest.Permission.BluetoothAdmin, Manifest.Permission.BluetoothPrivileged,
                                    Manifest.Permission.AccessCoarseLocation,Manifest.Permission.AccessFineLocation};


        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);


            System.Diagnostics.Debugger.Log(requestCode, "OnRequestPermissionsResult " + requestCode, string.Join(", ", permissions));
            System.Diagnostics.Debugger.Log(requestCode, "OnRequestPermissionsResult " + requestCode, string.Join(", ", grantResults));
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            System.Diagnostics.Debugger.Log(11, "BLE-INTENT", intent.Action);

        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            System.Diagnostics.Debugger.Log(11, "BLE-OnActivityResult", data.Action);
        }

    }
}

