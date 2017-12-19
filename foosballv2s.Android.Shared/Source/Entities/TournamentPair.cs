﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Emgu.CV.Structure;
using foosballv2s.Droid.Shared.Source.Entities;
using foosballv2s.Droid.Shared.Source.Events;
using foosballv2s.Droid.Shared.Source.Services.GameLogger;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly: Dependency(typeof(TournamentPair))]
namespace foosballv2s.Droid.Shared.Source.Entities
{
    public class TournamentPair
    {
        public int Id { get; set; }

        public int GamePairNumberInStage { get; set; }

        public int StageNumber { get; set; } = 1;

        public Team Team1 { get; set; }
        public Team Team2 { get; set; }

        public Game Game { get; set; }
    }
} 
