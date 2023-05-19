namespace SomeCode;

public static class RowSumCalculator
{
    public static IEnumerable<double> GetRowSums(IEnumerable<double> row)
    {
        var sum = .0;
        foreach (var number in row)
        {
            sum += number;
            yield return sum;
        }
    }
    
    public static IEnumerable<double> GetRowSumsUsingLinq(IEnumerable<double> row)
    {
        var sum = .0;
        return row.Select(n => sum+=n);
    }
}