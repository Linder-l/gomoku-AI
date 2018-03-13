using System;

namespace Gomoku.Properties
{
    public class Population
    {

        const int ELITISM_K = 5;
        const int POP_SIZE = 200 + ELITISM_K; // population size
        const int MAX_ITER = 2000; // maw number of iterations
        const double MUTATION_RATE = 0.05; // probability of mutation
        const double CROSSOVER_RATE = 0.7; // probability of crossover

        private static Random m_rand = new Random();
        private Individual[] m_population;
        private double totalFitness;

        public Population()
        {
            m_population = new Individual[POP_SIZE];

            // init population
            for (int i = 0; i < POP_SIZE; i++)
            {
                m_population[i] = new Individual();
                m_population[i].randGenes();
            }

            // evaluate population
            this.evaluate();
        }

        public void setPopulation(Individual[] newPop)
        {
            Array.Copy(newPop, 0, m_population, 0, POP_SIZE);
        }

        public Individual[] getPopulation()
        {
            return m_population;
        }

        public double evaluate()
        {
            this.totalFitness = 0.0;
            for (int i = 0; i < POP_SIZE; i++)
                this.totalFitness += m_population[i].evaluate();
            return this.totalFitness;
        }

        public Individual rouletteWheelSelection()
        {
            double randNum = m_rand.NextDouble() * this.totalFitness;
            int idx;

            for (idx = 0; idx < POP_SIZE && randNum > 0; idx++)
                randNum -= m_population[idx].getFitnessValue();
            return m_population[idx - 1];
        }

        public Individual findBestIndividual()
        {
            int idxMax = 0, idxMin = 0;
            double currentMax = 0.0;
            double currentMin = 1.0;
            double currentVal;

            for (int idx = 0; idx < POP_SIZE; ++idx)
            {
                currentVal = m_population[idx].getFitnessValue();
                if (currentMax < currentMin)
                {
                    currentMax = currentMin = currentVal;
                    idxMax = idxMin = idx;
                }
                if (currentVal > currentMax)
                {
                    currentMax = currentVal;
                    idxMax = idx;
                }
                if (currentVal < currentMin)
                {
                    currentMin = currentVal;
                    idxMin = idx;
                }
            }
            //return m_population[idxMin];    // minimization
            return m_population[idxMax]; // maximization
        }

        public static Individual[] crossover(Individual indiv1, Individual indiv2)
        {
            Individual[] newIndiv = new Individual[2];
            newIndiv[0] = new Individual();
            newIndiv[1] = new Individual();

            int randPoint = m_rand.Next(Individual.SIZE);
            int i;
            for (i = 0; i < randPoint; ++i)
            {
                newIndiv[0].setGene(i, indiv1.getGene(i));
                newIndiv[1].setGene(i, indiv2.getGene(i));
            }
            for (; i < Individual.SIZE; ++i)
            {
                newIndiv[0].setGene(i, indiv2.getGene(i));
                newIndiv[1].setGene(i, indiv1.getGene(i));
            }
            return newIndiv;
        }

        public static void GeneratePopulation(String[] args)
        {
            Population pop = new Population();
            Individual[] newPop = new Individual[POP_SIZE];
            Individual[] indiv = new Individual[2];

            // current population
            Console.Write("Total Fitness = " + pop.totalFitness);
            Console.WriteLine(" ; Best Fitness = " +
                              pop.findBestIndividual().getFitnessValue());

            // main loop
            int count;
            for (int iter = 0; iter < MAX_ITER; iter++)
            {
                count = 0;

                // Elitism
                for (int i = 0; i < ELITISM_K; ++i)
                {
                    newPop[count] = pop.findBestIndividual();
                    count++;
                }

                // build new Population
                while (count < POP_SIZE)
                {
                    // Selection
                    indiv[0] = pop.rouletteWheelSelection();
                    indiv[1] = pop.rouletteWheelSelection();

                    // Crossover
                    if (m_rand.NextDouble() < CROSSOVER_RATE)
                    {
                        indiv = crossover(indiv[0], indiv[1]);
                    }

                    // Mutation
                    if (m_rand.NextDouble() < MUTATION_RATE)
                    {
                        indiv[0].mutate();
                    }
                    if (m_rand.NextDouble() < MUTATION_RATE)
                    {
                        indiv[1].mutate();
                    }

                    // add to new population
                    newPop[count] = indiv[0];
                    newPop[count + 1] = indiv[1];
                    count += 2;
                }
                pop.setPopulation(newPop);

                // reevaluate current population
                pop.evaluate();
                Console.Write("Total Fitness = " + pop.totalFitness);
                Console.WriteLine(" ; Best Fitness = " +
                                  pop.findBestIndividual().getFitnessValue());
            }

            // best indiv
            Individual bestIndiv = pop.findBestIndividual();
            Console.Write("END\n");
            Console.WriteLine("Best individual fitness = " + bestIndiv.getFitnessValue());
        }
    }
}