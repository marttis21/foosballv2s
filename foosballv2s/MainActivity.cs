﻿using Android.App;
using Android.Widget;
using Android.OS;
using Android.Hardware;
using Android.Views;
using System;
using Android;
using Android.Content;
using Android.Graphics;
using Android.Provider;
using Android.Runtime;
using Emgu.CV.Structure;
using Java.Interop;

namespace foosballv2s
{
    [Activity()]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
        }

        [Export("SubmitTeamNames")]
        public void SubmitTeamNames(View view)
        {
            //TODO: validate team names with Regex
            Intent intent = new Intent(this, typeof(BallImageActivity));
            StartActivity(intent);
        }

    }
}