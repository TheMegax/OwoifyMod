using System.Collections.Generic;
using System.IO;
using System.Linq;
using BTD_Mod_Helper;
using MelonLoader;
using Owoify;
using UnityEngine;
using Main = Owoify.Main;

[assembly: MelonInfo(typeof(Main), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace Owoify
{
    public class Main : BloonsTD6Mod
    {
        private static readonly Dictionary<string, string> Dict = new();
        
        public override void OnEarlyInitialize()
        {
            var dllLocation = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var assetBundlesLocation = new DirectoryInfo(dllLocation!).Parent?.FullName 
                                      + "\\BloonsTD6_Data\\StreamingAssets\\aa\\StandaloneWindows64\\Full\\";
            var localDataLocation = Directory.GetFiles(assetBundlesLocation, "local_data_assets_all*.bundle")[0];
            var localDataBundle = AssetBundle.LoadFromFile(localDataLocation);
            var csvEnglish = localDataBundle.LoadAsset("Assets/Data/Languages/English.csv").Cast<TextAsset>();
            var key = "";
            var chainValues = new List<string>();

            foreach (var line in csvEnglish.text.Split('\n'))
            {
                if (string.IsNullOrEmpty(line)) continue;
                if (line.StartsWith("//")) continue;
                var tokens = line.Split(new[] { ',' }, 2);
                if (tokens.Length == 1) continue;
                if (string.IsNullOrEmpty(tokens[0])) continue;
                if (string.IsNullOrEmpty(tokens[1])) continue;
                    
                if (tokens[1].StartsWith("\""))
                {
                    key = tokens[0];
                    tokens[1] = tokens[1].TrimStart('"');
                    if (tokens[1].EndsWith("\""))
                    {
                        tokens[1] = tokens[1].TrimEnd('"');
                        chainValues.Add(tokens[1]);
                    }
                    else
                    {
                        chainValues.Add(tokens[1]);
                        continue;
                    }
                }
                else if (tokens[1].EndsWith("\""))
                {
                    tokens[1] = tokens[1].TrimEnd('"');
                    chainValues.Add(tokens[1]);
                }
                else
                { 
                    key = tokens[0]; 
                    chainValues.Add(tokens[1]);
                }
                
                var valueString = chainValues.Aggregate("", (current, value) => current + (value + "\n")); 
                valueString = valueString.TrimEnd('\n'); 
                valueString = Owoifier.Owoify(valueString);
                
                Dict.Add(key, valueString); 
                chainValues.Clear();
            }
            localDataBundle.Unload(true);
        }

        public static Dictionary<string, string> GetDictionary()
        {
            return Dict;
        }
    }
}

