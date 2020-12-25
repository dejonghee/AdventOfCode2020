using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Core
{
    public class Day25 : Day<long, string>
    {
        protected override IProblem<long> GetPart1() => new Part1();

        protected override IProblem<string> GetPart2()
        {
            throw new NotImplementedException();
        }

        private class Part1 : IProblem<long>
        {
            private bool ValidateAnswer = true;

            public async Task<long> SolveAsync(IAsyncEnumerable<string> input)
            {
                var (publicKeyDoor, publicKeyCard) = await ParseInputAsync(input);

                long encryptionKey;
                if (publicKeyCard < publicKeyDoor)
                {
                    var loopSize = DetermineLoopSize(publicKeyCard);
                    encryptionKey = Transform(publicKeyDoor, loopSize);
                }
                else
                {
                    var loopSize = DetermineLoopSize(publicKeyDoor);
                    encryptionKey = Transform(publicKeyCard, loopSize);
                }

                if (ValidateAnswer)
                {
                    long encryptionKey2;
                    if (publicKeyCard < publicKeyDoor)
                    {
                        var loopSize = DetermineLoopSize(publicKeyDoor);
                        encryptionKey2 = Transform(publicKeyCard, loopSize);
                    }
                    else
                    {
                        var loopSize = DetermineLoopSize(publicKeyCard);
                        encryptionKey2 = Transform(publicKeyDoor, loopSize);
                    }

                    if (encryptionKey != encryptionKey2)
                        throw new Exception("Keys should be the same.");
                }

                return encryptionKey;
            }

            private async Task<(int publicKeyDoor, int publicKeyCard)> ParseInputAsync(IAsyncEnumerable<string> input)
            {
                var enumerator = input.GetAsyncEnumerator();
                await enumerator.MoveNextAsync();
                var publicKeyDoor = int.Parse(enumerator.Current);
                await enumerator.MoveNextAsync();
                var publicKeyCard = int.Parse(enumerator.Current);
                return (publicKeyDoor, publicKeyCard);
            }

            private int DetermineLoopSize(int publicKey)
            {
                const int subjectNumber = 7;

                var iteration = 0;
                var value = 1;

                while (value != publicKey)
                {
                    iteration++;

                    value *= subjectNumber;
                    value %= 20201227;
                }

                return iteration;
            }

            private long Transform(int subjectNumber, int loopSize)
            {
                var value = 1L;

                for (var iteration = 1; iteration <= loopSize; iteration++)
                {
                    value *= subjectNumber;
                    value %= 20201227;
                }

                return value;
            }
        }
    }
}
