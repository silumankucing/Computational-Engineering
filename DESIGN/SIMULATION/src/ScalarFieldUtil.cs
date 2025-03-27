using PicoGK;
using System.Numerics;

namespace Leap71
{
    namespace Simulation
    {
        public class ScalarFieldUtil : ITraverseVectorField
        {
            protected VectorField m_oInputField;
            protected ScalarField m_oOutputField;
            protected float       m_fConstValue;

            /// <summary>
            /// Utility: Returns a scalar field with a constant default value and the structure of the voxel field.
            /// </summary>
            public static ScalarField oGetConstScalarField(VectorField oInputField, float fConstValue)
            {
                ScalarFieldUtil oSetter = new(oInputField, fConstValue);
                oSetter.Run();
                return oSetter.oGetOutputField();
            }

            protected ScalarFieldUtil(VectorField oInputField, float fConstValue)
            {
                m_oInputField   = oInputField;
                m_fConstValue   = fConstValue;
                m_oOutputField  = new ScalarField();
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