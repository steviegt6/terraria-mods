using System.IO;

using Build.Shared;

namespace Build.Post;

internal static class Program
{
    public static void Main(string[] args)
    {
        var projectCtx = ProjectContext.Create(Directory.GetCurrentDirectory(), args);
        TaskManager.RunTasks(TaskManager.InitializeTasks(typeof(Program).Assembly), projectCtx);
    }
}