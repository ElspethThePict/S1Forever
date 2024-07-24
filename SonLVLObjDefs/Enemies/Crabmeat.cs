using SonicRetro.SonLVL.API;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1FObjectDefinitions.Enemies
{
	class Crabmeat : ObjectDefinition
	{
		private Sprite sprite;
		private PropertySpec[] properties = new PropertySpec[1];

		public override void Init(ObjectData data)
		{
			switch (LevelData.StageInfo.folder[LevelData.StageInfo.folder.Length-1])
			{
				case '1':
				default:
					sprite = new Sprite(LevelData.GetSpriteSheet("GHZ/Objects.gif").GetSection(138, 157, 42, 31), -21, -16);
					break;
				case '3':
					sprite = new Sprite(LevelData.GetSpriteSheet("SYZ/Objects.gif").GetSection(184, 1, 42, 31), -21, -16);
					break;
				case 'h': // "BossRush" (this works but yeah it's dumb, sorry-)
					sprite = new Sprite(LevelData.GetSpriteSheet("MBZ/Objects.gif").GetSection(168, 81, 42, 31), -21, -16);
					break;
				case '0':
					sprite = new Sprite(LevelData.GetSpriteSheet("GHZ/Objects3.gif").GetSection(168, 81, 42, 31), -21, -16);
					break;
			}
			
			sprite.Flip(true, false);
			
			properties[0] = new PropertySpec("Direction", typeof(int), "Extended",
				"Which way the Crabmeat is facing.", null, new Dictionary<string, int>
				{
					{ "Left", 0 },
					{ "Right", 1 }
				},
				(obj) => (obj.PropertyValue == 1) ? 1 : 0,
				(obj, value) => obj.PropertyValue = ((byte)((int)value)));
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
			return (subtype == 1) ? "Facing Right" : "Facing Left";
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
	}
}