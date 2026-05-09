using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using TerrariaInGameWorldEditor.Common;
using TerrariaInGameWorldEditor.Common.Utils;
using TerrariaInGameWorldEditor.Editor;
using TerrariaInGameWorldEditor.UIElements.CheckBox;
using TerrariaInGameWorldEditor.UIElements.DropDown;
using TerrariaInGameWorldEditor.UIElements.NumberField;

namespace TerrariaInGameWorldEditor.Content.Tools
{
    internal abstract class FillTool : Tool
    {
        protected int _tileCap = 10000;
        protected TIGWENumberField _tileCapField;
        protected TIGWECheckBox _includeCornersCheckBox;
        protected enum Target
        {
            Auto,
            Tiles,
            Walls,
            Liquid
        }
        protected Target _mode = Target.Auto;
        protected TIGWEDropDown<Target> _targetDropDown;
        protected Func<bool> _selectCondition;
        private HashSet<Point16> _lastFailedCoordsToAdd = new HashSet<Point16>();

        public FillTool()
        {
            _selectCondition = () =>
            {
                return PlayerInput.Triggers.JustPressed.MouseLeft;
            };

            // settings
            // tile cap
            _tileCapField = new TIGWENumberField(_tileCap, minValue: 1);
            _tileCapField.OnValueChanged += (newValue) => _tileCap = _tileCapField.GetValue();
            _tileCapField.Width.Set(120, 0);
            _tileCapField.Height.Set(26, 0);
            Settings.Add((LocalizationUtils.GetTextValue("Tools.FillTool.Settings.TileCap"), _tileCapField));

            // fill tiles connected at corners
            _includeCornersCheckBox = new TIGWECheckBox(false);
            Settings.Add((LocalizationUtils.GetTextValue("Tools.FillTool.Settings.CornerConnect"), _includeCornersCheckBox));

            // target
            _targetDropDown = new TIGWEDropDown<Target>();
            _targetDropDown.AddOption(Target.Auto, LocalizationUtils.GetTextValue("Tools.FillTool.Options.Targets.Auto"));
            _targetDropDown.AddOption(Target.Tiles, LocalizationUtils.GetTextValue("Tools.FillTool.Options.Targets.Tiles"));
            _targetDropDown.AddOption(Target.Walls, LocalizationUtils.GetTextValue("Tools.FillTool.Options.Targets.Walls"));
            _targetDropDown.AddOption(Target.Liquid, LocalizationUtils.GetTextValue("Tools.FillTool.Options.Targets.Liquid"));
            _targetDropDown.Height.Set(26, 0f);
            _targetDropDown.Width.Set(140, 0f);
            _targetDropDown.OnOptionChanged += (option) => _mode = option.Value;
            Settings.Add((LocalizationUtils.GetTextValue("Tools.FillTool.Settings.Target"), _targetDropDown));
        }

        protected virtual void OnFill(TileCollection tiles)
        {

        }

        protected virtual void OnFillFailed()
        {

        }

        public override void OnToolUnselect()
        {
            base.OnToolUnselect();
            _lastFailedCoordsToAdd.Clear();
        }

        protected virtual bool IsMatch(Point16 coords, TileCopy clickedTile)
        {
            if (clickedTile == null)
            {
                return false;
            }
            Tile tile = Main.tile[coords.X, coords.Y];
            switch (_mode)
            {
                case Target.Auto:
                    if (tile.HasTile || clickedTile.HasTile)
                    {
                        return tile.TileType == clickedTile.TileType && tile.HasTile == clickedTile.HasTile;
                    }
                    if (tile.WallType != WallID.None || clickedTile.WallType != WallID.None)
                    {
                        return tile.WallType != WallID.None && tile.WallType == clickedTile.WallType;
                    }
                    if (tile.LiquidAmount != 0 || clickedTile.LiquidAmount != 0)
                    {
                        if ((tile.LiquidAmount > 0 && clickedTile.LiquidAmount == 0) || (clickedTile.LiquidAmount > 0 && tile.LiquidAmount == 0))
                        {
                            return false;
                        }
                        return tile.LiquidAmount != 0 && tile.LiquidType == clickedTile.LiquidType;
                    }
                    return tile.TileType == clickedTile.TileType;

                case Target.Tiles:
                    return tile.TileType == clickedTile.TileType && tile.HasTile == clickedTile.HasTile;

                case Target.Walls:
                    if (tile.WallType != WallID.None || clickedTile.WallType != WallID.None)
                    {
                        return tile.WallType != WallID.None && tile.WallType == clickedTile.WallType;
                    }
                    return false;

                case Target.Liquid:
                    if (tile.LiquidAmount != 0 || clickedTile.LiquidAmount != 0)
                    {
                        return tile.LiquidAmount != 0 && tile.LiquidType == clickedTile.LiquidType;
                    }
                    return false;
            }
            return false;
        }

        public override void PostUpdateInput()
        {
            Point16 clickedTileCoords = new Point16(Player.tileTargetX, Player.tileTargetY);

            // left click
            if (_selectCondition() && !_lastFailedCoordsToAdd.Contains(clickedTileCoords))
            {
                TileCopy clickedTile = new TileCopy(clickedTileCoords.X, clickedTileCoords.Y);

                int count = 0;
                HashSet<Point16> coordsToAdd = new HashSet<Point16>();

                Queue<Point16> queue = new Queue<Point16>();
                queue.Enqueue(new Point16(clickedTileCoords.X, clickedTileCoords.Y));

                // go until we hit the tilecap or cant find any more tiles that we think match
                while (queue.Count > 0 && count <= _tileCap)
                {
                    Point16 coords = queue.Dequeue();

                    if (IsMatch(coords, clickedTile) && EditorSystem.Local.CurrentSelection.ContainsCoord(coords) == EditorSystem.Local.CurrentSelection.ContainsCoord(clickedTileCoords))
                    {
                        // if we dont already have it added, add it
                        if (coordsToAdd.Add(coords))
                        {
                            count++;
                        }

                        // tiles to check
                        List<Point16> directions = [
                            new Point16(coords.X + 1, coords.Y),
                            new Point16(coords.X - 1, coords.Y),
                            new Point16(coords.X, coords.Y + 1),
                            new Point16(coords.X, coords.Y - 1)
                        ];
                        if (_includeCornersCheckBox.IsChecked)
                        {
                            directions.AddRange(new List<Point16>
                            {
                                new Point16(coords.X + 1, coords.Y + 1),
                                new Point16(coords.X - 1, coords.Y + 1),
                                new Point16(coords.X - 1, coords.Y - 1),
                                new Point16(coords.X + 1, coords.Y - 1)
                            });
                        }

                        foreach (Point16 direction in directions)
                        {
                            if (!coordsToAdd.Contains(direction) && !queue.Contains(direction))
                            {
                                queue.Enqueue(direction);
                            }
                        }
                    }
                }
                if (count > _tileCap)
                {
                    OnFillFailed();
                    _lastFailedCoordsToAdd = coordsToAdd;
                }
                else
                {
                    TileCollection tilesToAdd = new TileCollection();
                    foreach (Point16 coord in coordsToAdd)
                    {
                        tilesToAdd.TryAddTile(coord, new TileCopy(coord.X, coord.Y));
                    }
                    OnFill(tilesToAdd);
                    _lastFailedCoordsToAdd.Clear();
                }
            }
        }
    }
}
