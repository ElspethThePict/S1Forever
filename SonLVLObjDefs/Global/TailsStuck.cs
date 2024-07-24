using SonicRetro.SonLVL.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1ObjectDefinitions.Global
{
	class TailsStuck : ObjectDefinition
	{
		private Sprite[] sprites = new Sprite[2];
		private PropertySpec[] properties = new PropertySpec[2];
		
		public override void Init(ObjectData data)
		{
			sprites[0] = new Sprite(LevelData.GetSpriteSheet("Global/Display.gif").GetSection(60, 108, 16, 14), -8, -7);
			
			// object icon, 2x2 box
			sprites[1] = new Sprite(new Sprite(sprites[0], -8, -8), new Sprite(sprites[0],  8, -8),
			                        new Sprite(sprites[0], -8,  8), new Sprite(sprites[0],  8,  8));
			
			properties[0] = new PropertySpec("Width", typeof(int), "Extended",
				"How wide the Invisible Block will be.", null,
				(obj) => (obj.PropertyValue >> 4) + 1,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~0xf0) | (Math.Min(Math.Max((int)value - 1, 0), 15) << 4))); // could've sworn a Math had a Clamp function.. but ig it doesn't?
			
			properties[1] = new PropertySpec("Height", typeof(int), "Extended",
				"How tall the Invisible Block will be.", null,
				(obj) => (obj.PropertyValue & 0x0f) + 1,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~0x0f) | Math.Min(Math.Max((int)value - 1, 0), 15)));
		}
		
		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[0]); }
		}
		
		public override bool Debug
		{
			get { return true; }
		}
		
		public override byte DefaultSubtype
		{
			get { return 0x11; }
		}
		
		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
		}

		public override string SubtypeName(byte subtype)
		{
			return ((subtype >> 4) + 1) + " x " + ((subtype & 0x0f) + 1) + " blocks";
		}

		public override Sprite Image
		{
			get { return sprites[1]; }
		}

		public override Sprite SubtypeImage(byte subtype)
		{
			return sprites[1];
		}

		public override Sprite GetSprite(ObjectEntry obj)
		{
			int width = (obj.PropertyValue >> 4) + 1;
			int height = (obj.PropertyValue & 0x0f) + 1;
			
			int sx = (obj.PropertyValue & 0xf0) >> 1;
			int sy = (obj.PropertyValue & 0x0f) << 3;
			
			List<Sprite> sprs = new List<Sprite>();
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					sprs.Add(new Sprite(sprites[0], -sx + (j * 16), -sy + (i * 16)));
				}
			}
			
			return new Sprite(sprs.ToArray());
		}
	}
}