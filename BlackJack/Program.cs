using BlackJack.Models.Participants;
using BlackJack.Models.Pokers;
using BlackJack.Models.Pokers.Cards;
using BlackJack.Strategies;
using System;
using System.Collections.Generic;

namespace BlackJack
{
    class Program
    {
        private const int MAXIMUM_PLAYER = 10;

        private static Deck _deck;
        private static Participant[] _participants;
        private static CalculationStrategy _playerCalculationStrategy;

        static void Main(string[] args)
        {
            bool startNewGame = true;
            while (true)
            {
                if (startNewGame)
                {
                    int playerCount = GetPlayerCount();
                    _playerCalculationStrategy = GetCalculationStrategy();
                    InitParticipants(playerCount);
                }

                InitGame();
                ProcessGame();

                bool exit;
                (startNewGame, exit) = RestartSelection();

                if (exit)
                    break;
            }

            Console.WriteLine("Thanks for playing! Good bye!");
        }

        static int GetPlayerCount()
        {
            int playerCount = 0;

            do
            {
                Console.WriteLine($"Please enter player number (maximum {MAXIMUM_PLAYER}): ");
                string userInput = Console.ReadLine();

                if (!int.TryParse(userInput, out int parsedUserInput))
                    Console.WriteLine("Please enter valid number.");
                else
                {
                    if (parsedUserInput < 1)
                        Console.WriteLine("Number cannot be smaller than 1.");
                    else if (parsedUserInput > MAXIMUM_PLAYER)
                        Console.WriteLine($"Number cannot be larger than {MAXIMUM_PLAYER}.");
                    else
                        playerCount = parsedUserInput;
                }
            }
            while (playerCount == 0);

            return playerCount;
        }

        private static CalculationStrategy GetCalculationStrategy()
        {
            CalculationStrategy strategy = null;

            do
            {
                Console.WriteLine("Please enter score calculation rules: ");
                Console.WriteLine("1. Normal Calculation");
                Console.WriteLine("2. Ace can be 1 or 11 (Player only)");
                string userInput = Console.ReadLine();

                if (!int.TryParse(userInput, out int parsedUserInput))
                    Console.WriteLine("Please enter valid number.");
                else
                {
                    if (parsedUserInput == 1)
                        strategy = new NormalCalculationStrategy();
                    else if (parsedUserInput == 2)
                        strategy = new MultipleAceScoreCalculationStrategy();
                    else
                        Console.WriteLine("Invalid selection");
                }
            }
            while (strategy == null);

            return strategy;
        }

        static void InitParticipants(int player)
        {
            // player + 1 Dealer
            _participants = new Participant[player + 1];
            for (int i = 0; i < player; i++)
                _participants[i] = new Player($"Player {i + 1}");

            _participants[player] = new Dealer("Dealer");
        }

        static void InitGame()
        {
            Console.WriteLine("Starting New Game.....");
            _deck = new Deck();

            #region Create Suit
            var suits = new List<CardSuit>();
            suits.Add(new CardSuit("♠", ConsoleColor.DarkGray));
            suits.Add(new CardSuit("♥", ConsoleColor.Red));
            suits.Add(new CardSuit("♣", ConsoleColor.DarkGray));
            suits.Add(new CardSuit("♦", ConsoleColor.Red));
            #endregion

            #region Create Card
            var cards = new List<Card>();
            foreach (var suit in suits)
            {
                cards.Add(new Ace(suit));

                for (int i = 2; i <= 10; i++)
                    cards.Add(new Card(i.ToString(), suit));

                cards.Add(new Card("J", suit));
                cards.Add(new Card("Q", suit));
                cards.Add(new Card("K", suit));
            }
            #endregion

            _deck.AddCards(cards);
            _deck.Shuffle();

            foreach (var participant in _participants)
                participant.Reset();

            // each participant will draw 2 card
            for (int i = 0; i < _participants.Length * 2; i++)
                _participants[i % _participants.Length].DrawCard(_deck);
        }

        static void ProcessGame()
        {
            var remainingPlayers = new List<Player>();

            // not including dealer
            for (int i = 0; i < _participants.Length - 1; i++)
            {
                ParticipantTurn(_participants[i], _playerCalculationStrategy);
                if (!_participants[i].IsExceedScore)
                    remainingPlayers.Add((Player)_participants[i]);

                // too less card in deck, collect card from ended player & reshuffle
                if (_deck.RemainingCards <= 15)
                {
                    for (int j = 0; j <= i; j++)
                        _deck.AddCards(_participants[j].Cards);

                    _deck.Shuffle();
                }
            }

            var dealer = (Dealer)_participants[_participants.Length - 1];
            dealer.ShowHiddenCard();

            if (remainingPlayers.Count > 0)
            {
                // dealer always normal calculation
                ParticipantTurn(dealer, new NormalCalculationStrategy());

                Console.WriteLine("Result for remaining players:");
                if (dealer.IsExceedScore)
                {
                    foreach (var player in remainingPlayers)
                        player.Win();
                }
                else
                {
                    foreach (var player in remainingPlayers)
                    {
                        if (player.Score < dealer.Score)
                            player.Lose();
                        else if (player.Score > dealer.Score)
                            player.Win();
                        else
                            player.Tie();
                    }
                }
            }
            else
            {
                Console.WriteLine("No remaining player, dealer win the game!");
            }
        }

        static void ParticipantTurn(Participant participant, CalculationStrategy calculationStrategy)
        {
            participant.CalculateScore(calculationStrategy);
            Console.WriteLine("```````````````````````````````````````````````````````````");
            Console.WriteLine(string.Format("{0}\'s Turn!", participant.Name));

            while (participant.IsAbleToDraw())
            {
                Console.Write("Current Cards: ");

                foreach (var card in participant.Cards)
                {
                    card.Show();
                    Console.Write(" | ");
                }

                Console.Write(Environment.NewLine);
                participant.ShowScore();

                if (!IsHit())
                    break;

                participant.DrawCard(_deck);
                participant.CalculateScore(calculationStrategy);
            }

            participant.ShowScore();

            if (participant.IsExceedScore)
            {
                Console.WriteLine(string.Format("Boom! {0} has exceed 21 score!", participant.Name));
                if (participant is Player)
                    ((Player)participant).Lose();
            }

            Console.WriteLine(string.Format("{0}\'s Turn End!", participant.Name));
            Console.WriteLine("```````````````````````````````````````````````````````````");
        }

        static bool IsHit()
        {
            bool? hit = null;

            do
            {
                Console.WriteLine("Please select your action:");
                Console.WriteLine("1. Hit");
                Console.WriteLine("2. Stay");
                string userInput = Console.ReadLine();

                if (!int.TryParse(userInput, out int parsedUserInput))
                    Console.WriteLine("Please enter valid number.");
                else
                {
                    if (parsedUserInput == 1)
                        hit = true;
                    else if (parsedUserInput == 2)
                        hit = false;
                    else
                        Console.WriteLine("Invalid selection");
                }
            }
            while (!hit.HasValue);

            return hit.Value;
        }

        static (bool, bool) RestartSelection()
        {
            bool? startNewGame = null;
            bool? exit = null;

            do
            {
                Console.WriteLine("Please select your action:");
                Console.WriteLine("1. New Game");
                Console.WriteLine("2. New Round");
                Console.WriteLine("3. Exit");
                string userInput = Console.ReadLine();

                if (!int.TryParse(userInput, out int parsedUserInput))
                    Console.WriteLine("Please enter valid number.");
                else
                {
                    if (parsedUserInput == 1)
                        startNewGame = true;
                    else if (parsedUserInput == 2)
                    {
                        startNewGame = false;
                        exit = false;
                    }
                    else if (parsedUserInput == 3)
                        exit = true;
                    else
                        Console.WriteLine("Invalid selection");
                }
            }
            while (!startNewGame.HasValue && !exit.HasValue);

            return (startNewGame.HasValue ? startNewGame.Value : false,
                exit.HasValue ? exit.Value : false);
        }
    }
}