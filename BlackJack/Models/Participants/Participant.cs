using BlackJack.Models.Pokers;
using BlackJack.Strategies;
using System;
using System.Collections.Generic;

namespace BlackJack.Models.Participants
{
    public abstract class Participant
    {
        public Participant(string name)
        {
            Name = name;
            Cards = new List<Card>();
        }

        public string Name { get; private set; }

        public int Score { get; private set; }

        public List<Card> Cards { get; private set; }

        public bool IsExceedScore => Score > 21;

        public virtual bool IsAbleToDraw() => !IsExceedScore;

        public virtual int CalculateScore(CalculationStrategy calculation)
        {
            Score = calculation.CalculateScore(this);
            return Score;
        }

        public void ShowScore()
        {
            var oriColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(string.Format("{0}\'s total score: {1}", Name, Score));
            Console.ForegroundColor = oriColor;
        }
        
        public abstract void DrawCard(Deck deck);

        public virtual void Reset()
        {
            Score = 0;
            Cards.Clear();
        }
    }
}