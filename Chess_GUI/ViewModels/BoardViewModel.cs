﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Animation;
using Chess_GUI.Annotations;
using Chess_GUI.Models;
using Chess_GUI.Models.Pieces;
using Chess_GUI.ViewModels.Commands;

namespace Chess_GUI.ViewModels
{
    public class BoardViewModel : INotifyPropertyChanged
    {
        // A string that gets built from button clicks on the board

        // Will be set to 1 when King is taken
        public int WonGame = 0;

        // Keeps track of who's turn it is
        public bool IsBlacksTurn { get; set; }

        // Not using yet, check
        //public ObservableCollection<Board> MyBoard { get; private set; }

        // Relay command is the general command handler, sets up the MoveCommand
        public RelayCommand MoveCommand { get; private set; }

        // Relay command is the general command handler, sets up the MoveCommand
        public RelayCommand ResetCommand { get; private set; }

        public RelayCommand ResetMoveTextCommand { get; set; }

        // Is the game board
        public Board Board { get; set; }

        // Move list
        public ObservableCollection<string> MovesList { get; set; }

        public string WhoTurn => IsBlacksTurn ? "Black's turn" : "White's turn";

        public string MoveText { get; set; }

        // CTOR for the viewmodel
        public BoardViewModel()
        {
            // Game starts with black's move
            IsBlacksTurn = true;

            // Creates a new instance of the model Board


            // The move starts out as an empty string until a button is clicked
            MoveText = "";


            Board = new Board();
            MovesList = new ObservableCollection<string>();

            // Is command sent by the button clicks
            MoveCommand = new RelayCommand(Move, Canexecute);
            ResetCommand = new RelayCommand(Reset, Canexecute);
            ResetMoveTextCommand = new RelayCommand(ResetMoveText, Canexecute);

        }

        public void Reset(object message)
        {
            // Make a new instance of a board which will have everything in default position
            Board = new Board();
            // Need to remove currently selected cell
            MoveText = "";
            OnPropertyChanged(nameof(MoveText));
            // Update GUI with new fresh board
            OnPropertyChanged(nameof(Board));

            // Creates a new movelist for the new game
            MovesList = new ObservableCollection<string>();
            OnPropertyChanged(nameof(MovesList));


            // Game always starts with black first
            IsBlacksTurn = true;
            // Notifies GUI who's turn it is
            OnPropertyChanged(nameof(WhoTurn));
        }

        // Entry point for moves which first validates input, then either passes on to validate move based on piece rules
        // Message is the button text
        public void Move(object message)
        {
            MoveText += (string)message;
            OnPropertyChanged(nameof(MoveText));

            // If moveText is over 4 it is an invalid move, so reset moveText and return
            if (MoveText.Length > 4)
            {
                MoveText = "";
                OnPropertyChanged(nameof(MoveText));
                return;
            }

            // Don't allow player to move other's pieces or select empty spots
            if (IsBlacksTurn ==
                !Board[8 - (int)char.GetNumericValue(MoveText[1])][(int)MoveText[0] - 65].Piece.IsBlack ||
                Board[8 - (int)char.GetNumericValue(MoveText[1])][(int)MoveText[0] - 65].Piece.Name == '\0')
            {
                MoveText = "";
                OnPropertyChanged(nameof(MoveText));
                return;
            }

            if (ValidInputCheck(MoveText))
            {
                // Converts input to valid Column/Row indices
                int sourceRow = 8 - (int)char.GetNumericValue(MoveText[1]);
                int sourceColumn = (int)MoveText[0] - 65;
                int destRow = 8 - (int)char.GetNumericValue(MoveText[3]);
                int destColumn = (int)MoveText[2] - 65;

                // Check if move if legal, if so legalMove will be 1, if game is won it will be 2

                int legalMove = Board[sourceRow][sourceColumn].Piece.LegalMove(Board, sourceRow, sourceColumn, destRow, destColumn);


                if (legalMove == 1)
                {
                    // Move the piece to it's new location in the GUI
                    OnPropertyChanged(nameof(Board));
                    // After a valid move, switch who's turn it is
                    IsBlacksTurn = !IsBlacksTurn;
                    // Notifies GUI who's turn it is
                    OnPropertyChanged(nameof(WhoTurn));

                    // Adds move to list and updates it on GUI
                    MovesList.Insert(0, MoveText);
                    OnPropertyChanged(nameof(MovesList));

                    ResetMoveText("");
                }
                // LegalMove is 2 when a king is taken, so winning condition goes here
                if (legalMove == 2)
                {
                    // Unused wongame variable, maybe remove
                    WonGame = 1;
                    // Move the piece to it's new location in the GUI
                    OnPropertyChanged(nameof(Board));

                    // Adds move to list and updates it on GUI
                    MovesList.Insert(0, MoveText);
                    OnPropertyChanged(nameof(MovesList));

                    // Implement winning dialog HERE
                    // Implement winning dialog HERE
                    // Implement winning dialog HERE

                    ResetMoveText("");
                }
                // LegalMove is 3 when a pawn makes it to the other side & needs to be promoted
                if (legalMove == 3)
                {
                    // IMPLEMENT PROMOTION HERE
                    // IMPLEMENT PROMOTION HERE
                    // IMPLEMENT PROMOTION HERE

                    // Move the piece to it's new location in the GUI
                    OnPropertyChanged(nameof(Board));

                    IsBlacksTurn = !IsBlacksTurn;
                    // Notifies GUI who's turn it is
                    OnPropertyChanged(nameof(WhoTurn));

                    // Adds move to list and updates it on GUI
                    MovesList.Insert(0, MoveText);
                    OnPropertyChanged(nameof(MovesList));

                    ResetMoveText("");
                }


            }

            if (MoveText.Length > 2)
            {
                MoveText = MoveText.Remove(2);
                OnPropertyChanged(nameof(MoveText));
            }

        }

        public void ResetMoveText(object message)
        {
            MoveText = "";
            OnPropertyChanged(nameof(MoveText));
            return;
        }

        // Greys out selected space on board, and disables the command
        public bool Canexecute(object message)
        {
            if (message == null)
            {
                return true;
            }
            else if ((string)message == MoveText)
            {
                return false;
            }

            return true;
        }

        // Checks to see if the input is formatted correctly
        public bool ValidInputCheck(string moveText)
        {
            // Remenant of when user could enter move by text, keeping for future protection of new UI
            moveText = moveText.ToLower();

            // a valid move will always be 4 chars
            if (moveText.Length != 4)
                return false;

            // can't move to the same spot
            if (moveText[0] == moveText[2] && moveText[1] == moveText[3])
                return false;


            var rgx = new Regex(@"[a-h][1-8][a-h][1-8]$");

            // Checking against pattern of CharIntCharInt (with chars between a-h, and ints between 1-8)
            return rgx.IsMatch(moveText);


        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
