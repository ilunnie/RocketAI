namespace AIContinuous.Nuenv;

public static class Search
{
    public static int BinarySearch(double[] x, double query)
    {
        int low = 0;
        int high = x.Length - 1;

        while (high - low > 1)
        {
            var mid = low + (high - low) / 2;
            var midValue = x[mid];
            
            if (query > midValue)
            {
                low = mid;
            }
            else
            {
                high = mid;
            }
        }

        return high;
    }
}