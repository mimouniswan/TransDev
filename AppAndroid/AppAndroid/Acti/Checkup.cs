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
using AppAndroid.Data;

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
        private Spinner _SpinnerGravity;

        private Button _BtnPrec;
        private Button _BtnSuiv;

        private CheckBox _CheckMode;

        private Color _ImgColor;

        private int _WidthInDp;
        private int _HeightInDp;

        private List<ImageView> _ListImg = new List<ImageView>();
        private List<Incident> _ListInc = new List<Incident>();

        private float _ViewX;
        private float _ViewY;

        private int _Gravite;
        private int _CurrentSide;
        private int _IDImgSelect;

        private List<int[]> _TmpCoordList = new List<int[]>();
        private List<int> _TmpTypeList = new List<int>();
        private List<int> _TmpGraviteList = new List<int>();
        //private List<string> _TmpTest = new List<string>();

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

                    Console.WriteLine($"{_IDImgSelect}");

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

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);

            _SpinnerGravity.ItemSelected += MenuClick;
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
            _SpinnerGravity = FindViewById<Spinner>(Resource.Id.spinnerGravity);
            _CheckMode = FindViewById<CheckBox>(Resource.Id.checkBoxInci);
            #endregion
            // Image du Bus
            layout.AddView(CreateImageView(layout, Resource.Drawable.bus_blank_right));
            //Get Bus number
            TextView busNumber = FindViewById<TextView>(Resource.Id.txtNumBus);
            busNumber.Text = $"Bus N°{SharedData.BusNumber}";

            DBWork DB = new DBWork();
            string[] str = DB.GetLastCheck(SharedData.BusID);
            TextView lastCheck = FindViewById<TextView>(Resource.Id.txtLastCheck);
            lastCheck.Text = $"Dernier check :  {str[1]} avec {str[0]}";

            _ImgCircle.Clickable = true;
            _ImgSquare.Clickable = true;
            _ImgTriangle.Clickable = true;
            _ImgTrash.Clickable = true;

            btnVal.Text = "Terminé";

            ChangeImgColor(Color.Green);

            List<string> listSpinner = new List<string>();

            listSpinner.Add($"Léger");
            listSpinner.Add($"Modéré");
            listSpinner.Add($"Important");

            ArrayAdapter<string> adapterGravity = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listSpinner);
            _SpinnerGravity.Adapter = adapterGravity;

            #region Les fonctions Click
            _ImgCircle.Click += delegate
            {
                if (!_CheckMode.Checked)
                {
                    AddImage(layout, Resource.Drawable.CircleT, _ImgColor);
                    btnVal.Text = "Valider";
                }
            };

            _ImgSquare.Click += delegate
            {
                if (!_CheckMode.Checked)
                {
                    AddImage(layout, Resource.Drawable.Square, _ImgColor);
                    btnVal.Text = "Valider";
                }
            };

            _ImgTriangle.Click += delegate
            {
                if (!_CheckMode.Checked)
                {
                    AddImage(layout, Resource.Drawable.TriangleT, _ImgColor);
                    btnVal.Text = "Valider";
                }
            };

            _ImgTrash.Click += delegate
            {
                if (_ImgSelect != null)
                {
                    layout.RemoveView(_ImgSelect);
                    _ListImg.RemoveAt(_IDImgSelect);

                    _ImgSelect = null;
                    btnVal.Text = "Terminé";
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
                        List<TmpCheck> ListTmpCheck = new List<TmpCheck>();
                        int side = 0;

                        switch (_CurrentSide)
                        {
                            case Resource.Drawable.bus_blank_front:
                                side = 1;
                                break;
                            case Resource.Drawable.bus_blank_right:
                                side = 0;
                                break;
                            case Resource.Drawable.bus_blank_back:
                                side = 3;
                                break;
                            case Resource.Drawable.bus_blank_left:
                                side = 2;
                                break;
                        }

                        if (_TmpCoordList.Count > 0)
                            ListTmpCheck.Add(new TmpCheck() { X = _TmpCoordList[0][0], Y = _TmpCoordList[0][1], Type = _TmpTypeList[0], Gravite = _TmpGraviteList[0], Cote = side });
                        else
                            ListTmpCheck[0] = new TmpCheck() { X = _TmpCoordList[0][0], Y = _TmpCoordList[0][1], Type = _TmpTypeList[0], Gravite = _TmpGraviteList[0], Cote = side };

                        SharedData.ListCheck = ListTmpCheck;

                        this.Finish();
                        StartActivity(typeof(IncidentReport));

                    });
                    builder.SetNegativeButton("Annuler", (s, e) => { });

                    builder.Create().Show();
                }
                else
                {
                    var builder = new AlertDialog.Builder(this);
                    builder.SetMessage($"Vous avez terminé ?");
                    builder.SetPositiveButton("Oui", (s, e) =>
                    {
                        var v = SharedData.ListIncident;
                        DB.DBInsertCheck(SharedData.ControleurID, SharedData.DriverID, SharedData.BusID, SharedData.ListIncident);

                        SharedData.ListIncident = new List<Data.Incident>();

                        this.Finish();
                    });
                    builder.SetNegativeButton("Non", (s, e) => { });

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
            
            _CheckMode.Click += delegate
            {
                //if (_CheckMode.Checked)
                //    _CheckMode.Checked = false;
                //else
                //    _CheckMode.Checked = true;

                _ListImg = new List<ImageView>();
                _TmpCoordList = new List<int[]>();
                _TmpGraviteList = new List<int>();
                _TmpTypeList = new List<int>();

                Console.WriteLine($"{_CheckMode.Checked}");
                if(_CheckMode.Checked)
                {
                    int c = 0;
                    if (_CurrentSide == Resource.Drawable.bus_blank_right)
                        c = 0;
                    else if (_CurrentSide == Resource.Drawable.bus_blank_front)
                        c = 1;
                    else if (_CurrentSide == Resource.Drawable.bus_blank_left)
                        c = 2;
                    else if (_CurrentSide == Resource.Drawable.bus_blank_back)
                        c = 3;

                    _ListInc = DB.GetBusIncident(SharedData.BusID, c);

                    layout.RemoveAllViews();
                    _ImgSelect = null;

                    layout.AddView(CreateImageView(layout, _CurrentSide));

                    int count = 0;
                    foreach (Incident item in _ListInc)
                    {
                        int t = 0;
                        Color color = Color.Gray;

                        if (item.Type == 0)
                            t = Resource.Drawable.CircleT;
                        else if (item.Type == 1)
                            t = Resource.Drawable.Square;
                        else if (item.Type == 2)
                            t = Resource.Drawable.TriangleT;

                        if (item.Gravite == 0)
                            color = Color.Green;
                        else if (item.Gravite == 1)
                            color = Color.Orange;
                        else if (item.Gravite == 2)
                            color = Color.Red;

                        AddImage(layout, t, color, item.Gravite, new int[4] { item.X + count * 4, item.Y + count * 4, 50, 50 }, false);

                        count++;
                    }
                }
                else
                {
                    layout.RemoveAllViews();
                    _ImgSelect = null;

                    layout.AddView(CreateImageView(layout, _CurrentSide));
                }
            };
            #endregion


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

            if (_TmpGraviteList.Count > 0)
                _TmpGraviteList[_IDImgSelect] = _Gravite;
            else
                _TmpGraviteList.Add(_Gravite);
        }

        private void ReplaceImg()
        {
            if (_ListImg.Count > 0)
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

        /// <summary>
        /// Ajout d'une ImageView
        /// </summary>
        /// <param name="layout">Le layout dans lequel il s'affichera.</param>
        /// <param name="idResource">Resource.Drawable</param>
        /// <param name="color">La couleur que l'image prendra.</param>
        private void AddImage(RelativeLayout layout, int idResource, Color color, int gravite = -1, int[] position = null, bool oneOnly = true)
        {
            ImageView img = CreateImageView(layout, idResource, false);

            img.LayoutParameters.Width = 50;
            img.LayoutParameters.Height = 50;
            img.SetColorFilter(color);
            img.Clickable = true;

            img.SetOnTouchListener(this);

            if (_ListImg.Count > 0 && oneOnly)
            {
                layout.RemoveView(_ListImg[0]);
                _ListImg[0] = img;
                if (position != null)
                    _TmpCoordList[0] = new int[4] { position[0], position[1], position[2], position[3] };
                else
                    _TmpCoordList[0] = new int[4] { img.Left, img.Top, img.Right, img.Bottom };
                if (gravite > -1)
                    _TmpGraviteList[0] = gravite;

                switch (idResource)
                {
                    case Resource.Drawable.CircleT:
                        _TmpTypeList[0] = 0;
                        //_TmpTest.Add("Cercle");
                        break;
                    case Resource.Drawable.Square:
                        _TmpTypeList[0] = 1;
                        //_TmpTest.Add("Carré");
                        break;
                    case Resource.Drawable.TriangleT:
                        _TmpTypeList[0] = 2;
                        //_TmpTest.Add("Triangle");
                        break;
                }
            }
            else {
                _ListImg.Add(img);
                if (position != null)
                    _TmpCoordList.Add(new int[4] { position[0], position[1], position[2], position[3] });
                else 
                    _TmpCoordList.Add(new int[4] { img.Left, img.Top, img.Right, img.Bottom });
                if (gravite > -1)
                    _TmpGraviteList.Add(gravite);

                switch (idResource)
                {
                    case Resource.Drawable.CircleT:
                        _TmpTypeList.Add(0);
                        //_TmpTest.Add("Cercle");
                        break;
                    case Resource.Drawable.Square:
                        _TmpTypeList.Add(1);
                        //_TmpTest.Add("Carré");
                        break;
                    case Resource.Drawable.TriangleT:
                        _TmpTypeList.Add(2);
                        //_TmpTest.Add("Triangle");
                        break;
                }
            } 

            int c = _ListImg.Count;
            img.Click += delegate { _IDImgSelect = c - 1; };

            layout.AddView(img);

            _ImgSelect = img;

            if(!oneOnly)
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
                    {
                        item.SetColorFilter(color);
                        }
                }

                _ImgColor = color;
            }
            ReplaceImg();
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