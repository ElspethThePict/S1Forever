using SonicRetro.SonLVL.API;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1ObjectDefinitions.SYZ
{
	class RotatingSpike : ObjectDefinition
	{
		private Sprite[] sprites = new Sprite[3];
		private Sprite debug;
		private PropertySpec[] properties = new PropertySpec[2];
		
		public override void Init(ObjectData data)
		{
			sprites[2] = new Sprite(LevelData.GetSpriteSheet("SYZ/Objects.gif").GetSection(61, 178, 48, 48), -24, -24);
			sprites[0] = new Sprite(sprites[2], 80, 0);
			sprites[1] = new Sprite(sprites[2], -80, 0);
			
			BitmapBits bitmap = new BitmapBits(161, 161);
			bitmap.DrawCircle(6, 80, 80, 80); // LevelData.ColorWhite
			debug = new Sprite(bitmap, -80, -80);
			
			properties[0] = new PropertySpec("Speed", typeof(int), "Extended",
				"How fast this Spike should spin.", null, new Dictionary<string, int>
				{
					{ "Slow", 2 },
					{ "Medium", 4 },
					{ "Fast", 0 }
				},
				(obj) => obj.PropertyValue & 6,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~6) | (int)value));
			
			properties[1] = new PropertySpec("Start From", typeof(int), "Extended",
				"Which side this Spike should start from.", null, new Dictionary<string, int>
				{
					{ "Right", 0 },
					{ "Left", 1 }
				},
				(obj) => obj.PropertyValue & 1,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~1) | (int)value));
		}
		
		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {0, 1, 4, 5, 2, 3}); }
		}
		
		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
		}

		public override string SubtypeName(byte subtype)
		{
			string name = "";
			
			switch (subtype & 6)
			{
				case 0: name = "Fast"; break;
				case 2: name = "Slow"; break;
				case 4: name = "Medium"; break;
				default: name = "Unknown"; break;
			}
			
			name += (((subtype & 1) == 0) ? " (Right)" : " (Left)");
			return name;
		}

		public override Sprite Image
		{
			get { return sprites[2]; }
		}

		public override Sprite SubtypeImage(byte subtype)
		{
			return sprites[2];
		}

		public override Sprite GetSprite(ObjectEntry obj)
		{
			return sprites[obj.PropertyValue & 1];
		}
		
		public override Sprite GetDebugOverlay(ObjectEntry obj)
		{
			return debug;
		}
	}
}