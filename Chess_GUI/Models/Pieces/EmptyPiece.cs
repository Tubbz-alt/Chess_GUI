﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Chess_GUI.Models.Pieces
{
    public class EmptyPiece : Piece
    {
        public EmptyPiece(bool isBlack) : base(isBlack)
        {
        }

        public override int LegalMove(Board internalBoard, int initalRow, int initialColumn, int targetRow, int targetColumn)
        {
            return 0;
        }
    }
}
