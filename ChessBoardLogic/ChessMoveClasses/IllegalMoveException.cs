//
//
namespace TrevyBurgess.Games.TrevyChess.ChessBoardLogic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Illegal chess move
    /// </summary>
    public class IllegalMoveException : InvalidOperationException
    {
        public IllegalMoveException() : base() { }

        public IllegalMoveException(string message) : base(message) { }
    }
}
