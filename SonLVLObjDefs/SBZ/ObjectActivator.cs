using SonicRetro.SonLVL.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1ObjectDefinitions.SBZ
{
	class ObjectActivator : ObjectDefinition
	{
		private Sprite sprite;
		
		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[0]); }
		}

		public override void Init(ObjectData data)
		{
			sprite = new Sprite(LevelData.GetSpriteSheet("Global/Display.gif").GetSection(239, 239, 16, 16), -8, -8);
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
			int index = LevelData.Objects.IndexOf(obj) - 1;
			
			if (index < 0)
				return null;
			
			try
			{
				short xmin = Math.Min(obj.X, LevelData.Objects[index].X);
				short ymin = Math.Min(obj.Y, LevelData.Objects[index].Y);
				short xmax = Math.Max(obj.X, LevelData.Objects[index].X);
				short ymax = Math.Max(obj.Y, LevelData.Objects[index].Y);
				
				BitmapBits bitmap = new BitmapBits(xmax - xmin + 1, ymax - ymin + 1);
				
				bitmap.DrawLine(6, obj.X - xmin, obj.Y - ymin, LevelData.Objects[index].X - xmin, LevelData.Objects[index].Y - ymin); // LevelData.ColorWhite
				
				return new Sprite(bitmap, xmin - obj.X, ymin - obj.Y);
			}
			catch
			{
			}
			
			return null;
		}
	}
}