using BlackJack.Models.Participants;
using BlackJack.Models.Pokers;

namespace BlackJack.Strategies
{
    public abstract class CalculationStrategy
    {
        protected int GetCardScore(Card card)
        {
            // able to parse 1 - 10
            if (int.TryParse(card.Value, out int score))
                return score;

            // J,Q,K = 10
            return 10;
        }

        public abstract int CalculateScore(Participant participant);
    }
}