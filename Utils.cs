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
}