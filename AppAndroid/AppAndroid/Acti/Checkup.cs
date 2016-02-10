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
using Android.Content.PM;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Graphics;
using Android.Util;

namespace AppAndroid.Acti
{
    [Activity(Label = "Checkupcs", ScreenOrientation = ScreenOrientation.Landscape)]
    public class Checkup : Activity, View.IOnTouchListener
    {
        private ImageView _ImgSelect;
        private ImageView _ImgSquare;
        private ImageView _ImgCircle;
        private ImageView _ImgTriangle;
        private ImageView _ImgTrash;

        private CheckBox _CheckMode;

        private Color _ImgColor;

        private int _WidthInDp;
        private int _HeightInDp;

        private List<ImageView> _ListImgAjoutM = new List<ImageView>();
        private List<ImageView> _ListImgCreaM = new List<ImageView>();

        private float _ViewX;
        private float _ViewY;

        private int _Type;
        private int _Gravite;
        private int _CurrentSide;
        private int[] _TmpCoord = new int[4];

        public bool OnTouch(View v, MotionEvent e)
        {
            int left = 0;
            int top = 0;
            int right = 0;
            int bottom = 0;

            switch (e.Action)
            {
                case MotionEventActions.Down:
                    _ImgSelect = (ImageView)v;

                    if (!_CheckMode.Checked)
                    {
                        _ViewX = e.GetX();
                        _ViewY = e.GetY();
                    }
                    else
                    {

                    }
                    break;
                case MotionEventActions.Move:
                    if (!_CheckMode.Checked)
                    {
                        left = (int)(e.RawX - _ViewX);
                        top = (int)(e.RawY - _ViewY - (_HeightInDp * 0.185));
                        right = (int)(left + v.Width);
                        bottom = (int)(top + v.Height);
                        v.Layout(left, top, right, bottom);

                        _TmpCoord[0] = left;
                        _TmpCoord[1] = top;
                        _TmpCoord[2] = right;
                        _TmpCoord[3] = bottom;
                    }
                    else
                    {

                    }
                    break;
            }

            return true;
        }

        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.lCheckup);

            _WidthInDp = Resources.DisplayMetrics.WidthPixels;
            _HeightInDp = Resources.DisplayMetrics.HeightPixels;

            _ImgColor = Color.Gray;
            RelativeLayout layout = FindViewById<RelativeLayout>(Resource.Id.lyMiddle);
            LinearLayout layoutTop = FindViewById<LinearLayout>(Resource.Id.lyTop);

            Button btnVal = FindViewById<Button>(Resource.Id.buttonValidateInci);
            Button btnPrec = FindViewById<Button>(Resource.Id.buttonPrec);
            Button btnSuiv = FindViewById<Button>(Resource.Id.buttonSuiv);
            _ImgCircle = FindViewById<ImageView>(Resource.Id.imgCircle);
            _ImgSquare = FindViewById<ImageView>(Resource.Id.imgSquare);
            _ImgTriangle = FindViewById<ImageView>(Resource.Id.imgTriangle);
            _ImgTrash = FindViewById<ImageView>(Resource.Id.imgTrash);
            Spinner spinner = FindViewById<Spinner>(Resource.Id.spinnerGravity);
            _CheckMode = FindViewById<CheckBox>(Resource.Id.checkBoxInci);

            _ImgCircle.Clickable = true;
            _ImgSquare.Clickable = true;
            _ImgTriangle.Clickable = true;
            _ImgTrash.Clickable = true;

            ChangeImgColor(Color.Green);

            List<string> listSpinner = new List<string>();

            listSpinner.Add($"Léger");
            listSpinner.Add($"Modéré");
            listSpinner.Add($"Important");

            ArrayAdapter<string> adapterGravity = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listSpinner);
            spinner.Adapter = adapterGravity;
            spinner.ItemSelected += MenuClick;

            _ImgCircle.Click += delegate
            {
                if (!_CheckMode.Checked)
                {
                    AddImage(layout, Resource.Drawable.CircleT, _ImgColor);
                    _Type = 0;
                }
            };

            _ImgSquare.Click += delegate
            {
                if (!_CheckMode.Checked)
                {
                    AddImage(layout, Resource.Drawable.Square, _ImgColor);
                    _Type = 1;
                }
            };

            _ImgTriangle.Click += delegate
            {
                if (!_CheckMode.Checked)
                {
                    AddImage(layout, Resource.Drawable.TriangleT, _ImgColor);
                    _Type = 2;
                }
            };

            _ImgTrash.Click += delegate
            {
                if (_ImgSelect != null)
                {
                    layout.RemoveView(_ImgSelect);
                }
            };

            btnVal.Click += delegate 
            {
                if (_ImgSelect != null)
                {
                    // X et Y de l'objet selectionné
                    int X = _ImgSelect.Left;
                    int Y = _ImgSelect.Top;

                    var builder = new AlertDialog.Builder(this);
                    builder.SetMessage($"Valider ?");
                    builder.SetPositiveButton("Oui", (s, e) =>
                    {
                        // X, Y, _Type, _Gravite
                    });
                    builder.SetNegativeButton("Annuler", (s, e) => { });

                    builder.Create().Show();
                }
            };

            btnPrec.Click += delegate
            {
                NextSide(layout, false);
            };

            btnSuiv.Click += delegate
            {
                NextSide(layout);
            };

            // Image du Bus
            ////////////// Mettre condition pour connaitre le coté à affiché //////////////
            layout.AddView(CreateImageView(layout, Resource.Drawable.bus_blank_right));
        }

        /// <summary>
        /// Créer un ImageView de la largeur du layout.
        /// </summary>
        /// <param name="layout">Layout dans lequel l'ImageView prendra les dimensions.</param>
        /// <param name="idResource">ID de la ressource.</param>
        /// <returns></returns>
        private ImageView CreateImageView(RelativeLayout layout, int idResource, bool side = true)
        {
            Bitmap _bit = BitmapFactory.DecodeResource(Resources, idResource);
            float scale = ((float)_bit.Height * 100 / (float)_bit.Width) / 100;

            ImageView imgView = new ImageView(this);
            imgView.SetImageBitmap(_bit);
            imgView.LayoutParameters = new ViewGroup.LayoutParams(_WidthInDp, (int)(_WidthInDp * scale));

            if(side)
                _CurrentSide = idResource;

            return imgView;
        }

        private void AddImage(RelativeLayout layout, int idResource, Color color)
        {
            ImageView img = CreateImageView(layout, idResource, false);

            img.LayoutParameters.Width = 50;
            img.LayoutParameters.Height = 50;
            img.SetColorFilter(color);

            img.SetOnTouchListener(this);
            img.Clickable = true;

            if (_ListImgAjoutM.Count > 0)
            {
                layout.RemoveView(_ListImgAjoutM[0]);
                _ListImgAjoutM[0] = img;
            }
            else
                _ListImgAjoutM.Add(img);

            layout.AddView(img);

            _ImgSelect = img;
            ReplaceImg();
        }

        private void ChangeImgColor(Color color)
        {
            if (_ImgColor != color)
            {
                _ImgSquare.SetColorFilter(color);
                _ImgCircle.SetColorFilter(color);
                _ImgTriangle.SetColorFilter(color);

                foreach (ImageView item in _ListImgAjoutM)
                    item.SetColorFilter(color);

                if (_ImgSelect != null)
                    _ImgSelect.SetColorFilter(color);

                _ImgColor = color;
            }
            ReplaceImg();
        }

        private void MenuClick(object sender, AdapterView.ItemSelectedEventArgs ea)
        {
            switch (ea.Id)
            {
                case 0:
                    // Léger
                    ChangeImgColor(Color.Green);
                    _Gravite = 0;
                    break;
                case 1:
                    // Modéré
                    ChangeImgColor(Color.Orange);
                    _Gravite = 1;
                    break;
                case 2:
                    // Important
                    ChangeImgColor(Color.Red);
                    _Gravite = 2;
                    break;
            }
        }

        private void ReplaceImg()
        {
            if(_ImgSelect != null)
                _ImgSelect.Layout(_TmpCoord[0], _TmpCoord[1], _TmpCoord[2], _TmpCoord[3]);
        }

        private void NextSide(RelativeLayout layout, bool asc = true)
        {
            layout.RemoveAllViews();
            _ListImgCreaM = new List<ImageView>();

            _ImgSelect = null;

            switch(_CurrentSide)
            {
                case Resource.Drawable.bus_blank_front:
                    if(asc)
                        layout.AddView(CreateImageView(layout, Resource.Drawable.bus_blank_right));
                    else
                        layout.AddView(CreateImageView(layout, Resource.Drawable.bus_blank_left));
                    break;
                case Resource.Drawable.bus_blank_right:
                    if(asc)
                        layout.AddView(CreateImageView(layout, Resource.Drawable.bus_blank_back));
                    else
                        layout.AddView(CreateImageView(layout, Resource.Drawable.bus_blank_front));
                    break;
                case Resource.Drawable.bus_blank_back:
                    if (asc)
                        layout.AddView(CreateImageView(layout, Resource.Drawable.bus_blank_left));
                    else
                        layout.AddView(CreateImageView(layout, Resource.Drawable.bus_blank_right));
                    break;
                case Resource.Drawable.bus_blank_left:
                    if (asc)
                        layout.AddView(CreateImageView(layout, Resource.Drawable.bus_blank_front));
                    else
                        layout.AddView(CreateImageView(layout, Resource.Drawable.bus_blank_back));
                    break;
            }
        }
    }
}