using SonicRetro.SonLVL.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1ObjectDefinitions.SBZ
{
	class Buzzsaw : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[1];
		private Sprite[] sprites = new Sprite[4];
		private Sprite[] debug = new Sprite[4];
		
		public override void Init(ObjectData data)
		{
			BitmapBits sheet = LevelData.GetSpriteSheet("SBZ/Objects.gif");
			sprites[0] = new Sprite(sheet.GetSection(319, 50, 64, 64), -32, -32); // stray
			sprites[1] = new Sprite(sheet.GetSection(189, 1, 64, 92), -32, -60); // attached
			sprites[2] = new Sprite(sprites[1], -96, 0); // start from left
			sprites[3] = new Sprite(sprites[1], 0, -50); // start from top
			
			// h hover line
			BitmapBits bitmap = new BitmapBits(97, 2);
			bitmap.DrawLine(6, 0, 0, 96, 0);
			debug[0] = new Sprite(bitmap, -96, 0);
			
			// v hover line
			bitmap = new BitmapBits(2, 51);
			bitmap.DrawLine(6, 0, 0, 0, 50);
			debug[1] = new Sprite(bitmap, 0, -50);
			
			// stray - right arrow
			bitmap = new BitmapBits(56, 10);
			bitmap.DrawArrow(6, 0, 5, 55, 5);
			debug[2] = new Sprite(bitmap, 0, -8);
			
			// stray - left arrow
			debug[3] = new Sprite(debug[2], true, false);
			
			// let's combine prop val and dir for this
			// not sure if putting Strays here is a good idea or not, but this is the cleanest solution i can come up with
			properties[0] = new PropertySpec("Start From", typeof(int), "Extended",
				"Which direction this Buzzsaw should start from.", null, new Dictionary<string, int>
				{
					// { "Static", 0x00 }, // not sure if this is supposed to be placed, actually? it doesn't really do anything, it's p much just deco
					{ "Right", 0x01 },
					{ "Left", 0x101 },
					{ "Bottom", 0x02 },
					{ "Top", 0x102 },
					{ "Stray (Move Right)", 0x03 },
					{ "Stray (Move Left)", 0x04 }
				},
				(obj) => {
						int result = obj.PropertyValue;
						if ((result == 1) || (result == 2)) // hover type?
							result |= ((int)((V4ObjectEntry)obj).Direction << 8) & 0x100;
						return result;
					},
				(obj, value) => {
						obj.PropertyValue = (byte)((int)value & 0xff);
						((V4ObjectEntry)obj).Direction = (RSDKv3_4.Tiles128x128.Block.Tile.Directions)((int)value >> 8);
					}
				);
		}
		
		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {1, 2, 3, 4}); } // can't stick in dir attr in here, 
		}
		
		public override byte DefaultSubtype
		{
			get { return 3; }
		}
		
		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
		}
		
		public override string SubtypeName(byte subtype)
		{
			switch (subtype)
			{
				default:
				case 0: return "Static";
				case 1: return "Hover Horizontally";
				case 2: return "Hover Vertically";
				case 3: return "Stray (Move Right)";
				case 4: return "Stray (Move Left)";
			}
		}
		
		public override Sprite Image
		{
			get { return sprites[0]; }
		}
		
		public override Sprite SubtypeImage(byte subtype)
		{
			return sprites[((subtype == 3) || (subtype == 4)) ? 0 : 1];
		}
		
		public override Sprite GetSprite(ObjectEntry obj)
		{
			// there's def a better way to do it, but this works
			
			int index = ((obj.PropertyValue == 3) || (obj.PropertyValue == 4)) ? 0 : 1;
			
			if (((V4ObjectEntry)obj).Direction.HasFlag(RSDKv3_4.Tiles128x128.Block.Tile.Directions.FlipX))
			{
				if (obj.PropertyValue == 1) index = 2;
				else if (obj.PropertyValue == 2) index = 3;
			}
			
			return sprites[index];
		}
		
		public override Sprite GetDebugOverlay(ObjectEntry obj)
		{
			if ((obj.PropertyValue == 0) || (obj.PropertyValue > 4)) return null;
			
			return debug[obj.PropertyValue - 1];
		}
	}
	
	public static class BitmapBitsExtensions
	{
		public static void DrawArrow(this BitmapBits bitmap, byte index, int x1, int y1, int x2, int y2)
		{
			bitmap.DrawLine(index, x1, y1, x2, y2);
			
			double angle = Math.Atan2(y1 - y2, x1 - x2);
			bitmap.DrawLine(index, x2, y2, x2 + (int)(Math.Cos(angle + 0.40) * 10), y2 + (int)(Math.Sin(angle + 0.40) * 10));
			bitmap.DrawLine(index, x2, y2, x2 + (int)(Math.Cos(angle - 0.40) * 10), y2 + (int)(Math.Sin(angle - 0.40) * 10));
		}
	}
}