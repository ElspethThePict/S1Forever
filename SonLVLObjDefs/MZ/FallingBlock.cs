using SonicRetro.SonLVL.API;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1ObjectDefinitions.MZ
{
	class FallingBlock : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[1];
		private Sprite sprite;
		private Sprite debug;
		
		public override void Init(ObjectData data)
		{
			sprite = new Sprite(LevelData.GetSpriteSheet("MZ/Objects.gif").GetSection(191, 289, 32, 32), -16, -16);
			
			// tagging this area with LevelData.ColorWhite
			BitmapBits bitmap = new BitmapBits(1, 16);
			bitmap.DrawLine(6, 0, 0, 0, 3);
			bitmap.DrawLine(6, 0, 6, 0, 9);
			bitmap.DrawLine(6, 0, 12, 0, 15);
			debug = new Sprite(bitmap, 0, 14);
			
			// Sonic Origins gives some blocks a prop val of 5... but that's literally the same thing as using a prop val of 0
			// various base game blocks have prop vals greater than 4 already, too
			// in-game, though, any invalid prop vals are reset to 0
			properties[0] = new PropertySpec("Behaviour", typeof(int), "Extended",
				"How this Block should act.", null, new Dictionary<string, int>
				{
					{ "Idle", 0 },
					{ "Floating (Hover)", 1 },
					{ "Floating (Drop)", 2 },
					// 3 is the falling state, but we don't wanna spawn that
					{ "Floating (In Lava)", 4 }
				},
				(obj) => ((obj.PropertyValue & 7) > 4) ? 0 : (obj.PropertyValue & 7),
				(obj, value) => obj.PropertyValue = (byte)((int)value));
		}
		
		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {0, 1, 2, 4}); }
		}
		
		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
		}

		public override string SubtypeName(byte subtype)
		{
			if (subtype > 4) subtype = 0;
			
			string[] names = {"Idle", "Floating (Hover)", "Floating (Drop)", "Falling", "Floating (In Lava)"}; // falling shouldn't be used
			return names[subtype];
		}

		public override Sprite Image
		{
			get { return sprite; }
		}

		public override Sprite SubtypeImage(byte subtype)
		{
			return sprite;
		}

		public override Sprite GetSprite(ObjectEntry obj)
		{
			return sprite;
		}
		
		public override Sprite GetDebugOverlay(ObjectEntry obj)
		{
			return ((obj.PropertyValue & 7) == 2) ? debug : null;
		}
	}
}