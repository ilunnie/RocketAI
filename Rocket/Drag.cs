using System;
using AI.Nuenv;

namespace AI.Rocket;

public static class Drag
{
    private const double RStar = 287.052874;
    private static readonly double[] TempData  = { -15.0, 0.0, 20.0, 100.0, 200.0, 400.0, 1000.0 };
    private static readonly double[] GammaData = { 1.404, 1.403, 1.400, 1.401, 1.398, 1.393, 1.365 };

    public static double IdealMach(double v, double T)
    {
        var gamma = Interp1D.Linear(TempData, GammaData, T, true);
        var c = Math.Sqrt(RStar * gamma * T);

        return Math.Abs(v) / c;
    }

    public static double Coefficient(double v, double T, double cd0)
    {
        const double rd = 1.1;
        const double m1 = 1.2;
        const double m2 = 1.325;
        const double a0 = 0.75;
        var a1 = (1.0 - a0) * Math.Pow(m1 - 1.0, 4.0);
        var a2 = 1.0 - a1 * Math.Pow(m2 - m1, 4.0);

        var mach = IdealMach(v, T);
        var fd = mach switch
        {
            <= 1.0 => a0 * Math.Pow(mach, 6.0),
            <= m2  => 1.0 - a1 * Math.Pow(mach - m1, 4.0),
            _      => a2 / (mach + 1.0 - m2)
        };

        var cd = cd0 * (1.0 + rd * fd);

        return cd;
    }
}