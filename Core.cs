using System;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Gomoku
{
    public class Core
    {
        private Board _board;

        public Core()
        {
            
        }

        public String cmdStart(int size)
        {
            _board = new Board(size);
            return "OK";
        }

        public String cmdTurn(int x, int y)
        {
            _board.setBoardAt(new Coordinates(x, y), '2');
            Coordinates coords = _board.evaluateMoves('1');
            _board.setBoardAt(coords, '1');
            return (coords.col + "," + coords.row);
        }

        public String cmdBegin()
        {
            Coordinates coords = _board.getRandomValidPoint();
            _board.setBoardAt(coords, '1');
            return (coords.col + "," + coords.row);
        }

        public String cmdBoard()
        {
            Coordinates coords = _board.evaluateMoves('1');
            _board.setBoardAt(coords, '1');
            return (coords.col + "," + coords.row);
        }

        public void AddToBoard(int x, int y, int player)
        {
            if (player == 1)
                _board.setBoardAt(new Coordinates(x, y), '1');
            _board.setBoardAt(new Coordinates(x, y), '2');
        }

    }
}