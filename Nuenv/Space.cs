namespace AIContinuous.Nuenv;

public static class Space
{
    public static double[] Uniform(double value, int count)
    {
        return Enumerable.Repeat(value, count).ToArray();
    }

    public static double[] Linear(double a, double b, int n)
    {
        var result = new double[n];
        var step = (b - a) / (n - 1);

        for (int i = 0; i < n; i++)
        {
            result[i] = a + i * step;
        }

        return result;
    }

    public static double[] Geometric(double a, double b, int n)
    {
        if (a == 0.0 || b == 0.0)
        {
            throw new ArgumentException("Arguments must not be 0", nameof(a));
        }

        var result = new double[n];
        var r = Math.Pow(b / a, 1.0 / (n - 1));

        for (int i = 0; i < n; i++)
        {
            result[i] = a * Math.Pow(r, i);
        }

        return result;
    }

    public static double[] Logarithmic(double a, double b, int n)
    {
        if (a <= 0.0 || b <= 0.0)
        {
            throw new ArgumentException("Arguments must be positive", nameof(a));
        }

        const double baseLog = 10.0;
        var result = new double[n];
        var aLog = Math.Log(a, baseLog);
        var bLog = Math.Log(b, baseLog);

        for (int i = 0; i < n; i++)
        {
            var xLog = aLog + i * (bLog - aLog) / (n - 1);
            result[i] = Math.Pow(baseLog, xLog);
        }

        return result;
    }
}