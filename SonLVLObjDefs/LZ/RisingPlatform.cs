using SonicRetro.SonLVL.API;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1ObjectDefinitions.LZ
{
	class RisingPlatform : ObjectDefinition
	{
		private Sprite sprite;
		private Sprite debug;
		
		public override void Init(ObjectData data)
		{
			sprite = new Sprite(LevelData.GetSpriteSheet("LZ/Objects.gif").GetSection(126, 137, 64, 24), -32, -12);
			
			// tagging this area with LevelData.ColorWhite
			BitmapBits bitmap = new BitmapBits(1, 39);
			for (int i = 0; i < bitmap.Height; i += 8)
				bitmap.DrawLine(6, 0, i, 0, i + 3);
			
			debug = new Sprite(bitmap, 0, -bitmap.Height);
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
		
		public override Sprite GetDebugOverlay(ObjectEntry obj)
		{
			return debug;
		}
	}
}