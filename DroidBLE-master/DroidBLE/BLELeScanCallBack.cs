using Android.Bluetooth;
using Newtonsoft.Json;
using System.Linq;

namespace DroidBLE
{
    public class BLELeScanCallBack : Java.Lang.Object, BluetoothAdapter.ILeScanCallback
    {

       

        public void OnLeScan(BluetoothDevice device, int rssi, byte[] scanRecord)
        {
            var record = string.Join(",", scanRecord.Select(s => s.ToString("X2")).ToArray());
            System.Diagnostics.Debugger.Log(3, "BLE-OnLeScan-ScanRecord", record);

            System.Diagnostics.Debugger.Log(2, "BLE-OnLeScan", device.ToString());

        }
    }
}

