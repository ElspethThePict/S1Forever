using SonicRetro.SonLVL.API;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace S1ObjectDefinitions.LZ
{
	class Eggman : ObjectDefinition
	{
		private Sprite sprite;
		private Sprite debug;
		
		public override void Init(ObjectData data)
		{
			sprite = new Sprite(LevelData.GetSpriteSheet("Global/Eggman.gif").GetSection(1, 1, 64, 56), -28, -32);
			
			// Adapted from values in the original script, changed up a bit to look better
			int[] nodePositionTable = new int[] {
				 // -0x700000, 0x0000 // this is a player trigger pos, not an eggman pos node (start trigger)
				 0x0000, 0x0000,
				 0x380000, -0xC00000,
				 0x600000, -0x1000000,
				 0x600000, -0x4C00000,
				 0x13C0000, -0x5000000,
				 0x15C0000, -0x5000000
				 // 0xB80000, -0x4D00000 // same thing here (tunnel exit node)
			};
			
			int xmin =  0x7fff;
			int ymin =  0x7fff;
			int xmax = -0x7fff;
			int ymax = -0x7fff;
			
			for (int i = 0; i < nodePositionTable.Length; i += 2)
			{
				xmin = Math.Min(xmin, nodePositionTable[i] >> 16);
				ymin = Math.Min(ymin, nodePositionTable[i+1] >> 16);
				xmax = Math.Max(xmax, nodePositionTable[i] >> 16);
				ymax = Math.Max(ymax, nodePositionTable[i+1] >> 16);
			}
			
			BitmapBits bitmap = new BitmapBits(xmax - xmin + 1, ymax - ymin + 1);
			
			for (int i = 2; i < nodePositionTable.Length; i += 2)
				bitmap.DrawLine(6, (nodePositionTable[i-2] >> 16) - xmin, (nodePositionTable[i-1] >> 16) - ymin, (nodePositionTable[i] >> 16) - xmin, (nodePositionTable[i+1] >> 16) - ymin); // LevelData.ColorWhite
			
			debug = new Sprite(bitmap, xmin, ymin);
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