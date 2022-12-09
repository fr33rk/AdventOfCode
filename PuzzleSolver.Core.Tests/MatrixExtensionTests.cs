namespace PuzzleSolver.Core.Tests;

[TestFixture]
public class MatrixExtensionTests
{
    [Test]
    public void RotateClockWise()
    {
        // Arrange
        var original = new char[3,4]
        {
            { 'A', 'B', 'C', 'D' },
            { 'a', 'b', 'c', 'd' },
            { '1', '2', '3', '4' }
        };

        var expected = new char[4, 3]
        {
            { '1', 'a', 'A' },
            { '2', 'b', 'B' },
            { '3', 'c', 'C' },
            { '4', 'd', 'D' }
        };

        // Act
        var actual = original.RotateClockWise();

        // Assert
        Assert.That(expected.IsEqualTo(actual));
    }
    
    [Test]
    public void IsEqual_WhenEqual_ReturnsTrue()
    {
        // Arrange
        var original = new char[3,4]
        {
            { 'A', 'B', 'C', 'D' },
            { 'a', 'b', 'c', 'd' },
            { '1', '2', '3', '4' }
        };
        
        var same = new char[3,4]
        {
            { 'A', 'B', 'C', 'D' },
            { 'a', 'b', 'c', 'd' },
            { '1', '2', '3', '4' }
        };
        
        // Act and assert
        Assert.That(original.IsEqualTo(same));
    }
    
    [Test]
    public void IsEqual_WhenNotEqual_ReturnsFalse()
    {
        // Arrange
        var original = new char[3,4]
        {
            { 'A', 'B', 'C', 'D' },
            { 'a', 'b', 'c', 'd' },
            { '1', '2', '3', '4' }
        };
        
        var same = new char[3,4]
        {
            { 'A', 'B', 'C', 'D' },
            { 'a', 'b', 'c', 'd' },
            { '1', '2', '3', '5' }
        };
        
        // Act and assert
        Assert.That(original.IsEqualTo(same), Is.False);
    }
}