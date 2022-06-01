﻿/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiVision
{
    public class GH_CurveRenderAttribute : GH_Goo<CurveDisplayAttribute>
    {
        public override bool IsValid => true;

        public override string TypeName => "Curve Render Attribute";

        public override string TypeDescription => "Curve Render Attribute";

        public GH_CurveRenderAttribute()
        {
        }

        public GH_CurveRenderAttribute(CurveDisplayAttribute element)
            : base(element)
        {
        }

        public override IGH_Goo Duplicate()
        {
            if (Value == null)
            {
                return new GH_CurveRenderAttribute(null);
            }
            return new GH_CurveRenderAttribute(Value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override bool CastFrom(object source)
        {
            CurveDisplayAttribute element = source as CurveDisplayAttribute;
            if (element != null)
            {
                Value = element;
                return true;
            }
            return false;
        }

        public override bool CastTo<TQ>(ref TQ target)
        {
            if (typeof(CurveDisplayAttribute).IsAssignableFrom(typeof(TQ)))
            {
                target = (TQ)(object)Value;
                return true;
            }
            return false;
        }
    }
}
