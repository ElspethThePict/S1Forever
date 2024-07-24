using SonicRetro.SonLVL.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1ObjectDefinitions.GHZ
{
	class CLedgeLeft : GHZ.CLedge
	{
		public override Sprite GetFrame()
		{
			return new Sprite(LevelData.GetSpriteSheet("GHZ/Objects.gif").GetSection(1, 51, 96, 96), -48, -64);
		}
	}
	
	class CLedgeRight : GHZ.CLedge
	{
		public override Sprite GetFrame()
		{
			return new Sprite(LevelData.GetSpriteSheet("GHZ/Objects.gif").GetSection(1, 148, 96, 96), -48, -64);
		}
	}

	abstract class CLedge : ObjectDefinition
	{
		private Sprite sprite;
		private Sprite debug;
		
		public abstract Sprite GetFrame();
		
		public override void Init(ObjectData data)
		{
			sprite = GetFrame();
			
			/*
			// ig?
			BitmapBits bitmap = new BitmapBits(96, 96);
			bitmap.DrawRectangle(6, 0, 0, 95, 95); // LevelData.ColorWhite
			debug = new Sprite(bitmap, -48, -64);
			*/
			
			// adapt the rectangle to fit around the sprite (cause half the sprite from the sheet is blank)
			Rectangle bounds = sprite.Bounds;
			BitmapBits overlay = new BitmapBits(bounds.Size);
			overlay.DrawRectangle(6, 0, 0, bounds.Width - 1, bounds.Height - 1); // LevelData.ColorWhite
			debug = new Sprite(overlay, bounds.X, bounds.Y);
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