/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using Rhino.Display;
using ArchiVision.Parameters;

namespace ArchiVision
{
    public class Param_DisplayItem : Param_ArchiVision<GH_DisplayItem>, IGH_PreviewObject
    {
        #region Values

        //private ArchiVisionConduitForParam<T> _conduit;
        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.hidden;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon => Properties.Resources.RenderItemParameter_24_24;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("8c28ec2c-06b6-40f5-91c5-d7c3faed3245");

        public bool IsPreviewCapable => true;

        public BoundingBox ClippingBox => ComputeClippingBox();

        public bool Hidden { get; set; }

        BoundingBox m_clippingBox;
        protected BoundingBox ComputeClippingBox()
        {
            if (m_clippingBox.IsValid)
            {
                return m_clippingBox;
            }
            m_clippingBox = BoundingBox.Empty;
            if (m_data.IsEmpty)
            {
                return m_clippingBox;
            }
            foreach (List<GH_DisplayItem> branch in m_data.Branches)
            {
                foreach (GH_DisplayItem item in branch)
                {
                    if (item == null || !item.IsValid)
                    {
                        continue;
                    }
                    m_clippingBox.Union(item.Value.ClippingBox);
                }
            }
            return m_clippingBox;
        }
        #endregion
        #endregion

        /// <summary>
        /// Initializes a new instance of the UIElementParameter class.
        /// </summary>
        public Param_DisplayItem(string name, string nickname, string description, GH_ParamAccess access)
                : base(name, nickname, description, access)
        {
        }

        public override void RemovedFromDocument(GH_Document document)
        {
            ArchiVisionInfo.Conduit.ItemParams.Remove(this);
            base.RemovedFromDocument(document);
        }

        public Param_DisplayItem()
        : this(GH_ParamAccess.item)
        {
        }

        public Param_DisplayItem(GH_ParamAccess access)
            : base("Render Item", "Ri", "Render Item", access)
        {

        }

        public override void PostProcessData()
        {
            if (!ArchiVisionInfo.Conduit.ItemParams.Contains(this))
                ArchiVisionInfo.Conduit.ItemParams.Add(this);
            base.PostProcessData();
        }

        public void DrawViewportWires(IGH_PreviewArgs args)
        {
            if (m_data.IsEmpty || Locked || Hidden) return;
            m_data.ToList().ForEach((item) => item.Value.DrawViewportWires(args.Viewport, args.Display, base.Attributes.Selected));
        }

        public void DrawViewportMeshes(IGH_PreviewArgs args)
        {
            if (m_data.IsEmpty || Locked || Hidden) return;
            m_data.ToList().ForEach((item) => item.Value.DrawViewportMeshes(args.Viewport, args.Display, base.Attributes.Selected));
        }

        //public List<DisplayItem> FindRenderItems(RhinoViewport viewport)
        //{
        //    List<DisplayItem> result = new List<DisplayItem>();
        //    foreach (GH_DisplayItem value in m_data)
        //    {
        //        result.Add(value.Value);
        //    }
        //    return result;
        //}
    }
}