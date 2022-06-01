using ArchiVision.Parameters;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ArchiVision
{
    public class Param_CurveDisplayAttribute : Param_ArchiVision<GH_CurveRenderAttribute>
    {
        #region Values
        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.hidden;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon => Properties.Resources.CurveRenderAttributeParameter_24_24;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("99e97220-998e-4f76-8f9f-1ccc1bb0b98e");


        #endregion
        #endregion

        /// <summary>
        /// Initializes a new instance of the CurveRenderAttributeParameter class.
        /// </summary>
        /// <summary>
        /// Initializes a new instance of the UIElementParameter class.
        /// </summary>
        public Param_CurveDisplayAttribute(string name, string nickname, string description, GH_ParamAccess access)
                : base(name, nickname, description, access)
        {
        }

        public Param_CurveDisplayAttribute()
        : base("Curve Render Attribute", "CA", "Curve Render Attribute", GH_ParamAccess.item)
        {
        }
    }
}