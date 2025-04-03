using System;
using System.Diagnostics;

namespace TrevyBurgess.Games.TrevyChess.ChessBoardLogic
{
    public static class Library
    {
        /// <summary>
        /// Parse ColPos from character
        /// </summary>
        [DebuggerHidden]
        public static ColPos ParseColPos(char colPos)
        {
            return (ColPos)Enum.Parse(typeof(ColPos), colPos.ToString());
        }

        /// <summary>
        /// Parse RowPos from character
        /// </summary>
        [DebuggerHidden]
        public static RowPos ParseRowPos(char colPos)
        {
            return (RowPos)Enum.Parse(typeof(RowPos), colPos.ToString());
        }

        /// <summary>
        /// Get code for describing a chess piece
        /// </summary>
        public static char GetPieceCode(ChessPieceType pieceType, ChessPieceColor pieceColor)
        {
            if (pieceColor == ChessPieceColor.Black)
            {
                switch (pieceType)
                {
                    case ChessPieceType.Bishop:
                        return 'b';

                    case ChessPieceType.EmptySquare:
                        return ' ';

                    case ChessPieceType.King:
                        return 'k';

                    case ChessPieceType.Knight:
                        return 'n';

                    case ChessPieceType.Pawn:
                        return 'p';

                    case ChessPieceType.Queen:
                        return 'q';

                    case ChessPieceType.Rook:
                        return 'r';

                    default:
                        throw new NotSupportedException("Invalid piece type.");
                }
            }
            else if (pieceColor == ChessPieceColor.White)
            {
                switch (pieceType)
                {
                    case ChessPieceType.Bishop:
                        return 'B';

                    case ChessPieceType.EmptySquare:
                        return ' ';

                    case ChessPieceType.King:
                        return 'K';

                    case ChessPieceType.Knight:
                        return 'N';

                    case ChessPieceType.Pawn:
                        return 'P';

                    case ChessPieceType.Queen:
                        return 'Q';

                    case ChessPieceType.Rook:
                        return 'R';

                    default:
                        throw new NotSupportedException("Invalid piece type.");
                }
            }
            else
            {
                throw new NotSupportedException("Invalid piece color");
            }
        }

        public static void GetPieceType(char pieceCode, out ChessPieceType pieceType, out ChessPieceColor pieceColor)
        {
            switch (pieceCode)
            {
                case 'b':
                    pieceColor = ChessPieceColor.Black;
                    pieceType = ChessPieceType.Bishop;
                    break;

                case 'B':
                    pieceColor = ChessPieceColor.White;
                    pieceType = ChessPieceType.Bishop;
                    break;

                case ' ':
                    pieceColor = ChessPieceColor.None;
                    pieceType = ChessPieceType.EmptySquare;
                    break;

                case 'k':
                    pieceColor = ChessPieceColor.Black;
                    pieceType = ChessPieceType.King;
                    break;

                case 'K':
                    pieceColor = ChessPieceColor.White;
                    pieceType = ChessPieceType.King;
                    break;

                case 'n':
                    pieceColor = ChessPieceColor.Black;
                    pieceType = ChessPieceType.Knight;
                    break;

                case 'N':
                    pieceColor = ChessPieceColor.White;
                    pieceType = ChessPieceType.Knight;
                    break;

                case 'p':
                    pieceColor = ChessPieceColor.Black;
                    pieceType = ChessPieceType.Pawn;
                    break;

                case 'P':
                    pieceColor = ChessPieceColor.White;
                    pieceType = ChessPieceType.Pawn;
                    break;

                case 'q':
                    pieceColor = ChessPieceColor.Black;
                    pieceType = ChessPieceType.Queen;
                    break;

                case 'Q':
                    pieceColor = ChessPieceColor.White;
                    pieceType = ChessPieceType.Queen;
                    break;

                case 'r':
                    pieceColor = ChessPieceColor.Black;
                    pieceType = ChessPieceType.Rook;
                    break;

                case 'R':
                    pieceColor = ChessPieceColor.White;
                    pieceType = ChessPieceType.Rook;
                    break;

                default:
                    throw new NotSupportedException("Invalid piece code.");
            }
        }
    }
}
