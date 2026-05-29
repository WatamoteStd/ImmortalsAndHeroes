using System;
using System.Threading;
using LiteNetLib;
using LiteNetLib.Utils;
using IaH.Server.Managers;

namespace IaH.Server
{
    
    class Program
    {
        private static NetworkManager? _server;
        static void Main(string[] args)
        {
            _server = new NetworkManager();
            _server.Start(9050);

            Thread commandThread = new Thread(ReadConsoleCommands);
            commandThread.IsBackground = true;
            commandThread.Start();

            Console.WriteLine("---------IaH Server--------");

            // Main threat
            while(true)
            {
                
                _server.Update(0.015f);
                
                Thread.Sleep(15);

            }

        }
        static void ReadConsoleCommands()
        {
            
            while(true)
            {
                
                string? input = Console.ReadLine();
                if (string.IsNullOrEmpty(input)) continue;
                if (input == "/lobbylist")
                {
                    _server?.LobbyListRequest();
                }
                if (input == "/matchlist")
                {
                    _server?.MatchListRequest();
                }


            }

        }

    }

}