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
using AppAndroid.Data;

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

        public static List<TmpCheck> ListCheck { get; internal set; } = new List<TmpCheck>();
        public static List<Incident> ListIncident { get; internal set; } = new List<Incident>();
    }

    public class TmpCheck
    {
        public int X { get; set; }
        public int Y { get; set; }
        /// <summary>
        /// 0 = Cercle, 1 = Carré, 2 = Triangle
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 0 = Léger, 1 = Modérer, 2 = Important
        /// </summary>
        public int Gravite { get; set; }
        /// <summary>
        /// 0 = Droit, 1 = Avant, 2 = Gauche, 3 = Arrière
        /// </summary>
        public int Cote { get; set; }
    }
}