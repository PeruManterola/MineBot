using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace MineBot.config
{
    internal class JSONReader
    {

        public string? Token { get; set; }
        public string? Prefix { get; set; }

        public async Task ReadJson()
        {
            using (StreamReader sr = new StreamReader("config.json"))
            {
                string json = await sr.ReadToEndAsync();
                //JsonStructure data = JsonConvert.DeserializeObject<JsonStructure>(json);
                JsonStructure data = JsonConvert.DeserializeObject<JsonStructure>(json);

                this.Token = data.Token;
                this.Prefix = data.Prefix;
            }
        }
    }

    internal sealed class JsonStructure
    {
        public required string Token { get; set; }
        public required string Prefix { get; set; }
    }
}