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
using AppAndroid.Work;
using System.Threading;
using AppAndroid.Data;
using System.IO;

namespace AppAndroid
{
    [Activity(Label = "TestDB")]
    public class TestDB : Activity
    {
        protected TextView _RessourceText { set; get; }
        protected TextView _ResultText { set; get; }
        protected ProgressBar _TestProgressBar { set; get; }
        protected DBWork _DBSQLite { set; get; }

        private void InitDB()
        {
            _DBSQLite = new DBWork();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            InitDB();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.TestDB);

            // Get our button from the layout resource,
            // and attach an event to it
            Button LaunchButton1 = FindViewById<Button>(Resource.Id.LaunchButtonDB1);
            Button LaunchButton2 = FindViewById<Button>(Resource.Id.LaunchButtonDB2);
            _ResultText = FindViewById<TextView>(Resource.Id.ResultTextDB);
            _TestProgressBar = FindViewById<ProgressBar>(Resource.Id.TestProgressBarDB);

            _ResultText.Text = "";

            // Ce que faront les boutons
            // Création
            LaunchButton1.Click += delegate {
                _ResultText.Text = "";
                _TestProgressBar.Progress = 0;

                new Thread(new ThreadStart(() => {
                    string[] s = _DBSQLite.DBCreateDB();

                    RunOnUiThread(() => { _ResultText.Text = s[1]; });
                })).Start();
            };

            LaunchButton2.Click += delegate {
                _ResultText.Text = "";
                _TestProgressBar.Progress = 0;

                new Thread(new ThreadStart(() => {
                    string[] s = _DBSQLite.DBSelectConducteur(1);

                    RunOnUiThread(() => { _ResultText.Text = s[1]; });

                    ////////////////////////////////

                    Java.IO.File z = Android.OS.Environment.ExternalStorageDirectory;
                    Java.IO.File f = new Java.IO.File(z.Path + "/Android/data/AppAndroid.AppAndroid/files/Fichier.txt");

                    using (var streamWriter = new StreamWriter(f.Path, false))
                    {
                        streamWriter.WriteLine(DateTime.UtcNow);
                    }

                    using (var streamReader = new StreamReader(f.Path))
                    {
                        string content = streamReader.ReadToEnd();
                        System.Diagnostics.Debug.WriteLine(content);

                        RunOnUiThread(() => { _ResultText.Text += $"Contenu fichier :\n{content}\n"; });
                    }
                    ////////////////////////////////
                })).Start();
            };
        }
    }
}