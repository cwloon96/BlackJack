using BlackJack.Models.Participants;
using BlackJack.Models.Pokers.Cards;
using System.Linq;

namespace BlackJack.Strategies
{
    public class MultipleAceScoreCalculationStrategy : CalculationStrategy
    {
        public override int CalculateScore(Participant participant)
        {
            var aces = participant.Cards.Where(x => x is Ace).ToList();

            if (aces.Count == 0)
                return participant.Cards.Sum(x => GetCardScore(x));

            // count only not Ace
            int finalScore = participant.Cards.Where(x => x is not Ace).Sum(x => GetCardScore(x));

            // Only one Ace can be 11, so add extra Ace score
            while (aces.Count > 1)
            {
                finalScore += 1;
                aces.RemoveAt(0);
            }

            // if score less than 10, ace can be used as 11
            if (finalScore <= 10)
                return finalScore + 11;

            return finalScore + 1;
        }
    }
}