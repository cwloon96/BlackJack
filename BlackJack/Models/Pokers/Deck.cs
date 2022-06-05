using System;
using System.Collections.Generic;

namespace BlackJack.Models.Pokers
{
    public class Deck
    {
        private Random _random;

        private List<Card> _cards { get; set; }

        public Deck()
        {
            _random = new Random();
            _cards = new List<Card>();
        }

        public int RemainingCards => _cards.Count;

        public void AddCards(List<Card> cards) => _cards.AddRange(cards);

        public void Shuffle()
        {
            for (int i = 0; i < _cards.Count; i++)
            {
                int newIndex = _random.Next(i, _cards.Count);
                Card tempCard = _cards[newIndex];
                _cards[newIndex] = _cards[i];
                _cards[i] = tempCard;
            }
        }

        public Card Draw()
        {
            Card drawnCard = _cards[0];
            _cards.RemoveAt(0);
            return drawnCard;
        }
    }
}