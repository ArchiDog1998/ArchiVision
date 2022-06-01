using Eto.Forms;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiVision
{
    public class MeshOutlineDisplayItem : CurveDisplayItem
    {
        public Linetype LineType { get; set; }


        public MeshOutlineDisplayItem(IGH_DocumentObject owner, GH_Mesh mesh, CurveDisplayAttribute att)
            : base(owner, null, att.Colour, att.Thickness, att.LineType, true)
        {
            Geometry = mesh;
            LineType = att.LineType;
        }


        public override void DrawViewportWires(RhinoViewport Viewport, DisplayPipeline Display, Rectangle3d drawRect, double unitPerPx)
        {
            PatternCurve.Clear();
            Points.Clear();
            var polys = ((GH_Mesh)Geometry).Value.GetOutlines(new ViewportInfo(Viewport), drawRect.Plane);


            if (polys != null) foreach (var item in polys)
                {
                    //Polyline poly = CurveExtend(polyline, 5 * (thickness + offsetDistance));
                    if (item == null) continue;

                    Curve poly = item.ToPolylineCurve();
                    if (poly == null) continue;

                    if (!item.IsClosed)
                        poly = poly.Extend(CurveEnd.Both, Size / unitPerPx, CurveExtensionStyle.Line);


                    //Dash is not so Great. Unit issue.
                    CreatePatternCurve(new GH_Curve(poly), LineType, unitPerPx);
                }

            base.DrawViewportWires(Viewport, Display, drawRect, unitPerPx);
        }
    }
}
