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
using AppAndroid.Work;

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

        private Button _BtnPrec;
        private Button _BtnSuiv;

        private CheckBox _CheckMode;

        private Color _ImgColor;

        private int _WidthInDp;
        private int _HeightInDp;

        private List<ImageView> _ListImg = new List<ImageView>();

        private float _ViewX;
        private float _ViewY;

        private int _Type;
        private int _Gravite;
        private int _CurrentSide;
        private int _IDImgSelect;

        private List<int[]> _TmpCoordList = new List<int[]>();
        private List<int> _TmpTypeList = new List<int>();
        private List<int> _TmpGraviteList = new List<int>();

        public bool OnTouch(View v, MotionEvent e)
        {
            int left = 0;
            int top = 0;
            int right = 0;
            int bottom = 0;

            switch (e.Action)
            {
                case MotionEventActions.Down:
                    v.PerformClick();
                    _ImgSelect = _ListImg[_IDImgSelect];

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

                        _TmpCoordList[_IDImgSelect] = new int[4] { left, top, right, bottom };
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
            #region Initilisation des objets d'interface
            RelativeLayout layout = FindViewById<RelativeLayout>(Resource.Id.lyMiddle);
            LinearLayout layoutTop = FindViewById<LinearLayout>(Resource.Id.lyTop);

            Button btnVal = FindViewById<Button>(Resource.Id.buttonValidateInci);
            _BtnPrec = FindViewById<Button>(Resource.Id.buttonPrec);
            _BtnSuiv = FindViewById<Button>(Resource.Id.buttonSuiv);
            _ImgCircle = FindViewById<ImageView>(Resource.Id.imgCircle);
            _ImgSquare = FindViewById<ImageView>(Resource.Id.imgSquare);
            _ImgTriangle = FindViewById<ImageView>(Resource.Id.imgTriangle);
            _ImgTrash = FindViewById<ImageView>(Resource.Id.imgTrash);
            Spinner spinner = FindViewById<Spinner>(Resource.Id.spinnerGravity);
            _CheckMode = FindViewById<CheckBox>(Resource.Id.checkBoxInci);
            #endregion

            //Get Bus number
            var insertNumber = SharedData.BusNumber;
            TextView busNumber = FindViewById<TextView>(Resource.Id.txtNumBus);
            busNumber.Text = "Bus N°" + insertNumber;

            //Get Driver Name
            var insertName = SharedData.DriverName;
            busNumber = FindViewById<TextView>(Resource.Id.txtDriver);
            busNumber.Text = "Conducteur : " + insertName;

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

            #region Clicks
            _ImgCircle.Click += delegate
            {
                if (!_CheckMode.Checked)
                {
                    AddImage(layout, Resource.Drawable.CircleT, _ImgColor);
                    _Type = 0;

                    _TmpTypeList[_IDImgSelect] = _Type;
                }
            };

            _ImgSquare.Click += delegate
            {
                if (!_CheckMode.Checked)
                {
                    AddImage(layout, Resource.Drawable.Square, _ImgColor);
                    _Type = 1;

                    _TmpTypeList[_IDImgSelect] = _Type;
                }
            };

            _ImgTriangle.Click += delegate
            {
                if (!_CheckMode.Checked)
                {
                    AddImage(layout, Resource.Drawable.TriangleT, _ImgColor);
                    _Type = 2;

                    //_TmpTypeList[_IDImgSelect] = _Type;
                }
            };

            _ImgTrash.Click += delegate
            {
                if (_ImgSelect != null)
                {
                    layout.RemoveView(_ImgSelect);
                    _ListImg.RemoveAt(_IDImgSelect);
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

            _BtnPrec.Click += delegate
            {
                NextSide(layout, false);
            };

            _BtnSuiv.Click += delegate
            {
                NextSide(layout);
            };
            #endregion

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

            if (side)
            {
                _CurrentSide = idResource;

                switch (idResource)
                {
                    case Resource.Drawable.bus_blank_front:
                        _BtnPrec.Text = "Droite";
                        _BtnSuiv.Text = "Gauche";
                        break;
                    case Resource.Drawable.bus_blank_right:
                        _BtnPrec.Text = "Arrière";
                        _BtnSuiv.Text = "Avant";
                        break;
                    case Resource.Drawable.bus_blank_back:
                        _BtnPrec.Text = "Gauche";
                        _BtnSuiv.Text = "Droite";
                        break;
                    case Resource.Drawable.bus_blank_left:
                        _BtnPrec.Text = "Avant";
                        _BtnSuiv.Text = "Arrière";
                        break;
                }
            }

            return imgView;
        }

        private void AddImage(RelativeLayout layout, int idResource, Color color)
        {
            ImageView img = CreateImageView(layout, idResource, false);

            img.LayoutParameters.Width = 50;
            img.LayoutParameters.Height = 50;
            img.SetColorFilter(color);
            img.Clickable = true;

            img.SetOnTouchListener(this);

            if (_ListImg.Count > 0)
            {
                layout.RemoveView(_ListImg[0]);
                _ListImg[0] = img;
                //_TmpCoordList[0] = new int[4] { img.Left, img.Top, img.Right, img.Bottom };
            }
            else {
                _ListImg.Add(img);
                _TmpCoordList.Add(new int[4] { img.Left, img.Top, img.Right, img.Bottom });
                _TmpGraviteList.Add(_Gravite);


            }

            int c = _ListImg.Count;
            img.Click += delegate { _IDImgSelect = c - 1; };

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

                if (_ImgSelect != null)
                    _ImgSelect.SetColorFilter(color);
                else
                {
                    foreach (ImageView item in _ListImg)
                        item.SetColorFilter(color);
                }

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

            //[_IDImgSelect] = _Gravite;
        }

        private void ReplaceImg()
        {
            if(_ListImg.Count > 0)
            {
                int i = 0;
                foreach (var item in _ListImg)
                {
                    int[] coord = _TmpCoordList[i];
                    item.Layout(coord[0], coord[1], coord[2], coord[3]);
                    i++;
                }
            }
        }

        private void NextSide(RelativeLayout layout, bool asc = true)
        {
            layout.RemoveAllViews();

            _ImgSelect = null;

            switch(_CurrentSide)
            {
                case Resource.Drawable.bus_blank_front:
                    if (asc)
                        layout.AddView(CreateImageView(layout, Resource.Drawable.bus_blank_left));
                    else
                        layout.AddView(CreateImageView(layout, Resource.Drawable.bus_blank_right));
                    break;
                case Resource.Drawable.bus_blank_right:
                    if (asc)
                        layout.AddView(CreateImageView(layout, Resource.Drawable.bus_blank_front));
                    else
                        layout.AddView(CreateImageView(layout, Resource.Drawable.bus_blank_back));
                    break;
                case Resource.Drawable.bus_blank_back:
                    if (asc)
                        layout.AddView(CreateImageView(layout, Resource.Drawable.bus_blank_right));
                    else
                        layout.AddView(CreateImageView(layout, Resource.Drawable.bus_blank_left));
                    break;
                case Resource.Drawable.bus_blank_left:
                    if (asc)
                        layout.AddView(CreateImageView(layout, Resource.Drawable.bus_blank_back));
                    else
                        layout.AddView(CreateImageView(layout, Resource.Drawable.bus_blank_front));
                    break;
            }
        }
    }
}