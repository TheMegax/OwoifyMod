using System.Collections.Generic;
using BTD_Mod_Helper;
using MelonLoader;
using Owoify;
using Main = Owoify.Main;

[assembly: MelonInfo(typeof(Main), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace Owoify
{
    public class Main : BloonsTD6Mod
    {
        private static Dictionary<string, string> _dict = new();

        public override void OnEarlyInitialize()
        {
            
        }

        public static Dictionary<string, string> GetDictionary()
        {
            return _dict;
            /*
            var dllLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            
            using var reader = new StreamReader(stream);
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var tokens = line?.Split(new[] { ',' }, 2);
                tokens[1] = tokens[1].TrimEnd('"');
                tokens[1] = tokens[1].TrimStart('"');
                _dict.Add(tokens[0], tokens[1]);
            }

            return _dict;
            */
        }
    }
}

