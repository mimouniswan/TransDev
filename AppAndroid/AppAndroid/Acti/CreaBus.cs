
using Android.App;
using Android.Database;
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
        List<string> listSpinnerColor;
        List<string> listSpinnerModel;
        List<string> listSpinnerNumberBus;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.CreaBus);

            listSpinnerColor = new List<string>();
            listSpinnerColor.Add("Gris");
            listSpinnerColor.Add("Bleu");
            listSpinnerColor.Add("Blanc");
            listSpinnerColor.Add("Vert");
            listSpinnerColor.Add("Autre");

            listSpinnerModel = new List<string>();
            listSpinnerModel.Add("1");
            listSpinnerModel.Add("2");

            DBWork DB = new DBWork();

            List<Data.Model> listSpinnerSelectModel = new List<Data.Model>();

            ////////////INSERT////////////
            Button AddButtonBus = FindViewById<Button>(Resource.Id.buttonAddAdminBus);
            Button AddButtonModel = FindViewById<Button>(Resource.Id.buttonAddModelAdminBus);
            Spinner colorSpinner = FindViewById<Spinner>(Resource.Id.spinnerColorAdminBus);
            Spinner modelSpinner = FindViewById<Spinner>(Resource.Id.spinnerModelAdminBus);
            EditText _EditTextAddBus = FindViewById<EditText>(Resource.Id.editTextNumeroBus);
            EditText _EditTextAddModel = FindViewById<EditText>(Resource.Id.editTextAddModel);

            ArrayAdapter<string> adapterAddColorBus = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listSpinnerColor);
            colorSpinner.Adapter = adapterAddColorBus;
            ArrayAdapter<string> adapterAddModelBus = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listSpinnerModel);
            modelSpinner.Adapter = adapterAddModelBus;

            AddButtonBus.Click += delegate
            {
                var builder = new AlertDialog.Builder(this);

                if (_EditTextAddBus.Text.Equals(string.Empty))
                {
                    builder.SetMessage("Ce bus n'a pas de numéro.");
                    builder.SetPositiveButton("D'accord", (s, e) => { });
                    //builder.SetNegativeButton("Cancel", (s, e) => { });
                }
                else
                {
                    listSpinnerSelectModel = DB.GetModel();
                    builder.SetMessage("Êtes-vous sûr ?");
                    builder.SetPositiveButton("Oui", (s, e) => {
                        //DB.DBInsertBus(int.Parse(_EditTextAddBus.Text), colorSpinner.SelectedItem.ToString());
                        this.Finish();
                    });
                    builder.SetNegativeButton("Annuler", (s, e) => { });
                }

                builder.Create().Show();
            };

            AddButtonModel.Click += delegate
            {
                var builder = new AlertDialog.Builder(this);

                if (_EditTextAddModel.Text.Equals(string.Empty))
                {
                    builder.SetMessage("Ce modèle n'a pas de numéro.");
                    builder.SetPositiveButton("D'accord", (s, e) => { });
                    //builder.SetNegativeButton("Cancel", (s, e) => { });
                }
                else
                {
                    builder.SetMessage("Êtes-vous sûr ?");
                    builder.SetPositiveButton("Oui", (s, e) => {
                        DB.DBInsertModel(_EditTextAddModel.Text);
                        this.Finish();
                    });
                    builder.SetNegativeButton("Annuler", (s, e) => { });
                }

                builder.Create().Show();
            };


            ////////////UPDATE\DELETE////////////
            Button UpdateButtonBus = FindViewById<Button>(Resource.Id.buttonUpdateAdminBus);
            Button DeleteButtonBus = FindViewById<Button>(Resource.Id.buttonDeleteAdminBus);
            Spinner SelectBusSpinner = FindViewById<Spinner>(Resource.Id.spinnerSelectAdminBus);
            Spinner colorUpdateSpinner = FindViewById<Spinner>(Resource.Id.spinnerUpdateColorAdminBus);
            Spinner numberUpdateSpinner = FindViewById<Spinner>(Resource.Id.spinnerSelectAdminBus);
            EditText _EditTextUpdateBus = FindViewById<EditText>(Resource.Id.editTextNumeroBus);

            Button UpdateButtonModel = FindViewById<Button>(Resource.Id.buttonUpdateAdminModel);
            Button DeleteButtonModel = FindViewById<Button>(Resource.Id.buttonDeleteAdminModel);
            Spinner SelectModelSpinner = FindViewById<Spinner>(Resource.Id.spinnerSelectAdminModel);
            EditText _EditTextUpdateModel = FindViewById<EditText>(Resource.Id.editTextNumeroBus);

            List<Data.Bus> listSpinnerSelectBus = new List<Data.Bus>();

            listSpinnerSelectBus = DB.GetBus();
            listSpinnerNumberBus = new List<string>();
            foreach (var item in listSpinnerSelectBus)
                listSpinnerNumberBus.Add($"{item.Number}");

            ArrayAdapter<string> adapterUpdateNumberBus = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listSpinnerNumberBus);
            SelectBusSpinner.Adapter = adapterUpdateNumberBus;
            ArrayAdapter<string> adapterUpdateColorBus = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listSpinnerColor);
            colorUpdateSpinner.Adapter = adapterUpdateColorBus;
            ArrayAdapter<string> adapterUpdateModelBus = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listSpinnerModel);
            numberUpdateSpinner.Adapter = adapterUpdateModelBus;
            ArrayAdapter<string> adapterUpdateModel = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listSpinnerModel);
            SelectModelSpinner.Adapter = adapterUpdateModel;





            UpdateButtonBus.Click += delegate
            {
                var builder = new AlertDialog.Builder(this);

                if (_EditTextUpdateBus.Text.Equals(string.Empty))
                {
                    builder.SetMessage("Ce bus n'a pas de numéro.");
                    builder.SetPositiveButton("D'accord", (s, e) => { });
                    //builder.SetNegativeButton("Cancel", (s, e) => { });
                }
                else
                {
                    builder.SetMessage($"Êtes-vous sûr de vouloir modifier le bus n°{_EditTextUpdateModel.Text} ?");
                    builder.SetPositiveButton("Oui", (s, e) => {
                        DB.DBUpdateBus(int.Parse(SelectBusSpinner.SelectedItem.ToString()), int.Parse(_EditTextUpdateBus.Text), colorUpdateSpinner.SelectedItem.ToString());
                        this.Finish();
                    });
                    builder.SetNegativeButton("Annuler", (s, e) => { });
                }

                builder.Create().Show();
            };

            UpdateButtonModel.Click += delegate
            {
                var builder = new AlertDialog.Builder(this);

                if (_EditTextUpdateModel.Text.Equals(string.Empty))
                {
                    builder.SetMessage("Ce modèle n'a pas de numéro.");
                    builder.SetPositiveButton("D'accord", (s, e) => { });
                    //builder.SetNegativeButton("Cancel", (s, e) => { });
                }
                else
                {
                    builder.SetMessage($"Êtes-vous sûr de vouloir modifier le modèle n°{_EditTextUpdateModel.Text} ?");
                    builder.SetPositiveButton("Oui", (s, e) => {
                      //  DB.DBUpdateModel(int.Parse(), _EditTextUpdateModel.Text);
                        this.Finish();
                    });
                    builder.SetNegativeButton("Annuler", (s, e) => { });
                }

                builder.Create().Show();
            };

            DeleteButtonBus.Click += delegate
            {
                var builder = new AlertDialog.Builder(this);

                if (_EditTextUpdateBus.Text.Equals(string.Empty))
                {
                    builder.SetMessage("Ce modèle n'a pas de numéro.");
                    builder.SetPositiveButton("D'accord", (s, e) => { });
                    //builder.SetNegativeButton("Cancel", (s, e) => { });
                }
                else
                {
                    builder.SetMessage($"Êtes-vous sûr de vouloir supprimer le bus n°{_EditTextUpdateBus.Text} ?");
                    builder.SetPositiveButton("Oui", (s, e) => {
                      //  DB.DBDeleteBus(int.Parse());
                        this.Finish();
                    });
                    builder.SetNegativeButton("Annuler", (s, e) => { });
                }

                builder.Create().Show();
            };

            DeleteButtonModel.Click += delegate
            {
                var builder = new AlertDialog.Builder(this);

                if (_EditTextUpdateModel.Text.Equals(string.Empty))
                {
                    builder.SetMessage("Ce modèle n'a pas de numéro.");
                    builder.SetPositiveButton("D'accord", (s, e) => { });
                    //builder.SetNegativeButton("Cancel", (s, e) => { });
                }
                else
                {
                    builder.SetMessage($"Êtes-vous sûr de vouloir supprimer le modèle n°{_EditTextUpdateModel.Text} ?");
                   
                    builder.SetPositiveButton("Oui", (s, e) => {
                        //DB.DBDeleteModel(int.Parse());
                        this.Finish();
                    });
                    builder.SetNegativeButton("Annuler", (s, e) => { });
                }

                builder.Create().Show();
            };

        }
    }
}