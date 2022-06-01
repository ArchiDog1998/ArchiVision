using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;

namespace ArchiVision
{
    public abstract class Component_ArchiVision : GH_Component
    {
        public Component_ArchiVision(string name, string nickname, string description, Subcategory subcategory)
            : base(name, nickname, description, "ArchiVision", subcategory.ToString().Replace('_', ' '))
        {
        }

        protected void AddSection(GH_Component.GH_InputParamManager pManager)
        {
            Param_GenericObject obj = new Param_GenericObject();
            obj.Optional = true;
            pManager.AddParameter(obj, "------", "---", "------", GH_ParamAccess.tree);
        }
    }
}
