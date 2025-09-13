﻿using PicoGK;
using System.Numerics;

namespace Leap71
{
    using ShapeKernel;

    namespace Simulation
    {
        public class SimpleFluidSimulationOutput
        {
            // fluid
            protected ScalarField   m_oFluidDensityField;       // density in kg/m3
            protected ScalarField   m_oFluidViscosityField;     // kin. viscosity in m2/s
            protected VectorField   m_oFluidVelocityField;      // flow speed in m/s
            protected Voxels        m_voxFluidDomain;

            // solid part
            protected Voxels        m_voxSolidDomain;


            /// <summary>
            /// Class to create a VDB file from physical and geometric input data.
            /// This information can be used for automated simulation setup.
            /// </summary>
            public SimpleFluidSimulationOutput( string strVDBFilePath,
                                                float  fFluidDensity,
                                                float  fFluidViscosity,
                                                float  fFluidInletVelocity,
                                                Voxels voxFluidDomain,
                                                Voxels voxSolidDomain,
                                                Voxels voxInletPatch)
            {
                // set domains
                m_voxSolidDomain            = voxSolidDomain;
                m_voxFluidDomain            = voxFluidDomain;


                // generate inlet velocity vector field
                voxInletPatch               = Sh.voxIntersect(m_voxFluidDomain, voxInletPatch);
                Vector3 vecInletFlowDir     = -Vector3.UnitZ;
                Vector3 vecSurfaceDir       = -vecInletFlowDir;
                VectorField oInletField
                    = SurfaceNormalFieldExtractor.oExtract( voxInletPatch,      // voxel field for the surface voxels
                                                            0.5f,               // max distance to the surface in voxels 
                                                            vecSurfaceDir,      // direction filter
                                                            0.0f,               // direction tolerance
                                                            (-fFluidInletVelocity) * Vector3.One);

                // generate velocity vector field
                Vector3 vecDefaultVelocity  = new Vector3(0, 0, 0);
                m_oFluidVelocityField       = new VectorField(m_voxFluidDomain, vecDefaultVelocity);
                VectorFieldMerge.Merge(oInletField, m_oFluidVelocityField);


                // density scalar field from voxel field
                m_oFluidDensityField = ScalarUtil.oGetConstScalarField(m_oFluidVelocityField, fFluidDensity);


                // viscosity scalar field from voxel field
                m_oFluidViscosityField = ScalarUtil.oGetConstScalarField(m_oFluidVelocityField, fFluidViscosity);


                // write VDB file
                OpenVdbFile oFile = new();
                oFile.nAdd(m_voxFluidDomain,        $"Simulation.Domain_{SimulationKeyWords.m_strFluidKey}");
                oFile.nAdd(m_voxSolidDomain,        $"Simulation.Domain_{SimulationKeyWords.m_strSolidKey}");
                oFile.nAdd(m_oFluidVelocityField,   $"Simulation.Field_{SimulationKeyWords.m_strVelocityKey}");
                oFile.nAdd(m_oFluidDensityField,    $"Simulation.Field_{SimulationKeyWords.m_strDensityKey}");
                oFile.nAdd(m_oFluidViscosityField,  $"Simulation.Field_{SimulationKeyWords.m_strViscosityKey}");
                oFile.SaveToFile(strVDBFilePath);
                Library.Log($"Exported VdbFile {strVDBFilePath} successfully.");
            }

            /// <summary>
            /// Returns the fluid domain as a voxel field.
            /// </summary>
            public Voxels voxGetFluidDomain()
            {
                return m_voxFluidDomain;
            }

            /// <summary>
            /// Returns the solid domain as a voxel field.
            /// </summary>
            public Voxels voxGetSolidDomain()
            {
                return m_voxSolidDomain;
            }

            /// <summary>
            /// Returns the fluid flow speeds / velocities as a vector field.
            /// All values are specified in m/s.
            /// </summary>
            public VectorField oGetVelocityField()
            {
                return m_oFluidVelocityField;
            }

            /// <summary>
            /// Returns the fluid densities as a scalar field.
            /// All values are specified in kg/m3.
            /// </summary>
            public ScalarField oGetDensityField()
            {
                return m_oFluidDensityField;
            }

            /// <summary>
            /// Returns the fluid (kinematic) viscosities as a scalar field.
            /// All values are specified in m2/s.
            /// </summary>
            public ScalarField oGetViscosityField()
            {
                return m_oFluidViscosityField;
            }
        }

        public class ScalarUtil : ITraverseVectorField
        {
            protected VectorField   m_oInputField;
            protected ScalarField   m_oOutputField;
            protected float         m_fConstValue;

            /// <summary>
            /// Utility: Returns a scalar field with a constant default value and the structure of the voxel field.
            /// </summary>
            public static ScalarField oGetConstScalarField(VectorField oInputField, float fConstValue)
            {
                ScalarUtil oSetter  = new(oInputField, fConstValue);
                oSetter.Run();
                return oSetter.oGetOutputField();
            }

            protected ScalarUtil(VectorField oInputField, float fConstValue)
            {
                m_oInputField       = oInputField;
                m_fConstValue       = fConstValue;
                m_oOutputField      = new ScalarField();
            }

            protected void Run()
            {
                m_oInputField.TraverseActive(this);
            }

            /// <summary>
            /// Sets the constant value in the output field for every active voxel in the input field.
            /// </summary>
            public void InformActiveValue(in Vector3 vecPosition, in Vector3 vecValue)
            {
                m_oOutputField.SetValue(vecPosition, m_fConstValue);
                bool bValid = m_oOutputField.bGetValue(vecPosition, out float fCheckValue);

                if (bValid == false)
                {
                    throw new Exception();
                }

                if (fCheckValue != m_fConstValue)
                {
                    throw new Exception();
                }
            }

            /// <summary>
            /// Returns the output scalar field.
            /// </summary>
            protected ScalarField oGetOutputField()
            {
                return m_oOutputField;
            }
        }
    }
}