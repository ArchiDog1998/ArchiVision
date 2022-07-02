using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiVision
{
    public class MergedMeshDisplayItem: DisplayItem
    {
        public MergedMeshDisplayItem(IGH_DocumentObject owner, List<MeshDisplayItem> meshRenderItems, 
            CurveDisplayAttribute outlineAttribute, CurveDisplayAttribute intersectAttribute)
            : base(owner, 0)
        {
            SubRenderItem.Clear();

            double tolerance = RhinoDoc.ActiveDoc?.ModelAbsoluteTolerance ?? 0.001;
            tolerance *= Intersection.MeshIntersectionsTolerancesCoefficient;
            Mesh mesh = new Mesh();
            List<Curve> allInter = new List<Curve>();
            meshRenderItems.ForEach((meshItem) =>
            {
                //SubRenderItem.Add(new MeshRenderItem(owner, meshItem));

                Mesh relayMesh = ((GH_Mesh)meshItem.Geometry).Value;

                var polys = Intersection.MeshMeshAccurate(mesh, relayMesh, tolerance);
                if(polys != null)
                {
                    List<Curve> intersect = new List<Curve>();
                    polys.ToList().ForEach((poly) => intersect.Add(poly.ToNurbsCurve()));
                    allInter.AddRange(Curve.JoinCurves(intersect));
                }
                mesh.Append(relayMesh);
            });

            if (outlineAttribute != null)
                SubRenderItem.Add(new MeshOutlineDisplayItem(owner, new GH_Mesh(mesh), outlineAttribute));

            if (intersectAttribute != null)
                allInter.ForEach((crv) => SubRenderItem.Add(new CurveDisplayItem(owner, new GH_Curve(crv), intersectAttribute)));
        }

        //public TimeSpan TaskTestMulti(int count, int max)
        //{
        //    Stopwatch stopwatch = new Stopwatch();
        //    stopwatch.Start();

        //    Task[] tasks = new Task[count];
        //    for (int i = 0; i < count; i++)
        //    {
        //        tasks[i] = Task.Run(() =>
        //        {
        //            FindPrimeNumbers(max);
        //        });
        //    }
        //    Task.WaitAll(tasks);

        //    stopwatch.Stop();
        //    return stopwatch.Elapsed;
        //}

        //public TimeSpan TaskTestCount(int count, int max, int taskCount)
        //{
        //    Stopwatch stopwatch = new Stopwatch();
        //    stopwatch.Start();

        //    int oneTaskCount = count / taskCount + 1;
        //    Task[] tasks = new Task[taskCount];
        //    for (int i = 0; i < taskCount; i++)
        //    {
        //        tasks[i] = Task.Run(() =>
        //        {
        //            for (int j = 0; j < oneTaskCount; j++)
        //            {
        //                FindPrimeNumbers(max);
        //            }
        //        });
        //    }
        //    Task.WaitAll(tasks);

        //    stopwatch.Stop();
        //    return stopwatch.Elapsed;

        //}

        //private int[] FindPrimeNumbers(int max)
        //{
        //    List<int> primeNumber = new List<int>(max);
        //    for (int i = 2; i < max; i++)
        //    {
        //        bool isFind = false;
        //        for (int j = 2; j <= Math.Sqrt(i); j++)
        //        {
        //            if(i % j == 0)
        //            {
        //                isFind = true;
        //                break;
        //            }
        //        }
        //        if (!isFind)
        //        {
        //            primeNumber.Add(i);
        //        }
        //    }
        //    return primeNumber.ToArray();
        //}
    }
}
