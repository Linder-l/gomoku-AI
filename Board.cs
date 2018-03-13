using System;
using System.Data;
using System.Dynamic;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;

namespace Gomoku
{
    public class Board
    {
        private static Board ourInstance = new Board();
        private char[] _board;
        private int[] _query;
        private SequenceComponent _sequenceComponent = new SequenceComponent();
        private int size;
 
        public static Board getInstance() {
            return ourInstance;
        }
 
        public Board(int size = 19)
        {
            ourInstance = this;
            this.size = size;
            _board = new char[size * size];
            _query = new int[size * size];
            initBoard(size);
        }
 
        public void initBoard(int size)
        {
            _board.Initialize();
            for (int i = 0; i < _board.Length; i++)
                _board[i] = (char) 0;
        }
 
        public void setBoardAt(Coordinates pos, char team)
        {
            if (!pos.valid)
                return;
            _board[coordsToIndex(pos)] = team;
        }
 
        public char getBoardAt(Coordinates pos)
        {
            if (!pos.valid)
                return (char)1;
            return (_board[coordsToIndex(pos)]);
        }
 
        public int coordsToIndex(Coordinates pos)
        {
            return (pos.col + (size * pos.row));
        }

        public Coordinates indexToCoords(int index)
        {
            return new Coordinates(index % size, index / size);
        }
 
        public bool isEmptyAndValid(Coordinates pos)
        {
            return pos.valid && (_board[coordsToIndex(pos)] == 0);
        }
 
        public Coordinates evaluateMoves(char player)
        {
            _sequenceComponent.ClearComponent();
            fillMask(player);

            if (_sequenceComponent._indiceOpponent > _sequenceComponent._indiceSelf &&
                _sequenceComponent._indiceOpponent >= 3)
                return _sequenceComponent._highZoneOpponent;
            if (_sequenceComponent._indiceSelf >= _sequenceComponent._indiceOpponent &&
                _sequenceComponent._indiceSelf > 0)
                return _sequenceComponent._highZoneSelf;
            if (_sequenceComponent._indiceSelf > 0)
                return _sequenceComponent._highZoneSelf;
            return getRandomValidPoint();
        }

        public Coordinates getRandomValidPoint()
        {
            Random rand = new Random();
            for (int i = 0; i < 1500; i++)
            {
                int index = rand.Next(0, (size * size) - 1);
                Coordinates result = indexToCoords(index);
                if (result.valid && _board[index] == 0)
                    return result;
            }
            return new Coordinates(0, 0);
        }

        private int getMaxSequence(char player, Coordinates coords)
        {
            int sequence = 0;
            
            // Count neighbours
            sequence += isNeighbour(new Coordinates(coords), new Coordinates(0, -1), 0);
            sequence += isNeighbour(new Coordinates(coords), new Coordinates(1, -1), 0);
            sequence += isNeighbour(new Coordinates(coords), new Coordinates(1, 0), 0);
            sequence += isNeighbour(new Coordinates(coords), new Coordinates(1, 1), 0);
            sequence += isNeighbour(new Coordinates(coords), new Coordinates(0, 1), 0);
            sequence += isNeighbour(new Coordinates(coords), new Coordinates(-1, 1), 0);
            sequence += isNeighbour(new Coordinates(coords), new Coordinates(-1, 0), 0);
            sequence += isNeighbour(new Coordinates(coords), new Coordinates(-1, -1), 0);
            
            if (sequence == 0)
                return sequence;
            
            int tmp = 0;
            tmp = Math.Max(tmp, sequenceDepth('2', new Coordinates(coords), new Coordinates(0, -1), 0));
            tmp = Math.Max(tmp, sequenceDepth('2', new Coordinates(coords), new Coordinates(1, -1), 0));
            tmp = Math.Max(tmp, sequenceDepth('2', new Coordinates(coords), new Coordinates(1, 0), 0));
            tmp = Math.Max(tmp, sequenceDepth('2', new Coordinates(coords), new Coordinates(1, 1), 0));
            tmp = Math.Max(tmp, sequenceDepth('2', new Coordinates(coords), new Coordinates(0, 1), 0));
            tmp = Math.Max(tmp, sequenceDepth('2', new Coordinates(coords), new Coordinates(-1, 1), 0));
            tmp = Math.Max(tmp, sequenceDepth('2', new Coordinates(coords), new Coordinates(-1, 0), 0));
            tmp = Math.Max(tmp, sequenceDepth('2', new Coordinates(coords), new Coordinates(-1, -1), 0));
            _sequenceComponent.AddSequence(false, coords, tmp);
            sequence += tmp * 10;

            tmp = 0;
            tmp = Math.Max(tmp, sequenceDepth('1', new Coordinates(coords), new Coordinates(0, -1), 0));
            tmp = Math.Max(tmp, sequenceDepth('1', new Coordinates(coords), new Coordinates(1, -1), 0));
            tmp = Math.Max(tmp, sequenceDepth('1', new Coordinates(coords), new Coordinates(1, 0), 0));
            tmp = Math.Max(tmp, sequenceDepth('1', new Coordinates(coords), new Coordinates(1, 1), 0));
            tmp = Math.Max(tmp, sequenceDepth('1', new Coordinates(coords), new Coordinates(0, 1), 0));
            tmp = Math.Max(tmp, sequenceDepth('1', new Coordinates(coords), new Coordinates(-1, 1), 0));
            tmp = Math.Max(tmp, sequenceDepth('1', new Coordinates(coords), new Coordinates(-1, 0), 0));
            tmp = Math.Max(tmp, sequenceDepth('1', new Coordinates(coords), new Coordinates(-1, -1), 0));
            _sequenceComponent.AddSequence(true, coords, tmp);
            sequence += tmp * 100;
            
            return sequence;
        }

        private int isNeighbour(Coordinates start, Coordinates mask, int depth)
        {
            start.col += mask.col;
            start.row += mask.row;
            if (!start.CheckValidity())
                return depth;
            if (_board[coordsToIndex(start)] == 0)
                return depth;
            return 1;
        }

        private int sequenceDepth(char player, Coordinates start, Coordinates mask, int depth)
        {
            start.col += mask.col;
            start.row += mask.row;
            if (!start.CheckValidity())
                return depth;
            if (_board[coordsToIndex(start)] != player)
                return depth;
            return sequenceDepth(player, start, mask, depth + 1);
        }
        
        private void fillMask(char player)
        {
            for (int index = 0; index < (size * size); index++)
            {
                int fitness = 0;
                Coordinates currPos = indexToCoords(index);
                if (isEmptyAndValid(currPos))
                    fitness += getMaxSequence(player, currPos);
                else
                    fitness = -1;
                _query[index] += fitness;
            }
        }
        
        public override String ToString() {
            StringBuilder sb = new StringBuilder("Board:\n");
            for (int i = 0; i < (size * size); i++) {
                if (i % size == 0)
                    sb.Append("\n");
                if (_board[i] == 0)
                    sb.Append('_');
                else
                    sb.Append('#');
            }
            
            sb.Append("\nMask:\n");
            for (int i = 0; i < (size * size); i++) {
                if (i % size == 0)
                    sb.Append("\n");
                sb.Append(" ");
                sb.Append(_query[i].ToString());
            }

            sb.Append("\n\nOpponent high: " + _sequenceComponent._indiceOpponent + " at " + _sequenceComponent._highZoneOpponent);
            sb.Append("\nSelf high: " + _sequenceComponent._indiceSelf + " at " + _sequenceComponent._highZoneSelf);
            return sb.ToString();
        }
    }
}