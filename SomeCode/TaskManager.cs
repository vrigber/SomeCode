namespace SomeCode;

public class TaskManager
{
    private readonly Queue<Action?> _actionQueue = new();
    private volatile Task _currentTask = Task.CompletedTask;
    private readonly object _lockObject = new();

    public void AddTask(Action? action)
    {
        lock (_lockObject)
        {
            _actionQueue.Enqueue(action);
            if (_currentTask.IsCompleted)
            {
                _currentTask = ProcessTaskAsync();
            }
        }
    }
    
    public async Task WaitForCompletion()
    {
        await _currentTask;
    }

    private async Task ProcessTaskAsync()
    {
        while (true)
        {
            Action? action;
            lock (_lockObject)
            {
                if (!_actionQueue.TryDequeue(out action)) 
                    break;
            }
            await Task.Run(action!); 
        }
    }
}