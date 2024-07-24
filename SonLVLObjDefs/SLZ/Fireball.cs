using SonicRetro.SonLVL.API;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1ObjectDefinitions.SLZ
{
	class Fireball : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[2];
		private Sprite[] sprites = new Sprite[8];
		private Sprite[] debug = new Sprite[4];
		
		public override void Init(ObjectData data)
		{
			BitmapBits sheet = LevelData.GetSpriteSheet("SLZ/Objects.gif");
			Sprite[] frames = new Sprite[2];
			frames[0] = new Sprite(sheet.GetSection(1, 1, 15, 31), -7, -23);
			frames[1] = new Sprite(sheet.GetSection(2, 34, 31, 15), -23, -8);
			
			for (int i = 0; i < 8; i++)
			{
				sprites[i] = new Sprite(frames[(i > 5) ? 1 : 0], (i == 6), (i < 5));
			}
			
			properties[0] = new PropertySpec("Pattern", typeof(int), "Extended",
				"The pattern this Fireball is to follow.", null, new Dictionary<string, int>
				{
					{ "Jump (84 px)", 0 },
					{ "Jump (131 px)", 1 },
					{ "Jump (189 px)", 2 },
					{ "Jump (258 px)", 3 },
					{ "Travel Up", 4 },
					{ "Travel Down", 5 },
					{ "Travel Left", 6 },
					{ "Travel Right", 7 }
				},
				(obj) => obj.PropertyValue & 7,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~7) | (int)value));
			
			properties[1] = new PropertySpec("Interval", typeof(int), "Extended",
				"The timings this Fireball is to be based off.", null, new Dictionary<string, int>
				{
					{ "30 Frames", 0x00 },
					{ "60 Frames", 0x10 },
					{ "90 Frames", 0x20 },
					{ "120 Frames", 0x30 },
					{ "150 Frames", 0x40 },
					{ "180 Frames", 0x50 },
					{ "210 Frames", 0x60 },
					{ "240 Frames", 0x70 },
					{ "270 Frames", 0x80 },
					{ "300 Frames", 0x90 },
					{ "330 Frames", 0xa0 },
					{ "360 Frames", 0xb0 },
					{ "390 Frames", 0xc0 },
					{ "420 Frames", 0xd0 },
					{ "450 Frames", 0xe0 },
					{ "480 Frames", 0xf0 },
				},
				(obj) => obj.PropertyValue & 0xf0,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~0xf0) | (int)value));
			
			/*
			// maybe?
			properties[1] = new PropertySpec("Interval", typeof(int), "Extended",
				"The interval, in groups of 30 frames, at which the Fireball should fire.", null,
				(obj) => ((obj.PropertyValue >> 4) + 1) * 30,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~0xf0) | ((((int)value / 30) - 1) << 4)));
			*/
			
			// For the jumping fireballs, let's show how high they'll jump
			int[] distances = {84, 131, 189, 258};
			for (int i = 0; i < distances.Length; i++)
			{
				BitmapBits bitmap = new BitmapBits(2, distances[i] + 1);
				bitmap.DrawLine(6, 0, 0, 0, distances[i]); // LevelData.ColorWhite
				debug[i] = new Sprite(bitmap, 0, -distances[i]);
			}
		}

		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {0x30, 0x41, 0x42, 0x43, 0x34, 0x35, 0x36, 0x37}); } // some hidden intervals..
		}
		
		public override byte DefaultSubtype
		{
			get { return 0x30; }
		}
		
		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
		}

		public override string SubtypeName(byte subtype)
		{
			string[] patterns = {"Jump (84 px)", "Jump (131 px)", "Jump (189 px)", "Jump (258 px)", "Travel Up", "Travel Down", "Travel Left", "Travel Right"};
			return patterns[subtype & 7];
		}

		public override Sprite Image
		{
			get { return sprites[0]; }
		}

		public override Sprite SubtypeImage(byte subtype)
		{
			return sprites[subtype & 7];
		}

		public override Sprite GetSprite(ObjectEntry obj)
		{
			return sprites[obj.PropertyValue & 7];
		}
		
		// i'm kind of not sure.. maybe this one is too much? i'll leave it for now, but i might just remove this later, it feels a bit too much...
		public override Sprite GetDebugOverlay(ObjectEntry obj)
		{
			return ((obj.PropertyValue & 7) < 4) ? debug[obj.PropertyValue & 7] : null;
		}
	}
}