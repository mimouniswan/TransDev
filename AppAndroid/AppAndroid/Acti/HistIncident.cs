
using Android.App;
using Android.OS;
using Android.Views;
using Android.Content.PM;
using Android.Widget;
using System.Collections.Generic;
using AppAndroid.Work;
using Android.Graphics;

namespace AppAndroid.Acti
{
    [Activity(Label = "HistIncident", ScreenOrientation = ScreenOrientation.Landscape)]
    public class HistIncident : Activity
    {
        private object _DB;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.HistIncident);

            DBWork DB = new DBWork();

            TableLayout table = FindViewById<TableLayout>(Resource.Id.TableLayoutHistInc);
            table.SetBackgroundColor(Color.Rgb(14, 14, 14));
            TableRow row; // création d'un élément : ligne
            TextView cell1, cell2, cell3, cell4; // création des cellules

            // Récupération historique des incidents
            List<string[]> incidents = DB.GetHistIncident(true);

            foreach (var item in incidents)
            {
                row = new TableRow(this);
                row.SetBackgroundColor(Color.Rgb(28, 28, 28));

                cell1 = new TextView(this); 
                cell1.Text = item[0]; 
                cell1.Gravity = GravityFlags.Center;
                cell1.LayoutParameters = new TableRow.LayoutParams(1);

                cell2 = new TextView(this); 
                cell2.Text = item[1]; 
                cell2.Gravity = GravityFlags.Center;
                cell2.LayoutParameters = new TableRow.LayoutParams(2);

                cell3 = new TextView(this); 
                cell3.Text = item[2]; 
                cell3.Gravity = GravityFlags.Center;
                cell3.LayoutParameters = new TableRow.LayoutParams(3);

                cell4 = new TextView(this); 
                cell4.Text = item[3]; 
                cell4.Gravity = GravityFlags.Center;
                cell4.LayoutParameters = new TableRow.LayoutParams(4);

                // ajout des cellules à la ligne
                row.AddView(cell1);
                row.AddView(cell2);
                row.AddView(cell3);
                row.AddView(cell4);

                // ajout de la ligne au tableau
                table.AddView(row);
            }
        }
    }
}