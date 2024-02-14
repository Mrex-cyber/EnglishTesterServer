using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace EnglishTesterServer.DAL.Models.Models.Program
{
    public class Settings
    {
        public string ConnectionString { get; set; }

        public async Task<Settings> ReadFromFile(string filePath)
        {
            string json = await File.ReadAllTextAsync(filePath);

            Settings? readSettings = JsonConvert.DeserializeObject<Settings>(json);

            if (readSettings == null)
            {
                await Console.Out.WriteLineAsync("Can not read settings");
            }

            return readSettings!;
        }
    }
}
