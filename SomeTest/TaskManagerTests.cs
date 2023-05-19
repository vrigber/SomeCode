using SomeCode;
namespace SomeTest;

public class TaskManagerTests
{
    [Fact]
    public async Task ExecutesTasksSequentially()
    {
        // Arrange
        var taskManager = new TaskManager();
        var resultList = new List<int>();

        // Act
        taskManager.AddTask(() =>
        {
            Thread.Sleep(500);
            resultList.Add(1);
        });
        taskManager.AddTask(() =>
        {
            Thread.Sleep(200);
            resultList.Add(2);
        });
        taskManager.AddTask(() =>
        {
            Thread.Sleep(300);
            resultList.Add(3);
        });
        var completionTask = taskManager.WaitForCompletion();
        
        Assert.False(completionTask.IsCompleted);
        
        await Task.Delay(700);
        
        Assert.NotEqual(new List<int> { 1, 2, 3 }, resultList);
        
        await completionTask;
        
        Assert.True(completionTask.IsCompleted);
        
        Assert.Equal(new List<int> { 1, 2, 3 }, resultList);
    }

    [Fact]
    public async Task WaitsForCompletionBeforeReturning()
    {
        // Arrange
        var taskManager = new TaskManager();
        var i = 0;

        var start = DateTime.Now;
        // Act
        taskManager.AddTask(() =>
        {
            Thread.Sleep(100);
            Interlocked.Increment(ref i);
        });
        
        var completionTask = taskManager.WaitForCompletion();
        
        taskManager.AddTask(() =>
        {
            Thread.Sleep(100);
            Interlocked.Increment(ref i);
        });
        
        taskManager.AddTask(() =>
        {
            Thread.Sleep(100);
            Interlocked.Increment(ref i);
        });
        
        taskManager.AddTask(() =>
        {
            Thread.Sleep(100);
            Interlocked.Increment(ref i);
        });
        
        // Wait for completion
        await completionTask;
        Assert.True(400 < (DateTime.Now - start).TotalMilliseconds);
        // Assert
        Assert.Equal(4,i);
    }
}
