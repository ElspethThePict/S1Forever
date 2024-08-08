using SonicRetro.SonLVL.API;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1FObjectDefinitions.Enemies
{
	class Yadrin : ObjectDefinition
	{
		private Sprite[] sprites = new Sprite[2];
		private PropertySpec[] properties = new PropertySpec[1];

		public override void Init(ObjectData data)
		{
			switch (LevelData.StageInfo.folder[LevelData.StageInfo.folder.Length-1])
			{
				case '2':
				default:
					sprites[0] = new Sprite(LevelData.GetSpriteSheet("MZ/Objects.gif").GetSection(1, 2, 40, 38), -20, -20);
					break;
				case '3':
					sprites[0] = new Sprite(LevelData.GetSpriteSheet("SYZ/Objects.gif").GetSection(1, 2, 40, 38), -20, -20);
					break;
				case 'h': // "BossRush" (this works but yeah it's dumb, sorry-)
					sprites[0] = new Sprite(LevelData.GetSpriteSheet("MBZ/Objects.gif").GetSection(138, 2, 40, 38), -20, -20);
					break;
				case '0':
					sprites[0] = new Sprite(LevelData.GetSpriteSheet("GHZ/Objects3.gif").GetSection(138, 2, 40, 38), -20, -20);
					break;
			}
			
			sprites[1] = new Sprite(sprites[0], true, false);
			
			properties[0] = new PropertySpec("Direction", typeof(int), "Extended",
				"Which way the Yadrin is facing.", null, new Dictionary<string, int>
				{
					{ "Left", 0 },
					{ "Right", 1 }
				},
				(obj) => (obj.PropertyValue == 0) ? 0 : 1,
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
			return sprites[(subtype == 0) ? 0 : 1];
		}

		public override Sprite GetSprite(ObjectEntry obj)
		{
			return sprites[(obj.PropertyValue == 0) ? 0 : 1];
		}
	}
}