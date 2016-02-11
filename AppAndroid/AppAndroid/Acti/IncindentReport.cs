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

            TextView NumBus = FindViewById<TextView>(Resource.Id.textViewNumBus);
            TextView SideBus = FindViewById<TextView>(Resource.Id.textViewSideBus);
            Spinner controCreateSpinner = FindViewById<Spinner>(Resource.Id.spinnerControCreate);
            Spinner controUpdateSpinner = FindViewById<Spinner>(Resource.Id.spinnerControUpdate);
            Spinner conducSpinner = FindViewById<Spinner>(Resource.Id.spinnerConduc);
            Spinner typeIncidentSpinner = FindViewById<Spinner>(Resource.Id.spinnerTypeIncident);
            Spinner gravitySpinner = FindViewById<Spinner>(Resource.Id.spinnerGravityIncident);
            EditText _EditTextDescription = FindViewById<EditText>(Resource.Id.editTextNumeroBus);
            Button AddButtonIncident = FindViewById<Button>(Resource.Id.buttonValCreaIncident);

            List<Controleur> listSpinnerContro = new List<Controleur>();
            listSpinnerContro = DB.GetControleur();

            List<Conducteur> listSpinnerConduc = new List<Conducteur>();
            listSpinnerConduc = DB.GetConducteur();

            List<string> listType = new List<string>();
            listType.Add("Rayure");
            listType.Add("Enfoncement");
            listType.Add("Trou");

            List<string> listGravity = new List<string>();
            listGravity.Add("Faible");
            listGravity.Add("Moyen");
            listGravity.Add("Important");

            ArrayAdapter<Controleur> adapterControCrea = new ArrayAdapter<Controleur>(this, Android.Resource.Layout.SimpleListItem1, listSpinnerContro);
            controCreateSpinner.Adapter = adapterControCrea;
            ArrayAdapter<Controleur> adapterControUpdate = new ArrayAdapter<Controleur>(this, Android.Resource.Layout.SimpleListItem1, listSpinnerContro);
            controUpdateSpinner.Adapter = adapterControUpdate;
            ArrayAdapter<Conducteur> adapterConduc = new ArrayAdapter<Conducteur>(this, Android.Resource.Layout.SimpleListItem1, listSpinnerConduc);
            conducSpinner.Adapter = adapterConduc;
            ArrayAdapter<string> adapterType = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listType);
            typeIncidentSpinner.Adapter = adapterType;
            ArrayAdapter<string> adapterGravity = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listGravity);
            gravitySpinner.Adapter = adapterGravity;

            AddButtonIncident.Click += delegate
            {
                var builder = new AlertDialog.Builder(this);

                if (gravitySpinner.SelectedItem == null || typeIncidentSpinner.SelectedItem == null || conducSpinner.SelectedItem == null
                || controUpdateSpinner.SelectedItem == null || controCreateSpinner.SelectedItem == null)

                {
                    builder.SetMessage("La fiche incident a mal été remplie");
                    builder.SetPositiveButton("D'accord", (s, e) => { });
                    //builder.SetNegativeButton("Cancel", (s, e) => { });
                }
                else
                {
                    //string itemGravitySpinner = gravitySpinner.SelectedItem.ToString();
                    //var itemIdSpinner = listSpinnerSelectModel.Where(item => item.Name == itemSpinner).ToList();
                    string itemGravitySpinner = "Faible";
                    string itemTypeSpinner = "Rayure";
                    string conduc = "Amblard";
                    string controCreat = "Mimouni";
                    SideBus.Text = "Droit";
                    if (controCreat != null)
                    {
                        string controUpdate = controCreat;
                    }
                    else
                    {
                        //string controUpdate = currentUser;
                    }

                    builder.SetMessage("Êtes-vous sûr ?");
                    builder.SetPositiveButton("Oui", (s, e) => {
                        List<Incident> list = new List<Incident>();
                        list.Add(DB.CreateAndGetIncident(1, 1, SideBus.Text, 1, 1, controCreat, tw.Text, 1, 0, ""));
                        DB.DBInsertCheck(1, 1, 1, list);
                        this.Finish();
                    });
                    builder.SetNegativeButton("Annuler", (s, e) => { });
                }

                builder.Create().Show();
                StartActivity(typeof(Checkup));
            };

        }
    }
}