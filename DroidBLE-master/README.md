# Xamarin Android Bluetooth BLE
Simple Xamarin Android BLE scanning

Scanning for BLE devices is a very common task/request these days, expecialy with the advent and proliferation of IoT devices.

Also, google has made some profiles and security changes. Some permissions are required for your application to have Bluetooth BLE priviledge.

Below are the required permissions:<br/>
              <p style='color:red'>android.permission.BLUETOOTH<br/> 
              android.permission.BLUETOOTH_ADMIN<br/>
              android.permission.ACCESS_COARSE_LOCATION<br/>
              android.permission.ACCESS_FINE_LOCATION<p/>


The last two permissions are the new additions, for more info ; https://developer.android.com/reference/android/bluetooth/le/BluetoothLeScanner#startScan(java.util.List%3Candroid.bluetooth.le.ScanFilter%3E,%20android.bluetooth.le.ScanSettings,%20android.app.PendingIntent)

# Permissions at runtime
For apps that target Android 5.1 (API level 22) or lower, the permission request occurred when the app is installed. If the user did not grant the permissions, then the app would not be installed. Once the app is installed, there is no way to revoke the permissions except by uninstalling the app.

Starting from Android 6.0 (API level 23), users were given more control over permissions; they can grant or revoke permissions as long as the app is installed on the device.

Despite adding above permissions in the AndroidManifest.xml file, apps are required to check again at the rumtime if those previledges are still available to them.

Below code can be added in OnCreate or OnResume
           
           
           
           private readonly string[] requiredPermissions = new string[] {
                                        Manifest.Permission.AccessFineLocation,
                                        Manifest.Permission.Bluetooth,
                                        Manifest.Permission.BluetoothAdmin,
                                        Manifest.Permission.AccessCoarseLocation,
                                    };
                                    

            ActivityCompat.RequestPermissions(this, requiredPermissions, 10101);


# Location and Bluetooth Services 

Location and Bluetooth servics are required;


            bleAdapter = ((BluetoothManager)GetSystemService(BluetoothService)).Adapter;
            locManager = (LocationManager)GetSystemService(LocationService);
            
Also, before scanning these services need to be checked if available and enabled
       
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


# Scanning
For available scanning options; https://developer.android.com/reference/android/bluetooth/le/BluetoothLeScanner#startScan(java.util.List%3Candroid.bluetooth.le.ScanFilter%3E,%20android.bluetooth.le.ScanSettings,%20android.app.PendingIntent)

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

                        bleAdapter.StopLeScan(oldCallBack);
                    }
