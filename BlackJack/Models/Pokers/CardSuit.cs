using System;

namespace BlackJack.Models.Pokers
{
    public class CardSuit
    {
        public CardSuit(string symbol, ConsoleColor color)
        {
            Symbol = symbol;
            Color = color;
        }

        public string Symbol { get; private set; }

        public ConsoleColor Color { get; private set; }
    }
}