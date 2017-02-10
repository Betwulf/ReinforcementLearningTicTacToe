using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTicTac
{
    public class BoardMove
    {
        public BoardState boardState { get; set; }

        public int pos { get; set; }

        public double estimate { get; set; }

    }

    // for each hash of a boardstate, store a list of all next moves a player can take, with estimates
    public class MoveSet : Dictionary<int, List<BoardMove>>
    {

    }
}
