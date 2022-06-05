using BlackJack.Models.Participants;
using System.Linq;

namespace BlackJack.Strategies
{
    public class NormalCalculationStrategy : CalculationStrategy
    {
        public override int CalculateScore(Participant participant)
        {
            return participant.Cards.Sum(x => GetCardScore(x));
        }
    }
}