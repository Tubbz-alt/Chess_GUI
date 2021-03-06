﻿namespace Chess_GUI.Models
{
    public abstract class Piece
    {
        protected Piece(bool isBlack)
        {
            IsBlack = isBlack;
        }

        // Name will contain the piece's unicode character
        public char Name { get; set; }

        // Unicode characters for White chess pieces
        protected string WPiece = "♔♕♖♗♘♙";
        // Unicode characters for Black chess pieces
        protected string BPiece = "♚♛♜♝♞♟";

        // Used to check if empty space
        protected string Zero = "\0";

        protected string King = "♚♔";
        protected string Queen = "♛♕";
        protected string Rook = "♜♖";
        protected string Bishop = "♝♗";
        protected string Knight = "♞♘";
        protected string Pawn = "♟♙";

        // True if piece is black
        internal bool IsBlack;

        protected bool TakingKing;

        public abstract int LegalMove(Board internalBoard, int initalRow, int initialColumn, int targetRow, int targetColumn);

    }
}
