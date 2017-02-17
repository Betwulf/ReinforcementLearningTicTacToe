using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTicTac
{
    public class Trainer
    {
        public AIPlayer PlayerX { get; set; }

        public AIPlayer PlayerO { get; set; }

        // Set the 2 players
        public Trainer()
        {
            PlayerX = new AIPlayer(SpotState.X, 0.1, 0.1);
            PlayerO = new AIPlayer(SpotState.O, 0.1, 0.1);
        }


        public void Train(int epochs)
        {
            for (int i = 0; i < epochs; i++)
            {
                if (i % 1000 == 0)
                {
                    Console.WriteLine($"Epoch {i}");
                }

                BoardState state = new BoardState();
                bool isPlayerXTurn = true; // X always starts first
                while (!state.IsEnd() && state.GetWinner() == SpotState.Empty)
                {
                    if (isPlayerXTurn)
                    {
                        var p1move = PlayerX.NextMove(state);
                        state.BoardMap[p1move] = SpotState.X;
                    }
                    else
                    {
                        var p2move = PlayerO.NextMove(state);
                        state.BoardMap[p2move] = SpotState.O;
                    }
                    isPlayerXTurn = !isPlayerXTurn;
                }
                PlayerO.Reset(state.GetWinner());
                PlayerX.Reset(state.GetWinner());
            }
        }


        // seek command line user input in a game between man and machine.
        private int GetUserMove(BoardState state)
        {
            bool noMove = true;
            while (noMove)
            {
                Console.WriteLine("Type your move:");
                string userMove = Console.ReadLine();
                int userPos;
                if (int.TryParse(userMove, out userPos))
                {
                    if (state.BoardMap[userPos] == SpotState.Empty)
                    {
                        noMove = false;
                        return userPos;
                    }
                    else
                    {
                        Console.WriteLine("Spot taken. Try again.");
                    }
                }
            }
            return -1;
        }


        // pit a human againt an AI
        public void Play(SpotState userSymbol)
        {
            PlayerX.ExploreRate = 0.0;
            PlayerO.ExploreRate = 0.0;
            if (userSymbol == SpotState.Empty)
            {
                Console.WriteLine("pick a side");
                return;
            }
            BoardState state = new BoardState();
            bool isPlayerXTurn = true; // X always starts first
            while (!state.IsEnd() && state.GetWinner() == SpotState.Empty)
            {
                Console.Write(state.PrintBoard());
                if (isPlayerXTurn)
                {
                    int p1move;
                    if (userSymbol == SpotState.X)
                    {
                        p1move = GetUserMove(state);
                    }
                    else
                    {
                        p1move = PlayerX.NextMove(state, true);
                    }
                    state.BoardMap[p1move] = SpotState.X;
                }
                else
                {
                    int p2move;
                    if (userSymbol == SpotState.O)
                    {
                        p2move = GetUserMove(state);
                    }
                    else
                    {
                        p2move = PlayerO.NextMove(state, true);
                    }
                    state.BoardMap[p2move] = SpotState.O;
                }
                isPlayerXTurn = !isPlayerXTurn;
            }

            PlayerO.Reset(state.GetWinner());
            PlayerX.Reset(state.GetWinner());

            if (state.GetWinner() == SpotState.Empty)
            {
                Console.WriteLine("* TIE");
            }
            else
            {
                Console.WriteLine($"* {state.GetWinner()} WINS.");
            }
        }
    }
}
