using SonicRetro.SonLVL.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1ObjectDefinitions.SBZ
{
	class Piston : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[1];
		private Sprite sprite;
		private Sprite[] debug = new Sprite[3];
		
		public override void Init(ObjectData data)
		{
			sprite = new Sprite(LevelData.GetSpriteSheet("SBZ/Objects.gif").GetSection(132, 1, 56, 64), -28, -32);
			
			// tagging this area with LevelData.ColorWhite
			
			// copy of Piston_moveDistances from the orignal script
			int[] moveDistances = {0x380000, 0x400000, 0x600000};
			
			for (int i = 0; i < 3; i++)
			{
				int distance = moveDistances[i] >> 16;
				
				BitmapBits bitmap = new BitmapBits(57, distance + 33);
				bitmap.DrawLine(6, 28, 0, 28, distance); // Movement line
				bitmap.DrawRectangle(6, 0, distance-32, 56, 64); // Object frame
				debug[i] = new Sprite(bitmap, -28, 0);
			}
			
			properties[0] = new PropertySpec("Distance", typeof(int), "Extended",
				"How far this Piston should extend.", null, new Dictionary<string, int>
				{
					{ "56 Pixels", 0 },
					{ "64 Pixles", 1 },
					{ "96 Pixels", 2 }
				},
				(obj) => obj.PropertyValue % 3, // the game doesn't enforce a limit but there's only 3 entries in the table so let's enforce it ourselves
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
				case 0: return "Travel 56 Pixels";
				case 1: return "Travel 64 Pixels";
				case 2: return "Travel 96 Pixels";
				
				default: return "Unknown";
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
			return (obj.PropertyValue > 2) ? null : debug[obj.PropertyValue % 3];
		}
	}
}