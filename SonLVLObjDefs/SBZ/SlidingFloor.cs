using SonicRetro.SonLVL.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1ObjectDefinitions.SBZ
{
	class SlidingFloor : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[1];
		private Sprite sprite;
		private Sprite[] debug = new Sprite[2];
		
		public override void Init(ObjectData data)
		{
			sprite = new Sprite(LevelData.GetSpriteSheet("SBZ/Objects.gif").GetSection(383, 115, 128, 24), -64, -12);
			
			// tagging this area with LevelData.ColorWhite
			BitmapBits bitmap = new BitmapBits(193, 25);
			bitmap.DrawRectangle(6, 0, 0, 128, 24); // Object frame
			bitmap.DrawLine(6, 64, 12, 192, 12); // Movement line
			debug[1] = new Sprite(bitmap, -192, -12); // Moving left ver
			debug[0] = new Sprite(debug[1], true, false); // Moving right ver
			
			properties[0] = new PropertySpec("Behaviour", typeof(int), "Extended",
				"Which direction this Sliding Floor should move after player interaction.", null, new Dictionary<string, int>
				{
					{ "Move Right", 0 },
					{ "Move Left", 1 }
				},
				(obj) => obj.PropertyValue & 1,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~1) | ((int)value)));
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
			return (((subtype & 1) == 0) ? "Move Right" : "Move Left");
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
			return debug[obj.PropertyValue & 1];
		}
	}
}