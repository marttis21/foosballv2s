﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace foosballv2s.WebService.Models
{
    public class Game
    {
        public const int MAX_SCORE = 7;

        public int Id { get; set; }
        
        private int team1Score = 0;
        private int team2Score = 0;
        
        public Team Team1 { get; set; } = new Team();
        
        public Team Team2 { get; set; } = new Team();
        
        public int Team1Score
        {
            get { return team1Score; }
            set
            {
                if (HasEnded)
                {
                    return;
                }
                team1Score = value;
                CheckGameEnd();
            }
        }

        public int Team2Score
        {
            get { return team2Score; }
            set
            {
                if (HasEnded)
                {
                    return;
                }
                team2Score = value;
                CheckGameEnd();
            }
        }

        [NotMapped]
        public Boolean HasEnded { get; private set; } = false;
        
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        private void CheckGameEnd()
        {
            if (Team1Score == MAX_SCORE || Team2Score == MAX_SCORE)
            {
                HasEnded = true;
            }
        }
    }
}
