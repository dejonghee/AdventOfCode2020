using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Core
{
    public class Day22 : Day<int, int>
    {
        protected override IProblem<int> GetPart1() => new Part1();

        protected override IProblem<int> GetPart2() => new Part2();

        private class Part1 : IProblem<int>
        {
            public virtual async Task<int> SolveAsync(IAsyncEnumerable<string> input)
            {
                var (player1, player2) = await Parser.ParseInputAsync(input);

                while (player1.Count > 0 && player2.Count > 0)
                {
                    var cardPlayer1 = player1.Dequeue();
                    var cardPlayer2 = player2.Dequeue();

                    if (cardPlayer1 > cardPlayer2)
                    {
                        player1.Enqueue(cardPlayer1);
                        player1.Enqueue(cardPlayer2);
                    }
                    else
                    {
                        player2.Enqueue(cardPlayer2);
                        player2.Enqueue(cardPlayer1);
                    }
                }

                var winner = player1.Count > 0
                    ? player1
                    : player2;

                return CalculatorScore(winner);
            }



            protected int CalculatorScore(Queue<int> deck)
            {
                var score = 0;

                var multiplier = deck.Count;
                while (deck.Count > 0)
                {
                    var card = deck.Dequeue();
                    score += card * multiplier;
                    multiplier--;
                }

                return score;
            }
        }

        private class Part2 : Part1
        {
            public override async Task<int> SolveAsync(IAsyncEnumerable<string> input)
            {
                var (inputPlayer1, inputPlayer2) = await Parser.ParseInputAsync(input);

                var (player1, player2) = StartNewGame(inputPlayer1, inputPlayer2);

                var winner = player1.Count > 0
                    ? player1
                    : player2;

                return CalculatorScore(winner);
            }

            private (Queue<int> player1, Queue<int> player2) StartNewGame(Queue<int> player1, Queue<int> player2)
            {
                var previousRounds = new HashSet<string>();

                while (player1.Count > 0 && player2.Count > 0)
                {
                    var inputHash = GenerateHash(player1, player2);
                    if (previousRounds.Contains(inputHash))
                    {
                        // Game ends, player 1 wins.
                        return (player1, new Queue<int>());
                    }

                    var cardPlayer1 = player1.Dequeue();
                    var cardPlayer2 = player2.Dequeue();

                    if (player1.Count >= cardPlayer1 &&
                        player2.Count >= cardPlayer2)
                    {
                        var copyPlayer1 = new Queue<int>(player1.Take(cardPlayer1));
                        var copyPlayer2 = new Queue<int>(player2.Take(cardPlayer2));

                        var (resultPlayer1, resultPlayer2) = StartNewGame(copyPlayer1, copyPlayer2);

                        if (resultPlayer1.Count > 0)
                        {
                            player1.Enqueue(cardPlayer1);
                            player1.Enqueue(cardPlayer2);
                        }
                        else
                        {
                            player2.Enqueue(cardPlayer2);
                            player2.Enqueue(cardPlayer1);
                        }
                    }
                    else
                    {
                        if (cardPlayer1 > cardPlayer2)
                        {
                            player1.Enqueue(cardPlayer1);
                            player1.Enqueue(cardPlayer2);
                        }
                        else
                        {
                            player2.Enqueue(cardPlayer2);
                            player2.Enqueue(cardPlayer1);
                        }
                    }

                    previousRounds.Add(inputHash);
                }

                return (player1, player2);
            }

            private string GenerateHash(Queue<int> player1, Queue<int> player2)
            {
                return $"{string.Join(",", player1)}-{string.Join(",", player2)}";
            }
        }

        private static class Parser
        {
            public static async Task<(Queue<int> player1, Queue<int> player2)> ParseInputAsync(IAsyncEnumerable<string> input)
            {
                var enumerator = input.GetAsyncEnumerator();

                // Skip line 'Player 1:'
                await enumerator.MoveNextAsync();

                var player1 = await ParsePlayerAsync(enumerator);

                // Skip line 'Player 2:'
                await enumerator.MoveNextAsync();

                var player2 = await ParsePlayerAsync(enumerator);

                return (player1, player2);
            }

            private static async Task<Queue<int>> ParsePlayerAsync(IAsyncEnumerator<string> enumerator)
            {
                var cards = new Queue<int>();

                while (await enumerator.MoveNextAsync() && string.IsNullOrEmpty(enumerator.Current) == false)
                {
                    var card = int.Parse(enumerator.Current);
                    cards.Enqueue(card);
                }

                return cards;
            }
        }
    }
}
