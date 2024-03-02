using Newtonsoft.Json;
using RoguelikeBase.Serializaton;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RoguelikeBase.Utils
{
    internal class SaveGameManager
    {
        public static void SaveGame(GameWorld world) 
        {
            var data = SerializableWorld.CreateSerializableWorld(world.World);
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

        public static GameWorld LoadGame()
        {
            string data = string.Empty;
            using (StreamReader file = new StreamReader("savegame.json"))
            {
                data = file.ReadToEnd();
            }

            SerializableWorld newSerializableWorld = null;

            using (var sr = new StringReader(data))
            {
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    JsonSerializer serializer = new JsonSerializer();

                    newSerializableWorld = serializer.Deserialize<SerializableWorld>(reader);
                }
            }

            GameWorld world = new GameWorld();
            world.World = SerializableWorld.CreateWorldFromSerializableWorld(newSerializableWorld);

            return world;
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
