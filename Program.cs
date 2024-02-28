using System;
using AIContinuous.Nuenv;
using AIContinuous.Rocket;

double[] timeData = Space.Linear(0, 200, 11);
double[] massFlowData = Space.Uniform(17.5, 11);
var rocket = new Rocket((timeData, massFlowData));

Console.WriteLine(rocket.LaunchUntilMax());
