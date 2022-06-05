using System;

namespace BlackJack.Models.Pokers
{
    public class Card
    {
        private CardSuit _suit;

        public Card(string value, CardSuit suit)
        {
            Value = value;
            _suit = suit;
        }

        public string Value { get; private set; }

        public string GetFullName() => Value + _suit.Symbol;

        public ConsoleColor GetColor() => _suit.Color;

        public void Show()
        {
            var oriColor = Console.ForegroundColor;
            Console.ForegroundColor = GetColor();
            Console.Write(GetFullName());
            Console.ForegroundColor = oriColor;
        }
    }
}