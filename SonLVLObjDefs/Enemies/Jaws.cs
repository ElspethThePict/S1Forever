using SonicRetro.SonLVL.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1FObjectDefinitions.Enemies
{
	class Jaws : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[2];
		private Sprite[] sprites = new Sprite[2];
		
		public override void Init(ObjectData data)
		{
			if (LevelData.StageInfo.folder.EndsWith("BossRush"))
				sprites[0] = new Sprite(LevelData.GetSpriteSheet("MBZ/Objects.gif").GetSection(1, 264, 48, 24), -16, -12);
			else if (LevelData.StageInfo.folder.EndsWith("Zone00"))
				sprites[0] = new Sprite(LevelData.GetSpriteSheet("GHZ/Objects3.gif").GetSection(1, 264, 48, 24), -16, -12);
			else
				sprites[0] = new Sprite(LevelData.GetSpriteSheet("LZ/Objects.gif").GetSection(1, 105, 48, 24), -16, -12);
			
			sprites[1] = new Sprite(sprites[0], true, false);
			
			properties[0] = new PropertySpec("Distance", typeof(int), "Extended",
				"How far, in pixels, the Jaws will swim.", null,
				(obj) => (obj.PropertyValue & 0x7f) << 4,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~0x7f) | Math.Min(Math.Max((int)value >> 4, 0), 0x7f)));

			properties[1] = new PropertySpec("Direction", typeof(int), "Extended",
				"Which way the Jaws will be facing initially.", null, new Dictionary<string, int>
				{
					{ "Left", 0 },
					{ "Right", 0x80 }
				},
				(obj) => obj.PropertyValue & 0x80,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~0x80) | (int)value));
		}

		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {0x06, 0x86, 0x0A, 0x8A, 0x0C, 0x8C}); }
		}
		
		public override byte DefaultSubtype
		{
			get { return 6; }
		}
		
		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
		}

		public override string SubtypeName(byte subtype)
		{
			return "Swim " + ((subtype & 0x7f) << 4) + " Pixels" + ((subtype > 0x80) ? " (Facing Right)" : " (Facing Left)");
		}

		public override Sprite Image
		{
			get { return sprites[0]; }
		}

		public override Sprite SubtypeImage(byte subtype)
		{
			return sprites[subtype >> 7];
		}

		public override Sprite GetSprite(ObjectEntry obj)
		{
			return sprites[obj.PropertyValue >> 7];
		}
		
		public override Sprite GetDebugOverlay(ObjectEntry obj)
		{
			int dist = (obj.PropertyValue & 0x7f) << 4;
			BitmapBits bitmap = new BitmapBits(dist + 1, 2);
			bitmap.DrawLine(6, 0, 0, dist, 0); // LevelData.ColorWhite
			return new Sprite(bitmap, (obj.PropertyValue > 0x7f) ? 0 : -dist, 0);
		}
	}
}