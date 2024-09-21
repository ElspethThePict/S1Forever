using SonicRetro.SonLVL.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1ObjectDefinitions.GHZ
{
	class BreakWall : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[1];
		private Sprite[] sprites = new Sprite[3];
		private Sprite debug;
		
		public override void Init(ObjectData data)
		{
			BitmapBits sheet = LevelData.GetSpriteSheet("GHZ/Objects.gif");
			sprites[0] = new Sprite(sheet.GetSection(191, 146, 32, 64), -16, -32);
			sprites[1] = new Sprite(sheet.GetSection(207, 146, 32, 64), -16, -32);
			sprites[2] = new Sprite(sheet.GetSection(223, 146, 32, 64), -16, -32);
			
			BitmapBits bitmap = new BitmapBits(32, 64);
			bitmap.DrawRectangle(6, 0, 0, 31, 63); // LevelData.ColorWhite
			debug = new Sprite(bitmap, -16, -32);
			
			properties[0] = new PropertySpec("Side", typeof(int), "Extended",
				"Which side this Breakable Wall is facing.", null, new Dictionary<string, int>
				{
					{ "Left", 0 },
					{ "Middle", 1 },
					{ "Right", 2 },
				},
				(obj) => (int)obj.PropertyValue,
				(obj, value) => obj.PropertyValue = (byte)((int)value));
		}
		
		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {0, 1, 2}); }
		}
		
		public override byte DefaultSubtype
		{
			get { return 1; }
		}
		
		public override string SubtypeName(byte subtype)
		{
			switch (subtype)
			{
				case 0: return "Left Wall";
				case 1: return "Middle Wall";
				case 2: return "Right Wall";
				default: return "Unknown";
			}
		}
		
		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
		}
		
		public override Sprite Image
		{
			get { return sprites[1]; }
		}

		public override Sprite SubtypeImage(byte subtype)
		{
			return sprites[subtype];
		}

		public override Sprite GetSprite(ObjectEntry obj)
		{
			return sprites[(obj.PropertyValue < 3) ? obj.PropertyValue : 1];
		}
		
		public override Sprite GetDebugOverlay(ObjectEntry obj)
		{
			return debug;
		}
	}
}