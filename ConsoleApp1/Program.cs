using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {

        static void Main(string[] args)
        {
            var deck = new Deck();
            var rnd = new Random();

            Console.WriteLine("Press ENTER to drop X amount of cards");

            do
            {
                while (!Console.KeyAvailable)
                {
                    Console.WriteLine(string.Join(", ", deck.GetDroppedCards(rnd.Next(1, 6))));
                    Console.WriteLine($"Decks used: {deck.GetDecksUsedCount()}");
                    Console.ReadLine();
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }

        class Deck
        {
            private readonly List<Card> _cards;
            private static int _cardsUsedCounter = 0;
            private static int _decksUsedCounter = 0;
            private const int NumberOfCards = 52;

            public Deck()
            {
                _cards = new List<Card>();

                InitNewDeck();
            }

            public int GetDecksUsedCount()
            {
                return _decksUsedCounter;
            }

            public IEnumerable<Card> GetDroppedCards(int numberOfCards)
            {
                if (!_cards.Any())
                    InitNewDeck();

                if (numberOfCards > _cards.Count)
                    numberOfCards = _cards.Count;

                var result = _cards.Take(numberOfCards)
                    .GroupBy(o => o.Suit)
                    .OrderByDescending(g => g.Count())
                    .SelectMany(x => x.OrderByDescending(c => c.Value)).ToList();

                foreach (var card in result.ToList())
                {
                    _cards.Remove(card);
                    _cardsUsedCounter += 1;

                    if (_cardsUsedCounter == NumberOfCards)
                    {
                        _decksUsedCounter += 1;
                        _cardsUsedCounter = 0;
                    }
                }

                return result;
            }

            private void InitNewDeck()
            {
                for (var s = 0; s < Enum.GetNames(typeof(Suit)).Length; s++)
                {
                    for (var v = 0; v < Enum.GetNames(typeof(Value)).Length; v++)
                    {
                        _cards.Add(new Card
                        {
                            Suit = (Suit)s,
                            Value = (Value)v
                        });
                    }
                }

                Shuffle();
            }

            private void Shuffle()
            {
                var rnd = new Random();

                for (var i = 0; i < NumberOfCards; i++)
                {
                    var r = rnd.Next(i, _cards.Count);
                    var rc = _cards[r];
                    _cards[r] = _cards[i];
                    _cards[i] = rc;
                }
            }
        }

        class Card
        {
            public Value Value { get; set; }
            public Suit Suit { get; set; }

            public override string ToString()
            {
                return $"{Suit}{Value}";
            }
        }

        enum Suit
        {
            D,
            H,
            S,
            K
        }

        enum Value
        {
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Ace,
            Ten,
            Jack,
            Queen,
            King,
        }
    }
}
