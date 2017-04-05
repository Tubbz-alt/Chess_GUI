﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_GUI.Models.Pieces
{
    public class King : Piece
    {
        public King(bool isBlack) : base(isBlack)
        {
            base.Name = isBlack ? base.king[0] : base.king[1];
        }

        public override bool LegalMove(List<List<Piece>> internalBoard, int initalX, int initialY, int targetX, int targetY)
        {
            throw new NotImplementedException();
        }
    }
}
