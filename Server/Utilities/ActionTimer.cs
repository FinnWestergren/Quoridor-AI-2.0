using System;
using System.Diagnostics;

namespace Server.Utilities
{
    public static class ActionTimer
    {
        public static long TimeFunction<T>(Func<T> func, out T value)
        {
            var timer = new Stopwatch();
            timer.Start();
            value = func();
            timer.Stop();
            return timer.ElapsedMilliseconds;
        }
    }
}
