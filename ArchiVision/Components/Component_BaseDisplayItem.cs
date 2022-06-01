using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ArchiVision
{
    /// <summary>
    /// This class contains all Display Items.
    /// </summary>
    public abstract class Component_BaseDisplayItem : Component_ArchiVision
    {
        #region Values
        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.hidden;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("6ec227c9-51f3-49f1-8965-f8997a819ce9");


        #endregion
        #endregion

        /// <summary>
        /// Initializes a new instance of the BaseRenderItemComponent class.
        /// </summary>
        public Component_BaseDisplayItem(string name, string nickname, string description)
          : base(name, nickname, description, Subcategory.UI_RhinoView)
        {
        }


        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected sealed override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Param_DisplayItem());
        }

        protected void AddShaderParam(GH_Component.GH_InputParamManager pManager)
        {
            Param_OGLShader param_OGLShader = new Param_OGLShader();
            //param_OGLShader.SetPersistentData(new GH_Material(Color.White));
            pManager[pManager.AddParameter(param_OGLShader, "Material", "M", "The material override", GH_ParamAccess.item)].Optional = true;
        }
    }
}