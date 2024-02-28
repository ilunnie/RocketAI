using System;
using AIContinuous.Nuenv;
using AIContinuous.Rocket;

double[] timeData = Space.Linear(0, 100, 11);
double[] massFlowData = Space.Uniform(35, 11);
var rocket = new Rocket((timeData, massFlowData));

Console.WriteLine(rocket.LaunchUntilGround());
