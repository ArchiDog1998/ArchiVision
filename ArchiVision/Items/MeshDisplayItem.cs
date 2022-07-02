/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using Grasshopper.Kernel.Components;
using Grasshopper.Kernel.Types;
using Microsoft.VisualBasic;
using Rhino;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiVision
{
    public class MeshDisplayItem : GeometryDisplayItem
    {
		public CurveDisplayAttribute OutLineAtt { get; private set; }

        //public MeshRenderItem(IGH_DocumentObject owner, MeshRenderItem other)
        //    : base(owner, other.Geometry, false)
        //{
        //    SubRenderItem.Clear();
        //    this.Material = other.Material;
        //    foreach (var item in other.SubRenderItem)
        //    {
        //        if (item is MeshOutlineDisplayItem) continue;
        //        this.SubRenderItem.Add(item);
        //    }
        //}

        public MeshDisplayItem(IGH_DocumentObject owner, GH_Mesh geometry, GH_Material material, CurveDisplayAttribute nakedEdgeAtt, 
            CurveDisplayAttribute interiorEdgeAtt, CurveDisplayAttribute outLineAtt, CurveDisplayAttribute sharpEdgeAtt, double angle, int topmost)
            :base(owner, geometry, topmost)
        {
			Material = material?.Value;
			OutLineAtt = outLineAtt;

			SubRenderItem.Clear();

			if(nakedEdgeAtt != null || interiorEdgeAtt != null || sharpEdgeAtt != null)
            {
				List<GH_Curve> naked = new List<GH_Curve>();
				List<GH_Curve> interior = new List<GH_Curve>();
				List<GH_Curve> sharp = new List<GH_Curve>();
				checked
				{
                    geometry.Value.FaceNormals.ComputeFaceNormals();
                    for (int i = 0; i < geometry.Value.TopologyEdges.Count; i++)
					{
						if (!geometry.Value.TopologyEdges.IsNgonInterior(i))
						{
                            IndexPair verticeIndexPair = geometry.Value.TopologyEdges.GetTopologyVertices(i);
                            Vector3f vector1 = geometry.Value.Normals[verticeIndexPair.I];
                            Vector3f vector2 = geometry.Value.Normals[verticeIndexPair.J];
                            vector1.Unitize(); vector2.Unitize();
                            Vector3f normal = Vector3f.Add(vector1, vector2);

                            GH_Curve line = new GH_Curve(geometry.Value.TopologyEdges.EdgeLine(i).ToNurbsCurve());
                            int[] faces = geometry.Value.TopologyEdges.GetConnectedFaces(i);
                            switch (faces.Length)
                            {
                                case 1:
                                    naked.Add(line);
                                    break;
                                case 2:
                                    if(sharpEdgeAtt == null)
                                    {
                                        interior.Add(line);
                                        break;
                                    }
                                    Vector3d vec0 = geometry.Value.FaceNormals[faces[0]];
                                    Vector3d vec1 = geometry.Value.FaceNormals[faces[1]];
                                    if (Vector3d.VectorAngle(vec0, vec1) > angle)
                                        sharp.Add(line);
                                    else interior.Add(line);
                                    break;
                            }
                        }
                    }
				}

				if (nakedEdgeAtt != null)
					naked.ForEach((crv) => SubRenderItem.Add(new CurveDisplayItem(owner, crv, nakedEdgeAtt)));

				if (interiorEdgeAtt != null)
					interior.ForEach((crv) => SubRenderItem.Add(new CurveDisplayItem(owner, crv, interiorEdgeAtt)));

				if (sharpEdgeAtt != null)
					sharp.ForEach((crv) => SubRenderItem.Add(new CurveDisplayItem(owner, crv, sharpEdgeAtt)));
			}

			if (outLineAtt != null) SubRenderItem.Add(new MeshOutlineDisplayItem(owner, geometry, outLineAtt));
		}


        public override void DrawViewportMeshes(RhinoViewport Viewport, DisplayPipeline Display, Rectangle3d drawRect, double unitPerPx)
        {
            if (Material != null && Geometry != null && Geometry is GH_Mesh mesh && mesh.Value != null) 
                Display.DrawMeshShaded(mesh.Value, Material);

            base.DrawViewportMeshes(Viewport, Display, drawRect, unitPerPx);
        }
    }
}
