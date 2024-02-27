namespace AIContinuous.Rocket;

using System;

public static class Gravity
{
    private const double EarthRadius = 6.371e+06;
    private const double G0 = 9.7803267714;
    private const double GravityCorrectionCoeff = 0.00193185138639;
    private const double GravityCorrectionDenominatorCoeff = 0.00669437999013;

    public static double GetGravity(double height, double latitude = 0)
    {
        var sin2Lat = Math.Pow(Math.Sin(latitude), 2.0);
        var g0CorrectDenominator = Math.Sqrt(1.0 - GravityCorrectionDenominatorCoeff * sin2Lat);
        var g0Correct = G0 * (1.0 + GravityCorrectionCoeff * sin2Lat) / g0CorrectDenominator;

        var g = g0Correct * Math.Pow(EarthRadius / (EarthRadius + height), 2);

        return g;
    }
}