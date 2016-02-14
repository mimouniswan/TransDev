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
using AppAndroid.Data;
using Android.Content.PM;

namespace AppAndroid.Acti
{
    [Activity(Label = "CreaConduc", ScreenOrientation = ScreenOrientation.Landscape)]
    public class CreaConduc : Activity
    {
        EditText _EditText;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.CreaConduc);

            DBWork DB = new DBWork();

            ////////////INSERT////////////
            Button ValButton = FindViewById<Button>(Resource.Id.buttonValCreaConduc);
            _EditText = FindViewById<EditText>(Resource.Id.editTextNomCreaConduc);

            ValButton.Click += delegate
            {
                var builder = new AlertDialog.Builder(this);

                if (_EditText.Text.Equals(string.Empty))
                {
                    builder.SetMessage("Ce conducteur n'a pas de nom.");
                    builder.SetPositiveButton("D'accord", (s, e) => { });
                    //builder.SetNegativeButton("Cancel", (s, e) => { });
                }
                else
                {
                    builder.SetMessage("Êtes-vous sûr ?");
                    builder.SetPositiveButton("Oui", (s, e) => {
                        DB.DBInsertConducteur(_EditText.Text);
                        this.Finish();
                    });
                    builder.SetNegativeButton("Annuler", (s, e) => { });
                }

                builder.Create().Show();
            };

            ////////////UPDATE\DELETE////////////
            Button UpdateButtonConduc = FindViewById<Button>(Resource.Id.buttonUpdateAdminConduc);
            Button DeleteButtonConduc = FindViewById<Button>(Resource.Id.buttonDeleteAdminConduc);
            Spinner SelectConducSpinner = FindViewById<Spinner>(Resource.Id.spinnerSelectAdminConduc);
            EditText _EditTextUpdateConduc = FindViewById<EditText>(Resource.Id.editTextUpdateNameConduc);

            List<Conducteur> listSpinnerSelectConduc = new List<Conducteur>();
            listSpinnerSelectConduc = DB.GetConducteur();

            ArrayAdapter<Conducteur> adapterUpdateNameConduc = new ArrayAdapter<Conducteur>(this, Android.Resource.Layout.SimpleListItem1, listSpinnerSelectConduc);
            SelectConducSpinner.Adapter = adapterUpdateNameConduc;

            SelectConducSpinner.LayoutChange += delegate
            {
                if (SelectConducSpinner.SelectedItem != null)
                    _EditTextUpdateConduc.Text = SelectConducSpinner.SelectedItem.ToString();

                else
                {
                    List<string> listSpinnerEmptyConduc = new List<string>();
                    listSpinnerEmptyConduc.Add("Aucun conducteur");
                    ArrayAdapter<string> adapterUpdateEmptyConduc = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listSpinnerEmptyConduc);
                    SelectConducSpinner.Adapter = adapterUpdateEmptyConduc;
                }
            };

            UpdateButtonConduc.Click += delegate
            {
                var builder = new AlertDialog.Builder(this);

                if (_EditTextUpdateConduc.Text.Equals(string.Empty))
                {
                    builder.SetMessage("Ce conducteur n'a pas de nom.");
                    builder.SetPositiveButton("D'accord", (s, e) => { });
                    //builder.SetNegativeButton("Cancel", (s, e) => { });
                }
                else
                {
                    string itemUpdateSpinner = SelectConducSpinner.SelectedItem.ToString();
                    var itemIdUpdateConducSpinner = listSpinnerSelectConduc.Where(item => item.Name == itemUpdateSpinner).ToList();

                    builder.SetMessage($"Êtes-vous sûr de vouloir modifier le conducteur {_EditTextUpdateConduc.Text} ?");
                    builder.SetPositiveButton("Oui", (s, e) => {
                        DB.DBUpdateConducteur(itemIdUpdateConducSpinner[0].Id, _EditTextUpdateConduc.Text);
                        this.Finish();
                    });
                    builder.SetNegativeButton("Annuler", (s, e) => { });
                }

                builder.Create().Show();
            };

            DeleteButtonConduc.Click += delegate
            {
                var builder = new AlertDialog.Builder(this);

                if (_EditTextUpdateConduc.Text.Equals(string.Empty))
                {
                    builder.SetMessage("Ce conducteur n'a pas de nom.");
                    builder.SetPositiveButton("D'accord", (s, e) => { });
                    //builder.SetNegativeButton("Cancel", (s, e) => { });
                }
                else
                {
                    string itemDeleteSpinner = SelectConducSpinner.SelectedItem.ToString();
                    var itemIdDeleteConducSpinner = listSpinnerSelectConduc.Where(item => item.Name == itemDeleteSpinner).ToList();

                    builder.SetMessage($"Êtes-vous sûr de vouloir supprimer le modèle n°{_EditTextUpdateConduc.Text} ?");

                    builder.SetPositiveButton("Oui", (s, e) => {
                        DB.DBDeleteConducteur(itemIdDeleteConducSpinner[0].Id);
                        this.Finish();
                    });
                    builder.SetNegativeButton("Annuler", (s, e) => { });
                }

                builder.Create().Show();
            };
        }
    }
}