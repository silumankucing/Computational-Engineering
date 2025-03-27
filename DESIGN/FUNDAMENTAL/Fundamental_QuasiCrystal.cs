using PicoGK;
using System.Numerics;

namespace Leap71
{
    using ShapeKernel;
    using AperiodicTiling;
    using static AperiodicTiling.IcosehedralFace;

    namespace Fundamental_QuasiCrystal
    {
        class PenrosePatternTest
        {
            public static void PenrosePattern_Task()
            {
                uint nGenerations = 5;

                PenrosePattern oPattern = new PenrosePattern(nGenerations);

                oPattern.PreviewGeneration(nGenerations - 1);
            }

            public static void QuasiTiles_Task()
            {
                LocalFrame oFrame_01 = new LocalFrame(new Vector3(+100, 0, 0));
                LocalFrame oFrame_02 = new LocalFrame(new Vector3(+ 40, 0, 0));
                LocalFrame oFrame_03 = new LocalFrame(new Vector3(- 30, 0, 0));
                LocalFrame oFrame_04 = new LocalFrame(new Vector3(-100, 0, 0));

                QuasiTile oTile_01 = new QuasiTile_01(oFrame_01, 20);
                QuasiTile oTile_02 = new QuasiTile_02(oFrame_02, 20);
                QuasiTile oTile_03 = new QuasiTile_03(oFrame_03, 20);
                QuasiTile oTile_04 = new QuasiTile_04(oFrame_04, 20);

                oTile_01.Preview(QuasiTile.EPreviewFace.CONNECTOR);
                oTile_02.Preview(QuasiTile.EPreviewFace.CONNECTOR);
                oTile_03.Preview(QuasiTile.EPreviewFace.CONNECTOR);
                oTile_04.Preview(QuasiTile.EPreviewFace.CONNECTOR);
            }
            public static void CrystalFromFace_Task()
            {
                IcosehedralFace sInitialFace    = new IcosehedralFace(new LocalFrame(), EDef.CENTRE, EConnector.LINE, 200);
                //IcosehedralFace sInitialFace    = new IcosehedralFace(new LocalFrame(), EDef.CENTRE, EConnector.TRIANGLE, 200);
                //IcosehedralFace sInitialFace    = new IcosehedralFace(new LocalFrame(), EDef.CENTRE, EConnector.ARROW, 200);
                sInitialFace.Preview(true);

                uint nGenerations               = 2;
                QuasiCrystal oCrystal           = new QuasiCrystal(nGenerations, sInitialFace);

                oCrystal.PreviewGeneration(nGenerations - 1, QuasiTile.EPreviewFace.NONE);
            }
            public static void CrystalFromTile_Task()
            {
                QuasiTile oInitialTile          = new QuasiTile_04(new LocalFrame(), 50);
                List<QuasiTile> aInitialTiles   = new List<QuasiTile>() { oInitialTile };

                uint nGenerations               = 2;
                QuasiCrystal oCrystal           = new QuasiCrystal(nGenerations, aInitialTiles);

                oCrystal.PreviewGeneration(nGenerations - 1, QuasiTile.EPreviewFace.AXIS);
            }

            public static void WireframeFromCrystal_Task()
            {
                uint nGenerations               = 2;

                QuasiTile oInitialTile          = new QuasiTile_02(new LocalFrame(), 50);
                List<QuasiTile> aInitialTiles   = new List<QuasiTile>() { oInitialTile };
                QuasiCrystal oCrystal           = new QuasiCrystal(nGenerations, aInitialTiles);

                float fBeamRadius               = 1f;
                Voxels voxCrystalWireframe      = oCrystal.voxGetWireframe(nGenerations - 1, fBeamRadius);
                Sh.PreviewVoxels(voxCrystalWireframe, Cp.clrBlue);
            }
        }
    }
}