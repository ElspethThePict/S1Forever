using SonicRetro.SonLVL.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1ObjectDefinitions.SBZ
{
	class GirderDoor : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[1];
		private Sprite sprite;
		private Sprite[] debug = new Sprite[2];
		
		public override void Init(ObjectData data)
		{
			sprite = new Sprite(LevelData.GetSpriteSheet("SBZ/Objects.gif").GetSection(383, 140, 128, 24), -64, -12);
			
			// tagging this area with LevelData.ColorWhite
			BitmapBits bitmap = new BitmapBits(193, 25);
			bitmap.DrawRectangle(6, 0, 0, 128, 24); // Object frame
			bitmap.DrawLine(6, 64, 12, 192, 12); // Movement line
			debug[1] = new Sprite(bitmap, -192, -12); // Moving left ver
			debug[0] = new Sprite(debug[1], true, false); // Moving right ver
			
			properties[0] = new PropertySpec("Behaviour", typeof(int), "Extended",
				"How this Girder should behave upon button[-1] being pressed.", null, new Dictionary<string, int>
				{
					{ "No Movement", 0 },
					{ "Move Right", 1 },
					{ "Move Left", 2 }
				},
				(obj) => (obj.PropertyValue == 1 || obj.PropertyValue == 2) ? obj.PropertyValue : 0,
				(obj, value) => obj.PropertyValue = (byte)((int)value));
		}
		
		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {0, 1, 2}); }
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
				case 0: return "No Movement";
				case 1: return "Move Right";
				case 2: return "Move Left";
			}
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
			if (obj.PropertyValue == 1 || obj.PropertyValue == 2) return debug[obj.PropertyValue - 1];
			return null;
		}
	}
}