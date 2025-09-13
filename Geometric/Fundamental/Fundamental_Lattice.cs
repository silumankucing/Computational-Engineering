using PicoGK;

namespace Leap71
{
    using ShapeKernel;
    using LatticeLibrary;

    namespace Fundamental_Lattice
    {
        partial class ImplicitLibrary
        {
            public static void Implicit_Task()
            {
                BaseBox oBox				    = new BaseBox(new LocalFrame(), 50, 50, 50);
			    Voxels voxBounding              = oBox.voxConstruct();

                float fUnitSize                 = 10f;
                float fWallThickness            = 1f;

                IImplicit xImplicitPattern_01   = new ImplicitSplitWallGyroid(fUnitSize, fWallThickness, true);
                IImplicit xImplicitPattern_02   = new ImplicitSplitWallGyroid(fUnitSize, fWallThickness, false);

                Voxels voxImplicit_01           = Sh.voxIntersectImplicit(voxBounding, xImplicitPattern_01);
                Voxels voxImplicit_02           = Sh.voxIntersectImplicit(voxBounding, xImplicitPattern_02);

                ColorFloat clrColor             = Cp.clrRandom();

                Sh.PreviewVoxels(voxImplicit_01, clrColor);
                Sh.PreviewVoxels(voxImplicit_02, Cp.clrRandom());
                Sh.PreviewVoxels(voxBounding, clrColor, 0.3f);

                Sh.ExportVoxelsToSTLFile(voxImplicit_01, Sh.strGetExportPath(Sh.EExport.STL, "Gyroid01"));
                //Sh.ExportVoxelsToSTLFile(voxImplicit_02, Sh.strGetExportPath(Sh.EExport.STL, "Gyroid02"));

                Library.Log("Finished Task successfully.");
            }
        }
    }
}