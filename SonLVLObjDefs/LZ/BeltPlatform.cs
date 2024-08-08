using SonicRetro.SonLVL.API;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace S1ObjectDefinitions.SBZ
{
	class BeltPlatform : LZ.BeltPlatform
	{
		public override Sprite GetFrame()
		{
			return new Sprite(LevelData.GetSpriteSheet("SBZ/Objects.gif").GetSection(413, 166, 32, 14), -16, -7);
		}
		
		public override int[][] GetPaths()
		{
			// copied from the original script (with the first entry repeated at the end)
			return new int[][] {
				new int[] {
				3604, 880,
				3824, 770,
				3824, 832,
				3604, 942,
				3604, 880},
				new int[] {
				3860, 736,
				4080, 626,
				4080, 688,
				3860, 798,
				3860, 736},
				new int[] {
				4116, 624,
				4336, 514,
				4336, 576,
				4116, 686,
				4116, 624},
				new int[] {
				3860, 1392,
				4080, 1282,
				4080, 1344,
				3860, 1454,
				3860, 1392},
				new int[] {
				6932, 1648,
				7152, 1538,
				7152, 1600,
				6932, 1710,
				6932, 1648},
				new int[] {
				7188, 1504,
				7408, 1394,
				7408, 1456,
				7188, 1566,
				7188, 1504}
			};
		}
	}
}

namespace S1ObjectDefinitions.LZ
{
	class BeltPlatform : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[2];
		private Sprite sprite;
		private Sprite[] debug;
		
		public virtual Sprite GetFrame()
		{
			return new Sprite(LevelData.GetSpriteSheet("LZ/Objects.gif").GetSection(223, 154, 32, 16), -16, -8);
		}
		
		public virtual int[][] GetPaths()
		{
			// copied from the original script (with the first entry repeated at the end)
			return new int[][] {
				new int[] {
				4216, 538,
				4286, 608,
				4286, 915,
				4236, 965,
				4130, 912,
				4130, 580,
				4216, 538},
				new int[] {
				4734, 640,
				4814, 720,
				4814, 1134,
				4658, 1056,
				4658, 716,
				4734, 640},
				new int[] {
				3362, 1154,
				3362, 1502,
				3502, 1502,
				3502, 1154,
				3362, 1154},
				new int[] {
				3426, 930,
				3566, 930,
				3566, 1246,
				3426, 1246,
				3426, 930},
				new int[] {
				3244, 578,
				3550, 578,
				3550, 990,
				3154, 990,
				3154, 668,
				3244, 578},
				new int[] {
				4690, 522,
				5086, 522,
				5086, 702,
				4690, 702,
				4690, 522}
			};
		}
		
		public override void Init(ObjectData data)
		{
			sprite = GetFrame();
			int[][] paths = GetPaths();
			
			debug = new Sprite[paths.Length];
			for (int i = 0; i < paths.Length; i++)
			{
				int xmin = 0x7fff;
				int ymin = 0x7fff;
				int xmax = -0x7fff;
				int ymax = -0x7fff;
				
				for (int j = 0; j < paths[i].Length; j += 2)
				{
					xmin = Math.Min(xmin, paths[i][j]);
					ymin = Math.Min(ymin, paths[i][j+1]);
					xmax = Math.Max(xmax, paths[i][j]);
					ymax = Math.Max(ymax, paths[i][j+1]);
				}
				
				BitmapBits bitmap = new BitmapBits(xmax - xmin + 1, ymax - ymin + 1);
				
				for (int j = 2; j < paths[i].Length; j += 2)
					bitmap.DrawLine(6, paths[i][j-2] - xmin, paths[i][j-1] - ymin, paths[i][j] - xmin, paths[i][j+1] - ymin);
				
				debug[i] = new Sprite(bitmap, xmin, ymin);
			}
			
			// this system kinda sucks tbh
			// it uses absolute positions rather than relative ones, so they're kinda a bit too rigid to really be used in a level good
			
			// maybe it's worth mentioning in the desc that they're absolute pos's and not relative? not sure what a good way to phrase that would be like, though
			properties[0] = new PropertySpec("Path", typeof(int), "Extended",
				"Which path this object should follow.", null,
				(obj) => (obj.PropertyValue >> 4),
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~0xf0) | (((int)value & 15) << 4)));
			
			properties[1] = new PropertySpec("Start Offset", typeof(int), "Extended",
				"Where this object should start from, within its path.", null,
				(obj) => (obj.PropertyValue & 15),
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~15) | ((int)value & 15)));
		}
		
		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[0]); }
		}
		
		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
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
			// Was going to draw an arrow to dest pos based on start offset, but i don't think it'd really look that good, with how close the obj is to its dest sometimes
			// So, let's only show the path and that's it
			
			int index = obj.PropertyValue >> 4;
			if (index >= debug.Length)
				return null;
			
			Sprite dbg = new Sprite(debug[obj.PropertyValue >> 4], -obj.X, -obj.Y);
			
			for (int i = LevelData.Objects.IndexOf(obj); i >= 0; --i)
			{
				switch (LevelData.Objects[i].Name)
				{
					case "Belt Activation": // well technically any object can work.. but how about we don't loop around the entire object list every time
						LevelData.Objects[i].UpdateDebugOverlay();
						break;
					case "Belt Platform":
						break;
					default:
						return dbg;
				}
			}
			
			return dbg;
		}
	}
}