using RimWorld;
using Verse;

namespace FlickableTextureBuilding
{
	[StaticConstructorOnStartup]
	public class Building_WithFlickableTexture : Building
	{
		private CompFlickable flickableComp;
		public override Graphic Graphic => flickableComp.CurrentGraphic;

		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			flickableComp = GetComp<CompFlickable>();
		}

		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (flickableComp == null)
				{
					flickableComp = GetComp<CompFlickable>();
				}
			}
		}
	}
}
