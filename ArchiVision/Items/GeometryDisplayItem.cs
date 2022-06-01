using Grasshopper.Kernel;
using Rhino.Display;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiVision
{
    /// <summary>
    /// Make display item have some attributes.
    /// </summary>
    public abstract class GeometryDisplayItem : DisplayItem
    {
        private static readonly Color SelectedColor = Color.DarkBlue;
        private static readonly DisplayMaterial SelectedMaterial = new DisplayMaterial(SelectedColor);

        private Color _color;
        public Color Colour
        {
            get => Selected ? SelectedColor : _color;
            set => _color = value;
        }

        private DisplayMaterial _material;

        public DisplayMaterial Material
        {
            get => Selected ? SelectedMaterial : _material;
            set 
            { 
                _material = value;
                _color = value?.Diffuse ?? Color.Transparent;
            }
        }

        public IGH_PreviewData Geometry { get; protected set; }
        public override BoundingBox ClippingBox
        {
            get
            {
                BoundingBox box = Geometry.ClippingBox;
                box.Union(base.ClippingBox);
                return box;
            }
        }

        public GeometryDisplayItem(IGH_DocumentObject owner, IGH_PreviewData geometry, bool topMost)
            :base(owner, topMost)
        {
            Geometry = geometry;
        }
    }
}
