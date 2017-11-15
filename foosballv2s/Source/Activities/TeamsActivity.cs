﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using foosballv2s.Adapters;
using foosballv2s.Source.Services.FoosballWebService.Repository;
using Xamarin.Forms;
using ListView = Android.Widget.ListView;

namespace foosballv2s
{
    [Activity(ParentActivity=typeof(MainActivity))]
    public class TeamsActivity : AppCompatActivity
    {
        private TeamRepository teamRepository;
        private ListView teamListView;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Teams);
            NavigationHelper.SetupNavigationListener(this);
            NavigationHelper.SetActionBarNavigationText(this, Resource.String.nav_teams);

            teamRepository = DependencyService.Get<TeamRepository>();
            teamListView = (ListView) FindViewById(Resource.Id.team_list_view);
            
            FetchTeams();
        }

        private async void FetchTeams()
        {
            ProgressDialog dialog = ProgressDialog.Show(this, "", 
                Resources.GetString(Resource.String.retrieving_teams), true);
            
            Team[] teams = await teamRepository.GetAll();
            
            dialog.Dismiss();
            
            TeamAdapter teamAdapter = new TeamAdapter(this, new List<Team>(teams));
            teamListView.Adapter = teamAdapter;
        }
    }
}