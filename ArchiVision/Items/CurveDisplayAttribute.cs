using Rhino.DocObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiVision
{
    public class CurveDisplayAttribute
    {
        public Color Colour { get; set; } 
        public float Thickness { get; set; } 
        public Linetype LineType { get; set; } 

        public int TopMost { get; set; }

        public CurveDisplayAttribute()
        {
            this.Colour = Color.White;
            this.Thickness = 2;
            this.LineType = Rhino.RhinoDoc.ActiveDoc.Linetypes.FindIndex(-1);
            this.TopMost = 0;
        }

        public CurveDisplayAttribute(Color color, float thickness, Linetype linetype, int topMost)
        {
            this.Colour = color;
            this.Thickness = thickness;
            this.LineType = linetype;
            this.TopMost = topMost;
        }
    }
}
