using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;

namespace AppAndroid
{
    [Activity(Label = "TransDev", Icon = "@drawable/icon")]
    public class Menu : Activity
    {
        
        private List<string> listBus;
        private ListView ListViewBus;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Menu);

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
    }
}

