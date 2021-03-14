using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using static System.Console;
using static System.Diagnostics.Process;

namespace FinTrader.Pro.Bonds
{
    public static class Recorder
    {
        static Stopwatch timer = new Stopwatch();
        static long bytesPhysicalBefore = 0;
        static long bytesVirtualBefore = 0;
        public static void Start()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            bytesPhysicalBefore = GetCurrentProcess().WorkingSet64;
            bytesVirtualBefore = GetCurrentProcess().VirtualMemorySize64;
            timer.Restart();
        }
        public static void Stop(ILogger logger)
        {
            timer.Stop();
            long bytesPhysicalAfter = GetCurrentProcess().WorkingSet64;
            long bytesVirtualAfter = GetCurrentProcess().VirtualMemorySize64;
            logger.LogDebug("Stopped recording.");
            logger.LogDebug($"{bytesPhysicalAfter - bytesPhysicalBefore:N0} physical bytes used.");
            logger.LogDebug($"{bytesVirtualAfter - bytesVirtualBefore:N0} virtual bytes used.");
            logger.LogDebug($"{timer.Elapsed} time span ellapsed.");
            logger.LogDebug($"{timer.ElapsedMilliseconds:N0} total milliseconds ellapsed.");
        }
    }
}
