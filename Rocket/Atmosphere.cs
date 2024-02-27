using AIContinuous.Nuenv;

namespace AIContinuous.Rocket;

public static class Atmosphere
{
    private static readonly double[] HeightData =
    {
        0e3, 11e3, 20e3, 32e3, 47e3, 51e3, 70e3, 85e3, 90e3, 100e3,
        150e3, 200e3, 300e3, 400e3, 500e3, 600e3
    };

    private static readonly double[] TemperatureData =
    {
        288.15, 216.77, 216.65, 228.49, 269.68, 270.65, 217.45, 188.89,
        186.87, 195.88, 634.39, 854.36, 976.01, 995.83, 999.24, 999.85
    };

    private static readonly double[] PressureData =
    {
        1.0133e5, 2.2699e4, 5.5293e3, 8.8906e2, 1.1585e2, 7.0458e1, 4.6342e0,
        4.4563e-1, 1.8359e-2, 3.2011e-2, 4.5422e-4, 8.4736e-5, 8.7704e-6,
        1.4518e-6, 3.0236e-7, 8.2130e-8
    };

    public static double Temperature(double height)
    {
        return Interp1D.Linear(HeightData, TemperatureData, height, true);
    }

    public static double Pressure(double height)
    {
        return Interp1D.Exponential(HeightData, PressureData, height, true);
    }

    public static double Density(double height)
    {
        const double rSp = 287.05;
        var t = Temperature(height);
        var p = Pressure(height);
        var density = p / (rSp * t);

        return density;
    }
}