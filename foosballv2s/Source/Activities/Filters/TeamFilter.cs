﻿using System.Collections.Generic;
using System.Linq;
using Android.Widget;
using foosballv2s.Droid.Shared.Source.Entities;
using foosballv2s.Source.Activities.Adapters;

namespace foosballv2s.Source.Activities.Filters
{
    /// <summary>
    /// Defines how the team list should be filtered by a given string
    /// </summary>
    public class TeamFilter : Filter
    {
        TeamAutoCompleteAdapter teamAdapter;
            
        public TeamFilter (TeamAutoCompleteAdapter adapter) : base() {
            teamAdapter = adapter;
        }
        
        protected override Filter.FilterResults PerformFiltering (Java.Lang.ICharSequence constraint)
        {
            FilterResults results = new FilterResults();
            if (constraint != null) {

                if (teamAdapter.IgnoreFilter)
                {
                    teamAdapter.IgnoreFilter = false;
                }
                else
                {
                    teamAdapter.SelectedTeam = null; 
                }
                    
                var searchFor = constraint.ToString();
                var matchList = new List<Team>();
                    
                var matches = from i in teamAdapter.teams
                    where i.TeamName.Contains(searchFor)
                    select i;

                teamAdapter.matchTeams = matches.ToList<Team>();
                    
//                    results.Values = matchObjects;
                results.Count = matchList.Count;
            }
            return results;
        }
        
        protected override void PublishResults (Java.Lang.ICharSequence constraint, Filter.FilterResults results)
        {
            teamAdapter.NotifyDataSetChanged();
        }
    }
}