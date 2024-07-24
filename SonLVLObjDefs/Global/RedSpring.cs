using SonicRetro.SonLVL.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1ObjectDefinitions.Global
{
	class RedSpring : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[3];
		private Sprite[] sprites = new Sprite[4];
		
		public override void Init(ObjectData data)
		{
			BitmapBits sheet = LevelData.GetSpriteSheet("Global/Items.gif");
			sprites[0] = new Sprite(new Sprite(sheet.GetSection(84, 9, 32, 8), -16, 0), new Sprite(sheet.GetSection(84, 1, 32, 8), -16, -8));
			sprites[1] = new Sprite(new Sprite(sheet.GetSection(141, 26, 8, 32), -8, -16), new Sprite(sheet.GetSection(149, 26, 8, 32), 0, -16));
			sprites[2] = new Sprite(new Sprite(sheet.GetSection(141, 59, 8, 32), 0, -16), new Sprite(sheet.GetSection(133, 59, 8, 32), -8, -16));
			sprites[3] = new Sprite(new Sprite(sheet.GetSection(150, 101, 32, 8), -16, -8), new Sprite(sheet.GetSection(150, 109, 32, 8), -16, 0));
			
			properties[0] = new PropertySpec("Direction", typeof(int), "Extended",
				"Which direction this Spring should face.", null, new Dictionary<string, int>
				{
					{ "Up", 0 },
					{ "Right", 1 },
					{ "Left", 2 },
					{ "Down", 3 }
				},
				(obj) => (obj.PropertyValue & 0x7f),
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~0x7f) | (int)value));
			
			properties[1] = new PropertySpec("Enabled in Air", typeof(int), "Extended",
				"If the Spring should be usable when the player is in the air already. Only affects horizontal springs.", null,
				(obj) => (obj.PropertyValue >= 0x80),
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~0x80) | ((bool)value ? 0x80 : 0x00)));
			
			properties[2] = new PropertySpec("Platform", typeof(bool), "Extended",
				"If this Spring should allow as a platform rather than as a box. Upwards springs only.", null,
				(obj) => ((V4ObjectEntry)obj).Value1 != 0,
				(obj, value) => ((V4ObjectEntry)obj).Value1 = ((bool)value ? 1 : 0));
			
			// scale is set in m005 but it doesn't really mean anything lol
		}
		
		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {0, 1, 2, 3, 4 | 1, 4 | 2}); }
		}
		
		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
		}

		public override string SubtypeName(byte subtype)
		{
			string[] directions = {"Upwards", "Right", "Left", "Down"};
			string name = directions[subtype & 3];
			if (subtype > 3) name += " (Enabled in Air)";
			return name;
		}

		public override Sprite Image
		{
			get { return sprites[0]; }
		}

		public override Sprite SubtypeImage(byte subtype)
		{
			return sprites[subtype & 3];
		}

		public override Sprite GetSprite(ObjectEntry obj)
		{
			return sprites[obj.PropertyValue & 3];
		}
	}
}