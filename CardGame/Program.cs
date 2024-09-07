public class CardGame
{
    public static void Main(string[] args)
    {
        //enter player on seat
        Console.Write("Enter Player On Seat: ");
        var playerNumber = Convert.ToInt32(Console.ReadLine());

        //generate cards
        var cards = GenerateShuffledCards();
       
        //initialize number of players
        var players = InitializePlayers(playerNumber);

        //deal the cards for players
        var CardPlayerHolding = DealCards(players, cards);

        //show result
        GenerateResult(CardPlayerHolding);
    }

    private static List<string> GenerateShuffledCards()
    {
        var symbols = new List<string>()
        {
            "@", "#", "^", "*"
        };

        var cards = new List<string>();
        foreach (var symbol in symbols)
        {
            //generate cards
            for (var number = 1; number < 14; number++)
            {
                var card = number switch
                {
                    1 => "A",
                    11 => "J",
                    12 => "Q",
                    13 => "K",
                    _ => number.ToString()
                };

                card += symbol;
                cards.Add(card);
            }
        }

        //use random guid as order to shuffle the cards
        cards = cards.OrderBy(card => Guid.NewGuid()).ToList();

        //checking result
        // foreach (var card in cards)
        // {
        //     Console.WriteLine (card);
        // }
        Console.WriteLine("Cards Shuffled.");
        return cards;
    }

    private static List<string>[] InitializePlayers(int playerNumber)
    {
        var players = new List<string>[playerNumber];
        for (int i = 0; i < playerNumber; i++)
        {
            players[i] = new List<string>();
        }

        return players;
    }

    private static List<string>[] DealCards(List<string>[] players, List<string> cards)
    {
        Console.WriteLine("Dealing Cards.");
        for (int i = 0; i < cards?.Count(); i++)
        {
            var card = cards[i];

            int playerIndex = i % players.Count();
            players[playerIndex].Add(card);
        }

        Console.WriteLine("Cards in Player's Hand.");
        //checking result
        int seat = 1;
        foreach (var player in players)
        {
            Console.Write("Player " + seat + ": {");
            foreach (var pl in player)
            {
                Console.Write(pl + ", ");
            }
            Console.WriteLine("},");

            seat++;
        }
        return players;
    }

    private static void GenerateResult(List<string>[] playersCard)
    {
        var winnerSeat = 0;
        var highestOccurence = 0;
        var highestCardNumber = 0;
        var result = new Dictionary<int, List<string>>();

        for (int i = 0; i < playersCard.Count(); i++)
        {
            var cards = playersCard[i];
            var mappingResult = MapCardNumber(cards);
            var highestCard = mappingResult
                .OrderByDescending(x => x.Value.Count)
                .ThenByDescending(x => ConvertCardToNumber(x.Key))
                .FirstOrDefault();

            result[i] = highestCard.Value;

            var cardNumber = ConvertCardToNumber(highestCard.Key);

            if (highestCard.Value.Count() > highestOccurence)
            {
                winnerSeat = i + 1;
                highestOccurence = highestCard.Value.Count();
                highestCardNumber = cardNumber;
            }
            else if (highestCard.Value.Count() == highestOccurence)
            {
                if (cardNumber > highestCardNumber)
                {
                    winnerSeat = i + 1;
                    highestCardNumber = cardNumber;
                }
                else if (cardNumber == highestCardNumber)
                {
                    if (IsCurrentCardLargest(cards, result.Values.SelectMany(v => v).ToList()))
                    {
                        winnerSeat = i + 1;
                        highestCardNumber = cardNumber;
                    }
                }
            }
        }

        Console.WriteLine();
        Console.WriteLine("Result :");
        foreach (var data in result)
        {
            Console.WriteLine(
               $"Player {data.Key + 1}: {string.Join(",", data.Value)}"
           );
        }

        Console.WriteLine();
        Console.WriteLine("The Winner is Player " + winnerSeat);
    }

    private static Dictionary<string, List<string>> MapCardNumber(List<string> cards)
    {
        var cardNumberMap = new Dictionary<string, List<string>>();

        foreach (var card in cards)
        {
            var number = card.Substring(0, card.Length - 1);
            if (!cardNumberMap.ContainsKey(number))
            {
                cardNumberMap[number] = new List<string>();
            }
            cardNumberMap[number].Add(card);
        }

        // foreach (var map in cardNumberMap)
        // {
        //     Console.WriteLine(map.Key);
        //     Console.WriteLine(map.Value);
        // }

        return cardNumberMap;
    }

    private static int ConvertCardToNumber(string card)
    {
        return card switch
        {
            "J" => 11,
            "Q" => 12,
            "K" => 13,
            "A" => 14,
            _ => int.Parse(card)
        };
    }

    private static bool IsCurrentCardLargest(List<string> currentCards, List<string> previousCards)
    {
        var currentRank = GetHighestRank(currentCards);
        var previousRank = GetHighestRank(previousCards);

        return currentRank > previousRank;
    }

    private static int GetHighestRank(List<string> cards)
    {
        var symbolRankings = new Dictionary<char, int>
        {
            { '@', 4 },
            { '#', 3 },
            { '^', 2 },
            { '*', 1 }
        };

        foreach (var card in cards)
        {
            foreach (var symbol in symbolRankings.Keys)
            {
                if (card.Contains(symbol))
                {
                    return symbolRankings[symbol];
                }
            }
        }
        return 0;
    }
}





