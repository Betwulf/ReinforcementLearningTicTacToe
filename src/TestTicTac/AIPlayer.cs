using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace TestTicTac
{
    public class AIPlayer
    {
        public SpotState PlayerSymbol { get; set; }

        public MoveSet NextMoveTree { get; set; }

        public double LearningRate { get; set; }

        public double ExploreRate { get; set; }

        protected BoardMove LastMove { get; set; }

        public AIPlayer(SpotState symbol, double learnRate, double exploreRate)
        {
            LearningRate = learnRate;
            ExploreRate = exploreRate;
            PlayerSymbol = symbol;
            BoardState state = new BoardState();
            NextMoveTree = new MoveSet();
            // X always goes first
            Setup(state, SpotState.X);
            LastMove = null;
        }



        // populate all moves recursively until the game has ended
        private void Setup(BoardState state, SpotState playerTurn)
        {
            var currentMoves = new List<BoardMove>();

            for (int i = 0; i < BoardState.BoardSize * BoardState.BoardSize; i++)
            {
                if (state.BoardMap[i] == SpotState.Empty)
                {
                    // possible move
                    BoardState newState = new BoardState(state);
                    newState.BoardMap[i] = playerTurn;
                    var newMove = new BoardMove() { pos = i, boardState = newState, estimate = 0.5 };
                    var winner = newState.GetWinner();
                    if (winner == PlayerSymbol)
                    {
                        newMove.estimate = 1.0;
                    }
                    else if (winner == SpotState.Empty)
                    {
                        if (!newState.IsEnd())
                        {
                            var nextPlayerTurn = playerTurn == SpotState.X ? SpotState.O : SpotState.X;
                            Setup(newState, nextPlayerTurn);
                        }
                    }
                    else
                    {
                        newMove.estimate = 0.0;
                    }
                    currentMoves.Add(newMove);
                }
            }
            if (!NextMoveTree.ContainsKey(state.GetBoardHash()))
                NextMoveTree.Add(state.GetBoardHash(), currentMoves);
        }



        public int NextMove(BoardState state, bool printMoves = false)
        {
            var hash = state.GetBoardHash();
            if (state.IsEnd())
            {
                return -1;
            }
            // decide whether to explore a new random move, or pick the best known (greedy) move...
            var explore = RandomNumberGenerator.Create().GetDouble();
            if (explore < ExploreRate)
            {
                // then pick a move at random
                if (printMoves) Console.WriteLine($"Player {PlayerSymbol}: Random move");
                var rMoves = NextMoveTree[hash];
                rMoves.Shuffle();
                var randomMove = rMoves[0]; // shuffling randomizes the list, so just pick the first item in the list.
                Learn(randomMove.estimate);
                LastMove = randomMove;
                return randomMove.pos;
            }

            // otherwise pick the best move we know of (Greedy)
            var moves = NextMoveTree[hash];
            moves.Shuffle(); // this is an important step to randomly pick moves of equal probability, otherwise we would always pick the first value
            var topMove = moves.First(y => y.estimate == moves.Max(x => x.estimate));
            if (printMoves)
            {
                Console.WriteLine($"Player {PlayerSymbol}: top move = {topMove.pos} estimate = {topMove.estimate}");
                moves.ForEach(x => Console.WriteLine($"\t move: {x.pos} , estimate: {x.estimate}"));
            }

            Learn(topMove.estimate);
            LastMove = topMove;

            return topMove.pos;
        }

        // ITS ALMOST TOO EASY
        private void Learn(double chosenEstimate)
        {
            if (LastMove != null)
            {
                var moveEstimate = chosenEstimate;
                var oldEstimate = LastMove.estimate;
                LastMove.estimate = oldEstimate + LearningRate * (moveEstimate - oldEstimate);
            }
        }


        // at the end of the game you must learn even though you have no further moves.
        // also reset lastmove to nothing.
        public void Reset(SpotState winner)
        {
            if (winner != SpotState.Empty)
            {
                if (winner == PlayerSymbol)
                {
                    // winner winner chicken dinner!
                    Learn(1.0);
                }
                else
                {
                    // you lost.
                    Learn(0.0);
                }
            }
            LastMove = null;
        }


    }
}
