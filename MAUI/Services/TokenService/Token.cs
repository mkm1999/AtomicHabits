using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace MAUI.Services.TokenService
{
    public class Token
    {
        public static void SaveToken(string token)
        {
            var obj = new
            {
                Token = token
            };

            string jsonString = JsonSerializer.Serialize(obj);

            //saving
            string cacheDir = FileSystem.Current.CacheDirectory;
            string fullPath = Path.Combine(cacheDir, "Token.json");
            File.WriteAllText(fullPath, jsonString);
        }

        public static string GetToken()
        {
            string cacheDir = FileSystem.Current.CacheDirectory;
            string fullPath = Path.Combine(cacheDir, "Token.json");
            if(!File.Exists(fullPath))
            {
                return "";
            }
            string jsonString = File.ReadAllText(fullPath);
            dynamic jsonobj = JObject.Parse(jsonString);
            string token = jsonobj.Token;
            return token;
        }
    }
}
