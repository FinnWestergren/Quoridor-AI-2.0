using System;
using System.Diagnostics;

namespace Server.Utilities
{
    public static class ActionTimer
    {
        public static (long time, T result) TimeFunction<T>(Func<T> func)
        {
            var timer = new Stopwatch();
            timer.Start();
            var value = func();
            timer.Stop();
            return (timer.ElapsedMilliseconds, value);
        }
    }
}
