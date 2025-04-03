//
//
namespace TrevyBurgess.Games.TrevyChess.ChessGameUI.Controls
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using TrevyBurgess.Games.TrevyChess.ChessBoardLogic;
    using TrevyBurgess.Games.TrevyChess.ChessGameUI.HelperClasses.Interfaces;

    /// <summary>
    /// The chess piece knows where it is on the board
    /// </summary>
    public partial class ChessPieceControl : UserControl
    {
        #region Initialization code
        Action<ChessPieceControl, ChessPieceLocation, bool> moveAction;
        Func<ChessPieceLocation, bool> canMoveFunc;

        public ChessPieceControl(ChessBoardControl theChessBoard, Action<ChessPieceControl, ChessPieceLocation, bool> moveAction, Func<ChessPieceLocation, bool> canMoveFunc)
        {
            InitializeComponent();

            initialTransform = base.RenderTransform;

            this.theChessBoard = theChessBoard;
            this.moveAction = moveAction;
            this.canMoveFunc = canMoveFunc;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Set initial position of chess piece
                Grid.SetColumn(this, (int)PieceColumnPosition);
                Grid.SetRow(this, (int)(9 - PieceRowPosition));
                pieceImage = chessPieceImage.Source;
            }
            catch
            {
                throw new ArgumentException("Chess piece should be child of the grid of the ChessBoardUi UserControl.");
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// The Chess board the chess piece is located on
        /// </summary>
        private ChessBoardControl theChessBoard { get; set; }

        /// <summary>
        /// Chess piece has been selected
        /// </summary>
        private bool isSelected;

        private Transform initialTransform;
        private Point moveLocation;
        private Transform moveTransform;
        private ImageSource pieceImage;
        #endregion

        #region Dependency Properties (http://msdn.microsoft.com/en-us/magazine/cc794276.aspx)
        /// <summary>
        /// Piece Type: King, Queen, Knight, Bichop, Rook, Pawn
        /// </summary>
        public ChessPieceType PieceType
        {
            get { return (ChessPieceType)base.GetValue(PieceTypeProperty); }
            set { base.SetValue(PieceTypeProperty, value); }
        }
        public static readonly DependencyProperty PieceTypeProperty =
            DependencyProperty.Register("PieceType", typeof(ChessPieceType), typeof(ChessPieceControl));

        /// <summary>
        /// Piece color: Black, White
        /// </summary>
        public ChessPieceColor PieceColor
        {
            get { return (ChessPieceColor)base.GetValue(PieceColorProperty); }
            set { base.SetValue(PieceColorProperty, value); }
        }
        public static readonly DependencyProperty PieceColorProperty =
            DependencyProperty.Register("PieceColor", typeof(ChessPieceColor), typeof(ChessPieceControl));

        /// <summary>
        /// Piece location
        /// </summary>
        public ChessPieceLocation PieceLocation
        {
            get
            {
                return new ChessPieceLocation(PieceColumnPosition, PieceRowPosition);
            }
            set
            {
                PieceColumnPosition = PieceLocation.ColLoc;
                PieceRowPosition = PieceLocation.RowLoc;
            }
        }

        /// <summary>
        /// Piece ColPos
        /// </summary>
        public ColPos PieceColumnPosition
        {
            get { return (ColPos)this.GetValue(PieceColumnPositionProperty); }
            set { this.SetValue(PieceColumnPositionProperty, value); }
        }
        public static readonly DependencyProperty PieceColumnPositionProperty =
            DependencyProperty.Register
            (
                "PieceColumnPosition",
                typeof(ColPos),
                typeof(ChessPieceControl),
                new PropertyMetadata
                (
                    (obj, args) =>
                    {
                        ChessPieceControl piece = obj as ChessPieceControl;
                        ColPos colPos = (ColPos)args.NewValue;

                        piece.RenderTransform = piece.initialTransform;
                        Grid.SetColumn(piece, (int)colPos);
                    }
                )
            );

        /// <summary>
        /// Image RowPos
        /// </summary>
        public RowPos PieceRowPosition
        {
            get { return (RowPos)this.GetValue(PieceRowPositionProperty); }
            set { this.SetValue(PieceRowPositionProperty, value); }
        }
        public static readonly DependencyProperty PieceRowPositionProperty =
            DependencyProperty.Register
            (
                "PieceRowPosition",
                typeof(RowPos),
                typeof(ChessPieceControl),
                new PropertyMetadata
                    (new PropertyChangedCallback(
                        (obj, args) =>
                        {
                            ChessPieceControl piece = obj as ChessPieceControl;
                            RowPos rowPos = (RowPos)args.NewValue;

                            piece.RenderTransform = piece.initialTransform;
                            Grid.SetRow(piece, (int)(9 - rowPos));
                        }
                    ))
                );

        /// <summary>
        /// Image Source
        /// </summary>
        public ImageSource PieceImage
        {
            get { return (ImageSource)base.GetValue(PieceImageProperty); }
            set { base.SetValue(PieceImageProperty, value); }
        }
        public static readonly DependencyProperty PieceImageProperty =
            DependencyProperty.Register
            (
                "PieceImage",
                typeof(ImageSource),
                typeof(ChessPieceControl),
                new PropertyMetadata
                (
                    (obj, args) =>
                    {
                        ChessPieceControl chessPiece = obj as ChessPieceControl;
                        chessPiece.chessPieceImage.Source = args.NewValue as ImageSource;
                    }
                )
            );
        #endregion









        #region Chess Piece control
        /// <summary>
        /// Show hand when user moves cursor over piece
        /// </summary>
        private void ThePieceImage_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!theChessBoard.IsPlayerTurn)
            {
                return;
            }
            else if (theChessBoard.PlayWithoutRules)
            {
                this.Cursor = Cursors.Hand;
            }
            else if (e.LeftButton == MouseButtonState.Released && canMoveFunc(new ChessPieceLocation(PieceColumnPosition, PieceRowPosition)))
            {
                this.Cursor = Cursors.Hand;
            }
        }

        /// <summary>
        /// Move piece across board
        /// </summary>
        private void ThePieceImage_MouseMove(object sender, MouseEventArgs e)
        {
            Point currentLocation = e.MouseDevice.GetPosition(theChessBoard);

            if (isSelected && e.LeftButton == MouseButtonState.Pressed)
            {
                TransformGroup group = new TransformGroup();
                if (moveTransform != null)
                {
                    group.Children.Add(moveTransform);
                }

                TranslateTransform move = new TranslateTransform(
                        currentLocation.X - moveLocation.X, currentLocation.Y - moveLocation.Y);
                group.Children.Add(move);

                base.RenderTransform = group;
            }

            moveLocation = currentLocation;
            moveTransform = base.RenderTransform;
        }

        /// <summary>
        /// Show when user moves cursor away from piece
        /// </summary>
        private void ThePieceImage_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
            base.RenderTransform = initialTransform;
        }

        /// <summary>
        /// Selecting chess piece
        /// </summary>
        private void ThePieceImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!isSelected && this.Cursor == Cursors.Hand)
            {
                isSelected = true;
                Canvas.SetZIndex(this, 100);
            }
        }

        /// <summary>
        /// Drop piece in new location, if move is legeal
        /// </summary>
        private void ThePieceImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Point selectedPoint = e.MouseDevice.GetPosition(theChessBoard);
            ColPos colPos = GetColumn(selectedPoint.X);
            RowPos rowPos = GetRow(selectedPoint.Y);

            this.moveAction(this, new ChessPieceLocation(colPos, rowPos), theChessBoard.PlayWithoutRules);

            this.Cursor = null;
            isSelected = false;
            RenderTransform = initialTransform;
            Canvas.SetZIndex(this, 0);
        }
        #endregion


        #region Chess piece movement
        /// <summary>
        /// Reset piece to a specified location
        /// </summary>
        public void ResetPiece(ChessPieceLocation location)
        {
            PieceColumnPosition = location.ColLoc;
            PieceRowPosition = location.RowLoc;

            if (location.ColLoc == ColPos.Captured || location.RowLoc == RowPos.Captured)
            {
                base.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                base.Visibility = System.Windows.Visibility.Visible;
            }
        }


        /// <summary>
        /// Rotate chess piece
        /// </summary>
        internal void Rotate180(bool turnUpSideDown)
        {
            if (turnUpSideDown)
            {
                TransformGroup pieceGroup = new TransformGroup();

                pieceGroup.Children.Add(new RotateTransform(180));

                TranslateTransform pieceMove = new TranslateTransform(base.ActualWidth, base.ActualHeight);
                pieceGroup.Children.Add(pieceMove);

                base.RenderTransform = pieceGroup;
            }
            else
            {
                base.RenderTransform = MatrixTransform.Identity;
            }

            initialTransform = base.RenderTransform;
        }
        #endregion

        #region Chess board positioning methods
        /// <summary>
        /// Return column position of piece, given X position of mouse from origin of control
        /// </summary>
        private ColPos GetColumn(double xPosition)
        {
            if (xPosition < theChessBoard.ChessSquareWidth * 0 + theChessBoard.BoardOrigin.X)
            {
                return ColPos.Captured;
            }
            else if (xPosition < theChessBoard.ChessSquareWidth * 1 + theChessBoard.BoardOrigin.X)
            {
                return ColPos.A;
            }
            else if (xPosition < theChessBoard.ChessSquareWidth * 2 + theChessBoard.BoardOrigin.X)
            {
                return ColPos.B;
            }
            else if (xPosition < theChessBoard.ChessSquareWidth * 3 + theChessBoard.BoardOrigin.X)
            {
                return ColPos.C;
            }
            else if (xPosition < theChessBoard.ChessSquareWidth * 4 + theChessBoard.BoardOrigin.X)
            {
                return ColPos.D;
            }
            else if (xPosition < theChessBoard.ChessSquareWidth * 5 + theChessBoard.BoardOrigin.X)
            {
                return ColPos.E;
            }
            else if (xPosition < theChessBoard.ChessSquareWidth * 6 + theChessBoard.BoardOrigin.X)
            {
                return ColPos.F;
            }
            else if (xPosition < theChessBoard.ChessSquareWidth * 7 + theChessBoard.BoardOrigin.X)
            {
                return ColPos.G;
            }
            else if (xPosition < theChessBoard.ChessSquareWidth * 8 + theChessBoard.BoardOrigin.X)
            {
                return ColPos.H;
            }
            else
            {
                return ColPos.Captured;
            }
        }

        /// <summary>
        /// Return row position of piece, given Y position of mouse from origin of control
        /// </summary>
        private RowPos GetRow(double yPosition)
        {
            if (yPosition < theChessBoard.BoardOrigin.Y)
            {
                return RowPos.Captured;
            }
            else if (yPosition < theChessBoard.ChessSquareWidth * 1 + theChessBoard.BoardOrigin.Y)
            {
                return RowPos.R8;
            }
            else if (yPosition < theChessBoard.ChessSquareWidth * 2 + theChessBoard.BoardOrigin.Y)
            {
                return RowPos.R7;
            }
            else if (yPosition < theChessBoard.ChessSquareWidth * 3 + theChessBoard.BoardOrigin.Y)
            {
                return RowPos.R6;
            }
            else if (yPosition < theChessBoard.ChessSquareWidth * 4 + theChessBoard.BoardOrigin.Y)
            {
                return RowPos.R5;
            }
            else if (yPosition < theChessBoard.ChessSquareWidth * 5 + theChessBoard.BoardOrigin.Y)
            {
                return RowPos.R4;
            }
            else if (yPosition < theChessBoard.ChessSquareWidth * 6 + theChessBoard.BoardOrigin.Y)
            {
                return RowPos.R3;
            }
            else if (yPosition < theChessBoard.ChessSquareWidth * 7 + theChessBoard.BoardOrigin.Y)
            {
                return RowPos.R2;
            }
            else if (yPosition < theChessBoard.ChessSquareWidth * 8 + theChessBoard.BoardOrigin.Y)
            {
                return RowPos.R1;
            }
            else
            {
                return RowPos.Captured;
            }
        }
        #endregion
    }
}