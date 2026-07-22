using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using UDPServer.World;

namespace UDPServer;

public class ServerLoop
{
    
    [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
    private static extern uint timeBeginPeriod(uint uMilliseconds);
    public WorldHolder GameWorld {get; private set;}
    private bool _isRunning = true;

    private long _lastTimestamp;
    private double _targetFrameTimeMs;

    public ServerLoop(WorldHolder gameWorld)
    {
        
        GameWorld = gameWorld;
        _lastTimestamp = Stopwatch.GetTimestamp();
        _targetFrameTimeMs = 1000.0 / 60.0;

    }

    public void Start()
    {

        if (OperatingSystem.IsWindows())
        {
            timeBeginPeriod(1);
        }
        
        Thread loopThread = new Thread(NormalStart);
        loopThread.IsBackground = true;
        loopThread.Priority = ThreadPriority.Highest;
        loopThread.Start();

    }

    private void NormalStart()
    {
        
        while(_isRunning)
        {
            
            long currentTimeStamp = Stopwatch.GetTimestamp();
            float deltaTime = (float)(currentTimeStamp -  _lastTimestamp) / Stopwatch.Frequency;
            _lastTimestamp = currentTimeStamp;

            long workStart = Stopwatch.GetTimestamp();
            GameWorld.Update(deltaTime);
            long workEnd = Stopwatch.GetTimestamp();

            double workTimeMs = TickToMs(workEnd - workStart);
            double remainingMs = _targetFrameTimeMs - workTimeMs;

            if (remainingMs > 0)
            {
                
                if (remainingMs > 2.0f)
                {
                    Thread.Sleep((int)(remainingMs - 2.0));
                }

                long targetTicks = workStart + (long)(_targetFrameTimeMs * Stopwatch.Frequency / 1000.0f);

                while (Stopwatch.GetTimestamp() < targetTicks)
                {
                    Thread.SpinWait(10);
                }

            }

        }

    }

    private double TickToMs(long ticks)
    {
        return ticks * 1000.0 / Stopwatch.Frequency;
    }

}