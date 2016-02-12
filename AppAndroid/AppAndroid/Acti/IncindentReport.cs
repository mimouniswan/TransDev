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
    [Activity(Label = "IncidentReport", ScreenOrientation = ScreenOrientation.Landscape)]
    public class IncidentReport : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.IncidentReport);

            DBWork DB = new DBWork();
            TextView tw = FindViewById<TextView>(Resource.Id.textViewClock);
            //tw.Click += new EventHandler(tw_Click);
            tw.Text = DateTime.Now.ToString("HH:mm dd/MM/yyyy");

            //TextView NumBus = FindViewById<TextView>(Resource.Id.textViewNumBus);
            TextView SideBus = FindViewById<TextView>(Resource.Id.textViewSideBus);
            TextView controCreateTV = FindViewById<TextView>(Resource.Id.spinnerControCreate);
            TextView controUpdateTV = FindViewById<TextView>(Resource.Id.spinnerControUpdate);
            Spinner conducSpinner = FindViewById<Spinner>(Resource.Id.spinnerConduc);
            Spinner typeIncidentSpinner = FindViewById<Spinner>(Resource.Id.spinnerTypeIncident);
            Spinner gravitySpinner = FindViewById<Spinner>(Resource.Id.spinnerGravityIncident);
            EditText editTextDescription = FindViewById<EditText>(Resource.Id.editTextDescBus);
            Button AddButtonIncident = FindViewById<Button>(Resource.Id.buttonValCreaIncident);

            List<Controleur> listSpinnerContro = new List<Controleur>();
            listSpinnerContro = DB.GetControleur();

            List<Conducteur> listSpinnerConduc = new List<Conducteur>();
            listSpinnerConduc = DB.GetConducteur();

            List<string> listType = new List<string>();
            listType.Add("Trou");
            listType.Add("Enfoncement");
            listType.Add("Rayure");

            List<string> listGravity = new List<string>();
            listGravity.Add($"Léger");
            listGravity.Add($"Modéré");
            listGravity.Add($"Important");

            //ArrayAdapter<Controleur> adapterControCrea = new ArrayAdapter<Controleur>(this, Android.Resource.Layout.SimpleListItem1, listSpinnerContro);
            //controCreateSpinner.Adapter = adapterControCrea;
            //ArrayAdapter<Controleur> adapterControUpdate = new ArrayAdapter<Controleur>(this, Android.Resource.Layout.SimpleListItem1, listSpinnerContro);
            //controUpdateSpinner.Adapter = adapterControUpdate;
            ArrayAdapter<Conducteur> adapterConduc = new ArrayAdapter<Conducteur>(this, Android.Resource.Layout.SimpleListItem1, listSpinnerConduc);
            conducSpinner.Adapter = adapterConduc;
            ArrayAdapter<string> adapterType = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listType);
            typeIncidentSpinner.Adapter = adapterType;
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

            switch (SharedData.ListCheck[0].Cote)
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

            //NumBus.Text = $"Bus N° {SharedData.BusNumber}";
            controCreateTV.Text = SharedData.ControleurName;
            controUpdateTV.Text = SharedData.ControleurName;
            conducSpinner.SetSelection(indexSpConduc);
            typeIncidentSpinner.SetSelection(SharedData.ListCheck[0].Type);
            gravitySpinner.SetSelection(SharedData.ListCheck[0].Gravite);

            AddButtonIncident.Click += delegate
            {
                var builder = new AlertDialog.Builder(this);

                if (gravitySpinner.SelectedItem == null || typeIncidentSpinner.SelectedItem == null || conducSpinner.SelectedItem == null)

                {
                    builder.SetMessage("La fiche incident a mal été remplie");
                    builder.SetPositiveButton("D'accord", (s, e) => { });
                    //builder.SetNegativeButton("Cancel", (s, e) => { });
                }
                else
                {
                    builder.SetMessage("Êtes-vous sûr ?");
                    builder.SetPositiveButton("Oui", (s, e) => {
                        List<Incident> list = new List<Incident>();

                        var v = listIndexIDConduc[conducSpinner.SelectedItemPosition];
                        var v2 = SharedData.ControleurName;

                        // Mise en mémoire ou inscription en BDD ? Là BDD.
                        //list.Add(DB.CreateAndGetIncident(SharedData.ControleurID, listIndexIDConduc[conducSpinner.SelectedItemPosition], typeIncidentSpinner.SelectedItemPosition, gravitySpinner.SelectedItemPosition, 0, SharedData.ListCheck[0].Cote, tw.Text, editTextDescription.Text, SharedData.ListCheck[0].X, SharedData.ListCheck[0].Y, ""));
                        //DB.DBInsertCheck(SharedData.ControleurID, SharedData.DriverID, SharedData.BusID, list);
                        SharedData.ListIncident.Add(DB.CreateAndGetIncident(SharedData.ControleurID, listIndexIDConduc[conducSpinner.SelectedItemPosition], typeIncidentSpinner.SelectedItemPosition, gravitySpinner.SelectedItemPosition, 0, SharedData.ListCheck[0].Cote, tw.Text, editTextDescription.Text, SharedData.ListCheck[0].X, SharedData.ListCheck[0].Y, ""));

                        this.Finish();
                        StartActivity(typeof(Checkup));
                    });
                    builder.SetNegativeButton("Annuler", (s, e) => { });
                }

                builder.Create().Show();
            };

        }
    }
}