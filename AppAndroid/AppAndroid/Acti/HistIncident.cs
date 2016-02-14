
using Android.App;
using Android.OS;
using Android.Views;
using Android.Content.PM;
using Android.Widget;
using System.Collections.Generic;
using AppAndroid.Work;
using Android.Graphics;
using System.Linq;
using System;

namespace AppAndroid.Acti
{
    [Activity(Label = "HistIncident", ScreenOrientation = ScreenOrientation.Landscape)]
    public class HistIncident : Activity
    {
        private TextView _TVNumber, _TVConduc, _TVMaJ;
        private TableLayout _Table;
        private TableRow _Row;
        TextView _Cell1, _Cell2, _Cell3, _Cell4;
        private bool _TriNum, _TriCond, _TriMaJ;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.HistIncident);

            DBWork DB = new DBWork();

            _Table = FindViewById<TableLayout>(Resource.Id.TableLayoutHistInc);
            _Table.SetBackgroundColor(Color.Rgb(14, 14, 14));

            _TVNumber = FindViewById<TextView>(Resource.Id.HeadTableHistIncNum);
            _TVConduc = FindViewById<TextView>(Resource.Id.HeadTableHistIncCond);
            _TVMaJ = FindViewById<TextView>(Resource.Id.HeadTableHistIncMaJ);

            _TriNum = true; _TriCond = true; _TriMaJ = false; 

            // Récupération historique des incidents
            List<string[]> incidents = DB.GetHistIncident(true);

            FillTable(incidents);

            _TVNumber.Click += delegate {
                List<string[]> orderTable = new List<string[]>();

                if (_TriNum)
                {
                    orderTable = incidents.OrderBy(n => int.Parse(n[0])).ToList();
                    _TriNum = false;
                }
                else
                {
                    orderTable = incidents.OrderByDescending(n => int.Parse(n[0])).ToList();
                    _TriNum = true;
                }

                _Table.RemoveViews(1, orderTable.Count);
                _TriCond = true; _TriMaJ = false;

                FillTable(orderTable);
            };

            _TVConduc.Click += delegate {
                List<string[]> orderTable = new List<string[]>();

                if (_TriCond)
                {
                    orderTable = incidents.OrderBy(n => n[2]).ToList();
                    _TriCond = false;
                }
                else
                {
                    orderTable = incidents.OrderByDescending(n => n[2]).ToList();
                    _TriCond = true;
                }

                _Table.RemoveViews(1, orderTable.Count);
                _TriNum = true; _TriMaJ = false;

                FillTable(orderTable);
            };

            _TVMaJ.Click += delegate {
                List<string[]> orderTable = new List<string[]>();

                if (_TriMaJ)
                {
                    orderTable = incidents.OrderBy(n => n[3]).ToList();
                    _TriMaJ = false;
                }
                else
                {
                    orderTable = incidents.OrderByDescending(n => n[3]).ToList();
                    _TriMaJ = true;
                }

                _Table.RemoveViews(1, orderTable.Count);
                _TriNum = true; _TriCond = true;

                FillTable(orderTable);
            };
        }

        private void FillTable(List<string[]> table)
        {
            foreach (var item in table)
            {
                _Row = new TableRow(this);
                _Row.SetBackgroundColor(Color.Rgb(28, 28, 28));

                _Cell1 = new TextView(this);
                _Cell1.Text = item[0];
                _Cell1.Gravity = GravityFlags.Center;
                _Cell1.LayoutParameters = new TableRow.LayoutParams(1);

                _Cell2 = new TextView(this);
                _Cell2.Text = item[1];
                _Cell2.Gravity = GravityFlags.Center;
                _Cell2.LayoutParameters = new TableRow.LayoutParams(2);

                _Cell3 = new TextView(this);
                _Cell3.Text = item[2];
                _Cell3.Gravity = GravityFlags.Center;
                _Cell3.LayoutParameters = new TableRow.LayoutParams(3);

                _Cell4 = new TextView(this);
                _Cell4.Text = item[3];
                _Cell4.Gravity = GravityFlags.Center;
                _Cell4.LayoutParameters = new TableRow.LayoutParams(4);

                // ajout des cellules à la ligne
                _Row.AddView(_Cell1);
                _Row.AddView(_Cell2);
                _Row.AddView(_Cell3);
                _Row.AddView(_Cell4);

                // ajout de la ligne au tableau
                _Table.AddView(_Row);
            }
        }
    }
}