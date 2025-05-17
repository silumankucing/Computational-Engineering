using PicoGK;
using System.Numerics;

namespace Fundamental_PicoGK
{
    class BasicShape
    {
        public static void BasicBall()
        {
            Lattice lat = new();
			
            lat.AddBeam(new Vector3(0,0,0),
                        new Vector3(50,0,0),
                        5, 5, true);

            Voxels vox = new(lat);
        }
        
        public static void BooleanBall()
        {
            Lattice lat = new();
			
            lat.AddBeam(    new Vector3(0,0,0),
                            new Vector3(50,0,0),
                            5, 5, true);

            lat.AddBeam(    new Vector3(50,0,0),
                            new Vector3(50,50,0),
                            5, 5, true);

            lat.AddBeam(    new Vector3(50,50,0),
                            new Vector3(0,0,0),
                            5, 5, true);

            Voxels vox = new(lat);
        }

        public static void BasicTube()
        {
            Lattice MainBase = new();
            MainBase.AddBeam(new Vector3(0,0,0), new Vector3(40,0,0), 50, 50, false);

            Lattice AxisHole = new();
            AxisHole.AddBeam(new Vector3(-10,0,0), new Vector3(70,0,0), 5, 5, false);

            Voxels voxMainBase = new(MainBase);
            Voxels voxAxisHole = new(AxisHole);
            Voxels result = voxMainBase - voxAxisHole;

            Library.oViewer().Add(result);
        }

        public static void OffsetTube()
        {
            Lattice Base = new();
            Base.AddBeam(new Vector3(0,0,0), new Vector3(30,0,0), 50, 50, false);

            Voxels voxBase = new(Base);
            Voxels voxBaseOffset = voxBase.voxOffset(1);
            Voxels result = voxBaseOffset - voxBase;

            Library.oViewer().Add(result, 1);
        }

        public static void ComplexPipe()
        {
            Lattice lat = new();

            lat.AddBeam(Vector3.Zero, new(100,0,0), 10, 10);
            lat.AddBeam(new(100,0,0), new(100,100,0), 10, 10);
            lat.AddBeam(new(100,100,0), new(100,100,100), 10, 10);

            BBox3 oBox = new(new(0,-12,-12), new(112,112,100));

            Voxels voxInside    = new(lat);
            Voxels voxOutside   = voxInside.voxOffset(1);
            Voxels voxPipe      = (voxOutside - voxInside) & new Voxels(Utils.mshCreateCube(oBox));

            Library.oViewer().Add(voxPipe, 1);
        }

        public static void CombinedShape01(){
            Lattice lat = new();
			
            lat.AddBeam(new Vector3(0,0,0), new Vector3(50,0,0), 5, 5, true);
            lat.AddBeam(new Vector3(50,0,0), new Vector3(50,50,0), 5, 5, true);
            lat.AddBeam(new Vector3(50,50,0), new Vector3(0,0,0), 5, 5, true);

            Voxels vox = new(lat);

            Library.oViewer().Add(vox);
        }

        public static void CombinedShape02(){
            Lattice lat = new();
                        
            lat.AddBeam(new Vector3(0,0,0), new Vector3(50,0,0), 5, 20, true);

            Voxels vox = new(lat);

            Library.oViewer().Add(vox);
        }
    }
}