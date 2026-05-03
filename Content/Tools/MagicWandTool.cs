using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using TerrariaInGameWorldEditor.Common;
using TerrariaInGameWorldEditor.Common.Utils;
using TerrariaInGameWorldEditor.UIElements.Button;

namespace TerrariaInGameWorldEditor.Content.Tools
{
    internal class MagicWandTool : FillTool, ISelectionTool
    {
        private TileCollection _selection;

        public MagicWandTool()
        {
            ToggleToolButton = new TIGWEButton(ModContent.Request<Texture2D>($"{TerrariaInGameWorldEditor.ASSET_PATH}/Assets/Tools/MagicWandTool"));
            ToggleToolButton.HoverText = LocalizationUtils.GetTextValue("Tools.MagicWandTool.HoverText");
            _selection = new TileCollection();
        }

        public TileCollection GetSelection()
        {
            return _selection;
        }

        public void ResetSelection()
        {
            _selection.Clear();
        }

        public override string GetInfoText()
        {
            return LocalizationUtils.GetTextValue("Tools.MagicWandTool.InfoText", _selection.Count);
        }

        protected override void OnFill(TileCollection tiles)
        {
            base.OnFill(tiles);
            if (PlayerInput.GetPressedKeys().Contains(Keys.LeftShift) && _selection.Count > 0)
            {
                _selection.TryRemoveTiles(tiles);
            }
            else
            {
                _selection.TryAddTiles(tiles);
            }
        }

        public override void PostUpdateInput()
        {
            Main.blockMouse = true;

            // left click
            if (Main.mouseLeft && Main.mouseLeftRelease && !Main.LocalPlayer.mouseInterface)
            {
                // if ctrl is pressed down, keep adding tiles to our selection, if shift is pressed down remove tile from our selection
                if (!PlayerInput.GetPressedKeys().Contains(Keys.LeftControl) && !PlayerInput.GetPressedKeys().Contains(Keys.LeftShift))
                {
                    _selection.Clear();
                }
            }

            base.PostUpdateInput();

            // right click
            if (Main.mouseRight && Main.mouseRightRelease && !Main.LocalPlayer.mouseInterface)
            {
                _selection.Clear();
            }
        }
    }
}
