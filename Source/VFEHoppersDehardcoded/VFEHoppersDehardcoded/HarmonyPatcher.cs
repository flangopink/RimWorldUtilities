using System.Reflection;
using HarmonyLib;
using Verse;

namespace VEFHoppersDehardcoded 
{
	[StaticConstructorOnStartup]
	internal class Main
	{
		static Main()
		{
			Harmony val = new Harmony("com.flangopink.rimworld.mod.VEFHoppersDehardcoded");
			val.PatchAll(Assembly.GetExecutingAssembly());
		}
	} 
}