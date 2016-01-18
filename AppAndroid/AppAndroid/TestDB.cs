using System;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using AppAndroid.Work;
using System.Threading;
using System.Collections.Generic;
using AppAndroid.Data;
using Android.Content.PM;

namespace AppAndroid
{
    [Activity(Label = "TestDB", ScreenOrientation = ScreenOrientation.Landscape)]
    public class TestDB : Activity
    {
        protected TextView _RessourceText { set; get; }
        protected TextView _ResultText { set; get; }
        protected EditText _EditText { set; get; }
        protected ProgressBar _TestProgressBar { set; get; }
        protected DBWork _DBSQLite { set; get; }

        private void InitDB()
        {
            _DBSQLite = new DBWork();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

            InitDB();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.TestDB);

            // Get our button from the layout resource,
            // and attach an event to it
            Button LaunchButtonReinit = FindViewById<Button>(Resource.Id.LaunchButtonDB1);
            Button LaunchButtonCreaConduc = FindViewById<Button>(Resource.Id.LaunchButtonDB2);
            Button LaunchButtonCreaContro = FindViewById<Button>(Resource.Id.LaunchButtonDB3);
            Button LaunchButtonCreaBus = FindViewById<Button>(Resource.Id.LaunchButtonDB4);
            Button LaunchButtonCreaCheck = FindViewById<Button>(Resource.Id.LaunchButtonDB5);
            Button LaunchButtonSelConduc = FindViewById<Button>(Resource.Id.LaunchButtonDB6);
            Button LaunchButtonSelControl = FindViewById<Button>(Resource.Id.LaunchButtonDB7);
            Button LaunchButtonSelBus = FindViewById<Button>(Resource.Id.LaunchButtonDB8);
            Button LaunchButtonSelCheck = FindViewById<Button>(Resource.Id.LaunchButtonDB9);

            _EditText = FindViewById<EditText>(Resource.Id.TextBoxDB);
            _ResultText = FindViewById<TextView>(Resource.Id.ResultTextDB);
            _TestProgressBar = FindViewById<ProgressBar>(Resource.Id.TestProgressBarDB);

            _ResultText.Text = "";

            // Ce que faront les boutons
            // Création
            LaunchButtonReinit.Click += delegate
            {
                _ResultText.Text = "";
                _TestProgressBar.Progress = 0;

                new Thread(new ThreadStart(() =>
                {
                    string s = _DBSQLite.DBCreateDB();

                    RunOnUiThread(() => { _ResultText.Text = s; });
                })).Start();
            };

            LaunchButtonCreaConduc.Click += delegate
            {
                _ResultText.Text = "";
                _TestProgressBar.Progress = 0;

                new Thread(new ThreadStart(() =>
                {
                    string s = _DBSQLite.DBInsertConducteur("Cond - Jean-Gérard", "zef");

                    RunOnUiThread(() => { _ResultText.Text = s; });
                })).Start();
            };

            LaunchButtonCreaContro.Click += delegate
            {
                _ResultText.Text = "";
                _TestProgressBar.Progress = 0;

                new Thread(new ThreadStart(() =>
                {
                    string s = _DBSQLite.DBInsertControleur("Contr - Jean-Gérard");

                    RunOnUiThread(() => { _ResultText.Text = s; });
                })).Start();
            };

            LaunchButtonCreaBus.Click += delegate
            {
                _ResultText.Text = "";
                _TestProgressBar.Progress = 0;

                new Thread(new ThreadStart(() =>
                {
                    string s = _DBSQLite.DBInsertBus(1337, "Blouge");

                    RunOnUiThread(() => { _ResultText.Text = s; });
                })).Start();
            };

            LaunchButtonCreaCheck.Click += delegate
            {
                _ResultText.Text = "";
                _TestProgressBar.Progress = 0;

                new Thread(new ThreadStart(() =>
                {
                    List<Incident> list = new List<Incident>();
                    list.Add(_DBSQLite.GetIncident(1, 1, "Type", 1, 1, DateTime.Now.ToString(), "Rien non ?", 0, 0, "Image"));
                    list.Add(_DBSQLite.GetIncident(1, 1, "Type", 1, 1, DateTime.Now.ToString(), "Rien non ?", 0, 0, "Image"));

                    string s = _DBSQLite.DBInsertCheck(1, 1, 1, list);

                    RunOnUiThread(() => { _ResultText.Text = s; });
                })).Start();
            };

            LaunchButtonSelConduc.Click += delegate
            {
                _DBSQLite.DBDeleteCheck(1);
                _ResultText.Text = "";
                _TestProgressBar.Progress = 0;

                new Thread(new ThreadStart(() =>
                {
                    string s = _DBSQLite.DBSelectConducteur(int.Parse(_GetEditTextValue()));

                    RunOnUiThread(() => { _ResultText.Text = s; });
                })).Start();
            };

            LaunchButtonSelControl.Click += delegate
            {
                _ResultText.Text = "";
                _TestProgressBar.Progress = 0;

                new Thread(new ThreadStart(() =>
                {
                    string s = _DBSQLite.DBSelectControleur(int.Parse(_GetEditTextValue()));

                    RunOnUiThread(() => { _ResultText.Text = s; });
                })).Start();
            };

            LaunchButtonSelBus.Click += delegate
            {
                _ResultText.Text = "";
                _TestProgressBar.Progress = 0;

                new Thread(new ThreadStart(() =>
                {
                    string s = _DBSQLite.DBSelectBus(int.Parse(_GetEditTextValue()));

                    RunOnUiThread(() => { _ResultText.Text = s; });
                })).Start();
            };

            LaunchButtonSelCheck.Click += delegate
            {
                _ResultText.Text = "";
                _TestProgressBar.Progress = 0;

                new Thread(new ThreadStart(() =>
                {
                    string s = _DBSQLite.DBSelectCheck(int.Parse(_GetEditTextValue()));

                    RunOnUiThread(() => { _ResultText.Text = s; });
                })).Start();
            };
        }

        private string _GetEditTextValue()
        {
            string n = "";
            if (_EditText.Text.Equals(String.Empty))
                n = "0";
            else
                n = _EditText.Text;

            return n;
        }
    }
}