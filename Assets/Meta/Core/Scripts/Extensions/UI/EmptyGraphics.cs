using UnityEngine.UI;

namespace Core.UI
{
    public class EmptyGraphics : MaskableGraphic
    {
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            return;
        }
    }
}