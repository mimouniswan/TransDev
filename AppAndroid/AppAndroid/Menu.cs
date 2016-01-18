using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Content.PM;

namespace AppAndroid
{
    [Activity(Label = "TransDev", Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Landscape)]
    public class Menu : Activity
    {
        private List<string> listBus;
        private List<string> listLeft;
        private ListView ListViewBus;
        private ListView ListViewLeft;

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
            listBus.Add("N°1");
            listBus.Add("N°2");
            listBus.Add("N°3");

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
                    StartActivity(typeof(Checkup));
                    break;
                case 1:
                    StartActivity(typeof(Checkup));
                    break;
                case 2:
                    StartActivity(typeof(Checkup));
                    break;
                case 3:
                    StartActivity(typeof(Checkup));
                    break;
            }
        }
    }
}

