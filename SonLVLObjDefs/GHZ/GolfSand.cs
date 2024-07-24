using SonicRetro.SonLVL.API;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1FObjectDefinitions.GHZ
{
	class GolfSand : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[1];
		private Sprite[] sprites = new Sprite[3];
		
		public override void Init(ObjectData data)
		{
			BitmapBits sheet = LevelData.GetSpriteSheet("GHZ/Objects2.gif");
			sprites[0] = new Sprite(sheet.GetSection(135, 41, 16, 16)); // top (left)
			sprites[1] = new Sprite(sheet.GetSection(151, 41, 16, 16)); // top (right)
			
			Sprite[] sand = new Sprite[2];
			sand[0] = new Sprite(sheet.GetSection(135, 57, 16, 16)); // chain (left)
			sand[1] = new Sprite(sheet.GetSection(151, 57, 16, 16)); // chain (right)
			
			for (int i = 1; i < 5; i++)
			{
				sprites[0] = new Sprite(sprites[0], new Sprite(sand[0], 0, i * 16));
				sprites[1] = new Sprite(sprites[1], new Sprite(sand[1], 0, i * 16));
			}
			
			sprites[2] = new Sprite(sprites[0], new Sprite(sprites[1], 16, 0));
			
			properties[0] = new PropertySpec("Size", typeof(int), "Extended",
				"How long the sand pit is.", null,
				(obj) => obj.PropertyValue,
				(obj, value) => obj.PropertyValue = (byte)((int)value));
		}

		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {6, 8, 10, 12, 14, 16}); } // it can be any value, but why not give a few starting ones
		}
		
		public override byte DefaultSubtype
		{
			get { return 12; }
		}

		public override string SubtypeName(byte subtype)
		{
			return subtype + " blocks"; // "blocks" is a weird name but idk what else to call them-
		}
		
		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
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
			if (obj.PropertyValue <= 1)
				return sprites[0];
			
			List<Sprite> sprs = new List<Sprite>();
			for (int i = 0; i < obj.PropertyValue; i++)
				sprs.Add(new Sprite(sprites[i & 1], i * 16, 0));
			
			return new Sprite(sprs.ToArray());
		}
	}
}