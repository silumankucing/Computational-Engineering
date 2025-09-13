using System.Numerics;
using PicoGK;

namespace Leap71
{
    using ShapeKernel;

    namespace Simulation
    {
        public class SimulationSetup
        {
            public static void WriteSimulation()
            {
                // physical inputs
                float fFluidDensity         = 1000f;            // kg/m3
                float fFluidViscosity       = 0.00000897f;      // m2/s
                float fFluidInletVelocity   = 1.5f;             // m/s


                // geometric inputs
                SimpleFlowDevice oPipe      = new SimpleFlowDevice();
                Voxels voxFluidDomain       = oPipe.voxGetFluidDomain();
                Voxels voxSolidDomain       = oPipe.voxGetSolidDomain();
                Voxels voxInletPatch        = oPipe.voxGetInletPatch();


                // create VDB file from input data
                string strVDBFilePath       = Sh.strGetExportPath(Sh.EExport.VDB, "SimpleFluidSimulation");
                SimpleFluidSimulationOutput oOutput = new(  strVDBFilePath,
                                                            fFluidDensity,
                                                            fFluidViscosity,
                                                            fFluidInletVelocity,
                                                            voxFluidDomain,
                                                            voxSolidDomain,
                                                            voxInletPatch);
                
                Library.Log("Finished Task.");
                return;
            }

            public static void ReadSimulation()
            {
                // load VDB file and retreive simulation inputs
                string strVDBFilePath                   = Sh.strGetExportPath(Sh.EExport.VDB, "SimpleFluidSimulation");
                SimpleFluidSimulationInput oData        = new(strVDBFilePath);

                // get data
                Voxels voxFluidDomain                   = oData.voxGetFluidDomain();
                Voxels voxSolidDomain                   = oData.voxGetSolidDomain();
                ScalarField oDensityField               = oData.oGetDensityField();
                ScalarField oViscosityField             = oData.oGetViscosityField();
                VectorField oVelocityField              = oData.oGetVelocityField();


                // get bounding box and probe fluid domain values
                // use your own resolution / step length
                BBox3 oBBox                     = Sh.oGetBoundingBox(voxFluidDomain);
                float fStep                     = 2f;
                for (float fZ = oBBox.vecMin.Z; fZ <= oBBox.vecMax.Z; fZ += fStep)
                {
                    for (float fX = oBBox.vecMin.X; fX <= oBBox.vecMax.X; fX += fStep)
                    {
                        for (float fY = oBBox.vecMin.Y; fY <= oBBox.vecMax.Y; fY += fStep)
                        {
                            Vector3 vecPosition = new Vector3(fX, fY, fZ);

                            //query density
                            bool bSuccess = oDensityField.bGetValue(vecPosition, out float fFieldValue);
                            if (bSuccess == true)
                            {
                                float fDensityValue = fFieldValue;
                                // todo: do something with the value...
                            }

                            //query viscosity
                            bSuccess = oViscosityField.bGetValue(vecPosition, out fFieldValue);
                            if (bSuccess == true)
                            {
                                float fViscosity = fFieldValue;
                                // todo: do something with the value...
                            }

                            //query velocity
                            bSuccess = oVelocityField.bGetValue(vecPosition, out Vector3 vecFieldValue);
                            if (bSuccess == true)
                            {
                                Vector3 vecVelocity = vecFieldValue;
                                // todo: do something with the value...
                            }
                        }
                    }
                }

                // previews
                Sh.PreviewVoxels(voxFluidDomain, Cp.clrBlue);
                Sh.PreviewVoxels(voxSolidDomain, Cp.clrRock);

                Library.Log("Finished Task.");
                return;
            }
        }
    }
}