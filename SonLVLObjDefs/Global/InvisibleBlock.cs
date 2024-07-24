using SonicRetro.SonLVL.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1FObjectDefinitions.Global
{
	class InvisibleBlock : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[3];
		private readonly Sprite[] sprites = new Sprite[6];
		
		public override void Init(ObjectData data)
		{
			BitmapBits sheet = LevelData.GetSpriteSheet("Global/Display.gif");
			sprites[0] = new Sprite(sheet.GetSection(1, 176, 16, 14), -8, -7);
			sprites[1] = new Sprite(sheet.GetSection(17, 176, 16, 14), -8, -7);
			sprites[2] = new Sprite(sheet.GetSection(1, 190, 16, 14), -8, -7);
			sprites[4] = new Sprite(sheet.GetSection(77, 108, 16, 14), -8, -7); // right eject Knuckles icon, #Forever
			sprites[3] = new Sprite(sprites[4], true, false); // flip it for the left eject type
			
			// object icon, 2x2 box
			sprites[5] = new Sprite(new Sprite(sprites[0], -8, -8), new Sprite(sprites[0],  8, -8),
			                        new Sprite(sprites[0], -8,  8), new Sprite(sprites[0],  8,  8));
			
			properties[0] = new PropertySpec("Width", typeof(int), "Extended",
				"How wide the Invisible Block will be.", null,
				(obj) => (obj.PropertyValue >> 4) + 1,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~0xf0) | (Math.Min(Math.Max((int)value - 1, 0), 15) << 4))); // could've sworn a Math had a Clamp function.. but ig it doesn't?
			
			properties[1] = new PropertySpec("Height", typeof(int), "Extended",
				"How tall the Invisible Block will be.", null,
				(obj) => (obj.PropertyValue & 0x0f) + 1,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~0x0f) | Math.Min(Math.Max((int)value - 1, 0), 15)));
			
			properties[2] = new PropertySpec("Mode", typeof(int), "Extended",
				"How this Invisible Block will act.", null, new Dictionary<string, int>
				{
					{ "Solid", 0 },
					{ "Eject Left", 1 },
					{ "Eject Right", 2 },
					{ "Ledge Eject Left", 7 }, // new #Forever type
					{ "Ledge Eject Right", 0x107 } // let's keep them together here for simplicity's sake
				},
				(obj) => {
						int result = ((V4ObjectEntry)obj).State;
						if (result == 7) // Knuckles eject type?
							result |= (((int)((V4ObjectEntry)obj).Direction == 0) ? 0 : 1) << 8;
						return result;
					},
				(obj, value) => {
						((V4ObjectEntry)obj).State = (byte)((int)value & 0xff);
						((V4ObjectEntry)obj).Direction = (RSDKv3_4.Tiles128x128.Block.Tile.Directions)((int)value >> 8);
					}
				);
		}
		
		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {0x11}); }
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
			return ((subtype >> 4) + 1) + "x" + ((subtype & 0x0f) + 1) + " blocks";
		}

		public override Sprite Image
		{
			get { return sprites[5]; }
		}

		public override Sprite SubtypeImage(byte subtype)
		{
			return sprites[5];
		}

		public override Sprite GetSprite(ObjectEntry obj)
		{
			int width = (obj.PropertyValue >> 4) + 1;
			int height = (obj.PropertyValue & 0x0f) + 1;
			
			int sx = (obj.PropertyValue & 0xf0) >> 1;
			int sy = (obj.PropertyValue & 0x0f) << 3;
			
			int index = (((V4ObjectEntry)obj).State < 3) ? ((V4ObjectEntry)obj).State : 0;
			
			if (((V4ObjectEntry)obj).State == 7)
				index = (((V4ObjectEntry)obj).Direction == RSDKv3_4.Tiles128x128.Block.Tile.Directions.FlipNone) ? 3 : 4;
			
			Sprite row = new Sprite();
			for (int i = 0; i < width; i++) // make a row, first
				row = new Sprite(row, new Sprite(sprites[index], -sx + (i * 16), 0));
			
			Sprite sprite = new Sprite();
			for (int i = 0; i < height; i++) // now, combine all the rows
				sprite = new Sprite(sprite, new Sprite(row, 0, -sy + (i * 16)));
			
			return sprite;
		}
	}
}