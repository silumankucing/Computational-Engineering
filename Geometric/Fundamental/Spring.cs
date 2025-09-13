//--------------------------------------------------------------------
// Spring
// code by kuro
//--------------------------------------------------------------------

using PicoGK;
using System.Numerics;

namespace Fundamental_Geometry
{
    class Spring
    {
        public static void Spring_Task_1(){

            float pipeRadius = 0.1f;
            float helixRadius = 10f;
            float helixHeight = 25f;
            float helixPitch = 1f;

            int numSegmentsPerPitch = 100;
            int numPitches = (int)(helixHeight / helixPitch);

            List<System.Numerics.Vector3> helicalPath = [];
            for (int i = 0; i < numPitches; ++i)
            {
                for (int j = 0; j < numSegmentsPerPitch; ++j)
                {
                    double theta = j * (2 * Math.PI) / numSegmentsPerPitch;
                    helicalPath.Add(new(
                        helixRadius * (float)Math.Sin(theta),
                        helixRadius * (float)Math.Cos(theta),
                        i * helixPitch + (float)(theta / (2 * Math.PI) * helixPitch)
                    ));
                }
            }

            PicoGK.Lattice lat = new();

            for (int i = 1; i < helicalPath.Count; ++i)
            {
                bool rounded = !(i == 1 || i == helicalPath.Count - 1);
                lat.AddBeam(helicalPath[i], helicalPath[i - 1], pipeRadius, pipeRadius, rounded);
            }
    
            PicoGK.Voxels voxLat = new(lat);
            PicoGK.Library.oViewer().Add(voxLat);
        }

        public static void Spring_Task_2(){

            float pipeRadius = 0.25f;
            float helixRadius = 5f;
            float helixHeight = 10f;
            float helixPitch = 0.5f;

            int numSegmentsPerPitch = 100;
            int numPitches = (int)(helixHeight / helixPitch);

            List<System.Numerics.Vector3> helicalPath = [];
            for (int i = 0; i < numPitches; ++i)
            {
                for (int j = 0; j < numSegmentsPerPitch; ++j)
                {
                    double theta = j * (2 * Math.PI) / numSegmentsPerPitch;
                    helicalPath.Add(new(
                        helixRadius * (float)Math.Sin(theta),
                        helixRadius * (float)Math.Cos(theta),
                        i * helixPitch + (float)(theta / (2 * Math.PI) * helixPitch)
                    ));
                }
            }

            float fInnerRadius(float iPhi, float iLengthRatio)
            {
                return 0f;
            }
            float fOuterRadius(float iPhi, float iLengthRatio)
            {
                return pipeRadius;
            }

            Leap71.ShapeKernel.Frames aFrames = new Leap71.ShapeKernel.Frames(
                helicalPath, System.Numerics.Vector3.UnitY
            );
            Leap71.ShapeKernel.BasePipe oShape = new Leap71.ShapeKernel.BasePipe(aFrames);
            oShape.SetRadius(
                new Leap71.ShapeKernel.SurfaceModulation(fInnerRadius),
                new Leap71.ShapeKernel.SurfaceModulation(fOuterRadius)
            );
            PicoGK.Voxels oVoxels = oShape.voxConstruct();

            // Visualize in the viewer
            PicoGK.Library.oViewer().Add(oVoxels);
        }
    }
}