﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yahtzee_Game {
	public class Player {
        private const int BONUS_FOR_63 = 35;
        private string name;
		private int combinationsToDO;
		private Score[] scores;
		private int grandTotal;
        private Combination combination;
        private ScoreType[] scoretype = (ScoreType[])Enum.GetValues(typeof(ScoreType));
        private Form1 form = new Form1();
        private Label[] scoreTotals;
        
      
        public Player(string playerName, Label[] scoreTotals) {
			name = playerName;
            this.scoreTotals = scoreTotals;
            instantiate();
        }
        private void instantiate()
        {
            scores = new Score[scoreTotals.Length];
            for(int i = 0; i < scores.Length; i++)
            {
                switch (scoretype[i]) {
                    case ScoreType.Ones:
                    case ScoreType.Twos:
                    case ScoreType.Threes:
                    case ScoreType.Fours:
                    case ScoreType.Fives:
                    case ScoreType.Sixes:
                        scores[i] = new CountingCombination(scoretype[i], scoreTotals[i]);
                        break;
                    case ScoreType.SmallStraight:
                    case ScoreType.LargeStraight:
                    case ScoreType.FullHouse:
                    case ScoreType.Yahtzee:
                        scores[i] = new FixedScore(scoretype[i], scoreTotals[i]);
                        break;
                    case ScoreType.ThreeOfAKind:
                    case ScoreType.FourOfAKind:
                    case ScoreType.Chance:
                        scores[i] = new TotalOfDice(scoretype[i], scoreTotals[i]);
                        break;
                    case ScoreType.BonusFor63Plus:
                    case ScoreType.SubTotal:
                    case ScoreType.SectionATotal:
                    case ScoreType.YahtzeeBonus:
                    case ScoreType.SectionBTotal:
                    case ScoreType.GrandTotal:
                        scores[i] = new BonusOrTotal(scoreTotals[i]);
                        break;

                }
            }
        }

		public string Name {
			get {
				return name;
            }
            set
            {
                name = value;
            }
		}

		public void ScoreCombination(ScoreType scoretype, int[] integers) {
            combination = (Combination)scores[(int)scoretype];
            combination.CalculateScore(integers);
            GrandTotal = combination.Points;
            if((int)scoretype <= (int)ScoreType.Sixes)
            {
                scores[(int)ScoreType.SectionATotal].Points = combination.Points; 
                scores[(int)ScoreType.GrandTotal].Points = combination.Points;
                if(scores[(int)ScoreType.SectionATotal].Points >= 65)
                {
                    scores[(int)ScoreType.BonusFor63Plus].Points = BONUS_FOR_63;
                }
            }
            else if((int)scoretype > (int)ScoreType.Sixes)
            {
                scores[(int)ScoreType.SectionBTotal].Points = combination.Points;
                scores[(int)ScoreType.GrandTotal].Points = combination.Points;
            }
            //if(scores[(int)ScoreType.SubTotal].Points >= 63)
            //{
            //    scores[(int)ScoreType.BonusFor63Plus].Points = 35;
            //    scores[(int)ScoreType.BonusFor63Plus].ShowScore();
            //}
            //combination.ShowScore();
            if(scoretype != ScoreType.Yahtzee)
            {

            }
        }

		public int GrandTotal {
			get {
				return grandTotal;
			}
            set
            {
                grandTotal += value;
            }
		}

		public bool IsAvailable(ScoreType scoretype) {
			if(scores[(int)scoretype].Done == true)
            {
                return false;
            }
            else
            {
                return true;
            }
		}

		public void ShowScores() {
           combination.ShowScore();
            if (combination.Points > 0)
            {
                scores[(int)ScoreType.SectionATotal].ShowScore();
                scores[(int)ScoreType.BonusFor63Plus].ShowScore();
                scores[(int)ScoreType.GrandTotal].ShowScore();
                scores[(int)ScoreType.SectionBTotal].ShowScore();
                scores[(int)ScoreType.GrandTotal].ShowScore();
            }
		}

		public bool IsFinished() {
			if(combinationsToDO == 0) {
				return true;
			} else {
				return false;
			}
		}

		//public Label[] Load(){
			
		//}
	}
}
