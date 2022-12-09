namespace PuzzleSolver.Core;

public static class MatrixExtensions
{
    public static T[,] RotateClockWise<T>(this T[,] matrix)
    {
        var height = matrix.GetLength(0);
        var width = matrix.GetLength(1);
        
        var rotatedMatrix = new T[width, height];

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                rotatedMatrix[x, height-1-y] = matrix[y, x];
            }
        }

        return rotatedMatrix;
    }

    public static bool IsEqualTo<T>(this T[,] matrix, T[,] other)
    {
        return matrix.Rank == other.Rank 
               && Enumerable.Range(0,matrix.Rank)
                   .All(dimension => matrix.GetLength(dimension) == other.GetLength(dimension)) 
               && matrix.Cast<T>().SequenceEqual(other.Cast<T>());
    }
    
    public static T[,] ToMatrix<T>(this T[][] source) {
        try 
        {
            var firstDim = source.Length;
            var secondDim = source.GroupBy(row => row.Length).Single().Key; // throws InvalidOperationException if source is not rectangular

            var result = new T[firstDim, secondDim];
            for (var i = 0; i < firstDim; ++i)
                for (var j = 0; j < secondDim; ++j)
                    result[i, j] = source[i][j];

            return result;
        }
        catch (InvalidOperationException) 
        {
            throw new InvalidOperationException("The given jagged array is not rectangular.");
        }
    }

    public static IEnumerable<T> GetRow<T>(this T[,] matrix, int rowNumber)
    {
        // return Enumerable.Range(0, matrix.GetLength(0))
        //     .Select(x => matrix[rowNumber, x]);
        for (var i = 0; i < matrix.GetLength(1); i++)
        {
            yield return matrix[rowNumber, i];
        }
    }

    public static IEnumerable<T> AsEnumerable<T>(this T[,] matrix)
    {
        for (var rowIndex = 0; rowIndex < matrix.GetLength(0); rowIndex++)
            for (var columnIndex = 0; columnIndex < matrix.GetLength(1); columnIndex++)
                yield return matrix[rowIndex, columnIndex]; 
        
    }
}