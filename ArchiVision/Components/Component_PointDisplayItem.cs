using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Rhino.Display;
using Rhino.Geometry;

namespace ArchiVision.Components
{
    public class Component_PointDisplayItem : Component_BaseDisplayItem
    {
        /// <summary>
        /// Initializes a new instance of the Component_PointDisplayItem class.
        /// </summary>
        public Component_PointDisplayItem()
          : base("Point Display Item", "Pt DI","Point Display Item")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "P", "Point", GH_ParamAccess.item);
            pManager.AddColourParameter("Colour", "C", "Colour", GH_ParamAccess.item, Color.White);
            pManager.AddNumberParameter("Radius", "R", "Radius", GH_ParamAccess.item, 5);
            Param_Integer integer = (Param_Integer)pManager[pManager.AddIntegerParameter("Type", "T", "Type", GH_ParamAccess.item, (int)PointStyle.Circle)];

            var indexs = Enum.GetValues(typeof(PointStyle));
            var names = Enum.GetNames(typeof(PointStyle));
            for (int i = 0; i < indexs.Length; i++)
            {
                integer.AddNamedValue(names[i], (int)indexs.GetValue(i));
            }

            pManager.AddBooleanParameter("TopMost", "M", "TopMost", GH_ParamAccess.item, false);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Point pt = default;
            Color color = default;
            double radius = default;
            int type = default;
            bool topMost = false;

            DA.GetData(0, ref pt);
            DA.GetData(1, ref color);
            DA.GetData(2, ref radius);
            DA.GetData(3, ref type);
            DA.GetData(4, ref topMost);

            PointStyle style = (PointStyle)type;
            this.Message = style.ToString();

            DA.SetData(0, new PointDisplayItem(this, pt, color, (PointStyle)type, (float)radius, topMost));
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;


        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.PointRenderItemComponent_24_24;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("BEBECEE7-2FA3-404B-B342-48D847E6B1AA"); }
        }
    }
}