
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using AppAndroid.Work;
using AppAndroid.Data;
using System.Collections.Generic;
using System.Linq;
using Android.Content.PM;

namespace AppAndroid.Acti
{
    [Activity(Label = "CreaContro", ScreenOrientation = ScreenOrientation.Landscape)]
    public class CreaContro : Activity
    {
        EditText _EditText;
        EditText _EditTextPass;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.CreaContro);

            DBWork DB = new DBWork();

            ////////////INSERT////////////
            Button ValButton = FindViewById<Button>(Resource.Id.buttonValCreaContro);
            _EditText = FindViewById<EditText>(Resource.Id.editTextNomCreaContro);
            _EditTextPass = FindViewById<EditText>(Resource.Id.editTextMdPCreaContro);

            ValButton.Click += delegate
            {

                var builder = new AlertDialog.Builder(this);

                if (_EditText.Text.Equals(string.Empty) || _EditTextPass.Text.Equals(string.Empty))
                {
                    builder.SetMessage("Ce controleur n'a pas de nom ou de mot de passe.");
                    builder.SetPositiveButton("D'accord", (s, e) => { });
                    //builder.SetNegativeButton("Cancel", (s, e) => { });
                }
                else
                {
                    builder.SetMessage("Êtes-vous sûr ?");
                    builder.SetPositiveButton("Oui", (s, e) => {
                        DB.DBInsertControleur(_EditText.Text, _EditTextPass.Text);
                        this.Finish();
                    });
                    builder.SetNegativeButton("Annuler", (s, e) => { });
                }

                builder.Create().Show();
            };

            ////////////UPDATE\DELETE////////////
            Button UpdateButtonContro = FindViewById<Button>(Resource.Id.buttonUpdateAdminContro);
            Button DeleteButtonContro = FindViewById<Button>(Resource.Id.buttonDeleteAdminContro);
            Spinner SelectControSpinner = FindViewById<Spinner>(Resource.Id.spinnerSelectAdminContro);
            EditText _EditTextUpdateNameContro = FindViewById<EditText>(Resource.Id.editTextUpdateNameContro);
            EditText _EditTextUpdateMdpContro = FindViewById<EditText>(Resource.Id.editTextUpdateMdpContro);

            List<Controleur> listSpinnerSelectContro = new List<Controleur>();
            listSpinnerSelectContro = DB.GetControleur();

            ArrayAdapter<Controleur> adapterUpdateNameContro = new ArrayAdapter<Controleur>(this, Android.Resource.Layout.SimpleListItem1, listSpinnerSelectContro);
            SelectControSpinner.Adapter = adapterUpdateNameContro;

            SelectControSpinner.LayoutChange += delegate
            {
                if (SelectControSpinner.SelectedItem != null)
                    _EditTextUpdateNameContro.Text = SelectControSpinner.SelectedItem.ToString();

                else
                {
                    List<string> listSpinnerEmptyContro = new List<string>();
                    listSpinnerEmptyContro.Add("Aucun contrôleur");
                    ArrayAdapter<string> adapterUpdateEmptyContro = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listSpinnerEmptyContro);
                    SelectControSpinner.Adapter = adapterUpdateEmptyContro;
                }
            };

            UpdateButtonContro.Click += delegate
            {
                var builder = new AlertDialog.Builder(this);

                if (_EditTextUpdateNameContro.Text.Equals(string.Empty) || _EditTextUpdateMdpContro.Text.Equals(string.Empty))
                {
                    builder.SetMessage("Ce contôleur n'a pas de nom ou de mot de passe");
                    builder.SetPositiveButton("D'accord", (s, e) => { });
                    //builder.SetNegativeButton("Cancel", (s, e) => { });
                }
                else
                {
                    string itemUpdateSpinner = SelectControSpinner.SelectedItem.ToString();
                    var itemIdUpdateConducSpinner = listSpinnerSelectContro.Where(item => item.Name == itemUpdateSpinner).ToList();

                    builder.SetMessage($"Êtes-vous sûr de vouloir modifier le contrôleur {_EditTextUpdateNameContro.Text} ?");
                    builder.SetPositiveButton("Oui", (s, e) => {
                        DB.DBUpdateControleur(itemIdUpdateConducSpinner[0].Id, _EditTextUpdateNameContro.Text, _EditTextUpdateMdpContro.Text);
                        this.Finish();
                    });
                    builder.SetNegativeButton("Annuler", (s, e) => { });
                }

                builder.Create().Show();
            };

            DeleteButtonContro.Click += delegate
            {
                var builder = new AlertDialog.Builder(this);

                if (_EditTextUpdateNameContro.Text.Equals(string.Empty))
                {
                    builder.SetMessage("Ce contrôleur n'a pas de nom.");
                    builder.SetPositiveButton("D'accord", (s, e) => { });
                    //builder.SetNegativeButton("Cancel", (s, e) => { });
                }
                else
                {
                    string itemDeleteSpinner = SelectControSpinner.SelectedItem.ToString();
                    var itemIdDeleteConducSpinner = listSpinnerSelectContro.Where(item => item.Name == itemDeleteSpinner).ToList();

                    builder.SetMessage($"Êtes-vous sûr de vouloir supprimer le modèle n°{_EditTextUpdateNameContro.Text} ?");

                    builder.SetPositiveButton("Oui", (s, e) => {
                        DB.DBDeleteControleur(itemIdDeleteConducSpinner[0].Id);
                        this.Finish();
                    });
                    builder.SetNegativeButton("Annuler", (s, e) => { });
                }

                builder.Create().Show();
            };
        }
    }
}