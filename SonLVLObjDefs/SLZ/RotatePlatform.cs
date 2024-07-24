using SonicRetro.SonLVL.API;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1ObjectDefinitions.SLZ
{
	class RotatePlatform : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[2];
		private Sprite[] sprites = new Sprite[5];
		private Sprite debug;
		
		public override void Init(ObjectData data)
		{
			sprites[4] = new Sprite(LevelData.GetSpriteSheet("SLZ/Objects.gif").GetSection(1, 196, 48, 16), -24, -8);
			
			Point[] offsets = { new Point(-80, 0), new Point(80, 0), new Point(0, 80), new Point(0, -80) };
			for (int i = 0; i < 4; i++)
				sprites[i] = new Sprite(sprites[4], offsets[i]);
			
			int radius = 80;
			var overlay = new BitmapBits(radius * 2 + 1, radius * 2 + 1);
			overlay.DrawCircle(6, radius, radius, radius); // LevelData.ColorWhite
			debug = new Sprite(overlay, -radius, -radius);
			
			// instead of saying udlr, since that wouldn't play nice with the direction property, i decided to go with angles instead
			// it's kind of not as good but hopefully it's not _the worst_
			properties[0] = new PropertySpec("Start From", typeof(int), "Extended",
				"The angle from which the Platform will start.", null, new Dictionary<string, int>
				{
					{ "0 Degrees", 1 },
					{ "90 Degrees", 3 },
					{ "180 Degrees", 0 },
					{ "270 Degrees", 2 }
				},
				(obj) => obj.PropertyValue & 3,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~3) | (int)value));
			
			properties[1] = new PropertySpec("Direction", typeof(int), "Extended",
				"The direction in which the Platform moves.", null, new Dictionary<string, int>
				{
					{ "Counter-Clockwise", 0 },
					{ "Clockwise", 4 }
				},
				(obj) => obj.PropertyValue & 4,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~4) | (int)value));
		}

		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {0, 1, 2, 3, 4, 5, 6, 7}); }
		}
		
		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
		}
		
		// unlike the main property, we can use normal directions here rather than angles, so let's do that
		public override string SubtypeName(byte subtype)
		{
			string[] names = {"Left (Counter-Clockwise)", "Right (Counter-Clockwise)", "Downwards (Counter-Clockwise)", "Upwards (Counter-Clockwise)", 
			                   "Right (Clockwise)", "Left (Clockwise)", "Upwards (Clockwise)", "Downwards (Clockwise)"};
			return "Start From " + names[subtype & 7];
		}

		public override Sprite Image
		{
			get { return sprites[4]; }
		}

		public override Sprite SubtypeImage(byte subtype)
		{
			return sprites[4];
		}

		public override Sprite GetSprite(ObjectEntry obj)
		{
			int[] indexes = {0, 1, 2, 3,   1, 0, 3, 2};
			return sprites[indexes[obj.PropertyValue & 7]];
		}
		
		public override Sprite GetDebugOverlay(ObjectEntry obj)
		{
			return debug;
		}
	}
}