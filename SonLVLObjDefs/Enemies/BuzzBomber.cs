using SonicRetro.SonLVL.API;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1FObjectDefinitions.Enemies
{
	class BuzzBomber : ObjectDefinition
	{
		private readonly Sprite[] sprites = new Sprite[2];
		private PropertySpec[] properties = new PropertySpec[3];

		public override void Init(ObjectData data)
		{
			Sprite[] frames = new Sprite[2];
			switch (LevelData.StageInfo.folder[LevelData.StageInfo.folder.Length-1])
			{
				case '1':
				default:
					BitmapBits sheet = LevelData.GetSpriteSheet("GHZ/Objects.gif");
					frames[0] = new Sprite(sheet.GetSection(98, 74, 45, 19), -23, -9);
					frames[1] = new Sprite(sheet.GetSection(144, 79, 35, 8), -17, -15);
					break;
				case '2':
					sheet = LevelData.GetSpriteSheet("MZ/Objects.gif");
					frames[0] = new Sprite(sheet.GetSection(1, 127, 45, 19), -23, -9);
					frames[1] = new Sprite(sheet.GetSection(38, 147, 35, 8), -17, -15);
					break;
				case '3':
					sheet = LevelData.GetSpriteSheet("SYZ/Objects.gif");
					frames[0] = new Sprite(sheet.GetSection(1, 81, 45, 19), -23, -9);
					frames[1] = new Sprite(sheet.GetSection(38, 101, 35, 8), -17, -15);
					break;
				case 'h': // "BossRush" (this works but yeah it's dumb, sorry-)
					sheet = LevelData.GetSpriteSheet("MBZ/Objects.gif");
					frames[0] = new Sprite(sheet.GetSection(1, 1, 45, 19), -23, -9);
					frames[1] = new Sprite(sheet.GetSection(38, 21, 35, 8), -17, -15);
					break;
				case '0':
					sheet = LevelData.GetSpriteSheet("GHZ/Objects3.gif");
					frames[0] = new Sprite(sheet.GetSection(1, 1, 45, 19), -23, -9);
					frames[1] = new Sprite(sheet.GetSection(38, 21, 35, 8), -17, -15);
					break;
			}
			
			sprites[0] = new Sprite(frames);
			sprites[1] = new Sprite(sprites[0], true, false);
			
			properties[0] = new PropertySpec("Direction", typeof(int), "Extended",
				"Which way the Buzz Bomber is facing.", null, new Dictionary<string, int>
				{
					{ "Left", 0 },
					{ "Right", 1 }
				},
				(obj) => obj.PropertyValue & 1,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~1) | (int)value));
			
			properties[1] = new PropertySpec("Range", typeof(int), "Extended",
				"The range of the Buzz Bomber's activation trigger.", null, new Dictionary<string, int>
				{
					{ "Large", 0 },
					{ "Small", 2 }
				},
				(obj) => obj.PropertyValue & 2,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~2) | (int)value));
			
			properties[2] = new PropertySpec("Hide On Off Screen", typeof(bool), "Extended",
				"If this Buzz Bomber should hide after going off screen.", null,
				(obj) => (((V4ObjectEntry)obj).Value3 == 1),
				(obj, value) => ((V4ObjectEntry)obj).Value3 = ((bool)value ? 1 : 0));
		}

		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {0, 1, 2, 3}); }
		}
		
		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
		}

		public override string SubtypeName(byte subtype)
		{
			switch (subtype & 3)
			{
				default:
				case 0: return "Facing Left (Large Range)";
				case 1: return "Facing Right (Large Range)";
				case 2: return "Facing Left (Small Range)";
				case 3: return "Facing Right (Small Range)";
			}
		}

		public override Sprite Image
		{
			get { return sprites[0]; }
		}

		public override Sprite SubtypeImage(byte subtype)
		{
			return sprites[subtype & 1];
		}

		public override Sprite GetSprite(ObjectEntry obj)
		{
			return sprites[obj.PropertyValue & 1];
		}
	}
}