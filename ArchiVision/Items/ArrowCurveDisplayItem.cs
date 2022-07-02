/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ArchiVision
{
    public class ArrowCurveDisplayItem : CurveDisplayItem
    {
        public double StartArrow { get; protected set; }
        public double EndArrow { get; protected set; }


        public ArrowCurveDisplayItem(IGH_DocumentObject owner, GH_Curve curve, CurveDisplayAttribute att, double start, double end)
            :base(owner, curve, att)
        {
            StartArrow = start;
            EndArrow = end;

        }

        //public ArrowCurveDisplayItem(IGH_DocumentObject owner, GH_Curve curve, Color color, float thickness, Linetype linetype, double start, double end, double mult, bool absolute)
        //    : base(owner,curve, color, thickness, linetype, absolute)
        //{
        //    StartArrow = start;
        //    EndArrow = end;
        //    ArrowMult = mult;
        //}

        public override void DrawViewportWires(RhinoViewport Viewport, DisplayPipeline Display, Rectangle3d drawRect, double unitPerPx)
        {

            Curve curve = ((GH_Curve)Geometry).Value;

            double tol = Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance;

            double t0 = curve.Domain.T0;
            double t1 = curve.Domain.T1;

            if (StartArrow > 0)
            {
                Point3d start = curve.PointAtStart;
                Viewport.GetWorldToScreenScale(start, out double ratio);

                var intersect = Intersection.CurveSurface(curve, curve.Domain, 
                    new Sphere(start, StartArrow / ratio).ToNurbsSurface(), tol, tol);

                if(intersect.Count > 0)
                {
                    t0 = intersect[0].ParameterA;

                    Display.DrawArrowHead(start, start - curve.PointAt(t0), Colour, StartArrow, StartArrow);

                }

            }
            if (EndArrow > 0)
            {
                Point3d end = curve.PointAtEnd;
                Viewport.GetWorldToScreenScale(end, out double ratio);

                var intersect = Intersection.CurveSurface(curve, curve.Domain,
                    new Sphere(end, EndArrow / ratio).ToNurbsSurface(), tol, tol);

                if (intersect.Count > 0)
                {
                    t1 = intersect[0].ParameterA;

                    Display.DrawArrowHead(end, end - curve.PointAt(t1), Colour, EndArrow, EndArrow);
                }

            }

            //Display.DrawCurve(curve.Trim(t0, t1), Colour, 20);

            if(t0 < t1)
            {
                PatternCurve.Clear();
                Points.Clear();
                CreatePatternCurve(new GH_Curve(curve.Trim(t0, t1)), Linetype);
                base.DrawViewportWires(Viewport, Display, drawRect, unitPerPx);
            }

        }
    }
}