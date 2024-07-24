using SonicRetro.SonLVL.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1ObjectDefinitions.SBZ
{
	class TrapDoor : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[1];
		private Sprite sprite;
		private Sprite debug;
		
		public override void Init(ObjectData data)
		{
			sprite = new Sprite(LevelData.GetSpriteSheet("SBZ/Objects.gif").GetSection(318, 115, 64, 24), 0, -12);
			sprite = new Sprite(sprite, new Sprite(sprite, true, false));
			
			// tagging this area with LevelData.ColorWhite
			BitmapBits bitmap = new BitmapBits(24, 64);
			bitmap.DrawRectangle(6, 0, 0, 23, 63);
			debug = new Sprite(bitmap, -12 - 64, 12);
			
			bitmap = new BitmapBits(61 - 12, 72);
			bitmap.DrawCircle(6, -12, 0, 60);
			debug = new Sprite(debug, new Sprite(bitmap, -64, 0));

			debug = new Sprite(debug, new Sprite(debug, true, false));
			
			// (top 4 bits are unused)
			// also timer's a bit of a weird name but i hope it's good enough?
			properties[0] = new PropertySpec("Timer", typeof(int), "Extended",
				"How many seconds this Trap Door should spend in each cycle.", null,
				(obj) => obj.PropertyValue & 15,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~15) | ((int)value & 15)));
		}
		
		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15}); }
		}
		
		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
		}
		
		public override string SubtypeName(byte subtype)
		{
			return subtype + ((subtype < 2) ? " Second" : " Seconds") + " Timer";
		}

		public override Sprite Image
		{
			get { return sprite; }
		}

		public override Sprite SubtypeImage(byte subtype)
		{
			return sprite;
		}

		public override Sprite GetSprite(ObjectEntry obj)
		{
			return sprite;
		}
		
		public override Sprite GetDebugOverlay(ObjectEntry obj)
		{
			return debug;
		}
	}
}