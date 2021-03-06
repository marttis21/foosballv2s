﻿using Android;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using foosballv2s.Droid.Shared;
using foosballv2s.Source.Activities.Helpers;
using Resource = Android.Resource;

namespace foosballv2s.Source.Activities
{
    /// <summary>
    /// An activity for displaying all tournaments
    /// </summary>
    [Activity(ParentActivity=typeof(MainActivity))]
    public class TournamentsActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Tournaments);
            NavigationHelper.SetupNavigationListener(this);
            NavigationHelper.SetActionBarNavigationText(this, Resource.String.nav_tournaments);
        }
    }
}