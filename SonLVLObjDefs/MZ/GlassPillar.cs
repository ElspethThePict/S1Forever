using SonicRetro.SonLVL.API;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1ObjectDefinitions.MZ
{
	class GlassPillar : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[2];
		private Sprite[,] sprites = new Sprite[2,2]; // frame, direction
		private Sprite[,] debug = new Sprite[2,2]; // frame, direction
		
		public override void Init(ObjectData data)
		{
			BitmapBits sheet = LevelData.GetSpriteSheet("MZ/Objects.gif");
			Sprite shine = new Sprite(sheet.GetSection(159, 114, 31, 32), -16, -16);
			
			sprites[0,0] = new Sprite(new Sprite(sheet.GetSection(191, 1, 64, 144), -32, -72), shine);
			sprites[1,0] = new Sprite(new Sprite(sheet.GetSection(126, 1, 64, 112), -32, -56), shine);
			
			sprites[0,1] = new Sprite(sprites[0,0], 0, -64);
			sprites[1,1] = new Sprite(sprites[1,0], 0, -64);
			
			// tagging this area with LevelData.ColorWhite
			BitmapBits bitmap = new BitmapBits(65, 145);
			bitmap.DrawRectangle(6, 0, 0, 63, 143); // top box
			bitmap.DrawLine(6, 32, 72, 32, 136); // movement line
			debug[0,0] = new Sprite(bitmap, -32, -136);
			
			bitmap = new BitmapBits(65, 113);
			bitmap.DrawRectangle(6, 0, 0, 63, 111); // top box
			bitmap.DrawLine(6, 32, 56, 32, 120); // movement line
			debug[1,0] = new Sprite(bitmap, -32, -120);
			
			debug[0,1] = new Sprite(debug[0,0], 0, -64, false, true);
			debug[1,1] = new Sprite(debug[1,0], 0, -64, false, true);
			
			properties[0] = new PropertySpec("Frame", typeof(int), "Extended",
				"What sprite this Pillar should display.", null, new Dictionary<string, int>
				{
					{ "Long", 0 },
					{ "Medium", 0x10 }
				},
				(obj) => obj.PropertyValue & 0x10,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~0x10) | (int)value));
			
			properties[1] = new PropertySpec("Movement", typeof(bool), "Extended",
				"What pattern this Pillar's movement should follow.", null, new Dictionary<string, int>
				{
					{ "Static", 0 },
					{ "Vertical", 1 },
					{ "Vertical (Reverse)", 2 }
				},
				(obj) => ((obj.PropertyValue & 7) > 5) ? 0 : (obj.PropertyValue & 7), // where'd that 5 come from? yeah, i'm wondering the same thing too..
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~7) | (int)value));
		}
		
		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {0, 1, 2, 0x10, 0x11, 0x12}); }
		}
		
		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
		}

		public override string SubtypeName(byte subtype)
		{
			string[] styles = {"Static", "Vertical", "Vertical (Reverse)"};
			string name = styles[((subtype & 7) > 5) ? 0 : (subtype & 7)];
			name += ((subtype & 0x10) == 0x10) ? " (Medium)" : " (Long)";
			return name;
		}

		public override Sprite Image
		{
			get { return sprites[0,0]; }
		}

		public override Sprite SubtypeImage(byte subtype)
		{
			return sprites[(subtype & 0x10) >> 4,0];
		}

		public override Sprite GetSprite(ObjectEntry obj)
		{
			return sprites[(obj.PropertyValue & 0x10) >> 4,((obj.PropertyValue & 7) == 2) ? 1 : 0];
		}
		
		public override Sprite GetDebugOverlay(ObjectEntry obj)
		{
			return debug[(obj.PropertyValue & 0x10) >> 4,((obj.PropertyValue & 7) == 2) ? 1 : 0];
		}
	}
}