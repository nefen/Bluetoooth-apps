using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DroidBLE
{
   public static class LogManager
    {
        public static void Log(this object obj)
        {
            Console.WriteLine(obj);
        }
    }
}