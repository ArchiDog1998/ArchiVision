using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ArchiVision.Components
{
    public class Component_CurveDisplayItemAttribute : Component_ArchiVision
    {
        #region Values
        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon => Properties.Resources.CurveRenderItemAttributeComponent_24_24;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("1f886b58-9e34-4cf4-9b43-07b8adf3c2b4");


        #endregion
        private Param_Integer _lineType = null;
        #endregion

        /// <summary>
        /// Initializes a new instance of the CurveRenderItemAttributeComponent class.
        /// </summary>
        public Component_CurveDisplayItemAttribute()
          : base("Curve Display Attribute", "Nickname",
              "Description", Subcategory.UI_RhinoView)
        {
        }

        #region Calculate
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddColourParameter("Colour", "C", "Colour", GH_ParamAccess.item, Color.White);
            pManager.AddNumberParameter("Thickness", "t", "Thickness", GH_ParamAccess.item, 2);
            _lineType = (Param_Integer)pManager[pManager.AddIntegerParameter("LineType", "T", "LintType", GH_ParamAccess.item, -1)];
            UpdateNamedValue();
            pManager.AddBooleanParameter("TopMost", "M", "TopMost", GH_ParamAccess.item, false);
        }

        private void UpdateNamedValue()
        {
            if (_lineType == null) return;
            _lineType.AddNamedValue(Rhino.RhinoDoc.ActiveDoc.Linetypes.FindIndex(-1).Name, -1);
            foreach (var type in Rhino.RhinoDoc.ActiveDoc.Linetypes)
            {
                if(type.HasIndex && type.HasName)
                {
                    _lineType.AddNamedValue(type.Name, type.Index);
                }
            }
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Param_CurveDisplayAttribute());
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Color colour = Color.White;
            double thickness = 2;
            int lineT = 0;
            bool abso = true;
            bool topMost = false;

            DA.GetData(0, ref colour);
            DA.GetData(1, ref thickness);
            DA.GetData(2, ref lineT);
            DA.GetData(3, ref topMost);

            Linetype realtype = Rhino.RhinoDoc.ActiveDoc.Linetypes.FindIndex(lineT);
            this.Message = realtype.Name;

            DA.SetData(0, new CurveDisplayAttribute(colour, (float)thickness, realtype, topMost));

            UpdateNamedValue();
        }
        #endregion
    }
}