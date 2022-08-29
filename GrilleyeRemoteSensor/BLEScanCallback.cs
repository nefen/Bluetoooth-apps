using Android.App;
using Android.Bluetooth.LE;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrilleyeRemoteSensor
{

    public class BLEScanCallback : ScanCallback
    {
        private Activity _activity;


        byte[] uuid = new byte[] { 0x0e, 0x76, 0x19, 0xec, 0xa5, 0x92, 0x47, 0x30, 0xba, 0x59, 0xab, 0x9d, 0xab, 0x8f, 0x6c, 0x6d };

        public BLEScanCallback(Activity activity) : base()
        {
            _activity = activity;
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

            System.Diagnostics.Debugger.Log(1, "BLE-OnScanFailed", "0");// JsonConvert.SerializeObject(errorCode));
        }

        public override void OnScanResult([GeneratedEnum] ScanCallbackType callbackType, ScanResult result)
        {
            base.OnScanResult(callbackType, result);
            byte[] scanRecord = result.ScanRecord.GetBytes();
            //System.Diagnostics.Debugger.Log(2, "BLE-OnScanResult", $"Device : {result.Device.Name} {result.Device.Address}:"+scanRecord.ToString());

            // if uuid is ok
            if (scanRecord.Length >= 31 & equals(uuid, scanRecord, 16, 4))
            {
                byte[] deviceId = new byte[8];
                byte[] sTemp = new byte[3];

                sTemp[0] = scanRecord[28];
                sTemp[1] = scanRecord[29];
                sTemp[2] = scanRecord[30];

                string temperature = ToTemperature(sTemp).ToString("0.00");

                for(int i=0;i<8;i++)
                    deviceId[i]=scanRecord[i+20];

                string sDeviceId = BitConverter.ToString(deviceId);


                // Find Corresponding displayed device
                if ((((MainActivity)_activity).Resources.GetString(Resource.String.deviceid)).Equals(((MainActivity)_activity).txtDeviceid1.Text) | sDeviceId.Equals(((MainActivity)_activity).txtDeviceid1.Text))
                {
                    ((MainActivity)_activity).Temperature1.Text = temperature;
                    ((MainActivity)_activity).txtDeviceid1.Text = sDeviceId;
                    if (((MainActivity)_activity).txtDeviceid1.Visibility != ViewStates.Visible)
                    {
                        ((MainActivity)_activity).txtDeviceid1.Visibility = ViewStates.Visible;
                        ((MainActivity)_activity).txtLabel1.Visibility = ViewStates.Visible;
                        ((MainActivity)_activity).txtTempLabel1.Visibility = ViewStates.Visible;
                        ((MainActivity)_activity).Temperature1.Visibility = ViewStates.Visible;
                    }

                }
                else if ((((MainActivity)_activity).Resources.GetString(Resource.String.deviceid)).Equals(((MainActivity)_activity).txtDeviceid2.Text) | sDeviceId.Equals(((MainActivity)_activity).txtDeviceid2.Text)) 
                {
                    ((MainActivity)_activity).Temperature2.Text = temperature;
                    ((MainActivity)_activity).txtDeviceid2.Text = sDeviceId;
                    if (((MainActivity)_activity).txtDeviceid2.Visibility != ViewStates.Visible)
                    {
                        ((MainActivity)_activity).txtDeviceid2.Visibility = ViewStates.Visible;
                        ((MainActivity)_activity).txtLabel2.Visibility = ViewStates.Visible;
                        ((MainActivity)_activity).txtTempLabel2.Visibility = ViewStates.Visible;
                        ((MainActivity)_activity).Temperature2.Visibility = ViewStates.Visible;
                    }

                }
                else if ((((MainActivity)_activity).Resources.GetString(Resource.String.deviceid)).Equals(((MainActivity)_activity).txtDeviceid3.Text) | sDeviceId.Equals(((MainActivity)_activity).txtDeviceid3.Text))
                {
                    ((MainActivity)_activity).Temperature3.Text = temperature;
                    ((MainActivity)_activity).txtDeviceid3.Text = sDeviceId;
                    if (((MainActivity)_activity).txtDeviceid3.Visibility != ViewStates.Visible)
                    {
                        ((MainActivity)_activity).txtDeviceid3.Visibility = ViewStates.Visible;
                        ((MainActivity)_activity).txtLabel3.Visibility = ViewStates.Visible;
                        ((MainActivity)_activity).txtTempLabel3.Visibility = ViewStates.Visible;
                        ((MainActivity)_activity).Temperature3.Visibility = ViewStates.Visible;
                    }
                }
                else if ((((MainActivity)_activity).Resources.GetString(Resource.String.deviceid)).Equals(((MainActivity)_activity).txtDeviceid4.Text) | sDeviceId.Equals(((MainActivity)_activity).txtDeviceid4.Text))
                {
                    ((MainActivity)_activity).Temperature4.Text = temperature;
                    ((MainActivity)_activity).txtDeviceid4.Text = sDeviceId;
                    if (((MainActivity)_activity).txtDeviceid4.Visibility != ViewStates.Visible)
                    {
                        ((MainActivity)_activity).txtDeviceid4.Visibility = ViewStates.Visible;
                        ((MainActivity)_activity).txtLabel4.Visibility = ViewStates.Visible;
                        ((MainActivity)_activity).txtTempLabel4.Visibility = ViewStates.Visible;
                        ((MainActivity)_activity).Temperature4.Visibility = ViewStates.Visible;
                    }
                }
                else
                {
                    System.Diagnostics.Debugger.Log(1, "BLE-OnScanResult", "I got a 5th Device Id or smething different shit");// JsonConvert.SerializeObject(errorCode));
                }

            }
        }

        public bool equals(byte[] a1, byte[] a2, int length, int a2IgnoreFirst)
        {
            for (int i = 0; i < length; i++)
                if (a1[i] != a2[i + a2IgnoreFirst])
                    return false;
            return true;
        }

        public float ToTemperature(byte[] sTemp)
        {
            float result = 0;
            result += ((float)sTemp[2]) / 100.0f;
            result += (float)sTemp[1];
            if (sTemp[0] != 0) result = -result;
            return result;
        }
    }

}