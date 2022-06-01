/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ArchiVision
{
    public class Component_CurveDisplayItem : Component_BaseDisplayItem
    {
        #region Values
        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon => Properties.Resources.CurveRenderItemComponent_24_24;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("c8ce6db4-5412-44c3-9b66-f433c421a37a");


        #endregion
        #endregion

        /// <summary>
        /// Initializes a new instance of the CurveRenderItemComponent class.
        /// </summary>
        public Component_CurveDisplayItem()
          : base("Curve Display Item", "Crv DI", "Curve Display Item")
        {
        }

        #region Calculate
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "Curve", GH_ParamAccess.item);
            pManager.AddParameter(new Param_CurveDisplayAttribute());
            pManager[1].Optional = true;

            AddSection(pManager);

            pManager.AddNumberParameter("Start Arrow", "S", "Start Arrow", GH_ParamAccess.item, -1);
            pManager.AddNumberParameter("End Arrow", "E", "End Arrow", GH_ParamAccess.item, -1);
            pManager.AddNumberParameter("Arrow Mult", "M", "Arrow Mult, bigger than 1!", GH_ParamAccess.item, 10);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Curve curve = null;
            CurveDisplayAttribute att = new CurveDisplayAttribute();

            double start = -1;
            double end = -1;
            double mult = 10;

            DA.GetData(0, ref curve);
            DA.GetData(1, ref att);

            DA.GetData(3, ref start);
            DA.GetData(4, ref end);
            DA.GetData(5, ref mult);

            mult = Math.Max(mult, 1);

            DA.SetData(0, new ArrowCurveDisplayItem(this, curve, att, start ,end, mult));
        }
        #endregion
    }
}