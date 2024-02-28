using System;

namespace AI.Nuenv;

public static class Interp1D
{
    public static double Linear(double[] x, double[] y, double query, bool checkBounds = true)
    {
        if (x.Length != y.Length)
        {
            throw new ArgumentException("Arrays 'x' and 'y' must have the same size");
        }

        if (checkBounds && (query < x[0] || query > x[^1]))
        {
            throw new ArgumentOutOfRangeException(nameof(query), $"query {query} is out of bounds");
        }

        var index = Search.BinarySearch(x, query);

        return y[index] + ((y[index] - y[index - 1]) / (x[index] - x[index - 1])) * (query - x[index]);
    }

    public static double CubicSpline(double[] x, double[] y, double query, bool checkBounds = true)
    {
        if (x.Length != y.Length)
        {
            throw new ArgumentException("Arrays 'x' and 'y' must have the same size");
        }

        if (checkBounds && (query < x[0] || query > x[^1]))
        {
            throw new ArgumentOutOfRangeException(nameof(query), $"query {query} is out of bounds");
        }

        var index = Search.BinarySearch(x, query) - 1;

        if (index <= 1 || index >= x.Length - 2)
            return Linear(x, y, query, checkBounds);

        var t = (query - x[index]) / (x[index + 1] - x[index]);

        var mk0 = 0.5 * ((y[index + 1] - y[index]) / (x[index + 1] - x[index]) +
                         (y[index] - y[index - 1]) / (x[index] - x[index - 1]));
        var mk1 = 0.5 * ((y[index + 2] - y[index + 1]) / (x[index + 2] - x[index + 1]) +
                         (y[index + 1] - y[index]) / (x[index + 1] - x[index]));

        var h00 = (1.0 + 2.0 * t) * (1.0 - t) * (1.0 - t);
        var h10 = t * (1.0 - t) * (1.0 - t);
        var h01 = t * t * (3.0 - 2.0 * t);
        var h11 = t * t * (t - 1.0);

        var interpolation = h00 * y[index]
                            + h10 * (x[index + 1] - x[index]) * mk0
                            + h01 * y[index + 1]
                            + h11 * (x[index + 1] - x[index]) * mk1;


        return interpolation;
    }

    public static double Exponential(double[] x, double[] y, double query, bool checkBounds = true)
    {
        if (x.Length != y.Length)
        {
            throw new ArgumentException("Arrays 'x' and 'y' must have the same size");
        }

        if (checkBounds && (query < x[0] || query > x[^1]))
        {
            throw new ArgumentOutOfRangeException(nameof(query), $"query {query} is out of bounds");
        }

        var index = Search.BinarySearch(x, query);

        var zeta = Math.Log(y[index] / y[index - 1]) / (x[index] - x[index - 1]);

        return y[index - 1] * Math.Exp(zeta * (query - x[index - 1]));
    }
}