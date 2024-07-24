using SonicRetro.SonLVL.API;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1FObjectDefinitions.Enemies
{
	class Splats : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[2];
		private Sprite[] sprites = new Sprite[2];
		
		public override void Init(ObjectData data)
		{
			if (LevelData.StageInfo.folder.EndsWith("BossRush"))
				sprites[0] = new Sprite(LevelData.GetSpriteSheet("MBZ/Objects.gif").GetSection(220, 254, 21, 40), -11, -15);
			else if (LevelData.StageInfo.folder.EndsWith("Zone00"))
				sprites[0] = new Sprite(LevelData.GetSpriteSheet("GHZ/Objects3.gif").GetSection(220, 254, 21, 40), -11, -15);
			else
				sprites[0] = new Sprite(LevelData.GetSpriteSheet("GHZ/Objects.gif").GetSection(214, 211, 21, 40), -11, -15);
			
			sprites[1] = new Sprite(sprites[0], true, false);
			
			properties[0] = new PropertySpec("Direction", typeof(int), "Extended",
				"Which way the Splats will be facing intially.", null, new Dictionary<string, int>
				{
					{ "Left", 0 },
					{ "Right", 1 }
				},
				(obj) => obj.PropertyValue & 1,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~1) | (int)value));
			
			properties[1] = new PropertySpec("Behaviour", typeof(int), "Extended",
				"How the Splats will act.", null, new Dictionary<string, int>
				{
					{ "Advance", 0 },
					{ "Hover", 2 }
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
			string result = ((subtype & 2) == 2) ? "Hover" : "Advance";
			result += ((subtype & 1) == 1) ? " (Facing Right)" : " (Facing Left)";
			return result;
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