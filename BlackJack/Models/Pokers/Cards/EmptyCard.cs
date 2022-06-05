using System;

namespace BlackJack.Models.Pokers.Cards
{
    public class EmptyCard : Card
    {
        public EmptyCard() : base("Empty", new CardSuit("", ConsoleColor.White))
        {
        }
    }
}