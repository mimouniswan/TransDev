using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Content.PM;
using AppAndroid.Acti;
using AppAndroid.Work;
using AppAndroid.Data;

namespace AppAndroid
{
    [Activity(Label = "TransDev", Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Landscape)]
    public class Menu : Activity
    {
        private List<string> listBus;
        private List<string> listLeft;
        private ListView ListViewBus;
        private ListView ListViewLeft;

        DBWork _DB = new DBWork();

        protected override void OnResume()
        {
            base.OnResume();

            listBus = new List<string>();
            ListViewBus = FindViewById<ListView>(Resource.Id.listBus);

            List<Bus> bus = _DB.GetBus();
            foreach (var item in bus)
                listBus.Add($"Bus [{item.Color}] N°{item.Number}");

            ArrayAdapter<string> adapterBus = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listBus);
            ListViewBus.Adapter = adapterBus;
        }

        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Menu);

            // Liste de gauche
            listLeft = new List<string>();
            ListViewLeft = FindViewById<ListView>(Resource.Id.listViewLeftMenu);
            listLeft.Add("Ajouter Bus");
            listLeft.Add("Ajouter Controleur");
            listLeft.Add("Ajouter Conducteur");
            listLeft.Add("Historique");

            ArrayAdapter<string> adapterLeft = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listLeft);
            ListViewLeft.Adapter = adapterLeft;
            ListViewLeft.ItemClick += LeftMenuClick;

            // Liste de droite
            listBus = new List<string>();
            ListViewBus = FindViewById<ListView>(Resource.Id.listBus);

            List<Bus> bus = _DB.GetBus();
            foreach(var item in bus)
                listBus.Add($"Bus [{item.Color}] N°{item.Number}");

            ArrayAdapter<string> adapterBus = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listBus);
            ListViewBus.Adapter = adapterBus;
            ListViewBus.ItemClick += ListViewBus_ItemClick;
        }

        private void ListViewBus_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            StartActivity(typeof(Checkup));
        }   

        private void LeftMenuClick(object sender, AdapterView.ItemClickEventArgs ea)
        {
            switch(ea.Id)
            {
                case 0:
                    // Bus
                    StartActivity(typeof(CreaBus));
                    break;
                case 1:
                    // Controleur
                    StartActivity(typeof(CreaContro));
                    break;
                case 2:
                    // Conducteur
                    StartActivity(typeof(CreaConduc));
                    break;
                case 3:
                    // Historique
                    StartActivity(typeof(Checkup));
                    break;
            }
        }
    }
}

