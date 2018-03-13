using System;
using System.Text;

namespace Gomoku
{
    public class Individual {

        public const int SIZE = 5;
        private Coordinates[] genes = new Coordinates[SIZE];
        private int fitnessValue;
        private Board board = Board.getInstance();

        public Individual() {}

        public int getFitnessValue() {
            return (fitnessValue);
        }

        public void setFitnessValue(int fitnessValue) {
            this.fitnessValue = fitnessValue;
        }

        public Coordinates getGene(int index) {
            return genes[index];
        }

        public void setGene(int index, Coordinates gene) {
            this.genes[index] = gene;
        }

        public void randGenes() {
            Random rand = new Random();
            for (int i = 0; i < SIZE; i++)
                this.setGene(i, new Coordinates(rand.Next(0, 19), rand.Next(0, 19)));
        }

        public void mutate() {
            Random rand = new Random();
            int index = rand.Next(SIZE);
            this.setGene(index, new Coordinates(rand.Next(0, 19), rand.Next(0, 19)));
        }

        public int evaluate() {
            return 0;
        }

        public override String ToString() {
            StringBuilder sb = new StringBuilder("Individual:");
            foreach (Coordinates gene in genes)
            {
                sb.Append(" ");
                sb.Append(gene);
            }
            return sb.ToString();
        }
    }

}