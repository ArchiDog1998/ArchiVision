using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ArchiVision
{
    public class Component_MergedShaderDisplayItem : Component_BaseDisplayItem
    {
        #region Values
        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon => Properties.Resources.MergedRenderItemComponent_24_24;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("b980896c-e690-4a6b-b8d0-1eb5461ada9d");


        #endregion
        #endregion

        /// <summary>
        /// Initializes a new instance of the MergedShaderRenderItemComponent class.
        /// </summary>
        public Component_MergedShaderDisplayItem()
          : base("Merged Shader Render Item", "MS Ri", "Merged Shader Render Item For OutLine")
        {
        }

        #region Calculate
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Param_DisplayItem(GH_ParamAccess.list) { Hidden = true});
            pManager.AddParameter(new Param_CurveDisplayAttribute(), "Intersect Line Attribute", "IA", "Intersect Line Attribute", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddParameter(new Param_CurveDisplayAttribute(), "Out Line Attribute", "OA", "Out Line Attribute", GH_ParamAccess.item);
            pManager[2].Optional = true;
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<MeshDisplayItem> meshRenderItems = new List<MeshDisplayItem>();
            CurveDisplayAttribute interAtt = null;
            CurveDisplayAttribute outAtt = null;

            DA.GetDataList(0, meshRenderItems);
            DA.GetData(1, ref interAtt);
            DA.GetData(2, ref outAtt);

            DA.SetData(0, new MergedMeshDisplayItem(this, meshRenderItems, outAtt, interAtt));
        }
        #endregion
    }
}