/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
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
    /// <summary>
    /// General Display Item.
    /// </summary>
    public abstract class DisplayItem
    {
        protected List<DisplayItem> SubRenderItem { get; } = new List<DisplayItem>();
        public bool IsTopMost { get; }
        private IGH_DocumentObject _owner;
        public bool Selected => _owner.Attributes.Selected;
        public virtual BoundingBox ClippingBox 
        {
            get
            {
                BoundingBox box = BoundingBox.Empty;
                SubRenderItem.ForEach((item) => box.Union(item.ClippingBox));
                return box;
            }
        }

        public DisplayItem(IGH_DocumentObject owner, bool topMost)
        {
            IsTopMost = topMost;
            _owner = owner;
        }

        public virtual void DrawViewportWires(RhinoViewport Viewport, DisplayPipeline Display,
            Rectangle3d drawRect, double unitPerPx)
        {

        }

        public virtual void DrawViewportMeshes(RhinoViewport Viewport, DisplayPipeline Display,
            Rectangle3d drawRect, double unitPerPx)
        {

        }

        public void DrawViewportWires(RhinoViewport Viewport, DisplayPipeline Display, bool selected) 
        {
            if (_owner.OnPingDocument() != Grasshopper.Instances.ActiveCanvas.Document) return;
            SubRenderItem.ForEach((sub) => sub.DrawViewportWires(Viewport, Display, selected));
            if (IsTopMost) return;
            DrawViewportWires(Viewport, Display, default(Rectangle3d), double.NaN);
        }

        public void DrawViewportMeshes(RhinoViewport Viewport, DisplayPipeline Display, bool selected) 
        {
            if (_owner.OnPingDocument() != Grasshopper.Instances.ActiveCanvas.Document) return;

            SubRenderItem.ForEach((sub) => sub.DrawViewportMeshes(Viewport, Display, selected));
            if (IsTopMost) return;
            DrawViewportMeshes(Viewport, Display, default(Rectangle3d), double.NaN);
        }

        public void DrawViewportWires(DrawEventArgs e, Rectangle3d drawRect, double unitPerPx)
        {
            if (_owner.OnPingDocument() != Grasshopper.Instances.ActiveCanvas.Document) return;

            SubRenderItem.ForEach((sub) => sub.DrawViewportWires(e, drawRect, unitPerPx));
            if (!IsTopMost) return;
            DrawViewportWires(e.Viewport, e.Display, drawRect, unitPerPx);
        }

        public void DrawViewportMeshes(DrawEventArgs e, Rectangle3d drawRect, double unitPerPx)
        {
            if (_owner.OnPingDocument() != Grasshopper.Instances.ActiveCanvas.Document) return;

            SubRenderItem.ForEach((sub) => sub.DrawViewportMeshes(e, drawRect, unitPerPx));
            if (!IsTopMost) return;
            DrawViewportMeshes(e.Viewport, e.Display, drawRect, unitPerPx);
        }
    }
}
