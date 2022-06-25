using Android.Bluetooth.LE;
using Android.Runtime;
using System.Collections.Generic;
using Newtonsoft.Json;
using Android.Widget;

namespace BLE_nefe
{
    public class BLEScanCallback : ScanCallback
    {
        private TextView _sensor1;
        public BLEScanCallback(TextView sensor1) : base()
        {
            _sensor1 = sensor1;
        }



        public override void OnBatchScanResults(IList<ScanResult> results)
        {
            base.OnBatchScanResults(results);

            foreach (var result in results)
            {
                System.Diagnostics.Debugger.Log(0, "BLE-OnBatchScanResults", $"Device : {result.Device.Name} {result.Device.Address}");
            }
        }
        public override void OnScanFailed([GeneratedEnum] ScanFailure errorCode)
        {
            base.OnScanFailed(errorCode);

            System.Diagnostics.Debugger.Log(1, "BLE-OnScanFailed", JsonConvert.SerializeObject(errorCode));
        }

        public override void OnScanResult([GeneratedEnum] ScanCallbackType callbackType, ScanResult result)
        {
            base.OnScanResult(callbackType, result);
            byte[] scanRecord = result.ScanRecord.GetBytes();
            //System.Diagnostics.Debugger.Log(2, "BLE-OnScanResult", $"Device : {result.Device.Name} {result.Device.Address}:"+scanRecord.ToString());
            if (scanRecord[5] == 82)
            {
                System.Diagnostics.Debugger.Log(2, "Sensor Data", scanRecord[4].ToString("x"));
                _sensor1.Text = scanRecord[4].ToString("x");
            }; 
                //for (int i = 0; i < scanRecord.Length; i++)
                //    System.Diagnostics.Debugger.Log(2, "BLE-OnScanResult", scanRecord[i].ToString("x"));

        }
    }
}

