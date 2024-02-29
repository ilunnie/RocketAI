using System.Collections.Generic;
using System.Text;

namespace AI;

public static class Utils
{
    public static double Rescale(double x, double min, double max)
        => (max - min) * x - min;

    public static string ToString<T>(this double[] arr)
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("[");
        builder.Append(string.Join(", ", arr));
        builder.Append("]");
        return builder.ToString();
    }

    public static double[][] ToBounds(this double[] bound, int length)
    {
        List<double[]> bounds = new();

        for (int i = 0; i < length; i++)
            bounds.Add((double[])bound.Clone());

        return bounds.ToArray();
    }
    public static double[][] LinearBounds(double min, double max, int length)
        => ToBounds(new double[] {min, max}, length);
}