

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Effects/Extensions/CurvedText")]
public class CurvedText : BaseMeshEffect {
    public AnimationCurve curve = AnimationCurve.Linear(0,0,1,10);
    public float multiplier = 1;
    
    public override void ModifyMesh(VertexHelper vh) {
        List<UIVertex> verts = new List<UIVertex>();
        vh.GetUIVertexStream(verts);
        for (int i = 0; i < verts.Count; i++) {
            UIVertex vert = verts[i];
            vert.position.y += curve.Evaluate(vert.position.x) * multiplier;
            verts[i] = vert;
        }
        vh.Clear();
        vh.AddUIVertexTriangleStream(verts);
    }
}