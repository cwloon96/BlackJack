using BlackJack.Models.Pokers;
using System;

namespace BlackJack.Models.Participants
{
    public class Player : Participant
    {
        public Player(string name) : base(name)
        {
        }

        public int Point { get; private set; }

        public override void DrawCard(Deck deck)
        {
            var card = deck.Draw();
            Cards.Add(card);
            Console.Write(string.Format("{0} has drawn one card: ", Name));
            card.Show();
            Console.Write(Environment.NewLine);
        }

        public bool IsTwoCardBlackJack() => Cards.Count == 2 && Point == 21;

        public void Win()
        {
            Point += IsTwoCardBlackJack() ? 15 : 10;

            var oriColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(string.Format("{0} has win the game, current points: {1}", Name, Point));
            Console.ForegroundColor = oriColor;
        }

        public void Tie()
        {
            Console.WriteLine(string.Format("{0} Tie, current points: {1}", Name, Point));
        }

        public void Lose()
        {
            Point -= 10;
            var oriColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("{0} has lose the game, current points: {1}", Name, Point));
            Console.ForegroundColor = oriColor;
        }
    }
}