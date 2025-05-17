using Leap71.ShapeKernel;
using Leap71.RoverExamples;
using Leap71.LatticeLibraryExamples;
using PicoGK;

using Leap71.Simulation;

try
{
	// -----------------------------FUNDAMENTAL------------------------------
	//
	//PicoGK.Library.Go(0.5f, Fundamental_Geometry.FixtureDesign.Fixture_Task);
	//
	// -------------------------------------------------------------------------
	
	// -----------------------------HEAT EXCHANGER------------------------------
	//
	// PicoGK.Library.Go(0.5f, Leap71.CoolCube.HelixHeatX.Task);
	//
	// -------------------------------------------------------------------------
	
	// ------------------------------WHEEL ROVER--------------------------------
	//
	//PicoGK.Library.Go(0.5f, WheelShowCase.PresetWheelTask);
	//
	// -------------------------------------------------------------------------
	
	// -----------------------------FLOW SIMULATION----------------------------------
	//
    PicoGK.Library.Go(0.3f, SimulationSetup.WriteSimulation);
	PicoGK.Library.Go(0.3f, SimulationSetup.ReadSimulation);
	//
	// -------------------------------------------------------------------------
}

catch (Exception)
{
	Console.WriteLine("Gagal, HEHE");
}