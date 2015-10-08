﻿using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.Wearable.Views;
using Android.Views.Animations;
using Android.Hardware;
using Android.Hardware.Location;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Java.Interop;

namespace Compass
{
    [Activity(Label = "Compass", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity,ISensorEventListener
    {
        private SensorManager sensorManager;

        private float currentDegree = 0f;
        ImageView image;
        ImageView dialImage;
        TextView headingDirection;
        TextView headingDegree;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            sensorManager = (SensorManager)GetSystemService(SensorService);

            var v = FindViewById<WatchViewStub>(Resource.Id.watch_view_stub);
            v.LayoutInflated += delegate
            {

                // Get our button from the layout resource,
                // and attach an event to it
                image = FindViewById<ImageView>(Resource.Id.imageViewCompass);
                headingDirection = FindViewById<TextView>(Resource.Id.headingDirection);
                headingDegree = FindViewById<TextView>(Resource.Id.headingDegree);
                dialImage = FindViewById<ImageView>(Resource.Id.imageViewDial);
               
            };
        }

        protected override void OnResume()
        {
            base.OnResume();

            sensorManager.RegisterListener(this, sensorManager.GetDefaultSensor(SensorType.Orientation),SensorDelay.Game);
        }


        protected override void OnPause()
        {
            base.OnPause();
            sensorManager.UnregisterListener(this);
        }

        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {

        }

        public void OnSensorChanged(SensorEvent e)
        {
            float degree = (float)Math.Round(e.Values[0]);
            headingDirection.Text = CalculateDirection(degree);
            headingDegree.Text = Convert.ToString(degree) + "\u00B0";
            var ra = new RotateAnimation(currentDegree, -degree, Dimension.RelativeToSelf, 0.5f, Dimension.RelativeToSelf, 0.5f);
            ra.Duration = 210;
            ra.FillAfter = true;
            image.Animation = ra;
            currentDegree = -degree;
        }
        private string CalculateDirection(float degree)
        {
            if (degree > 304 && degree < 35)
                return "NE";
            if (degree > 35 && degree < 134)
                return "NW";
            if (degree > 134 && degree < 213)
                return "SW";
            if (degree > 213 && degree < 304)
                return "SE";
            if (degree > 35 && degree < 134)
                return "NE";
            if (degree == 35)
                return "N";
            if (degree == 134)
                return "W";
            if (degree == 304)
                return "E";
            if (degree == 213)
                return "S";

            return "NE"; //Default :)

        }
    }
}

