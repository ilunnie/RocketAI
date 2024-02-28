using System;
using System.Collections.Generic;

namespace AI.Optimize;

public class Evolution
{
    protected Func<double[], double> Fitness { get; }
    protected Func<double[], double> Restriction { get; }

    protected int Dimension { get; }
    protected double[][] Bounds { get; }
    protected double MutationMin { get; set; }
    protected double MutationMax { get; set; }
    protected double Recombination { get; set; }

    protected List<double[]> Individuals { get; }
    protected int NPop { get; set; }

    protected int BestIndividualIndex { get; set; }
    private double[] IndividualsRestrictions { get; set; }
    private double[] IndividualsFitness { get; set; }

    public Evolution(
        Func<double[], double> fitness,
        Func<double[], double> restriction,
        double[][] bounds,
        int nPop,
        double mutationMin = .5,
        double mutationMax = .9,
        double recombination = .8
    )
    {
        this.Fitness = fitness;
        this.Restriction = restriction;

        this.Dimension = bounds.Length;
        this.Bounds = bounds;
        this.MutationMin = mutationMin;
        this.MutationMax = mutationMax;
        this.Recombination = recombination;

        this.NPop = nPop;
        this.Individuals = new List<double[]>(nPop);

        this.IndividualsRestrictions = new double[NPop];
        this.IndividualsFitness = new double[NPop];
    }

    private void findBestIndividual()
    {
        var fitnessBest = IndividualsFitness[BestIndividualIndex];

        for (int i = 0; i < NPop; i++)
        {
            var fitnessCurrent = Fitness(Individuals[i]);

            if (fitnessCurrent < fitnessBest)
            {
                BestIndividualIndex = i;
                fitnessBest = fitnessCurrent;
            }
        }
        IndividualsFitness[BestIndividualIndex] = fitnessBest;
    }

    private void GeneratePopulation()
    {
        int dimension = Dimension;

        for (int i = 0; i < this.NPop; i++)
        {
            Individuals[i] = new double[dimension];

            for (int j = 0; j < dimension; j++)
                Individuals[i][j] = Utils.Rescale(
                    Random.Shared.NextDouble(),
                    Bounds[j][0],
                    Bounds[j][1]
                );

            IndividualsRestrictions[i] = Restriction(Individuals[i]);
            IndividualsFitness[i] = IndividualsRestrictions[i] <= 0
                                        ? Fitness(Individuals[i])
                                        : double.MaxValue;
        }
    }

    private double[] Mutate(int index)
    {
        var newIndividual = (double[])Individuals[BestIndividualIndex].Clone();

        int individualRand1;
        int individualRand2;

        do individualRand1 = Random.Shared.Next(NPop);
        while (individualRand1 == index);

        do individualRand2 = Random.Shared.Next(NPop);
        while (individualRand1 == individualRand2);

        for (int i = 0; i < Dimension; i++)
            newIndividual[i] += Utils.Rescale(Random.Shared.NextDouble(), MutationMin, MutationMax) *
                                (Individuals[individualRand1][i] - Individuals[individualRand2][i]);

        return newIndividual;
    }

    protected double[] Crossover(int index)
    {
        var trial = Mutate(index);
        var result = (double[])Individuals[index].Clone();

        for (int i = 0; i < Dimension; i++)
            if (!(Random.Shared.NextDouble() < Recombination) || (i == Random.Shared.Next(Dimension)))
                result[i] = trial[i];

        return result;
    }

    protected void Iterate()
    {
        for (int i = 0; i < NPop; i++)
        {
            var trial = Crossover(i);
            var restTrial = Restriction(trial);
            double fitnessTrial = restTrial <= 0
                                    ? Fitness(trial)
                                    : double.MaxValue;

            var restIndividual = IndividualsRestrictions[i];

            if ((restIndividual > 0 && restTrial < restIndividual)
                || (restTrial <= 0 && restIndividual > 0)
                || (restTrial <= 0 && fitnessTrial < Fitness(Individuals[i])))
            {
                Individuals[i] = trial;
                IndividualsRestrictions[i] = restTrial;
                IndividualsFitness[i] = fitnessTrial;
            }
        }

        findBestIndividual();
    }

    public double[] Optimize(int n)
    {
        GeneratePopulation();

        Iterate();

        return Individuals[BestIndividualIndex];
    }
}