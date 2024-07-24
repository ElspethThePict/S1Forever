using SonicRetro.SonLVL.API;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1FObjectDefinitions.Enemies
{
	class Bomb : ObjectDefinition
	{
		private Sprite[] sprites = new Sprite[4];
		private PropertySpec[] properties = new PropertySpec[2];

		public override void Init(ObjectData data)
		{
			switch (LevelData.StageInfo.folder[LevelData.StageInfo.folder.Length-1])
			{
				case '5':
				default:
					sprites[0] = new Sprite(LevelData.GetSpriteSheet("SLZ/Objects.gif").GetSection(53, 131, 20, 37), -10, -21);
					break;
				case '6':
					sprites[0] = new Sprite(LevelData.GetSpriteSheet("SBZ/Objects.gif").GetSection(52, 40, 20, 37), -10, -21);
					break;
				case 'h': // "BossRush" (this works but yeah it's dumb, sorry-)
					sprites[0] = new Sprite(LevelData.GetSpriteSheet("MBZ/Objects.gif").GetSection(53, 328, 20, 37), -10, -21);
					break;
				case '0':
					sprites[0] = new Sprite(LevelData.GetSpriteSheet("GHZ/Objects3.gif").GetSection(53, 328, 20, 37), -10, -21);
					break;
			}
			
			sprites[1] = new Sprite(sprites[0], true, false);
			sprites[2] = new Sprite(sprites[0], false, true);
			sprites[3] = new Sprite(sprites[0], true, true);
			
			properties[0] = new PropertySpec("Direction", typeof(int), "Extended",
				"Which way the Bomb is facing.", null, new Dictionary<string, int>
				{
					{ "Left", 0 },
					{ "Right", 1 }
				},
				(obj) => obj.PropertyValue & 1,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~1) | (int)value));
			
			properties[1] = new PropertySpec("On Roof", typeof(int), "Extended",
				"If the Bomb is on a roof or not.", null, new Dictionary<string, int>
				{
					{ "False", 0 },
					{ "True", 2 }
				},
				(obj) => obj.PropertyValue & 2,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~2) | (int)value));
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
			switch (subtype)
			{
				case 0: return "Left";
				case 1: return "Right";
				case 2: return "Left - Roof";
				case 3: return "Right - Roof";
				default: return "Unknown";
			}
		}

		public override Sprite Image
		{
			get { return sprites[0]; }
		}

		public override Sprite SubtypeImage(byte subtype)
		{
			return sprites[subtype & 3];
		}

		public override Sprite GetSprite(ObjectEntry obj)
		{
			return sprites[obj.PropertyValue & 3];
		}
	}
}