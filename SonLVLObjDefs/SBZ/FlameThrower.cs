using SonicRetro.SonLVL.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1ObjectDefinitions.SBZ
{
	class FlameThrower : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[3];
		private Sprite[] sprites = new Sprite[4];
		
		public override void Init(ObjectData data)
		{
			BitmapBits sheet = LevelData.GetSpriteSheet("SBZ/Objects.gif");
			sprites[0] = new Sprite(sheet.GetSection(276, 397, 22, 79), -11, -23); // No Flip
			sprites[1] = new Sprite(sprites[0], true, false); // Flip X
			
			sprites[2] = new Sprite(sheet.GetSection(489, 397, 22, 79), -11, -23); // Flip Y
			sprites[2].Flip(false, true);
			sprites[3] = new Sprite(sprites[2], true, false); // Flip XY
			
			properties[0] = new PropertySpec("Off Time", typeof(int), "Extended",
				"How long this Flame Thrower should be off for.", null, new Dictionary<string, int>
				{
					{ "0 Frames", 0 },
					{ "32 Frames", 1 },
					{ "64 Frames", 2 },
					{ "96 Frames", 3 },
					{ "128 Frames", 4 },
					{ "160 Frames", 5 },
					{ "192 Frames", 6 },
					{ "224 Frames", 7 },
					{ "256 Frames", 8 },
					{ "288 Frames", 9 },
					{ "320 Frames", 10 },
					{ "352 Frames", 11 },
					{ "384 Frames", 12 },
					{ "416 Frames", 13 },
					{ "448 Frames", 14 },
					{ "480 Frames", 15 },
				},
				(obj) => obj.PropertyValue & 0x0F,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~0x0F) | ((int)value)));
			
			properties[1] = new PropertySpec("On Time", typeof(int), "Extended",
				"How long this Flame Thrower should be on for.", null, new Dictionary<string, int>
				{
					{ "0 Frames", 0 },
					{ "32 Frames", 1 },
					{ "64 Frames", 2 },
					{ "96 Frames", 3 },
					{ "128 Frames", 4 },
					{ "160 Frames", 5 },
					{ "192 Frames", 6 },
					{ "224 Frames", 7 },
					{ "256 Frames", 8 },
					{ "288 Frames", 9 },
					{ "320 Frames", 10 },
					{ "352 Frames", 11 },
					{ "384 Frames", 12 },
					{ "416 Frames", 13 },
					{ "448 Frames", 14 },
					{ "480 Frames", 15 },
				},
				(obj) => (obj.PropertyValue & 0xF0) >> 4,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~0xF0) | (((int)value) << 4)));
			
			// You can set Flip X if you want to (some Flame Throwers in the scene do that too) but it doesn't look like there's much reason to?
			
			properties[2] = new PropertySpec("Direction", typeof(int), "Extended",
				"Which direction this Flame Thrower is facing. Horizontal flips are visual, hitbox only differs between vertical flips.", null, new Dictionary<string, int>
				{
					{ "Up, Left Lean", 0 },
					{ "Up, Right Lean", 1 },
					{ "Down, Left Lean", 2 },
					{ "Down, Right Lean", 3 },
				},
				(obj) => (int)(((V4ObjectEntry)obj).Direction),
				(obj, value) => ((V4ObjectEntry)obj).Direction = (RSDKv3_4.Tiles128x128.Block.Tile.Directions)value);
		}
		
		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[0]); }
		}
		
		public override byte DefaultSubtype
		{
			get { return 0x43; }
		}
		
		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
		}

		public override string SubtypeName(byte subtype)
		{
			return null;
		}

		public override Sprite Image
		{
			get { return sprites[0]; }
		}

		public override Sprite SubtypeImage(byte subtype)
		{
			return sprites[0];
		}

		public override Sprite GetSprite(ObjectEntry obj)
		{
			return sprites[(int)(((V4ObjectEntry)obj).Direction)];
		}
	}
}