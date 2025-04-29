namespace AuthyDecryptor.UI.Wpf;

public class UiThreadHelper
{
    #region Private variables

    private static int _uiThreadId;
    private static TaskFactory? _taskFactory;

    #endregion Private variables

    #region Public methods

    #region Initialization

    public static void Initialize()
    {
        _uiThreadId = Environment.CurrentManagedThreadId;

        var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        _taskFactory = new TaskFactory(scheduler);
    }

    #endregion Initialization

    public static void RunOnUiThread(Action action)
    {
        if (IsOnUi())
        {
            action?.Invoke();
        }
        else
        {
            _taskFactory.StartNew(action);
        }
    }

    public static async Task RunAsyncOnUiThread(Func<Task> asyncAction)
    {
        if (IsOnUi())
        {
            await asyncAction();
        }
        else
        {
            await _taskFactory.StartNew(asyncAction).Unwrap();
        }
    }

    #endregion Public methods

    #region Private methods

    private static bool IsOnUi()
    {
        return Environment.CurrentManagedThreadId == _uiThreadId;
    }

    #endregion Private methods
}