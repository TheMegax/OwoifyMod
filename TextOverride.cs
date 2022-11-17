using System.Collections.Generic;
using BTD_Mod_Helper.Api.Data;

// ReSharper disable UnusedType.Global

namespace Owoify;

public class TextOverride : ModMultiTextOverride
{
    public override bool Active => true;
    public override Dictionary<string, string> Table => Main.GetDictionary();
}