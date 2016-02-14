using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content.PM;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AppAndroid.Data;
using AppAndroid.Work;

namespace AppAndroid.Acti
{
    [Activity(Label = "IncidentMaJ", ScreenOrientation = ScreenOrientation.Landscape)]
    public class IncidentMaJ : Activity
    {
        private DBWork _DB = new DBWork();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.IncidentMaJ);

            TextView tw = FindViewById<TextView>(Resource.Id.textViewClock);
            //tw.Click += new EventHandler(tw_Click);
            tw.Text = $"Création le {SharedData.CheckSelect.DateBDD}, dernière mise à jour : {SharedData.CheckSelect.DateMaJBDD}";

            //TextView NumBus = FindViewById<TextView>(Resource.Id.textViewNumBus);
            TextView SideBus = FindViewById<TextView>(Resource.Id.textViewSideBus);
            TextView controCreateTV = FindViewById<TextView>(Resource.Id.spinnerControCreate);
            TextView controUpdateTV = FindViewById<TextView>(Resource.Id.spinnerControUpdate);
            Spinner conducSpinner = FindViewById<Spinner>(Resource.Id.spinnerConduc);
            Spinner typeSpinner = FindViewById<Spinner>(Resource.Id.spinnerTypeIncident);
            Spinner gravitySpinner = FindViewById<Spinner>(Resource.Id.spinnerGravityIncident);
            EditText editTextDescription = FindViewById<EditText>(Resource.Id.editTextDescBus);
            Button SupprButtonIncident = FindViewById<Button>(Resource.Id.buttonSupprIncident);
            Button MaJButtonIncident = FindViewById<Button>(Resource.Id.buttonMaJIncident);

            List<Controleur> listSpinnerContro = new List<Controleur>();
            listSpinnerContro = _DB.GetControleur();

            List<Conducteur> listSpinnerConduc = new List<Conducteur>();
            listSpinnerConduc = _DB.GetConducteur();

            string[] strs = _DB.GetAllIncidentInfos(SharedData.CheckSelect.IdBDD, SharedData.CheckSelect.DateBDD);
            int idCheck = int.Parse(strs[0]);
            string observation = strs[1];
            string controCreaName = strs[2];

            List<string> listType = new List<string>();
            listType.Add("Trou");
            listType.Add("Enfoncement");
            listType.Add("Rayure");

            List<string> listGravity = new List<string>();
            listGravity.Add($"Léger");
            listGravity.Add($"Modéré");
            listGravity.Add($"Important");

            ArrayAdapter<Conducteur> adapterConduc = new ArrayAdapter<Conducteur>(this, Android.Resource.Layout.SimpleListItem1, listSpinnerConduc);
            conducSpinner.Adapter = adapterConduc;
            ArrayAdapter<string> adapterType = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listType);
            typeSpinner.Adapter = adapterType;
            ArrayAdapter<string> adapterGravity = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listGravity);
            gravitySpinner.Adapter = adapterGravity;

            List<int> listIndexIDConduc = new List<int>();
            int indexSpConduc = 0, i = 0;
            foreach (var item in listSpinnerConduc)
            {
                if (item.Id == SharedData.DriverID)
                    indexSpConduc = i;

                listIndexIDConduc.Add(item.Id);
                i++;
            }

            switch (SharedData.CheckSelect.Cote)
            {
                case 0:
                    SideBus.Text = $"Coté droit du bus N° {SharedData.BusNumber}";
                    break;
                case 1:
                    SideBus.Text = $"Avant du bus N° {SharedData.BusNumber}";
                    break;
                case 2:
                    SideBus.Text = $"Coté  gauche du bus N° {SharedData.BusNumber}";
                    break;
                case 3:
                    SideBus.Text = $"Arrière du bus N° {SharedData.BusNumber}";
                    break;
            }

            editTextDescription.Text = observation;

            controCreateTV.Text = $"Création : {controCreaName}";
            controUpdateTV.Text = $"Mies à jour : {SharedData.ControleurName}";
            conducSpinner.SetSelection(indexSpConduc);
            typeSpinner.SetSelection(SharedData.CheckSelect.Type);
            gravitySpinner.SetSelection(SharedData.CheckSelect.Gravite);

            SupprButtonIncident.Click += delegate
            {
                var builder = new AlertDialog.Builder(this);

                if (gravitySpinner.SelectedItem == null || typeSpinner.SelectedItem == null || conducSpinner.SelectedItem == null)

                {
                    builder.SetMessage("La fiche incident a mal été remplie");
                    builder.SetPositiveButton("D'accord", (s, e) => { });
                    //builder.SetNegativeButton("Cancel", (s, e) => { });
                }
                else
                {
                    builder.SetMessage("Êtes-vous sûr ?");
                    builder.SetPositiveButton("Oui", (s, e) => {
                        _DB.UpdateEtatIncident(SharedData.CheckSelect.IdBDD, 1);

                        this.Finish();
                    });
                    builder.SetNegativeButton("Annuler", (s, e) => { });
                }

                builder.Create().Show();
            };

            MaJButtonIncident.Click += delegate
            {
                var builder = new AlertDialog.Builder(this);

                if (gravitySpinner.SelectedItem == null || typeSpinner.SelectedItem == null || conducSpinner.SelectedItem == null)

                {
                    builder.SetMessage("La fiche incident a mal été remplie");
                    builder.SetPositiveButton("D'accord", (s, e) => { });
                    //builder.SetNegativeButton("Cancel", (s, e) => { });
                }
                else
                {
                    builder.SetMessage("Êtes-vous sûr ?");
                    builder.SetPositiveButton("Oui", (s, e) => {
                        var v = DateTime.Now.ToString("HH:mm dd/MM/yyyy");
                        int vv = SharedData.CheckSelect.IdBDD;
                        _DB.DBUpdateIncident(SharedData.CheckSelect.IdBDD, SharedData.DriverID, typeSpinner.SelectedItemPosition, gravitySpinner.SelectedItemPosition, 0, DateTime.Now.ToString("HH:mm dd/MM/yyyy"), editTextDescription.Text, SharedData.CheckSelect.X, SharedData.CheckSelect.Y, "");

                        this.Finish();
                    });
                    builder.SetNegativeButton("Annuler", (s, e) => { });
                }

                builder.Create().Show();
            };

        }
    }
}