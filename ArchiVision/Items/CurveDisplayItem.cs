using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
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
    public class CurveDisplayItem : GeometryDisplayItem
    {
        public float Size { get; }
        public List<Curve> PatternCurve { get; } = new List<Curve>();

        public List<Point3d> Points { get; } = new List<Point3d>();

        public override BoundingBox ClippingBox
        {
            get
            {
                BoundingBox box = base.ClippingBox;
                foreach (var item in PatternCurve)
                {
                    box.Union(item.GetBoundingBox(true));
                }
                foreach (var item in Points)
                {
                    box.Union(item);
                }
                return box;
            }
        }

        public CurveDisplayItem(IGH_DocumentObject owner, GH_Curve curve, CurveDisplayAttribute att)
            : this(owner, curve, att.Colour, att.Thickness, att.LineType, att.TopMost)
        {
        }

        public CurveDisplayItem(IGH_DocumentObject owner, GH_Curve curve, Color color, float thickness, Linetype linetype, bool topMost)
            : base(owner, curve, topMost)
        {
            this.Colour = color;
            this.Size = thickness;
            PatternCurve.Clear();
            Points.Clear();

            if (curve != null) CreatePatternCurve(curve, linetype);
        }

        protected void CreatePatternCurve(GH_Curve curve, Linetype linetype, double unitPerPx = 1)
        {
            if (linetype == null || linetype.Index == -1)
            {
                PatternCurve.Add(curve.Value);
                return;
            }

            double scale = Rhino.RhinoDoc.ActiveDoc.Linetypes.LinetypeScale / unitPerPx;

            List<double> lengths = new List<double>();
            double totalLen = 0;
            for (int i = 0; i < linetype.SegmentCount; i++)
            {
                double length;
                bool solid;
                linetype.GetSegment(i, out length, out solid);

                double realLength = length * scale;
                totalLen += realLength;
                lengths.Add(realLength);
            }

            foreach (Curve crvDiv in CurvePeriod(curve.Value, totalLen))
            {
                PartternCurve(crvDiv, lengths);
            }
        }
        private void PartternCurve(Curve curve, List<double> pattern)
        {
            for (int i = 0; i < pattern.Count; i++)
            {
                double length = pattern[i];
                if (length == 0)
                {
                    Points.Add(new Point3d(curve.PointAtStart));
                    continue;
                }
                if (curve.GetLength() <= length)
                {
                    if (i % 2 == 0) PatternCurve.Add(curve);
                    continue;
                }

                double t;
                curve.LengthParameter(length, out t);
                Curve[] crvs = curve.Split(t);
                if (crvs == null) continue;

                curve = crvs[1];
                if (i % 2 == 0) PatternCurve.Add(crvs[0]);
            }
        }

        private Curve[] CurvePeriod(Curve curve, double length)
        {
            List<Curve> curves = new List<Curve>();
            bool goon = true;
            while (goon)
            {
                double t;
                goon = curve.LengthParameter(length, out t);
                if (goon)
                {
                    Curve[] crvs = curve.Split(t);
                    if (crvs == null) break;
                    curves.Add(crvs[0]);
                    curve = crvs[1];
                }
            }
            curves.Add(curve);

            return curves.ToArray();
        }

        public override void DrawViewportWires(RhinoViewport Viewport, DisplayPipeline Display, Rectangle3d drawRect, double unitPerPx)
        {
            PatternCurve.ForEach((crv) => Display.DrawCurve(crv, Colour, (int)Size));
            Display.DrawPoints(Points, PointStyle.Circle, Size / 2, Colour);
        }
    }
}
