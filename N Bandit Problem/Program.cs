using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Distributions;
namespace N_Bandit_Problem
{
    internal class Program
    {
        public static Random random = new Random();

        public static int GenerateReward(double prob)
        {
            int Reward = 0;
            Random rnd = new Random();
            for (int i = 0; i < 10; i++)
            {
                if (rnd.NextDouble() < prob) { Reward++; }
            }
            return Reward;
        }

        public static List<double> GetBestMachine(List<List<double>> ActionList)
        {
            var ActionListSorted = ActionList.OrderByDescending(List => List[2]).ToList();

            return ActionListSorted[0];
        }

        public static int GeneratedWeighted_RND(List<double> Items)
        {
                        
            // set up a discrete uniform distrubution
            double max = Items.Max();
            for (int i = 0; i < Items.Count; i++)
            {
                //Normal Distrubution = new Normal();
                Poisson Distrubution = new Poisson(1);
                //DiscreteUniform Distrubution = new DiscreteUniform(0,1);
                Items[i] = Distrubution.CumulativeDistribution(Items[i]);
            }

            double cumulatedProbability = random.NextDouble() * Items.Sum();

            for (int i = 0; i < Items.Count; i++)
            {
                if ((cumulatedProbability -= Items[i]) <= 0)
                    return i;
            }

            throw new InvalidOperationException();
        }

        private static void Main(string[] args)
        {
            int n = 10;
            int Best_Arm = 0;
           
            double best_average = 0;
            List<double> Machines = new List<double> { };
            List<double> Probability = new List<double> { };
            List<double> WeightsArray = new List<double> { };

            Random Rnd = new Random();
            for (int i = 0; i < n; i++) { Probability.Add(Rnd.NextDouble()); Machines.Add(0); WeightsArray.Add(0); }

            List<List<double>> AV2 = new List<List<double>> { };
            for (int i = 0; i < n; i++)
            {
                List<double> row = new List<double> { i, 0, 0 };
                AV2.Add(row);
            }

            
            for (int i = 0; i < 10000; i++)
            {
                List<double> ThisAV = new List<double> { };
                
                Best_Arm = GeneratedWeighted_RND(WeightsArray);
                ThisAV.Add(Best_Arm);
                ThisAV.Add(GenerateReward(Probability[Best_Arm]));
                AV2[Best_Arm][1] = AV2[Best_Arm][1] + 1;
                AV2[Best_Arm][2] = AV2[Best_Arm][2] + ((ThisAV[1] - AV2[Best_Arm][2]) / AV2[Best_Arm][1]);
                List<double> BestMachine = GetBestMachine(AV2);
                Best_Arm = (int)BestMachine[0];
                best_average = BestMachine[2];
                for (int j = 0; j < WeightsArray.Count; j++) { WeightsArray[j] = AV2[j][2]; }
            }
            Console.Write("best machine: " + Best_Arm + Environment.NewLine);
            Console.Write("best Average: " + best_average + Environment.NewLine);

            for (int i = 0; i < AV2.Count; i++)
            {
                for (int j = 0; j < AV2[i].Count; j++)
                {
                    Console.Write(AV2[i][j] + " ");
                }
                Console.Write(Environment.NewLine);
            }
            Console.Write(Environment.NewLine);
            Probability.ForEach(Console.WriteLine);
        }
    }
}