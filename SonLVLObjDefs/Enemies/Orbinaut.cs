using SonicRetro.SonLVL.API;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1FObjectDefinitions.Enemies
{
	class Orbinaut : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[2];
		private readonly Sprite[] sprites = new Sprite[2];
		
		public override void Init(ObjectData data)
		{
			Sprite[] frames = new Sprite[2];
			switch (LevelData.StageInfo.folder[LevelData.StageInfo.folder.Length-1])
			{
				case '4':
				default:
					BitmapBits sheet = LevelData.GetSpriteSheet("LZ/Objects.gif");
					frames[0] = new Sprite(sheet.GetSection(50, 105, 20, 20), -10, -10);
					frames[1] = new Sprite(sheet.GetSection(107, 1, 16, 16), -8, -8);
					break;
				case '5':
					sheet = LevelData.GetSpriteSheet("SLZ/Objects.gif");
					frames[0] = new Sprite(sheet.GetSection(51, 1, 20, 20), -10, -10);
					frames[1] = new Sprite(sheet.GetSection(114, 1, 16, 16), -8, -8);
					break;
				case '6':
					sheet = LevelData.GetSpriteSheet("SBZ/Objects.gif");
					frames[0] = new Sprite(sheet.GetSection(1, 138, 20, 20), -10, -10);
					frames[1] = new Sprite(sheet.GetSection(64, 142, 16, 16), -8, -8);
					break;
				case 'h': // "BossRush" (this works but yeah it's dumb, sorry-)
					sheet = LevelData.GetSpriteSheet("MBZ/Objects.gif");
					frames[0] = new Sprite(sheet.GetSection(119, 114, 20, 20), -10, -10);
					frames[1] = new Sprite(sheet.GetSection(140, 135, 16, 16), -8, -8);
					break;
				case '0':
					sheet = LevelData.GetSpriteSheet("GHZ/Objects3.gif");
					frames[0] = new Sprite(sheet.GetSection(119, 114, 20, 20), -10, -10);
					frames[1] = new Sprite(sheet.GetSection(140, 135, 16, 16), -8, -8);
					break;
			}
			
			// Orb circle
			frames[1] =  new Sprite(new Sprite(frames[1], -18, 0),
									new Sprite(frames[1], 0, -18),
									new Sprite(frames[1],  18, 0),
									new Sprite(frames[1], 0,  18));
			
			sprites[0] = new Sprite(frames[0], frames[1]);
			
			frames[0].Flip(true, false);
			sprites[1] = new Sprite(frames[0], frames[1]);
			
			properties[0] = new PropertySpec("Direction", typeof(int), "Extended",
				"Which way the Orbinaut is facing.", null, new Dictionary<string, int>
				{
					{ "Left", 0 },
					{ "Right", 1 }
				},
				(obj) => obj.PropertyValue & 1,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~1) | (int)value));
			
			properties[1] = new PropertySpec("Fire Orbs", typeof(int), "Extended",
				"If the Orbinaut should fire its orbs or not.", null, new Dictionary<string, int>
				{
					{ "True", 0 }, // just a little something
					{ "False", 2 }
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
			switch (subtype & 3)
			{
				default:
				case 0: return "Facing Left (Fire Orbs)";
				case 1: return "Facing Right (Fire Orbs)";
				case 2: return "Facing Left (Keep Orbs)";
				case 3: return "Facing Right (Keep Orbs)";
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