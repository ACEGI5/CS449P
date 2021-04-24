﻿using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Forms;
using NineMansMorrisLib;
using static NineMansMorrisLib.Board;

namespace NineMansMorrisUi
{
    public class BoardFormHelper
    {
        private readonly Button[,] _btnGrid = new Button[BoardSize, BoardSize];
        private readonly NineMansMorrisLogic _nineMansMorrisGame;
        private bool _newMillFormed;

        public BoardFormHelper(Button[,] btnGrid,NineMansMorrisLogic nineMansMorrisGame)
        {
            _btnGrid = btnGrid;
            _nineMansMorrisGame = nineMansMorrisGame;
        }
         public bool PiecePlacement(int row, int col, Control clickedButton)
        {
            switch (_nineMansMorrisGame.gameTurn)
            {
                case NineMansMorrisLogic.Turn.White when CanPlacePiece(row, col,_nineMansMorrisGame.WhitePlayer):
                {
                    if (_nineMansMorrisGame.PlacePiece(_nineMansMorrisGame.WhitePlayer, row, col))
                    {

                        CheckMillFormed(row, col, _nineMansMorrisGame.WhitePlayer);
                        return true;
                    }

                    break;
                }
                case NineMansMorrisLogic.Turn.Black when  CanPlacePiece(row, col,_nineMansMorrisGame.BlackPlayer):
                {
                    if (_nineMansMorrisGame.PlacePiece(_nineMansMorrisGame.BlackPlayer, row, col))
                    {

                        CheckMillFormed(row, col, _nineMansMorrisGame.BlackPlayer);
                            return true;
                    }

                    break;
                }
            }

            return false;
        }

         private bool CanPlacePiece(int row, int col,Player player)
         {
             return player.AllPiecesPlaced == false &&
                    _nineMansMorrisGame.GameBoard.GameBoard[row, col].PieceState ==
                    PieceState.Open;
         }

         public bool FlyPiece(int row, int col, Button clickedButton, ref Button _selectButton)
        {
            var oldLocation = (Point) _selectButton.Tag;
            var oldRow = oldLocation.Y;
            var oldCol = oldLocation.X;
            if (!ValidPieceMovement(row, col, clickedButton, _selectButton)) return false;

            if (_nineMansMorrisGame.gameTurn == NineMansMorrisLogic.Turn.Black)
            {
                if (!_nineMansMorrisGame.FlyPiece(_nineMansMorrisGame.BlackPlayer, row, col, oldRow,
                    oldCol))
                {
                    _selectButton = clickedButton;
                    return false;
                }

                if (!CheckMillFormed(row, col, _nineMansMorrisGame.WhitePlayer))
                {
                    _selectButton = null;
                }
            }
            else
            {
                if (!_nineMansMorrisGame.FlyPiece(_nineMansMorrisGame.WhitePlayer, row, col, oldRow,
                    oldCol))
                {
                    _selectButton = clickedButton;
                    return false;
                }

                if (!CheckMillFormed(row, col, _nineMansMorrisGame.WhitePlayer))
                {
                    _selectButton = null;
                }
            }

            return true;
        }

        public bool PieceMovement(int row, int col, Button clickedButton, ref Button _selectButton)
        {
            var oldLocation = (Point) _selectButton.Tag;
            var oldRow = oldLocation.Y;
            var oldCol = oldLocation.X;
            if (!ValidPieceMovement(row, col, clickedButton, _selectButton)) return false;
            if (_nineMansMorrisGame.gameTurn == NineMansMorrisLogic.Turn.Black)
            {
                if (!_nineMansMorrisGame.MovePiece(_nineMansMorrisGame.BlackPlayer, row, col, oldRow,
                    oldCol))
                {
                    _selectButton = clickedButton;
                    return false;
                }

                CheckMillFormed(row, col, _nineMansMorrisGame.WhitePlayer);

            }
            else
            {
                if (!_nineMansMorrisGame.MovePiece(_nineMansMorrisGame.WhitePlayer, row, col, oldRow,
                    oldCol))
                {
                    _selectButton = clickedButton;
                    return false;
                }

                CheckMillFormed(row, col, _nineMansMorrisGame.WhitePlayer);

            }

            return true;
        }

        private bool CheckMillFormed(int row, int col, Player player)
        {
            _newMillFormed = _nineMansMorrisGame.CheckMill(row, col, player);
            return _newMillFormed;
        }

        private bool ValidPieceMovement(int row, int col, Button clickedButton, Button _selectButton)
        {
            return _selectButton != clickedButton && _selectButton != null &&
                   _nineMansMorrisGame.GameBoard.GameBoard[row, col].PieceState == PieceState.Open;
        }
    }
}