using SonicRetro.SonLVL.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1ObjectDefinitions.SBZ
{
	class HCrushCover : SBZ.DecoSprite
	{
		public override Dictionary<string, int> names { get { return new Dictionary<string, int>{{ "Wall Frame", 0 }, { "Roof Frame", 1 }}; } }
		
		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {0, 1}); }
		}
		
		public override Sprite[] GetFrames()
		{
			BitmapBits sheet = LevelData.GetSpriteSheet("SBZ/Objects.gif");
			Sprite[] sprites = new Sprite[2];
			sprites[0] = new Sprite(sheet.GetSection(107, 62, 16, 96), -8, -48);
			sprites[1] = new Sprite(sheet.GetSection(50, 261, 32, 32), -16, -16);
			return sprites;
		}
	}
	
	class TransSprite : SBZ.DecoSprite
	{
		public override Dictionary<string, int> names { get { return new Dictionary<string, int>{{ "Left Frame", 0 }, { "Right Frame", 1 }}; } }
		
		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {0, 1}); }
		}
		
		public override Sprite[] GetFrames()
		{
			BitmapBits sheet = LevelData.GetSpriteSheet("SBZ/Objects.gif");
			Sprite[] sprites = new Sprite[2];
			sprites[0] = new Sprite(sheet.GetSection(268, 274, 56, 64), -28, -32);
			sprites[1] = new Sprite(sheet.GetSection(325, 274, 56, 64), -28, -32);
			return sprites;
		}
	}
	
	class TubeCover : SBZ.DecoSprite
	{
		public override Sprite[] GetFrames()
		{
			// well yeah it's technically two frames in-game but the tube frame completely covers the device frame, so..
			return new Sprite[] { new Sprite(LevelData.GetSpriteSheet("SBZ/Objects.gif").GetSection(317, 339, 64, 64), -32, -32) };
		}
	}
	
	class RoofCover : SBZ.DecoSprite
	{
		public override Dictionary<string, int> names { get { return new Dictionary<string, int>{{ "Left Frame", 0 }, { "Right Frame", 1 }}; } }
		
		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {0, 1}); }
		}
		
		public override Sprite[] GetFrames()
		{
			BitmapBits sheet = LevelData.GetSpriteSheet("SBZ/Objects.gif");
			Sprite[] sprites = new Sprite[2];
			sprites[0] = new Sprite(sheet.GetSection(2, 388, 128, 8), -64, -4);
			sprites[1] = new Sprite(sheet.GetSection(130, 388, 128, 8), -64, -4);
			return sprites;
		}
	}
	
	abstract class DecoSprite : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[1];
		private Sprite[] sprites;
		private Sprite[] debug;
		
		public virtual Dictionary<string, int> names { get { return null; } }
		public abstract Sprite[] GetFrames();
		
		public override void Init(ObjectData data)
		{
			sprites = GetFrames();
			
			debug = new Sprite[sprites.Length];
			
			for (int i = 0; i < sprites.Length; i++)
			{
				Rectangle bounds = sprites[i].Bounds;
				BitmapBits overlay = new BitmapBits(bounds.Size);
				overlay.DrawRectangle(6, 0, 0, bounds.Width - 1, bounds.Height - 1); // LevelData.ColorWhite
				debug[i] = new Sprite(overlay, bounds.X, bounds.Y);
			}
			
			if (sprites.Length > 1)
			{
				properties[0] = new PropertySpec("Frame", typeof(int), "Extended",
					"Which sprite this object should display.", null, names,
					(obj) => (int)obj.PropertyValue,
					(obj, value) => obj.PropertyValue = (byte)((int)value));
			}
		}
		
		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[0]); }
		}
		
		public override PropertySpec[] CustomProperties
		{
			get { return (sprites.Length > 1) ? properties : null; }
		}
		
		public override string SubtypeName(byte subtype)
		{
			return (sprites.Length > 1) ? names.GetKey(subtype) : null;
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
			return sprites[(sprites.Length > 1) ? obj.PropertyValue : 0];
		}
		
		public override Sprite GetDebugOverlay(ObjectEntry obj)
		{
			return sprites[(sprites.Length > 1) ? obj.PropertyValue : 0];
		}
	}
}