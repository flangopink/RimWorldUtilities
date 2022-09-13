using System.Reflection;
using HarmonyLib;
using Verse;

namespace CustomAmmo
{
    [StaticConstructorOnStartup]
    class CustomAmmoPatch
    {
        static CustomAmmoPatch()
        {
            var harmony = new Harmony("com.flangopink.CustomAmmoPatch");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
