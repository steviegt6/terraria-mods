using System.IO;

using Build.Shared;

namespace Build.Pre;

internal static class Program
{
    public static void Main(string[] args)
    {
        var projectCtx = new ProjectContext(Directory.GetCurrentDirectory(), args[0]);
        TaskManager.RunTasks(TaskManager.InitializeTasks(typeof(Program).Assembly), projectCtx);
    }
}