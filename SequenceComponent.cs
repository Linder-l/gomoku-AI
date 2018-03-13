namespace Gomoku
{
    public class SequenceComponent
    {
        // Opponent data
        public Coordinates _highZoneOpponent;
        public int _indiceOpponent;

        // Self data
        public Coordinates _highZoneSelf;
        public int _indiceSelf;

        private bool clean;

        public SequenceComponent()
        {
            clean = true;
        }

        public void ClearComponent()
        {
            _indiceOpponent = 0;
            _indiceSelf = 0;
            clean = true;
        }
        
        public void AddSequence(bool self, Coordinates pos, int indice)
        {
            clean = false;
            if (self && indice > _indiceOpponent)
            {
                _highZoneSelf = pos;
                _indiceSelf = indice;
            }
            if (!self && indice > _indiceOpponent)
            {
                _highZoneOpponent = pos;
                _indiceOpponent = indice;
            }
        }
    }
}