using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiVision.Parameters
{
    public abstract class Param_ArchiVision<T> : GH_Param<T> where T : class, IGH_Goo
    {
        public Param_ArchiVision(string name, string nickname, string description, GH_ParamAccess access)
            : base(name, nickname, description, "ArchiVision", Subcategory.UI_Element.ToString().Replace('_', ' '), access)
        {
        }
    }
}
