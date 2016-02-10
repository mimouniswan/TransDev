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

namespace AppAndroid.Work
{
    static class SharedData
    {
        //private SharedData() { }
        public static int BusID { get; internal set; }
        public static int BusNumber { get; internal set; }

        public static int DriverID { get; internal set; }
        public static string DriverName { get; internal set; }

        public static int ControleurID { get; internal set; }
        public static string ControleurName { get; internal set; }
    }
}