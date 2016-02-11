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
    [Activity(Label = "TransDev", ScreenOrientation = ScreenOrientation.Landscape, MainLauncher = true, Icon = "@drawable/icon")]
    public class Main : Activity    
    {
        private Spinner userSpinner;
        private Button btnMenu;
        List<Controleur> controleurs;

        DBWork _DB = new DBWork();

        protected override void OnResume()
        {
            base.OnResume();

            userSpinner = FindViewById<Spinner>(Resource.Id.spinner1);
            List<string> listSpinner = new List<string>();

            controleurs = _DB.GetControleur();
            foreach (var item in controleurs)
                listSpinner.Add($"{item.Name}");

            ArrayAdapter<string> adapterDriver = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listSpinner);
            userSpinner.Adapter = adapterDriver;
        }

        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource"""""
            SetContentView(Resource.Layout.Main);

            List<string> listSpinner = new List<string>();
            userSpinner = FindViewById<Spinner>(Resource.Id.spinner1);
            btnMenu = FindViewById<Button>(Resource.Id.button1);
            Button btnDB = FindViewById<Button>(Resource.Id.buttonDB);
            Button btnDD = FindViewById<Button>(Resource.Id.buttonDD);

            controleurs = _DB.GetControleur();
            foreach(var item in controleurs)
                listSpinner.Add($"{item.Name}");

            ArrayAdapter<string> adapterDriver = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listSpinner);
            userSpinner.Adapter = adapterDriver;

            btnMenu.Click += delegate
            {
                SharedData.ControleurID = controleurs[userSpinner.SelectedItemPosition].Id;
                SharedData.ControleurName = controleurs[userSpinner.SelectedItemPosition].Name;
                StartActivity(typeof(Menu));
            };

            btnDB.Click += delegate
            {
                //StartActivity(typeof(TestDB));
                DBWork DB = new DBWork();
                var v = DB.GetLastCheck(1);
            };

            btnDD.Click += delegate
            {
                StartActivity(typeof(Checkup));
            };
        }

        private void ListViewBus_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            StartActivity(typeof(Menu));
        }
    }
}