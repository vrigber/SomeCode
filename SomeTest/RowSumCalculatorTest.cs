using SomeCode;
namespace SomeTest;

public class RowSumCalculatorTest
{
    private const double _tolerance = 0.0001;

    [Fact]
    public void GetRowSums_ReturnsCorrectSums()
    {
        ReturnsCorrectSums(RowSumCalculator.GetRowSums);
    }
    
    [Fact]
    public void GetRowSumsUsingLinq_ReturnsCorrectSums()
    {
        ReturnsCorrectSums(RowSumCalculator.GetRowSumsUsingLinq);
    }
    
    [Fact]
    public void GetRowSums_ReturnsPartialSumsUsingIterator()
    {
        ReturnsPartialSumsUsingIterator(RowSumCalculator.GetRowSums);
    }
    
    [Fact]
    public void GetRowSumsUsingLinq_ReturnsPartialSumsUsingIterator()
    {
        ReturnsPartialSumsUsingIterator(RowSumCalculator.GetRowSumsUsingLinq);
    }

    private static void ReturnsCorrectSums(Func<IEnumerable<double>,IEnumerable<double>> checkedMethod)
    {
        var row = new List<double> { 1.5, 2.0, 3.5 };
        var expectedSums = new List<double> { 1.5, 3.5, 7.0 };

        void Check()
        {
            var actualSums = checkedMethod(row);
            Assert.Equal(expectedSums, actualSums, new DoubleComparer(_tolerance));
        }
        Check();
        
        row.Clear();
        expectedSums.Clear();
        
        Check();
        
        row.Add(-2.5);
        expectedSums.Add(-2.5);
        
        Check();
        
        row.Add(-1.0);
        row.Add(-3.5);
        row.Add(-4.0);

        expectedSums.Add(-3.5);
        expectedSums.Add(-7.0);
        expectedSums.Add(-11.0);
        
        Check();
        
        
    }
    
    private static void ReturnsPartialSumsUsingIterator(Func<IEnumerable<double>,IEnumerable<double>> checkedMethod)
    {
        // Arrange
        var row = new List<double> { 1.5, 2.0, 3.5 };
        var index = 0;

        IEnumerable<double> IteratorChecker(IEnumerable<double> row)
        {
            foreach (var item in row)
            {
                yield return item;
                index++;
            }
        }
        var iteratorChecker = IteratorChecker(row);
        var actualSums = checkedMethod(iteratorChecker);

        var i = 0;
        foreach (var item in actualSums)
        {
            Assert.Equal(i++, index);
        }
    }

    private static IEnumerable<T> IteratorChecker<T>(IEnumerable<T> row, Action callback)
    {
        foreach (var item in row)
        {
            yield return item;
            callback();
        }
    }

    private class DoubleComparer : IEqualityComparer<double>
    {
        private readonly double _tolerance;

        public DoubleComparer(double tolerance)
        {
            _tolerance = tolerance;
        }

        public bool Equals(double x, double y)
        {
            return Math.Abs(x - y) < _tolerance;
        }

        public int GetHashCode(double obj)
        {
            return obj.GetHashCode();
        }
    }
}