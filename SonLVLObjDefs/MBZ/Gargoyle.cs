using SonicRetro.SonLVL.API;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1FObjectDefinitions.MBZ
{
	class Gargoyle : ObjectDefinition
	{
		private Sprite[] sprites = new Sprite[2];
		private PropertySpec[] properties = new PropertySpec[1];

		public override void Init(ObjectData data)
		{
			BitmapBits sheet = LevelData.GetSpriteSheet("MBZ/Objects.gif");
			sprites[0] = new Sprite(new Sprite(sheet.GetSection(1, 438, 16, 32), 0, -16), new Sprite(sheet.GetSection(1, 423, 12, 12), -5, -6));
			sprites[1] = new Sprite(new Sprite(sheet.GetSection(18, 438, 16, 32), -16, -16), new Sprite(sheet.GetSection(29, 393, 12, 12), -7, -6));
			
			properties[0] = new PropertySpec("Direction", typeof(int), "Extended",
				"Which way the Gargoyle is pointing.", null, new Dictionary<string, int>
				{
					{ "Left", 0 },
					{ "Right", 1 }
				},
				(obj) => (int)obj.PropertyValue,
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
			return sprites[subtype];
		}

		public override Sprite GetSprite(ObjectEntry obj)
		{
			return sprites[(obj.PropertyValue == 0) ? 0 : 1];
		}
	}
}