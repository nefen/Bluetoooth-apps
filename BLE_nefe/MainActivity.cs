using Android;
using Android.App;
using Android.Bluetooth;
using Android.Bluetooth.LE;
using Android.Content;
using Android.Content.PM;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.CoordinatorLayout.Widget;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Google.Android.Material.Snackbar;
using Plugin.Permissions;
using System;
using System.Collections.Generic;
using System.Threading;
using Xamarin.Essentials;

namespace BLE_nefe
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, Exported = true)]
    public class MainActivity : AppCompatActivity
    {
        /**
        * Id to identify a camera permission request.
        */
        static readonly int REQUEST_BLUETOOTH = 0;
        static readonly int REQUEST_LOCATION = 1;
        static readonly int REQUEST_BLUETOOTH_2=2;

        bool waitForPermissions = true;
        bool permissionsGranted = true;
        CoordinatorLayout rootView;
        public string TAG
        {
            get
            {
                return "BLE_nefen";
            }
        }
        protected BluetoothAdapter bleAdapter;
        private LocationManager locManager;
        private readonly string[] requiredPermissionsBluetooth = new string[] {
                                        Manifest.Permission.BluetoothScan,
                                        Manifest.Permission.BluetoothConnect,
                                        Manifest.Permission.Bluetooth,
                                        Manifest.Permission.BluetoothAdmin,
                                    };
        private readonly string[] requiredPermissions = new string[] {
                                        Manifest.Permission.BluetoothScan,
                                        Manifest.Permission.BluetoothConnect,
                                        Manifest.Permission.Bluetooth,
                                        Manifest.Permission.BluetoothAdmin,
                                        Manifest.Permission.AccessBackgroundLocation,
                                        Manifest.Permission.AccessCoarseLocation,
                                        Manifest.Permission.AccessFineLocation
                                    };
        private readonly string[] requiredPermissionsLocation = new string[] {
                                        Manifest.Permission.AccessBackgroundLocation,
                                        //Manifest.Permission.AccessCoarseLocation,
                                        //Manifest.Permission.AccessFineLocation
                                    };
        //LeDeviceListAdapter mLeDeviceListAdapter;
        //BluetoothManager manager;
        //BluetoothAdapter adapter;
        //BluetoothLeScanner scanner;

        //private BluetoothAdapter mBTA;
        //BluetoothAdapter mBluetoothAdapter;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            var scanButton = FindViewById<Button>(Resource.Id.scanButton);
            scanButton.Enabled = false;

            //var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            //if (status != PermissionStatus.Granted)
            //{
            //    System.Diagnostics.Debugger.Log(0, "Initial", $"LOcation permission denied");


            //    Finish();
            //}
            ActivityCompat.RequestPermissions(this, requiredPermissionsBluetooth, REQUEST_BLUETOOTH);

            // AFter permissions are returned I allow or not the press of the button SCAN
            //while (waitForPermissions)
            //{
            //    Thread.Sleep(500);
            //}
            //if (!permissionsGranted) Finish();

            //rootView = FindViewById<CoordinatorLayout>(Resource.Id.root_view);
            //if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.AccessFineLocation))
            //{
            //    // Provide an additional rationale to the user if the permission was not granted
            //    // and the user would benefit from additional context for the use of the permission.
            //    // For example if the user has previously denied the permission.
            //    Log.Info(TAG, "Displaying location permission rationale to provide additional context.");

            //    var requiredPermissions = new String[] { Manifest.Permission.AccessFineLocation };
            //    Snackbar.Make(rootView,
            //                   Resource.String.permission_location_rationale,
            //                   Snackbar.LengthIndefinite)
            //            .SetAction(Resource.String.ok,v=> ActivityCompat.RequestPermissions(this, requiredPermissions, REQUEST_LOCATION
            //            //new Action<View>(delegate (View obj)
            //            //{
            //            //    ActivityCompat.RequestPermissions(this, requiredPermissions, REQUEST_LOCATION);
            //            //}
            //            )
            //    ).Show();
            //}
            //else
            //{
            //    ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.AccessFineLocation }, REQUEST_LOCATION);
            //}


            //while ((ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != (int)Permission.Granted))
            //    //&& (ContextCompat.CheckSelfPermission(this, Manifest.Permission.BluetoothConnect) != (int)Permission.Granted))
            //{

            //    System.Diagnostics.Debugger.Log(0, "MainActivity","sleeping");

            //    Thread.Sleep(1000);
            //}
            /// I do not know...
            //0 StartActivity(new Intent(BluetoothAdapter.ActionRequestEnable));

            bleAdapter = ((BluetoothManager)GetSystemService(BluetoothService)).Adapter;
            locManager = (LocationManager)GetSystemService(LocationService);
            TextView sensr01 = FindViewById<TextView>(Resource.Id.txtSensor1);

            var newCallback = new BLEScanCallback(sensr01);
            var oldCallBack = new BLELeScanCallBack();


            //0 var intent = new Intent(this, typeof(BleReceiver));

            //0 var pendingIntent = PendingIntent.GetBroadcast(this, 111, intent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Mutable);


            //int requestCode = 1;
            //// Check for permissions
            //if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.BluetoothScan) != (int)Permission.Granted)
            //{
            //    ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.BluetoothScan, Manifest.Permission.BluetoothConnect }, requestCode);
            //}

            var _isScanning = false;


            scanButton.Click += delegate
            {

                if (!IsLocAndBleEnabled())
                    return;

                if (_isScanning || bleAdapter.IsDiscovering)
                {
                    _isScanning = false;
                    scanButton.Text = "Start Scanning";
                    if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                    {
                        bleAdapter.BluetoothLeScanner.StopScan(newCallback);//0 pendingIntent);
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
                        List<ScanFilter> filters = new List<ScanFilter>();

                        //todo : set filter uuid
                        //var filter = new ScanFilter.Builder()
                        //                            .SetServiceUuid(ParcelUuid.FromString(""))
                        //                            .Build();
                        //filters.Add(filter);
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
                                                       //.SetMatchMode(BluetoothScanMatchMode.Aggressive)
                                                       .Build();


                        //System.Diagnostics.Debugger.Log(0, "BLE-OnBatchScanResults", $"Device : skatoules");

                        bleAdapter.BluetoothLeScanner.StartScan(filters, settings, newCallback);//0 pendingIntent);
                        //bleAdapter.BluetoothLeScanner.StartScan(filters, settings, oldCallBack);


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
                    scanButton.Text = "Stop Scanning";
                }

            };






            // Bluetooth Low Energyがサポートされているかのチェック。
            //if (!PackageManager.HasSystemFeature(Android.Content.PM.PackageManager.FeatureBluetoothLe))
            //{
            //    Toast.MakeText(this, Resource.String.ble_not_supported, ToastLength.Short).Show();
            //    Finish();
            //}

            // BluetoothManager,BluetoothAdapter,BluetoothLeScannerをインスタンス化。
            //manager = (BluetoothManager)GetSystemService(BluetoothService);
            //adapter = manager.Adapter;
            //scanner = adapter.BluetoothLeScanner;

            // BluetoothのAdapterが取得できているか＝Bluetoothがサポートされているかのチェック。
            //if (adapter == null)
            //{
            //    Toast.MakeText(this, Resource.String.error_bluetooth_not_supported, ToastLength.Short).Show();
            //    Finish();
            //    return;
            //}
            //var scanCallback = new BluetoothScanCallback();



            //scanButton.Click += (sender, e) =>
            //{
            //    //bleDevices = new List<BleDeviceData>();

            //    //scanCallback.ScanResultEvent += ScanCallback_ScanResultEvent;
            //    //ScanSettings settings = new ScanSettings() { ScanMode= Android.Bluetooth.LE.ScanMode.LowLatency };


            //    scanner.StartScan(  scanCallback);
            //    Thread.Sleep(15000);
            //    scanner.StopScan(scanCallback);
            //    int i = 0;
            //};

            ////listView = FindViewById<ListView>(Resource.Id.deviceList);
            //listView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
            //{
            //    var intent = new Intent(this, typeof(ServiceListActivity));
            //    var sendData = new string[] { bleDevices[e.Position].Name, bleDevices[e.Position].Id };
            //    intent.PutExtra("data", sendData);
            //    StartActivity(intent);
            //};
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

            //var requiredPermissions = new String[] { Manifest.Permission.AccessFineLocation, Manifest.Permission.Bluetooth, Manifest.Permission.BluetoothAdmin, Manifest.Permission.BluetoothPrivileged,
            //                        Manifest.Permission.AccessCoarseLocation,Manifest.Permission.AccessFineLocation};


        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);


            System.Diagnostics.Debugger.Log(requestCode, "OnRequestPermissionsResult " + requestCode, string.Join(", ", permissions));
            System.Diagnostics.Debugger.Log(requestCode, "OnRequestPermissionsResult " + requestCode, string.Join(", ", grantResults));


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
                    var scanButton = FindViewById<Button>(Resource.Id.scanButton);
                    scanButton.Enabled = true;
                }
            }

            if (requestCode == REQUEST_LOCATION)
            {
                permissionsGranted = true;

                foreach (Permission p in grantResults)
                {
                    if (p == Permission.Denied) permissionsGranted = false;
                }

                if (!permissionsGranted)
                    System.Diagnostics.Debugger.Log(1, "Permissions Location", "Some Permissions have been denied");
                else
                    System.Diagnostics.Debugger.Log(1, "Permissions Location", "Permissions Granted");
                if (permissionsGranted)
                {
                    var scanButton = FindViewById<Button>(Resource.Id.scanButton);
                    scanButton.Enabled = true;
                }
            }

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