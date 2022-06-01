/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ArchiVision
{
    public class ArrowCurveDisplayItem : CurveDisplayItem
    {

        public double StartArrow { get; protected set; }
        public double EndArrow { get; protected set; }

        public double ArrowMult { get; protected set; }

        public ArrowCurveDisplayItem(IGH_DocumentObject owner, GH_Curve curve, CurveDisplayAttribute att, double start, double end, double mult)
            :base(owner, curve, att)
        {
            StartArrow = start;
            EndArrow = end;
            ArrowMult = mult;
        }

        public ArrowCurveDisplayItem(IGH_DocumentObject owner, GH_Curve curve, Color color, float thickness, Linetype linetype, double start, double end, double mult, bool absolute)
            : base(owner,curve, color, thickness, linetype, absolute)
        {
            StartArrow = start;
            EndArrow = end;
            ArrowMult = mult;
        }

        public override void DrawViewportWires(RhinoViewport Viewport, DisplayPipeline Display, Rectangle3d drawRect, double unitPerPx)
        {
            base.DrawViewportWires(Viewport, Display, drawRect, unitPerPx);

            Curve curve = ((GH_Curve)Geometry).Value;
            double arrowSize = Size * ArrowMult;

            if (StartArrow > 0)
                Display.DrawArrowHead(curve.PointAtStart - curve.TangentAtStart * StartArrow, -curve.TangentAtStart, Colour, arrowSize, arrowSize);
            if (EndArrow > 0)
                Display.DrawArrowHead(curve.PointAtEnd + curve.TangentAtEnd * EndArrow, curve.TangentAtEnd, Colour, arrowSize, arrowSize);
        }
    }
}