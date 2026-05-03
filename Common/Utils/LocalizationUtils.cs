using Terraria.Localization;

namespace TerrariaInGameWorldEditor.Common.Utils
{
    internal static class LocalizationUtils
    {
        public static string GetTextValue(string key, params object[] args)
        {
            return Language.GetTextValue($"Mods.TerrariaInGameWorldEditor.{key}", args);
        }

        public static LocalizedText GetText(string key)
        {
            return Language.GetText($"Mods.TerrariaInGameWorldEditor.{key}");
        }
    }
}
