using SonicRetro.SonLVL.API;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace S1ObjectDefinitions.LZ
{
	class CorkActivation : LZ.BeltActivation
	{
		public override string targetName { get { return "Cork"; } }
		
		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {3}); }
		}
		
		public override byte DefaultSubtype
		{
			get { return 3; }
		}
	}
	
	class BeltActivation : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[1];
		private Sprite sprite;
		
		public virtual string targetName { get { return "Belt Platform"; } }
		
		public override void Init(ObjectData data)
		{
			sprite = new Sprite(LevelData.GetSpriteSheet("Global/Display.gif").GetSection(239, 239, 16, 16), -8, -8);
			
			properties[0] = new PropertySpec("Activate Count", typeof(int), "Extended",
				"How many of the following objects should be activated by this Activator.", null,
				(obj) => obj.PropertyValue,
				(obj, value) => obj.PropertyValue = (byte)((int)value));
		}
		
		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {8, 9, 10, 11, 12}); }
		}
		
		public override bool Debug
		{
			get { return true; }
		}
		
		public override byte DefaultSubtype
		{
			get { return 8; }
		}
		
		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
		}

		public override string SubtypeName(byte subtype)
		{
			return "Active " + subtype + " " + targetName + "s";
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
			if (obj.PropertyValue == 0)
				return null;
			
			try
			{
				int index = LevelData.Objects.IndexOf(obj) + 1;
				while (LevelData.Objects[index].Name != targetName)
				{
					if (++index >= LevelData.Objects.Count)
						return null;
				}
				
				List<ObjectEntry> objs = LevelData.Objects.Skip(index - 1).Take(obj.PropertyValue + 1).ToList();
				if (objs.Count == 0)
					return null;
				
				short xmin = Math.Min(obj.X, objs.Min(a => a.X));
				short ymin = Math.Min(obj.Y, objs.Min(a => a.Y));
				short xmax = Math.Max(obj.X, objs.Max(a => a.X));
				short ymax = Math.Max(obj.Y, objs.Max(a => a.Y));
				BitmapBits bitmap = new BitmapBits(xmax - xmin + 1, ymax - ymin + 1);
				
				for (int i = 0; i < objs.Count - 1; i++)
					bitmap.DrawLine(6, obj.X - xmin, obj.Y - ymin, objs[i + 1].X - xmin, objs[i + 1].Y - ymin); // LevelData.ColorWhite
				
				return new Sprite(bitmap, xmin - obj.X, ymin - obj.Y);
			}
			catch
			{
			}
			
			return null;
		}
	}
}