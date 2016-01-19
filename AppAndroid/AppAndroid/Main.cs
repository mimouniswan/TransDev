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

namespace AppAndroid
{
    [Activity(Label = "TransDev", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Landscape)]
    public class Main : Activity
    {
        private Spinner userSpinner;
        private Button btnMenu;
        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource"""""
            SetContentView(Resource.Layout.Main);

            List<String> listSpinner = new List<String>();
            userSpinner = FindViewById<Spinner>(Resource.Id.spinner1);
            btnMenu = FindViewById<Button>(Resource.Id.button1);
            Button btnDB = FindViewById<Button>(Resource.Id.buttonDB);

            listSpinner.Add("Amblard");
            listSpinner.Add("Mimouni");
            listSpinner.Add("Dupont");

            ArrayAdapter<string> adapterDriver = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listSpinner);
            userSpinner.Adapter = adapterDriver;

            btnMenu.Click += delegate
            {
                StartActivity(typeof(Menu));

            };

            btnDB.Click += delegate
            {
                StartActivity(typeof(TestDB));
            };
        }

        private void ListViewBus_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            StartActivity(typeof(Menu));
        }
    }
}