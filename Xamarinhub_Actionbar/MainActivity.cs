using System;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Views.Animations;
using System.Linq;
using Android.Graphics;
using Xamarinhub_Actionbar.Classes;

namespace Xamarinhub_Actionbar
{
    [Activity(Label = "Xamarinhub_Actionbar", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        GestureDetector gestureDetector;
        GestureListener gestureListener;

        ListView menuListView;
        MenuListAdapterClass objAdapterMenu;
        ImageView menuIconImageView;
        int intDisplayWidth;
        bool isSingleTapFired = false;
        TextView txtActionBarText;
        TextView txtPageName;
        TextView txtDescription;
        ImageView btnDescExpander;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Window.RequestFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Main);
            FnInitialization();
            TapEvent();
            FnBindMenu(); //find definition in below steps
        }
        void TapEvent()
        {
            //title bar menu icon
            menuIconImageView.Click += delegate (object sender, EventArgs e)
            {
                if (!isSingleTapFired)
                {
                    FnToggleMenu();  //find definition in below steps
                    isSingleTapFired = false;
                }
            };
            //bottom expandable description window
            btnDescExpander.Click += delegate (object sender, EventArgs e)
            {
                FnDescriptionWindowToggle();
            };
        }
        void FnInitialization()
        {
            //gesture initialization
            gestureListener = new GestureListener();
            gestureListener.LeftEvent += GestureLeft; //find definition in below steps
            gestureListener.RightEvent += GestureRight;
            gestureListener.SingleTapEvent += SingleTap;
            gestureDetector = new GestureDetector(this, gestureListener);

            menuListView = FindViewById<ListView>(Resource.Id.menuListView);
            menuIconImageView = FindViewById<ImageView>(Resource.Id.menuIconImgView);
            txtActionBarText = FindViewById<TextView>(Resource.Id.txtActionBarText);
            txtPageName = FindViewById<TextView>(Resource.Id.txtPage);
            txtDescription = FindViewById<TextView>(Resource.Id.txtDescription);
            btnDescExpander = FindViewById<ImageView>(Resource.Id.btnImgExpander);

            //changed sliding menu width to 3/4 of screen width 
            Display display = this.WindowManager.DefaultDisplay;
            var point = new Point();
            display.GetSize(point);
            intDisplayWidth = point.X;
            intDisplayWidth = intDisplayWidth - (intDisplayWidth / 3);
            using (var layoutParams = (RelativeLayout.LayoutParams)menuListView.LayoutParameters)
            {
                layoutParams.Width = intDisplayWidth;
                layoutParams.Height = ViewGroup.LayoutParams.MatchParent;
                menuListView.LayoutParameters = layoutParams;
            }
        }
        void FnBindMenu()
        {
            string[] strMnuText = { GetString(Resource.String.Home), GetString(Resource.String.AboutUs), GetString(Resource.String.Products), GetString(Resource.String.Events), GetString(Resource.String.Serivce), GetString(Resource.String.Clients), GetString(Resource.String.Help), GetString(Resource.String.Solution), GetString(Resource.String.ContactUs) };
            int[] strMnuUrl = { Resource.Drawable.Icon, Resource.Drawable.Icon, Resource.Drawable.Icon, Resource.Drawable.Icon, Resource.Drawable.Icon, Resource.Drawable.Icon, Resource.Drawable.Icon, Resource.Drawable.Icon, Resource.Drawable.Icon };
            if (objAdapterMenu != null)
            {
                objAdapterMenu.actionMenuSelected -= FnMenuSelected;
                objAdapterMenu = null;
            }
            objAdapterMenu = new MenuListAdapterClass(this, strMnuText, strMnuUrl);
            objAdapterMenu.actionMenuSelected += FnMenuSelected;
            menuListView.Adapter = objAdapterMenu;
        }
        void FnMenuSelected(string strMenuText)
        {
            txtActionBarText.Text = strMenuText;
            txtPageName.Text = strMenuText;
            //selected action goes here
        }
        void GestureLeft()
        {
            if (!menuListView.IsShown)
                FnToggleMenu();
            isSingleTapFired = false;
        }
        void GestureRight()
        {
            if (menuListView.IsShown)
                FnToggleMenu();
            isSingleTapFired = false;
        }
        void SingleTap()
        {
            if (menuListView.IsShown)
            {
                FnToggleMenu();
                isSingleTapFired = true;
            }
            else
            {
                isSingleTapFired = false;
            }
        }
        public override bool DispatchTouchEvent(MotionEvent ev)
        {
            gestureDetector.OnTouchEvent(ev);
            return base.DispatchTouchEvent(ev);
        }
        void FnToggleMenu()
        {
            Console.WriteLine(menuListView.IsShown);
            if (menuListView.IsShown)
            {
                menuListView.Animation = new TranslateAnimation(0f, -menuListView.MeasuredWidth, 0f, 0f);
                menuListView.Animation.Duration = 300;
                menuListView.Visibility = ViewStates.Gone;
            }
            else
            {
                menuListView.Visibility = ViewStates.Visible;
                menuListView.RequestFocus();
                menuListView.Animation = new TranslateAnimation(-menuListView.MeasuredWidth, 0f, 0f, 0f);//starting edge of layout 
                menuListView.Animation.Duration = 300;
            }
        }

        //bottom desription window sliding 
        void FnDescriptionWindowToggle()
        {
            if (txtDescription.IsShown)
            {
                txtDescription.Visibility = ViewStates.Gone;
                txtDescription.Animation = new TranslateAnimation(0f, 0f, 0f, txtDescription.MeasuredHeight);
                txtDescription.Animation.Duration = 300;
                btnDescExpander.SetImageResource(Resource.Drawable.Arrowup);
            }
            else
            {
                txtDescription.Visibility = ViewStates.Visible;
                txtDescription.RequestFocus();
                txtDescription.Animation = new TranslateAnimation(0f, 0f, txtDescription.MeasuredHeight, 0f);
                txtDescription.Animation.Duration = 300;
                btnDescExpander.SetImageResource(Resource.Drawable.Downarrow);
            }
        }
    }
}

