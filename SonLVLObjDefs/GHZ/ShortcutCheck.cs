using SonicRetro.SonLVL.API;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1FObjectDefinitions.GHZ
{
	class ShortcutCheck : ObjectDefinition
	{
		private Sprite[] sprites = new Sprite[2];
		
		public override void Init(ObjectData data)
		{
			sprites[0] = new Sprite(LevelData.GetSpriteSheet("Global/Display.gif").GetSection(239, 239, 16, 16), -8, -8);
			
			// this looks so goofy lol-
			BitmapBits bitmap = new BitmapBits(48, 304);
			bitmap.DrawRectangle(6, 0, 0, 47, 303); // LevelData.ColorWhite
			sprites[1] = new Sprite(sprites[0], new Sprite(bitmap, -16, -240));
		}
		
		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[0]); }
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
			return sprites[1];
		}
	}
}