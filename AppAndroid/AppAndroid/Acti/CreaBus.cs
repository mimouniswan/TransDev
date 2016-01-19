
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using AppAndroid.Work;
using System.Collections.Generic;

namespace AppAndroid.Acti
{
    [Activity(Label = "CreaBus")]
    public class CreaBus : Activity
    {
        EditText _EditText;
        List<string> listSpinner;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.CreaBus);

            Button ValButton = FindViewById<Button>(Resource.Id.buttonValCreaBus);
            Spinner colorSpinner = FindViewById<Spinner>(Resource.Id.spinnerColorCreaBus);
            _EditText = FindViewById<EditText>(Resource.Id.editTextNumeroBus);

            listSpinner = new List<string>();

            listSpinner.Add("Noir");
            listSpinner.Add("Blanc");
            listSpinner.Add("Rouge");
            listSpinner.Add("Bleu");
            listSpinner.Add("Vert");
            listSpinner.Add("Jaune");
            listSpinner.Add("Orange");
            listSpinner.Add("Violet");
            listSpinner.Add("Marron");
            listSpinner.Add("Rose");

            ArrayAdapter<string> adapterDriver = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listSpinner);
            colorSpinner.Adapter = adapterDriver;

            ValButton.Click += delegate
            {
                DBWork DB = new DBWork();

                var builder = new AlertDialog.Builder(this);

                if (_EditText.Text.Equals(string.Empty))
                {
                    builder.SetMessage("Ce bus n'a pas de numéro.");
                    builder.SetPositiveButton("D'accord", (s, e) => { });
                    //builder.SetNegativeButton("Cancel", (s, e) => { });
                }
                else
                {
                    builder.SetMessage("Êtes-vous sûr ?");
                    builder.SetPositiveButton("Oui", (s, e) => {
                        DB.DBInsertBus(int.Parse(_EditText.Text), colorSpinner.SelectedItem.ToString());
                        this.Finish();
                    });
                    builder.SetNegativeButton("Annuler", (s, e) => { });
                }

                builder.Create().Show();
            };
        }
    }
}