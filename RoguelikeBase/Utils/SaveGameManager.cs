using Newtonsoft.Json;
using RoguelikeBase.Serializaton;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeBase.Utils
{
    internal class SaveGameManager
    {
        public static void SaveGame(GameWorld world) 
        {
            var data = SerializableWorld.SerializeWorld(world.World);
            string jsonData;
            using (var sw = new StringWriter())
            {
                if (data != null)
                {
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(writer, data);
                    }

                }
                jsonData = sw.ToString();
            }

            deleteSaveData();

            using (StreamWriter file = new StreamWriter("savegame.json"))
            {
                file.Write(jsonData);
            }
        }

        private static void deleteSaveData()
        {
            if (File.Exists("savegame.json"))
            {
                File.Delete("savegame.json");
            }
        }
    }
}
