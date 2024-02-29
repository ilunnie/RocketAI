using System;
using AI;
using AI.Nuenv;
using AI.Optimize;
using AI.Rocket;

double[] timeData = Space.Linear(0, 100, 11);
var evolution = new Evolution(TestRocket, Restriction, Utils.LinearBounds(0, Rocket.TotalFuelMass_Kg, 11), 200);
double[] massFlowData = evolution.Optimize(1000);

double TestRocket(double[] x)
{
    if (Restriction(x) > 0) return .0;
    
    Rocket rocket = new((timeData, x));
    return rocket.LaunchUntilGround();
}

double Restriction(double[] x)
    => Integrate.Romberg(timeData, x) - 3500;

// var rocket = new Rocket((timeData, massFlowData));

// Console.WriteLine(rocket.LaunchUntilGround());
