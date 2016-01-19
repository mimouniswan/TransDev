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

namespace AppAndroid.Acti
{
    [Activity(Label = "CreaConduc")]
    public class CreaConduc : Activity
    {
        EditText _EditText;
        EditText _EditTextPass;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.CreaConduc);

            Button ValButton = FindViewById<Button>(Resource.Id.buttonValCreaConduc);
            _EditText = FindViewById<EditText>(Resource.Id.editTextNomCreaConduc);
            _EditTextPass = FindViewById<EditText>(Resource.Id.editTextMdPCreaConduc);

            ValButton.Click += delegate
            {
                DBWork DB = new DBWork();

                var builder = new AlertDialog.Builder(this);

                if (_EditText.Text.Equals(string.Empty) || _EditTextPass.Text.Equals(string.Empty))
                {
                    builder.SetMessage("Ce conducteur n'a pas de nom ou de mot de passe.");
                    builder.SetPositiveButton("D'accord", (s, e) => { });
                    //builder.SetNegativeButton("Cancel", (s, e) => { });
                }
                else
                {
                    builder.SetMessage("Êtes-vous sûr ?");
                    builder.SetPositiveButton("Oui", (s, e) => {
                        DB.DBInsertConducteur(_EditText.Text, _EditTextPass.Text);
                        this.Finish();
                    });
                    builder.SetNegativeButton("Annuler", (s, e) => { });
                }

                builder.Create().Show();
            };
        }
    }
}