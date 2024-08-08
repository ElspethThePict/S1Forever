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
			
			// this object's kind of.. weird, so
			// yeah this is the easy way out, but let's just do this instead of doing trig stuff
			Point[] offsets = { new Point(-80, 0), new Point(80, 0), new Point(0, 80), new Point(0, -80) };
			for (int i = 0; i < 4; i++)
				sprites[i] = new Sprite(sprites[4], offsets[i]);
			
			int radius = 80;
			BitmapBits overlay = new BitmapBits(radius * 2 + 1, radius * 2 + 1);
			overlay.DrawCircle(6, radius, radius, radius); // LevelData.ColorWhite
			debug = new Sprite(overlay, -radius, -radius);
			
			properties[0] = new PropertySpec("Start From", typeof(int), "Extended",
				"The angle from which the Platform will start.", null, new Dictionary<string, int>
				{
					{ "Left", 0 },
					{ "Bottom", 2 },
					{ "Right", 1 },
					{ "Top", 3 }
				},
				(obj) => (obj.PropertyValue & 3) ^ (((obj.PropertyValue & 4) == 4) ? 1 : 0), // if we're a clockwise platform, we need to flip it around
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~3) | ((int)value ^ (((obj.PropertyValue & 4) == 4) ? 1 : 0))));
			
			properties[1] = new PropertySpec("Direction", typeof(int), "Extended",
				"The direction in which the Platform moves.", null, new Dictionary<string, int>
				{
					{ "Counter-Clockwise", 0 },
					{ "Clockwise", 4 }
				},
				(obj) => obj.PropertyValue & 4,
				(obj, value) => {
						int val = (int)value;
						if ((obj.PropertyValue & 4) != val) // if we're switching dir, let's preserve starting position too
							obj.PropertyValue ^= 1;
						
						obj.PropertyValue = (byte)((obj.PropertyValue & ~4) | val);
					}
				);
		}

		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {5, 7, 4, 6, 0, 2, 1, 3}); }
		}
		
		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
		}
		
		public override string SubtypeName(byte subtype)
		{
			string[] directions = {"Left", "Right", "Bottom", "Top"};
			string name = "Start From " + directions[(subtype & 3) ^ (((subtype & 4) == 4) ? 1 : 0)];
			name += ((subtype & 4) == 4) ? " (Clockwise)" : " (Counter-clockwise)";
			return name;
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
			int index = (obj.PropertyValue & 3) ^ (((obj.PropertyValue & 4) == 4) ? 1 : 0);
			return sprites[index];
		}
		
		public override Sprite GetDebugOverlay(ObjectEntry obj)
		{
			return debug;
		}
	}
}