using SonicRetro.SonLVL.API;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1FObjectDefinitions.Enemies
{
	class Caterkiller : ObjectDefinition
	{
		private readonly Sprite[] sprites = new Sprite[2];
		private PropertySpec[] properties = new PropertySpec[1];

		public override void Init(ObjectData data)
		{
			Sprite[] frames = new Sprite[3];
			switch (LevelData.StageInfo.folder[LevelData.StageInfo.folder.Length-1])
			{
				case '2':
				default:
					BitmapBits sheet = LevelData.GetSpriteSheet("MZ/Objects.gif");
					frames[0] = new Sprite(sheet.GetSection(18, 81, 16, 22), -8, -14);
					frames[1] = new Sprite(sheet.GetSection(1, 81, 16, 24), -8, -14);
					frames[2] = new Sprite(sheet.GetSection(35, 81, 16, 16), -8, -8);
					break;
				case '3':
					sheet = LevelData.GetSpriteSheet("SYZ/Objects.gif");
					frames[0] = new Sprite(sheet.GetSection(98, 98, 16, 22), -8, -14);
					frames[1] = new Sprite(sheet.GetSection(81, 98, 16, 24), -8, -14);
					frames[2] = new Sprite(sheet.GetSection(98, 121, 16, 16), -8, -8);
					break;
				case '6':
					sheet = LevelData.GetSpriteSheet("SBZ/Objects.gif");
					frames[0] = new Sprite(sheet.GetSection(75, 26, 16, 22), -8, -14);
					frames[1] = new Sprite(sheet.GetSection(75, 1, 16, 24), -8, -14);
					frames[2] = new Sprite(sheet.GetSection(75, 49, 16, 16), -8, -8);
					break;
				case 'h': // "BossRush" (this works but yeah it's dumb, sorry-)
					sheet = LevelData.GetSpriteSheet("MBZ/Objects.gif");
					frames[0] = new Sprite(sheet.GetSection(1, 68, 16, 22), -8, -14);
					frames[1] = new Sprite(sheet.GetSection(18, 68, 16, 24), -8, -14);
					frames[2] = new Sprite(sheet.GetSection(35, 68, 16, 16), -8, -8);
					break;
				case '0':
					sheet = LevelData.GetSpriteSheet("GHZ/Objects3.gif");
					frames[0] = new Sprite(sheet.GetSection(1, 68, 16, 22), -8, -14);
					frames[1] = new Sprite(sheet.GetSection(18, 68, 16, 24), -8, -14);
					frames[2] = new Sprite(sheet.GetSection(35, 68, 16, 16), -8, -8);
					break;
			}
			
			for (int s = 0; s < 2; s++)
			{
				List<Sprite> sprs = new List<Sprite>();
				bool flip = (s != 0);
				
				for (int i = 4; i > 0; i--)
				{
					Sprite tmp = new Sprite(frames[(i == 1) ? 0 : 2], flip, false);
					tmp.Offset(((i - 1) * 12) * (flip ? (-1) : (1)), 0);
					sprs.Add(tmp);
				}
				
				sprites[s] = new Sprite(sprs.ToArray());
			}
			
			properties[0] = new PropertySpec("Direction", typeof(int), "Extended",
				"Which way the Caterkiller is facing.", null, new Dictionary<string, int>
				{
					{ "Left", 0 },
					{ "Right", 1 }
				},
				(obj) => (obj.PropertyValue == 1) ? 1 : 0,
				(obj, value) => obj.PropertyValue = (byte)((int)value));
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
			return (subtype == 0) ? "Facing Left" : "Facing Right";
		}

		public override Sprite Image
		{
			get { return sprites[0]; }
		}

		public override Sprite SubtypeImage(byte subtype)
		{
			return sprites[(subtype == 1) ? 1 : 0];
		}

		public override Sprite GetSprite(ObjectEntry obj)
		{
			return sprites[(obj.PropertyValue == 1) ? 1 : 0];
		}
	}
}