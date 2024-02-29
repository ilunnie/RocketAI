using System;
using System.Linq;
using AI;
using AI.Nuenv;
using AI.Optimize;
using AI.Rocket;

double[] timeData = Space.Geometric(1.0, 1001.0, 11).Select(x => x - 1.0).ToArray();
var evolution = new Evolution(
    Fitness, // Test Function
    Restriction, // Calculate the constraint
    Utils.LinearBounds(0, Rocket.TotalFuelMass_Kg / 3.5, timeData.Length), // Dimensions (min, max)
    timeData.Length * 15); // Population size
double[] massFlowData = evolution.Optimize(50000);

double Simulate(double[] x)
{
    if (Restriction(x) > 0) return .0;
    
    Rocket rocket = new((timeData, x));
    return rocket.LaunchUntilMax(1e-3);
}
double Fitness(double[] x) => -Simulate(x);

double Restriction(double[] x)
    => Integrate.Romberg(timeData, x) - Rocket.TotalFuelMass_Kg;

var rocket = new Rocket((timeData, massFlowData));

Console.WriteLine(rocket.LaunchUntilMax(1e-3));
Console.WriteLine(massFlowData.ToString<double[]>());
