using SonicRetro.SonLVL.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1ObjectDefinitions.Global
{
	class YellowSpring : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[2];
		private Sprite[] sprites = new Sprite[4];
		
		public override void Init(ObjectData data)
		{
			BitmapBits sheet = LevelData.GetSpriteSheet("Global/Items.gif");
			sprites[0] = new Sprite(new Sprite(sheet.GetSection(84, 9, 32, 8), -16, 0), new Sprite(sheet.GetSection(84, 18, 32, 8), -16, -8));
			sprites[1] = new Sprite(new Sprite(sheet.GetSection(141, 26, 8, 32), -8, -16), new Sprite(sheet.GetSection(133, 26, 8, 32), 0, -16));
			sprites[2] = new Sprite(new Sprite(sheet.GetSection(141, 59, 8, 32), 0, -16), new Sprite(sheet.GetSection(149, 59, 8, 32), -8, -16));
			sprites[3] = new Sprite(new Sprite(sheet.GetSection(150, 101, 32, 8), -16, -8), new Sprite(sheet.GetSection(150, 92, 32, 8), -16, 0));
			
			properties[0] = new PropertySpec("Direction", typeof(int), "Extended",
				"Which direction this Spring should face.", null, new Dictionary<string, int>
				{
					{ "Up", 0 },
					{ "Right", 1 },
					{ "Left", 2 },
					{ "Down", 3 },
					{ "Up (Stronger)", 5 },
				},
				(obj) => (int)obj.PropertyValue,
				(obj, value) => obj.PropertyValue = (byte)((int)value));
			
			properties[1] = new PropertySpec("Platform", typeof(bool), "Extended",
				"If this Spring should allow as a platform rather than as a box. Upwards springs only.", null,
				(obj) => ((V4ObjectEntry)obj).Value1 != 0,
				(obj, value) => ((V4ObjectEntry)obj).Value1 = ((bool)value ? 1 : 0));
			
			// RE2 has "extraVelocity" as a property.. but i don't think it's supposed to be set from editor?
			// sure it works, but then it makes the "Up (Stronger)" option redundant too, so i'm not sure..
			// i guess i could quietly convert "Up (Stronger)" springs from prop val to extraVelocity? feels kinda iffy, though
		}
		
		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {0, 1, 2, 3, 5}); }
		}
		
		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
		}

		public override string SubtypeName(byte subtype)
		{
			string[] directions = {"Upwards", "Right", "Left", "Down", "Upwards (Stronger)"};
			string name = directions[(subtype == 5) ? 4 : subtype & 3];
			if ((subtype > 3) && (subtype != 5)) name += " (Enabled in Air)";
			return name;
		}

		public override Sprite Image
		{
			get { return sprites[0]; }
		}

		public override Sprite SubtypeImage(byte subtype)
		{
			return sprites[((subtype == 5) ? 0 : subtype) & 3];
		}

		public override Sprite GetSprite(ObjectEntry obj)
		{
			return sprites[((obj.PropertyValue == 5) ? 0 : obj.PropertyValue) & 3];
		}
	}
}