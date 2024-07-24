using SonicRetro.SonLVL.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1ObjectDefinitions.Global
{
	class YBoundAdjust : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[1];
		private Sprite[] sprites = new Sprite[2];
		
		public override void Init(ObjectData data)
		{
			sprites[0] = new Sprite(LevelData.GetSpriteSheet("Global/Display.gif").GetSection(205, 239, 16, 16), -8, -8);
			
			properties[0] = new PropertySpec("Mode", typeof(int), "Extended",
				"This object's mode. Box Adjust checks if the player is between this and object[+1] before changing bounds.", null, new Dictionary<string, int>
				{
					{ "Regular Adjust", 0 },
					{ "Box Adjust", 1 }
				},
				(obj) => (obj.PropertyValue == 0) ? 0 : 1,
				(obj, value) => obj.PropertyValue = (byte)((int)value));
			
			BitmapBits bitmap = new BitmapBits(48, 1);
			bitmap.DrawLine(6, 0, 0, 47, 0); // LevelData.ColorWhite
			sprites[1] = new Sprite(new Sprite(bitmap, -24, 0), sprites[0]);
		}
		
		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {0, 1}); }
		}

		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
		}

		public override string SubtypeName(byte subtype)
		{
			return (subtype == 0) ? "Regular Adjust" : "Box Adjust";
		}

		public override Sprite Image
		{
			get { return sprites[0]; }
		}

		public override Sprite SubtypeImage(byte subtype)
		{
			return sprites[0];
		}

		public override Sprite GetSprite(ObjectEntry obj)
		{
			return sprites[1];
		}
	}
}