using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BossRush
{

    public static class ConfigElements
    {
        public static void Init(ConfigFile config)
        {
            ConfigElements.includeProgressMessage = config.Bind<bool>("Boss Rush", "Display Progress Messages", true);
        }
        // Floats
        public static ConfigEntry<bool> includeProgressMessage;

    }
}