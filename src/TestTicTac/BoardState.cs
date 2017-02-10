using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTicTac
{
    public enum SpotState
    {
        Empty = 0,
        X = 1,
        O = -1
    }
    public class BoardState
    {
        public SpotState[] BoardMap { get; set; }

        public static int BoardSize { get; private set; }

        public BoardState()
        {
            BoardSize = 3;
            BoardMap = new SpotState[BoardSize * BoardSize];
        }

        public BoardState(BoardState original)
        {
            BoardMap = (SpotState[])original.BoardMap.Clone();
        }

        // Determine if there are no moves left to play
        public bool IsEnd()
        {
            for (int i = 0; i < BoardSize * BoardSize; i++)
            {
                if (BoardMap[i] == SpotState.Empty)
                {
                    return false;
                }
            }
            return true;
        }


        // Who won? Returns empty if no one has won, or the game is not yet over.
        public SpotState GetWinner()
        {

            var results = new List<int>();

            // check horizontals
            for (int x = 0; x < BoardSize; x++)
            {
                int result = 0;
                for (int y = 0; y < BoardSize; y++)
                {
                    result += (int)BoardMap[x * BoardSize + y];
                }
                results.Add(result);
            }

            // check verticals
            for (int y = 0; y < BoardSize; y++)
            {
                int result = 0;
                for (int x = 0; x < BoardSize; x++)
                {
                    result += (int)BoardMap[x * BoardSize + y];
                }
                results.Add(result);
            }

            // check diagonals
            int diagonalResult = 0;
            int diagonalBackResult = 0;
            for (int i = 0; i < BoardSize; i++)
            {
                diagonalResult += (int)BoardMap[i * BoardSize + i];
                diagonalBackResult += (int)BoardMap[i * BoardSize + (BoardSize - i - 1)];
            }
            results.Add(diagonalResult);
            results.Add(diagonalBackResult);

            if (results.Any(x => x == (int)SpotState.X * BoardSize))
                return SpotState.X;
            if (results.Any(x => x == (int)SpotState.O * BoardSize))
                return SpotState.O;
            return SpotState.Empty;
        }


        // hardcoding to 3
        public string PrintBoard()
        {
            string retString = BoardMap[0] + " | " + BoardMap[1] + " | " + BoardMap[2] + "\r\n";
            retString +=  "--|--" + "-|--" + "\r\n";
            retString += BoardMap[3] + " | " + BoardMap[4] + " | " + BoardMap[5] + "\r\n";
            retString += "--|--" + "-|--" + "\r\n";
            retString += BoardMap[6] + " | " + BoardMap[7] + " | " + BoardMap[8] + "\r\n";            
            return retString.Replace("Empty", " ");
        }


        // need to refer to this unique Board state
        public int GetBoardHash()
        {
            int hash = 17;
            for (int i = 0; i < BoardSize*BoardSize; i++)
            {
                hash = hash * 19 + (int)BoardMap[i];
            }
            return hash;
        }
    }
}
