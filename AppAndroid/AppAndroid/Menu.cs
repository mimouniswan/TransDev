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
using Android.Graphics;

namespace AppAndroid
{
    [Activity(Label = "TransDev", Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Landscape)]
    public class Menu : Activity
    {
        private List<Bus> listBus;
        private List<Conducteur> listConduc;
        private List<string> listLeft;
        private ListView ListViewBus;
        private ListView ListViewConduc;
        private ListView ListViewLeft;

        DBWork _DB = new DBWork();

        protected override void OnResume()
        {
            base.OnResume();
            ListViewConduc.Selected = false;
            ListViewBus.Selected = false;

            listBus = new List<Bus>();
            ListViewBus = FindViewById<ListView>(Resource.Id.listBus);

            List<Bus> bus = _DB.GetBus();
            foreach (var item in bus)
                listBus.Add(item);

            ArrayAdapter<Bus> adapterBus = new ArrayAdapter<Bus>(this, Android.Resource.Layout.SimpleListItem1, listBus);
            ListViewBus.Adapter = adapterBus;

            listConduc = new List<Conducteur>();
            ListViewConduc = FindViewById<ListView>(Resource.Id.listConduc);
            List<Conducteur> conduc = _DB.GetConducteur();

            foreach (var item in conduc)
                listConduc.Add(item);

            ArrayAdapter<Conducteur> adapterConduc = new ArrayAdapter<Conducteur>(this, Android.Resource.Layout.SimpleListItem1, listConduc);
            ListViewConduc.Adapter = adapterConduc;
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
            listLeft.Add("Gestion Bus");
            listLeft.Add("Gestion Controleur");
            listLeft.Add("Gestion Conducteur");
            listLeft.Add("Historique");

            ArrayAdapter<string> adapterLeft = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listLeft);
            ListViewLeft.Adapter = adapterLeft;
            ListViewLeft.ItemClick += LeftMenuClick;

            // Liste des bus à selectionner
            listBus = new List<Bus>();
            ListViewBus = FindViewById<ListView>(Resource.Id.listBus);
            List<Bus> bus = _DB.GetBus();

            foreach (var item in bus)
                listBus.Add(item);
            ArrayAdapter<Bus> adapterBus = new ArrayAdapter<Bus>(this, Android.Resource.Layout.SimpleListItem1, listBus);

            ListViewBus.Adapter = adapterBus;
            ListViewBus.ItemClick += ListViewBus_ItemClick;


            // Liste des conducteurs à selectionner
            listConduc = new List<Conducteur>();
            ListViewConduc = FindViewById<ListView>(Resource.Id.listConduc);
            List<Conducteur> conduc = _DB.GetConducteur();

            foreach (var item in conduc)
                listConduc.Add(item);
            ArrayAdapter<Conducteur> adapterConduc = new ArrayAdapter<Conducteur>(this, Android.Resource.Layout.SimpleListItem1, listConduc);

            ListViewConduc.Adapter = adapterConduc;
            ListViewConduc.ItemClick += ListViewConduc_ItemClick;
        }

        private void ListViewConduc_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            SharedData.DriverName = listConduc[e.Position].Name;
            SharedData.DriverID = listConduc[e.Position].Id;

            if (ListViewBus.Selected == true)
            {
                StartActivity(typeof(Checkup));
            }
            else
            {
                ListViewConduc.Selected = true;
            }
        }

        private void ListViewBus_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            SharedData.BusNumber = listBus[e.Position].Number;
            SharedData.BusID = listBus[e.Position].Id;

            if (ListViewConduc.Selected == true)
            {
                StartActivity(typeof(Checkup));
            }
            else
            {
                ListViewBus.Selected = true;
            }
        }

        private void LeftMenuClick(object sender, AdapterView.ItemClickEventArgs ea)
        {
            switch (ea.Id)
            {
                case 0:
                    // Gestion des Bus
                    StartActivity(typeof(CreaBus));
                    break;
                case 1:
                    // Gestion des Controleurs
                    StartActivity(typeof(CreaContro));
                    break;
                case 2:
                    // Gestion des Conducteurs
                    StartActivity(typeof(CreaConduc));
                    break;
                case 3:
                    // Historique
                    StartActivity(typeof(HistIncident));
                    break;
            }
        }
    }
}

