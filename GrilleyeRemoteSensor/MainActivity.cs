using Android;
using Android.App;
using Android.Bluetooth;
using Android.Bluetooth.LE;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Google.Android.Material.BottomNavigation;
using System;
using System.Collections.Generic;

namespace GrilleyeRemoteSensor
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]//, MainLauncher = true)]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        static readonly int REQUEST_BLUETOOTH = 0;
        protected BluetoothAdapter bleAdapter;

        TextView textMessage;

        public TextView txtLabel1, txtLabel2, txtLabel3, txtLabel4;
        public TextView txtTempLabel1, txtTempLabel2, txtTempLabel3, txtTempLabel4;
        public TextView txtDeviceid1, txtDeviceid2, txtDeviceid3, txtDeviceid4;
        public TextView Temperature1, Temperature2, Temperature3, Temperature4;
        Button startButton;

        private readonly string[] requiredPermissionsBluetooth = new string[]
        {
                                        Manifest.Permission.BluetoothScan,
                                        Manifest.Permission.BluetoothConnect,
                                        Manifest.Permission.Bluetooth,
                                        Manifest.Permission.BluetoothAdmin,
        };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            txtLabel1 = FindViewById<TextView>(Resource.Id.label1);
            txtLabel2 = FindViewById<TextView>(Resource.Id.label2);
            txtLabel3 = FindViewById<TextView>(Resource.Id.label3);
            txtLabel4 = FindViewById<TextView>(Resource.Id.label4);
            txtTempLabel1 = FindViewById<TextView>(Resource.Id.templabel1);
            txtTempLabel2 = FindViewById<TextView>(Resource.Id.templabel2);
            txtTempLabel3 = FindViewById<TextView>(Resource.Id.templabel3);
            txtTempLabel4 = FindViewById<TextView>(Resource.Id.templabel4);
            txtDeviceid1 = FindViewById<TextView>(Resource.Id.deviceid1);
            txtDeviceid2 = FindViewById<TextView>(Resource.Id.deviceid2);
            txtDeviceid3 = FindViewById<TextView>(Resource.Id.deviceid3);
            txtDeviceid4 = FindViewById<TextView>(Resource.Id.deviceid4);
            Temperature1 = FindViewById<TextView>(Resource.Id.temperature1);
            Temperature2 = FindViewById<TextView>(Resource.Id.temperature2);
            Temperature3 = FindViewById<TextView>(Resource.Id.temperature3);
            Temperature4 = FindViewById<TextView>(Resource.Id.temperature4);





            startButton = FindViewById<Button>(Resource.Id.startButton);
            startButton.Enabled = false;

            ActivityCompat.RequestPermissions(this, requiredPermissionsBluetooth, REQUEST_BLUETOOTH);

            bleAdapter = ((BluetoothManager)GetSystemService(BluetoothService)).Adapter;
            var newCallback = new BLEScanCallback(this);
            var _isScanning = false;


            startButton.Click += delegate
            {

                if (!IsLocAndBleEnabled())
                    return;

                if (_isScanning || bleAdapter.IsDiscovering)
                {
                    // Stop scanning.. if button is pressed again.
                    _isScanning = false;
                    startButton.Text = "Start Scanning";
                    bleAdapter.BluetoothLeScanner.StopScan(newCallback);//0 pendingIntent);

                }
                else
                {
                    List<ScanFilter> filters = new List<ScanFilter>();
                    String[] names = null;// new String[] { "L1250 Series", "Nordic_LBS"};
                    if (names != null)
                    {
                        foreach (String name in names)
                        {
                            ScanFilter filter = new ScanFilter.Builder()
                                    .SetDeviceName(name).Build();
                            filters.Add(filter);
                        }
                    }
                    var settings = new ScanSettings.Builder()
                                                   .SetScanMode(Android.Bluetooth.LE.ScanMode.LowLatency)
                                                   .Build();
                    bleAdapter.BluetoothLeScanner.StartScan(filters, settings, newCallback);
                    _isScanning = true;
                    startButton.Text = Resources.GetString(Resource.String.stopbutton);
                }

            };






            textMessage = FindViewById<TextView>(Resource.Id.message);
            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);
        }

        bool permissionsGranted = true;

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            if (requestCode == REQUEST_BLUETOOTH)
            {
                permissionsGranted = true;

                foreach (Permission p in grantResults)
                {
                    if (p == Permission.Denied) permissionsGranted = false;
                }

                if (!permissionsGranted)
                    System.Diagnostics.Debugger.Log(1, "Permissions Bluetooth", "Some Permissions have been denied");
                else
                    System.Diagnostics.Debugger.Log(1, "Permissions Bluetooth", "Permissions Granted");

                //ActivityCompat.RequestPermissions(this, requiredPermissionsLocation, REQUEST_LOCATION);

                if (permissionsGranted)
                {
                    startButton.Enabled = true;
                }
            }
        }
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.navigation_home:
                    textMessage.SetText(Resource.String.title_home);
                    return true;
                case Resource.Id.navigation_dashboard:
                    textMessage.SetText(Resource.String.title_dashboard);
                    return true;
                case Resource.Id.navigation_notifications:
                    textMessage.SetText(Resource.String.title_notifications);
                    return true;
            }
            return false;
        }

    private bool IsLocAndBleEnabled()
    {

        int i = 0;
        foreach (var perm in requiredPermissionsBluetooth)
        {
            ++i;
            //var permissionGranted = Android.Support.V4.Content.ContextCompat.CheckSelfPermission(this, perm) == Permission.Granted;

            if (!(ContextCompat.CheckSelfPermission(this, perm) == Permission.Granted))
            {
                Toast.MakeText(this, $"{perm}, required", ToastLength.Long).Show();

                //ActivityCompat.RequestPermissions(this, new string[] { perm }, i);
                ActivityCompat.RequestPermissions(this, requiredPermissionsBluetooth, i);
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

        // I do not use at this moment the location. I just scan and not connect to bluetooth device
        //if (!locManager.IsProviderEnabled(LocationManager.NetworkProvider))
        //{

        //    StartActivity(new Intent(Android.Provider.Settings.ActionLocationSourceSettings));
        //    //locManager.RequestSingleUpdate(LocationManager.KeyProviderEnabled, null);
        //    Toast.MakeText(this, "Kindly enable location in the app setting, and try this action again later", ToastLength.Long).Show();

        //    return false;
        //}

        return true;
    }

    }
}

