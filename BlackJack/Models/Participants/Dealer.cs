using BlackJack.Models.Pokers;
using BlackJack.Models.Pokers.Cards;
using System;

namespace BlackJack.Models.Participants
{
    public class Dealer : Participant
    {
        public Dealer(string name) : base(name)
        {
        }

        public Card HiddenCard { get; private set; }

        public override void DrawCard(Deck deck)
        {
            var card = deck.Draw();

            if (HiddenCard == null)
            {
                HiddenCard = card;
                Console.WriteLine(string.Format("{0} has drawn one hidden card", Name));
            }
            else
            {
                Cards.Add(card);
                Console.Write(string.Format("{0} has drawn one card: ", Name));
                card.Show();
                Console.Write(Environment.NewLine);
            }
        }

        public void ShowHiddenCard()
        {
            Console.Write(string.Format("{0}\'s hidden card is: ", Name));
            HiddenCard.Show();
            Console.Write(Environment.NewLine);

            Cards.Add(HiddenCard);
            // Can't set to null else it will be setted while draw card
            HiddenCard = new EmptyCard();
        }

        public override bool IsAbleToDraw() => Score <= 16 && base.IsAbleToDraw();

        public override void Reset()
        {
            HiddenCard = null;
            base.Reset();
        }
    }
}