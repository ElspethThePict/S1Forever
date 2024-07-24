using SonicRetro.SonLVL.API;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1ObjectDefinitions.SLZ
{
	class RotatingStair : ObjectDefinition
	{
		private Sprite sprite;
		private Sprite debug;
		
		public override void Init(ObjectData data)
		{
			Sprite frame = new Sprite(LevelData.GetSpriteSheet("SLZ/Objects.gif").GetSection(67, 26, 32, 32), -16, -16);
			
			List<Sprite> sprs = new List<Sprite>();
			for (int i = 0; i < 8; i++)
				sprs.Add(new Sprite(frame, -112 + (i * 32), -112 + (i * 32)));
			
			sprite = new Sprite(sprs.ToArray());
			
			BitmapBits bitmap = new BitmapBits(257, 257);
			for (int i = 0; i < 8; i++)
				bitmap.DrawRectangle(6, (i * 32), (i * 32), 31, 31); // LevelData.ColorWhite
			bitmap.Flip(true, false);
			debug = new Sprite(bitmap, -128, -128);
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