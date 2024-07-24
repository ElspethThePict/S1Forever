using SonicRetro.SonLVL.API;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1ObjectDefinitions.SYZ
{
	class VPlatform : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[2];
		private Sprite[] sprites = new Sprite[3];
		private Sprite debug;
		
		public override void Init(ObjectData data)
		{
			sprites[2] = new Sprite(LevelData.GetSpriteSheet("SYZ/Objects.gif").GetSection(119, 1, 64, 32), -32, -10);
			sprites[0] = new Sprite(sprites[2], 0,  48);
			sprites[1] = new Sprite(sprites[2], 0, -48);
			
			BitmapBits bitmap = new BitmapBits(2, 97);
			bitmap.DrawLine(6, 0, 0, 0, 96); // LevelData.ColorWhite
			debug = new Sprite(bitmap, 0, -48);
			
			properties[0] = new PropertySpec("Start From", typeof(int), "Extended",
				"Which side this platform should start from.", null, new Dictionary<string, int>
				{
					{ "Bottom", 0 },
					{ "Top", 1 }
				},
				(obj) => obj.PropertyValue & 1,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~1) | (int)value));
			
			properties[1] = new PropertySpec("Cycle", typeof(int), "Extended",
				"Which cycle this Platform should follow.", null, new Dictionary<string, int>
				{
					{ "Stage Cycle", 0 },
					{ "Global Cycle", 2 }
				},
				(obj) => obj.PropertyValue & 2,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~2) | (int)value));
		}
		
		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {0, 1, 2, 3}); }
		}
		
		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
		}

		public override string SubtypeName(byte subtype)
		{
			switch (subtype)
			{
				case 0: return "Start From Bottom (Stage Cycle)";
				case 1: return "Start From Top (Stage Cycle)";
				case 2: return "Start From Bottom (Global Cycle)";
				case 3: return "Start From Top (Global Cycle)";
				
				default: return "Unknown"; // technically "static"
			}
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
			return (obj.PropertyValue < 4) ? debug : null;
		}
	}
}