using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Bluetooth;
using Android.Bluetooth.LE;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace BLE_nefe
{
    [BroadcastReceiver(Enabled = true, Exported =true)]
    [IntentFilter(new[] { BluetoothDevice.ActionFound, BluetoothDevice .ActionUuid, BluetoothDevice .ExtraRssi})]
    public class BleReceiver : BroadcastReceiver
    {

        public override void OnReceive(Context context, Intent intent)
        {
            System.Diagnostics.Debugger.Log(21, "BLE-BleReceiver", intent.Action);

            ///
           /*
            *You are after these three keys "android.bluetooth.le.extra.CALLBACK_TYPE", "android.bluetooth.le.extra.LIST_SCAN_RESULT" and "android.bluetooth.le.extra.ERROR_CODE"
            */
            foreach (var key in intent?.Extras?.KeySet())
            {
                var obj = intent.Extras.GetSerializable(key);

                //convert obj to appropriate Object - IList<ScanResult>, ScanFailure, or ScanCallbackType based on the key

                //
                System.Diagnostics.Debugger.Log(21, key, obj.ToString());
            }
        }
    }
}