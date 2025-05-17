using PicoGK;
using System.Numerics;

namespace Leap71
{
    using ShapeKernel;

    namespace ShapeKernel_Fundamental
    {
        class ShapeKernelTest
        {
            public static void Task()
            {
                try
                {
                    {
                        LocalFrame oLocalFrame  = new LocalFrame(new Vector3(-50, 0, 0));
                        BaseBox oShape          = new BaseBox(oLocalFrame, 20, 10, 15);
                        Voxels oVoxels          = oShape.voxConstruct();
                        Sh.PreviewVoxels(oVoxels, Cp.clrBlue);
                    }
                    {
                        LocalFrame oLocalFrame  = new LocalFrame(new Vector3(50, 0, 0));
                        BaseBox oShape          = new BaseBox(oLocalFrame, 20);
                        oShape.SetWidth(new LineModulation(fGetLineModulation2));
                        oShape.SetDepth(new LineModulation(fGetLineModulation1));
                        Voxels oVoxels          = oShape.voxConstruct();
                        Sh.PreviewVoxels(oVoxels, Cp.clrGreen);
                    }
                    {
                        ISpline oSpine          = new ExampleSpline();
                        Frames aFrames          = new Frames(oSpine.aGetPoints(), Vector3.UnitY);
                        BaseBox oShape          = new BaseBox(aFrames);
                        oShape.SetWidth(new LineModulation(fGetLineModulation2));
                        oShape.SetDepth(new LineModulation(fGetLineModulation1));
                        Voxels oVoxels          = oShape.voxConstruct();
                        Sh.PreviewVoxels(oVoxels, Cp.clrYellow);
                    }
                }
                catch (Exception e)
                {
                    Library.Log($"Failed run example: \n{e.Message}"); ;
                }
            }

            public static float fGetLineModulation1(float fLengthRatio)
            {
                float fWidth = 10f - 3f * MathF.Cos(8f * fLengthRatio);
                return fWidth;
            }

            public static float fGetLineModulation2(float fLengthRatio)
            {
                float fDepth = 8f - 1f * MathF.Cos(40f * fLengthRatio);
                return fDepth;
            }
        }
    }
}