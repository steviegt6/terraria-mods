using System;
using System.Collections.Generic;
using System.Reflection;

namespace Build.Shared;

public static class TaskManager
{
    public static IEnumerable<BuildTask> InitializeTasks(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            if (type.IsAbstract || !typeof(BuildTask).IsAssignableFrom(type))
            {
                continue;
            }

            if (Activator.CreateInstance(type) is BuildTask task)
            {
                yield return task;
            }
        }
    }
    
    public static void RunTasks(IEnumerable<BuildTask> tasks, ProjectContext ctx)
    {
        foreach (var task in tasks)
        {
            var name = task.GetType().Name;
            
            try
            {
                Console.WriteLine($"Running task: {name}");
                task.Run(ctx);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error running task {name}: {e}");
                throw;
            }
        }
    }
}