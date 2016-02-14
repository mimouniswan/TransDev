
using Android.App;
using Android.Database;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using AppAndroid.Work;
using System.Collections.Generic;
using System.Linq;
using AppAndroid.Data;


namespace AppAndroid.Acti
{
    [Activity(Label = "CreaBus", ScreenOrientation = ScreenOrientation.Landscape)]
    public class CreaBus : Activity
    {
        List<string> listSpinnerColor;

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

            DBWork DB = new DBWork();

            List<Model> listSpinnerSelectModel = new List<Model>();
            listSpinnerSelectModel = DB.GetModel();
            ////////////INSERT////////////
            Button AddButtonBus = FindViewById<Button>(Resource.Id.buttonAddAdminBus);
            Button AddButtonModel = FindViewById<Button>(Resource.Id.buttonAddModelAdminBus);
            Spinner colorSpinner = FindViewById<Spinner>(Resource.Id.spinnerColorAdminBus);
            Spinner modelSpinner = FindViewById<Spinner>(Resource.Id.spinnerModelAdminBus);
            EditText _EditTextAddBus = FindViewById<EditText>(Resource.Id.editTextAddNumeroBus);
            EditText _EditTextAddModel = FindViewById<EditText>(Resource.Id.editTextAddModel);



            ArrayAdapter<string> adapterAddColorBus = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listSpinnerColor);
            colorSpinner.Adapter = adapterAddColorBus;
            ArrayAdapter<Model> adapterAddModelBus = new ArrayAdapter<Model>(this, Android.Resource.Layout.SimpleListItem1, listSpinnerSelectModel);
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

                    string itemSpinner = modelSpinner.SelectedItem.ToString();
                    
                    var itemIdSpinner = listSpinnerSelectModel.Where(item => item.Name == itemSpinner).ToList();
                    builder.SetMessage("Êtes-vous sûr ?");
                    builder.SetPositiveButton("Oui", (s, e) => {
                        DB.DBInsertBus(int.Parse(_EditTextAddBus.Text), colorSpinner.SelectedItem.ToString(), itemIdSpinner[0].Id);
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
            Spinner modelUpdateSpinner = FindViewById<Spinner>(Resource.Id.spinnerUpdateModelAdminBus);
            EditText _EditTextUpdateBus = FindViewById<EditText>(Resource.Id.editTextUpdateNumeroBus);

            List<Bus> listSpinnerSelectBus = new List<Bus>();
            listSpinnerSelectBus = DB.GetBus();
         /*   foreach (var item in listSpinnerSelectBus)
                listSpinnerSelectBus.Add($"{item.Number}");*/

            Button UpdateButtonModel = FindViewById<Button>(Resource.Id.buttonUpdateAdminModel);
            Button DeleteButtonModel = FindViewById<Button>(Resource.Id.buttonDeleteAdminModel);
            Spinner SelectModelSpinner = FindViewById<Spinner>(Resource.Id.spinnerSelectAdminModel);
            EditText _EditTextUpdateModel = FindViewById<EditText>(Resource.Id.editTextUpdateModel);



            ArrayAdapter<Bus> adapterUpdateNumberBus = new ArrayAdapter<Bus>(this, Android.Resource.Layout.SimpleListItem1, listSpinnerSelectBus);
            SelectBusSpinner.Adapter = adapterUpdateNumberBus;
            ArrayAdapter<string> adapterUpdateColorBus = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listSpinnerColor);
            colorUpdateSpinner.Adapter = adapterUpdateColorBus;
            ArrayAdapter<Model> adapterUpdateModelBus = new ArrayAdapter<Model>(this, Android.Resource.Layout.SimpleListItem1, listSpinnerSelectModel);
            modelUpdateSpinner.Adapter = adapterUpdateModelBus;
            ArrayAdapter<Model> adapterUpdateModel = new ArrayAdapter<Model>(this, Android.Resource.Layout.SimpleListItem1, listSpinnerSelectModel);
            SelectModelSpinner.Adapter = adapterUpdateModel;

            SelectBusSpinner.LayoutChange += delegate
            {
                if (SelectModelSpinner.SelectedItem != null)
                    _EditTextUpdateBus.Text = SelectBusSpinner.SelectedItem.ToString();
                /* int itemUpdateColorSpinner = int.Parse(colorUpdateSpinner.SelectedItem.ToString());
                 var itemIdUpdateColorSpinner = listSpinnerSelectBus.Where(item => item.Id == itemUpdateColorSpinner).ToList();

                 colorUpdateSpinner.SelectedItem.Equals(itemIdUpdateColorSpinner[0].Color);*/

                else
                {
                    List<string> listSpinnerEmptyBus = new List<string>();
                    listSpinnerEmptyBus.Add("Aucun bus");
                    ArrayAdapter<string> adapterUpdateEmptyBus = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listSpinnerEmptyBus);
                    SelectBusSpinner.Adapter = adapterUpdateEmptyBus;
                }

            };

            SelectModelSpinner.LayoutChange += delegate
            {
                if(SelectModelSpinner.SelectedItem != null)
                _EditTextUpdateModel.Text = SelectModelSpinner.SelectedItem.ToString();

                else
                {
                    List<string> listSpinnerEmptyModel = new List<string>();
                    listSpinnerEmptyModel.Add("Aucun modèle");
                    ArrayAdapter<string> adapterUpdateEmptyModel = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listSpinnerEmptyModel);
                    SelectModelSpinner.Adapter = adapterUpdateEmptyModel;
                    modelSpinner.Adapter = adapterUpdateEmptyModel;
                    modelUpdateSpinner.Adapter = adapterUpdateEmptyModel;
                }
            };

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
                    string itemUpdateSpinner = modelUpdateSpinner.SelectedItem.ToString();
                    var itemIdUpdateModelSpinner = listSpinnerSelectModel.Where(item => item.Name == itemUpdateSpinner).ToList();

                    int itemUpdateIdBusSpinner = int.Parse(SelectBusSpinner.SelectedItem.ToString());
                    var itemIdUpdateBusSpinner = listSpinnerSelectBus.Where(item => item.Number == itemUpdateIdBusSpinner).ToList();

                    builder.SetMessage($"Êtes-vous sûr de vouloir modifier le bus n°{_EditTextUpdateModel.Text} ?");
                    builder.SetPositiveButton("Oui", (s, e) => {
                        DB.DBUpdateBus(itemIdUpdateBusSpinner[0].Id, int.Parse(_EditTextUpdateBus.Text), colorUpdateSpinner.SelectedItem.ToString(), itemIdUpdateModelSpinner[0].Id);
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
                    string itemUpdateModelSpinner = SelectModelSpinner.SelectedItem.ToString();
                    var itemIdUpdateModelSpinner = listSpinnerSelectModel.Where(item => item.Name == itemUpdateModelSpinner).ToList();

                    builder.SetMessage($"Êtes-vous sûr de vouloir modifier le modèle n°{_EditTextUpdateModel.Text} ?");
                    builder.SetPositiveButton("Oui", (s, e) => {
                        DB.DBUpdateModel(itemIdUpdateModelSpinner[0].Id, _EditTextUpdateModel.Text);
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
                    int itemDeleteIdBusSpinner = int.Parse(SelectBusSpinner.SelectedItem.ToString());
                    var itemIdDeleteBusSpinner = listSpinnerSelectBus.Where(item => item.Number == itemDeleteIdBusSpinner).ToList();

                    builder.SetMessage($"Êtes-vous sûr de vouloir supprimer le bus n°{_EditTextUpdateBus.Text} ?");
                    builder.SetPositiveButton("Oui", (s, e) => {
                        DB.DBDeleteBus(itemIdDeleteBusSpinner[0].Id);
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
                    string itemDeleteSpinner = SelectModelSpinner.SelectedItem.ToString();
                    var itemIdDeleteModelSpinner = listSpinnerSelectModel.Where(item => item.Name == itemDeleteSpinner).ToList();

                    builder.SetMessage($"Êtes-vous sûr de vouloir supprimer le modèle n°{_EditTextUpdateModel.Text} ?");
                   
                    builder.SetPositiveButton("Oui", (s, e) => {
                        DB.DBDeleteModel(itemIdDeleteModelSpinner[0].Id);
                        this.Finish();
                    });
                    builder.SetNegativeButton("Annuler", (s, e) => { });
                }

                builder.Create().Show();
            };

        }
    }
}