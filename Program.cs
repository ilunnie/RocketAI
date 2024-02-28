using System;
using AI.Nuenv;
using AI.Optimize;
using AI.Rocket;

double TestRocket(double[] x)
{

}

double Restriction(double[] x)
{

}

double[] timeData = Space.Linear(0, 100, 11);
var evolution = new Evolution(TestRocket, Restriction, timeData, 200);
double[] massFlowData = ;
var rocket = new Rocket((timeData, massFlowData));

Console.WriteLine(rocket.LaunchUntilGround());
