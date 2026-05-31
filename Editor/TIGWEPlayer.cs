using Terraria;
using Terraria.DataStructures;
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

        public override bool ImmuneTo(PlayerDeathReason damageSource, int cooldownCounter, bool dodgeable)
        {
            if (EditorSystem.Local.IsEditorVisible && EditorSystem.Local.Settings.ShouldEnableGodMode)
            {
                return true;
            }
            return base.ImmuneTo(damageSource, cooldownCounter, dodgeable);
        }

        public override void PreUpdate()
        {
            base.PreUpdate();
            if (EditorSystem.Local.IsEditorVisible && EditorSystem.Local.Settings.ShouldEnableGodMode)
            {
                Player.statLife = Player.statLifeMax2;
            }
        }
    }
}
