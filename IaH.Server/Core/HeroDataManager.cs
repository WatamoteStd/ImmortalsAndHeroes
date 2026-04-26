using IaH.Shared.Data;
using IaH.Shared.Networking;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;


namespace IaH.Server.Core
{
    public class HeroDataManager
    {

        private static Dictionary<ushort, HeroConfig> _heroes = new();

        public static void Initialize()
        {


            string pathToFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GameData");

            string[] pathToConfigs = Directory.GetFiles(pathToFolder, "*.json");

            foreach (string hero in pathToConfigs)
            {

                string jsonString = File.ReadAllText(hero);

                HeroConfig config = JsonSerializer.Deserialize<HeroConfig>(jsonString);

                if (config != null)
                {
                    _heroes.Add(config.HeroId, config);
                    Console.WriteLine($"[DataManager] Loaded hero: {config.Name} (ID: {config.HeroId})");
                }
            }
        }

        

        public static HeroConfig GetConfig(CharacterType type)
        {

            ushort id = (ushort)type;
            
            if (_heroes.ContainsKey(id))
            {
                return _heroes[id];
            }
            else
            {
                Console.WriteLine("HeroDataManager: Invalid JSON.");
                return null;
            }
                                             
        }

    }
}
