using Terraria.ModLoader;
using TerrariaInGameWorldEditor.Common.Utils;
using TerrariaInGameWorldEditor.Content;

namespace TerrariaInGameWorldEditor.Editor
{
    internal class TIGWEPlayer : ModPlayer
    {
        public override void OnEnterWorld()
        {
            base.OnEnterWorld();
            if (Keybinds.OpenEditorMK.GetAssignedKeys().Count == 0)
            {
                TerrariaInGameWorldEditor.NewText(LocalizationUtils.GetTextValue("ModPlayer.Messages.NoKeybindSet"));
            }

            if (EditorSystem.Local.LanguageChange)
            {
                TerrariaInGameWorldEditor.NewText(LocalizationUtils.GetTextValue("Editor.System.Messages.LanguageChange"));
            }
        }
    }
}
