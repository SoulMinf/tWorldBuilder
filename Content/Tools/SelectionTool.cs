using System.Drawing;
using TerrariaInGameWorldEditor.Common;

namespace TerrariaInGameWorldEditor.Content.Tools
{
    internal interface ISelectionTool
    {
        public TileCollection GetSelection();
        public void SetSelection(TileCollection selection);
        public void ResetSelection();
    }
}
