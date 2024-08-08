using SonicRetro.SonLVL.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1ObjectDefinitions.LZ
{
	class SlidingFloor : ObjectDefinition
	{
		private Sprite sprite;
		private Sprite debug;
		
		public override void Init(ObjectData data)
		{
			sprite = new Sprite(LevelData.GetSpriteSheet("LZ/Objects2.gif").GetSection(0, 0, 256, 125), -128, -64);
			
			Rectangle bounds = sprite.Bounds;
			BitmapBits bitmap = new BitmapBits(bounds.Size);
			bitmap.DrawRectangle(6, 0, 0, bounds.Width - 1, bounds.Height - 1);
			debug = new Sprite(bitmap, bounds.X - 256, bounds.Y + 128);
			
			bitmap = new BitmapBits(257, 129);
			bitmap.DrawLine(6, 0, 128, 256, 0);
			debug = new Sprite(debug, new Sprite(bitmap, -256, 0));
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