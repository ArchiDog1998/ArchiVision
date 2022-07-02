using Grasshopper;
using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace ArchiVision
{
    public class ArchiVisionInfo : GH_AssemblyInfo
    {
        internal static readonly ArchiVisionConduit Conduit = new ArchiVisionConduit() { Enabled = true };

        public override string Name => "ArchiVision";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => Properties.Resources.WindowComponent_24_24;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "Make diagrams easier to make!";

        public override Guid Id => new Guid("E63A0373-C29D-4787-926D-CFB6B990518B");

        //Return a string identifying you or your company.
        public override string AuthorName => "秋水";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "1123993881@qq.com";

        public override string Version => "0.9.1";
    }

    public class ArchiVisionPriority : GH_AssemblyPriority
    {
        public override GH_LoadingInstruction PriorityLoad()
        {
            Grasshopper.Instances.ComponentServer.AddCategoryIcon("ArchiVision", Properties.Resources.WindowComponent_24_24);
            return GH_LoadingInstruction.Proceed;
        }
    }
}