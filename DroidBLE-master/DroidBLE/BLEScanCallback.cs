using Android.Bluetooth.LE;
using Android.Runtime;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DroidBLE
{
    public class BLEScanCallback : ScanCallback
    {
        
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

            System.Diagnostics.Debugger.Log(2, "BLE-OnScanResult", $"Device : {result.Device.Name} {result.Device.Address}");
        }
    }
}

