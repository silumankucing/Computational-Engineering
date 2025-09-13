//--------------------------------------------------------------------
// fixture design for teapot
// code by kuro
//--------------------------------------------------------------------

using System.Numerics;
using PicoGK;

namespace Fundamental_Geometry
{
    public class FixtureDesign
    {
        public static void Fixture_Task()
        {
            Fixture.BasePlate oBase = new(new(300,200), 20, 8);
            
            Mesh mshSmall = Mesh.mshFromStlFile(Path.Combine(Utils.strPicoGKSourceCodeFolder(), "Examples/Testfiles/Teapot.stl"));
            Mesh mshObject = mshSmall.mshCreateTransformed(new(6, 6, 6), Vector3.Zero);

            Fixture.Object oObject = new(mshObject, 10, 15, 10, 25);
            
            Fixture oFixture = new( oBase, oObject, new ProgressReporterActive());

            oFixture.voxAsVoxels().mshAsMesh().SaveToStlFile(Path.Combine(Utils.strDocumentsFolder(), "Fixture.stl"));
        }
    }

    public abstract class ProgressReporter
    {
        public abstract void AddObject(Voxels vox, int iGroupID = 0);
        public abstract void AddObject(Mesh msh, int iGroupID = 0);
        public abstract void SetGroupMaterial(int iID, ColorFloat clr, float fMetallic, float fRoughness);
        public abstract void ReportTask(string strTask);
    }

    public class ProgressReporterSilent : ProgressReporter
    {
        public override void AddObject(Voxels vox, int iGroupID = 0)
        { }
        public override void AddObject(Mesh msh, int iGroupID = 0)
        { }
        public override void SetGroupMaterial(int iID, ColorFloat clr, float fMetallic, float fRoughness)
        { }
        public override void ReportTask(string strTask)
        { }
    }

    public class ProgressReporterActive : ProgressReporter
    {
        public override void AddObject(Voxels vox, int iGroupID = 0)
        {
            Library.oViewer().Add(vox, iGroupID);
        }

        public override void AddObject(Mesh msh, int iGroupID = 0)
        {
            Library.oViewer().Add(msh, iGroupID);
        }

        public override void SetGroupMaterial(int iID, ColorFloat clr, float fMetallic, float fRoughness)
        {
            Library.oViewer().SetGroupMaterial(iID, clr, fMetallic, fRoughness);
        }

        public override void ReportTask(string strTask)
        {
            Library.Log(strTask);
        }
    }

    public class Fixture
    {
        public class BasePlate
        {
            public BasePlate(Vector2 vecSizeMM, float fHoleSpacingMM, float fHoleDiameterMM)
            {
                m_vecSize       = vecSizeMM;
                m_fHoleSpacing  = fHoleSpacingMM;
                m_fHoleRadius   = fHoleDiameterMM / 2;
            }

            public bool bDoesFit(in Voxels voxFlange)
            {
                BBox3 oBox = voxFlange.mshAsMesh().oBoundingBox();

                if (oBox.vecSize().X > m_vecSize.X)
                    return false;

                if (oBox.vecSize().Y > m_vecSize.Y)
                    return false;

                return true;
            }

            public Voxels voxCreateMountableFlange(in Voxels voxFlange)
            {
                if (!bDoesFit(voxFlange))
                    throw new Exception("Flange doesn't fit onto base plate");

                BBox3 oBox = voxFlange.mshAsMesh().oBoundingBox();

                int nXCount = (int) float.Ceiling(oBox.vecSize().X / m_fHoleSpacing) + 1;
                int nYCount = (int) float.Ceiling(oBox.vecSize().Y / m_fHoleSpacing) + 1;

                Vector3 vecOrigin = oBox.vecCenter() - new Vector3(m_fHoleSpacing * (nXCount / 2), m_fHoleSpacing * (nYCount / 2), 0);

                Vector3 vecBegin    = vecOrigin;
                Vector3 vecEnd      = vecOrigin;

                vecBegin.Z  = oBox.vecMax.Z + 1;
                vecEnd.Z    = oBox.vecMin.Z - 1;

                Lattice latDrills = new();

                for (int x=0; x<nXCount; x++)
                {
                    vecBegin.Y  = vecOrigin.Y;
                    vecEnd.Y    = vecOrigin.Y;

                    for (int y=0; y<nYCount; y++)
                    {
                        latDrills.AddBeam(vecBegin, vecEnd, m_fHoleRadius, m_fHoleRadius);

                        vecBegin.Y += m_fHoleSpacing;
                        vecEnd.Y   += m_fHoleSpacing;
                    }

                    vecBegin.X += m_fHoleSpacing;
                    vecEnd.X   += m_fHoleSpacing;
                }

                return voxFlange - new Voxels(latDrills);
            }

            Vector2 m_vecSize;
            float   m_fHoleSpacing;
            float   m_fHoleRadius;
        }

        public class Object
        {
            public Object(Mesh msh, float fObjectBottomMM, float fSleeveMM, float fWallMM, float fFlangeMM, float fObjectTolerance = 0.1f, float fSmallFeatureSize = 1f)
            {
                if (fObjectBottomMM <= 0)
                    throw new Exception("Object cannot be placed below build plate");

                m_fTolerance = fObjectTolerance;

                if (fSmallFeatureSize < 0f)
                    throw new Exception("Small feature elimination size must be equal or larger than 0");

                m_fSmallFeature = fSmallFeatureSize;

                if (fObjectTolerance < 0f)
                    throw new Exception("Object tolerance must be equal or larger than 0");

                BBox3 oObjectBounds = msh.oBoundingBox();
                Vector3 vecOffset = new Vector3(-oObjectBounds.vecCenter().X, -oObjectBounds.vecCenter().Y, -oObjectBounds.vecMin.Z + fObjectBottomMM);

                m_voxObject = new Voxels(msh.mshCreateTransformed(Vector3.One, vecOffset));

                m_fObjectBottom = fObjectBottomMM;
                m_fSleeve = fSleeveMM;
                m_fWall = fWallMM;
                m_fFlange = fFlangeMM;
            }

            public Voxels voxObject()
            {
                return m_voxObject;
            }

            public float fWallMM()
            {
                return m_fWall;
            }

            public float fSleeveMM()
            {
                return m_fSleeve;
            }

            public float fFlangeHeightMM()
            {
                return m_fObjectBottom;
            }

            public float fFlangeWidthMM()
            {
                return m_fFlange;
            }

            public float fObjectTolerance()
            {
                return m_fTolerance;
            }

            public float fSmallFeatureSize()
            {
                return m_fSmallFeature;
            }

            Voxels m_voxObject;
            float m_fWall;
            float m_fSleeve;
            float m_fObjectBottom;
            float m_fFlange;
            float m_fTolerance;
            float m_fSmallFeature;
        }

        public Fixture(BasePlate oPlate, Object oObject, ProgressReporter oProgress)
        {
            oProgress.ReportTask("Creating a new fixture");

            m_voxFixture = new(oObject.voxObject());

            oProgress.ReportTask("Creating the sleeve");

            m_voxFixture.Offset(oObject.fWallMM());
            m_voxFixture.ProjectZSlice(oObject.fFlangeHeightMM() + oObject.fSleeveMM(), 0);

            BBox3 oFixtureBounds    = m_voxFixture.mshAsMesh().oBoundingBox();

            oFixtureBounds.vecMin.Z = 0;
            oFixtureBounds.vecMax.Z = oObject.fFlangeHeightMM() + oObject.fSleeveMM();

            m_voxFixture &= new Voxels(Utils.mshCreateCube(oFixtureBounds));

            oProgress.ReportTask("Building the flange");

            Voxels voxFlange = new(m_voxFixture);
            voxFlange.Offset(oObject.fFlangeWidthMM());

            BBox3 oFlangeBounds     = voxFlange.mshAsMesh().oBoundingBox();
            oFlangeBounds.vecMin.Z  = 0;
            oFlangeBounds.vecMax.Z  = oObject.fFlangeHeightMM();

            voxFlange &= new Voxels(Utils.mshCreateCube(oFlangeBounds));

            if (!oPlate.bDoesFit(voxFlange))
                throw new Exception("Flange doesn't fit onto base plate");

            m_voxFixture += oPlate.voxCreateMountableFlange(voxFlange);

            Voxels voxObjectRemovable = new(oObject.voxObject());
            voxObjectRemovable.Offset(oObject.fObjectTolerance());

            BBox3 oObjectBounds = voxObjectRemovable.mshAsMesh().oBoundingBox();

            voxObjectRemovable.ProjectZSlice(oObjectBounds.vecMin.Z, oObjectBounds.vecMax.Z);

            m_voxFixture -= voxObjectRemovable;
            m_voxFixture.OverOffset(-oObject.fSmallFeatureSize());

            oProgress.SetGroupMaterial(0, "da9c6b", 0.3f, 0.7f);
            oProgress.SetGroupMaterial(1, "FF0000AA", 0.5f, 0.5f);
            oProgress.SetGroupMaterial(2, "0000FF22", 0.5f, 0.5f);

            oProgress.AddObject(oObject.voxObject(), 1);
            //oProgress.AddObject(voxObjectRemovable, 2);
            oProgress.AddObject(m_voxFixture, 0);
        }

        public Voxels voxAsVoxels()
        {
            return m_voxFixture;
        }

        Voxels m_voxFixture;
    }
}
