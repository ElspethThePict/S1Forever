using SonicRetro.SonLVL.API;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1FObjectDefinitions.MBZ
{
	class Crust : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[1];
		private Sprite[] sprites = new Sprite[3];
		
		public override void Init(ObjectData data)
		{
			BitmapBits sheet = LevelData.GetSpriteSheet("MBZ/Objects.gif");
			sprites[0] = new Sprite(sheet.GetSection(222, 461, 16, 16), 0, -16);
			sprites[1] = new Sprite(sheet.GetSection(239, 461, 16, 16), 0, -16);
			
			// setup the object icon
			List<Sprite> sprs = new List<Sprite>();
			for (int i = 0; i < 8; i++)
				sprs.Add(new Sprite(sprites[i & 1], i * 16, 0));
			
			sprites[2] = new Sprite(sprs.ToArray());
			
			properties[0] = new PropertySpec("Size", typeof(int), "Extended",
				"How long the object should be.", null,
				(obj) => obj.PropertyValue,
				(obj, value) => obj.PropertyValue = (byte)((int)value));
		}

		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {6, 8, 10, 12, 14, 16}); } // it can be any value, but why not give a few starting ones
		}
		
		public override byte DefaultSubtype
		{
			get { return 8; }
		}

		public override string SubtypeName(byte subtype)
		{
			return subtype + " blocks"; // "blocks"? sure ig..
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