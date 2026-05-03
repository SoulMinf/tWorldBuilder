using Microsoft.Xna.Framework.Input;
using Terraria.ModLoader;

namespace TerrariaInGameWorldEditor.Content
{
    internal class Keybinds : ModSystem
    {
        public static ModKeybind OpenEditorMK { get; private set; }
        public static ModKeybind DeleteMK { get; private set; }
        public static ModKeybind CopyMK { get; private set; }
        public static ModKeybind PasteMK { get; private set; }
        public static ModKeybind ChangeCornerOnPasteMK { get; private set; }
        public static ModKeybind RotateMK { get; private set; }
        public static ModKeybind MirrorMK { get; private set; }
        public static ModKeybind CutMK { get; private set; }
        public static ModKeybind UndoMK { get; private set; }
        public static ModKeybind RedoMK { get; private set; }
        public static ModKeybind SaveMK { get; private set; }
        public static ModKeybind FastMoveMK { get; private set; }

        public override void Load()
        {
            base.Load();
            OpenEditorMK = KeybindLoader.RegisterKeybind(Mod, "OpenEditor", Keys.O);
            DeleteMK = KeybindLoader.RegisterKeybind(Mod, "Delete", Keys.Delete);
            CopyMK = KeybindLoader.RegisterKeybind(Mod, "Copy", Keys.C);
            PasteMK = KeybindLoader.RegisterKeybind(Mod, "Paste", Keys.V);
            ChangeCornerOnPasteMK = KeybindLoader.RegisterKeybind(Mod, "ChangeCorner", Keys.T);
            RotateMK = KeybindLoader.RegisterKeybind(Mod, "RotateSelection", Keys.R);
            MirrorMK = KeybindLoader.RegisterKeybind(Mod, "MirrorSelection", Keys.M);
            CutMK = KeybindLoader.RegisterKeybind(Mod, "Cut", Keys.X);
            UndoMK = KeybindLoader.RegisterKeybind(Mod, "Undo", Keys.Z);
            RedoMK = KeybindLoader.RegisterKeybind(Mod, "Redo", Keys.Y);
            SaveMK = KeybindLoader.RegisterKeybind(Mod, "Save", Keys.S);
            FastMoveMK = KeybindLoader.RegisterKeybind(Mod, "MoveFaster", Keys.LeftShift);
        }
    }
}
