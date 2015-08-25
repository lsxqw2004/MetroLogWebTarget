namespace MetroLogWebTarget.Core.Infrastructure
{
    /// <summary>
    ///     表示在启动时执行的操作
    /// </summary>
    public interface IStartupTask
    {
        void Execute();

        int Order { get; }
    }
}