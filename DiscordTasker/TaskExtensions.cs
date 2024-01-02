using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordTasker.Extensions
{
    static class TaskExtensions
    {
        /// <summary>
        /// Taskを投げっぱなしにする
        /// </summary>
        public static void Forget(this Task task)
        {
            task.ContinueWith(x =>
            {
                Console.WriteLine($"error{x.Exception}");
            }, TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}
