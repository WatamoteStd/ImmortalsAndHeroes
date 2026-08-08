using System;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Server.World;

public class Loop
{

    [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
    private static extern uint TimeBeginPeriod(uint uMilliseconds);
    [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
    private static extern uint TimeEndPeriod(uint uMilliseconds);


    public int TickrateHz {get; private set;}
    private readonly long _targetTicksPerFrame;
    public bool IsRunning {get; private set;} = false;
    private Thread? _loopThread;


    public Loop(int tickrateHz)
    {
        
        TickrateHz = tickrateHz;
        _targetTicksPerFrame = Stopwatch.Frequency / TickrateHz;
        Console.WriteLine($"[SERVER LOOP] Hz:{TickrateHz} Tacts:{_targetTicksPerFrame}");
        TimeBeginPeriod(1);

    }

    public void Start()
    {
        
        IsRunning = true;
        _loopThread = new Thread(WorldLoop);
        _loopThread.Priority = ThreadPriority.Highest;
        _loopThread.Start();

    }

    private void WorldLoop()
    {
        long startTick;
        long targetTick;
        long lastTickTime = Stopwatch.GetTimestamp();

        //DEBUG
        int tickCount = 0;
        float frameTimer = 0f;
        
        while(IsRunning)
        {
            
            startTick = Stopwatch.GetTimestamp();
            long elapsedTicks = startTick - lastTickTime;
            float deltaTime = (float)elapsedTicks / Stopwatch.Frequency;
            lastTickTime = startTick;

            targetTick = startTick + _targetTicksPerFrame;

            //DEBUG
            tickCount++;
            frameTimer += deltaTime;

            if (frameTimer > 1.0f)
            {
                Console.WriteLine($"TICKS: {tickCount} | FrameTimer:{frameTimer}");
                frameTimer -= 1.0f;
                tickCount = 0;
            }




            while (Stopwatch.GetTimestamp() < targetTick)
            {
                long remain = targetTick - Stopwatch.GetTimestamp();

                if (remain > Stopwatch.Frequency / 1000)
                {
                    Thread.Sleep(1);
                }
                else if (remain > 0)
                {
                    Thread.SpinWait(1);
                }
                

            }

        }

    }

    public void Stop()
    {
    IsRunning = false;
    _loopThread?.Join();
    TimeEndPeriod(1);
    }


}