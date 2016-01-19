
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using AppAndroid.Work;

namespace AppAndroid.Acti
{
    [Activity(Label = "CreaContro")]
    public class CreaContro : Activity
    {
        EditText _EditText;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.CreaContro);

            Button ValButton = FindViewById<Button>(Resource.Id.buttonValCreaContro);
            _EditText = FindViewById<EditText>(Resource.Id.editTextNomCreaContro);

            ValButton.Click += delegate
            {
                DBWork DB = new DBWork();

                var builder = new AlertDialog.Builder(this);

                if (_EditText.Text.Equals(string.Empty))
                {
                    builder.SetMessage("Ce controleur n'a pas de nom.");
                    builder.SetPositiveButton("D'accord", (s, e) => { });
                    //builder.SetNegativeButton("Cancel", (s, e) => { });
                }
                else
                {
                    builder.SetMessage("Êtes-vous sûr ?");
                    builder.SetPositiveButton("Oui", (s, e) => {
                        DB.DBInsertControleur(_EditText.Text);
                        this.Finish();
                    });
                    builder.SetNegativeButton("Annuler", (s, e) => { });
                }

                builder.Create().Show();
            };
        }
    }
}