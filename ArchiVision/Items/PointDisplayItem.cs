/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Display;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiVision
{
    public class PointDisplayItem : GeometryDisplayItem
    {
        public PointStyle PtStyle { get; protected set; }
        public float Radius { get; protected set; }
        public PointDisplayItem(IGH_DocumentObject owner, GH_Point point, Color color, PointStyle style, float radius, int topMost)
            :base(owner, point, topMost)
        {
            PtStyle = style;
            Radius = radius;
            Colour = color;
        }
        public override void DrawViewportWires(RhinoViewport Viewport, DisplayPipeline Display, Rectangle3d drawRect, double unitPerPx)
        {
            Display.DrawPoint(((GH_Point)Geometry).Value, PtStyle, Radius, Colour);
        }
    }
}
