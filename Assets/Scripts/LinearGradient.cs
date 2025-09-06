using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Effects/Linear Gradient")]
public class LinearGradient : BaseMeshEffect
{
    public Color leftColor = new Color32(192, 132, 252, 255);   // #C084FC
    public Color middleColor = new Color32(244, 114, 182, 255); // #F472B6
    public Color rightColor = new Color32(248, 113, 113, 255);  // #F87171

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive() || vh.currentVertCount == 0)
            return;

        UIVertex vert = new UIVertex();
        int count = vh.currentVertCount;

        // Find min and max X
        float minX = float.MaxValue;
        float maxX = float.MinValue;

        for (int i = 0; i < count; i++)
        {
            vh.PopulateUIVertex(ref vert, i);
            if (vert.position.x < minX) minX = vert.position.x;
            if (vert.position.x > maxX) maxX = vert.position.x;
        }

        float width = maxX - minX;
        if (Mathf.Approximately(width, 0f)) width = 1f;

        // Assign gradient colors
        for (int i = 0; i < count; i++)
        {
            vh.PopulateUIVertex(ref vert, i);
            float ratio = (vert.position.x - minX) / width;

            if (ratio < 0.5f)
                vert.color = Color.Lerp(leftColor, middleColor, ratio * 2f);
            else
                vert.color = Color.Lerp(middleColor, rightColor, (ratio - 0.5f) * 2f);

            vh.SetUIVertex(vert, i);
        }
    }
}
