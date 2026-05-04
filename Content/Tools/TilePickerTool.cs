using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Map;
using Terraria.ModLoader;
using TerrariaInGameWorldEditor.Common;
using TerrariaInGameWorldEditor.Common.Utils;
using TerrariaInGameWorldEditor.Editor;
using TerrariaInGameWorldEditor.UIElements.Button;

namespace TerrariaInGameWorldEditor.Content.Tools
{
    internal class TilePickerTool : Tool
    {
        public TilePickerTool()
        {
            ToggleToolButton = new TIGWEButton(ModContent.Request<Texture2D>($"{TerrariaInGameWorldEditor.ASSET_PATH}/Assets/Tools/TilePickerTool"));
            ToggleToolButton.HoverText = LocalizationUtils.GetTextValue("Tools.TilePickerTool.HoverText");
        }

        public override string GetInfoText()
        {
            return LocalizationUtils.GetTextValue("Tools.TilePickerTool.InfoText", (Main.tile[Player.tileTargetX, Player.tileTargetY].HasTile ? TileID.Search.GetName(Main.tile[Player.tileTargetX, Player.tileTargetY].TileType) : "Air"), Main.tile[Player.tileTargetX, Player.tileTargetY].TileType, (Main.tile[Player.tileTargetX, Player.tileTargetY].WallType != WallID.None ? WallID.Search.GetName(Main.tile[Player.tileTargetX, Player.tileTargetY].WallType) : "None"), Main.tile[Player.tileTargetX, Player.tileTargetY].WallType);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Point16 point = new Point16(Player.tileTargetX, Player.tileTargetY);
            TileCollection tc = new TileCollection();
            tc.TryAddTile(point, new TileCopy(point.X, point.Y));
            DrawUtils.DrawTileCollectionOutline(tc, point.ToPoint(), EditorSystem.Local.Settings.ToolColor);
        }

        public override void PostUpdateInput()
        {
            Main.blockMouse = true;

            // left click
            if (Main.mouseLeft && Main.mouseLeftRelease && !Main.LocalPlayer.mouseInterface)
            {
                Point point = new Point(Player.tileTargetX, Player.tileTargetY);
                EditorSystem.Local.SelectedTile = new TileCopy(point.X, point.Y);
            }
        }
    }
}
