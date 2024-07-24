using SonicRetro.SonLVL.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1ObjectDefinitions.Global
{
	class Spikes : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[3];
		private Sprite[] sprites = new Sprite[16];
		private Sprite[] debug = new Sprite[16];
		
		public override void Init(ObjectData data)
		{
			BitmapBits sheet = LevelData.GetSpriteSheet("Global/Items.gif");
			
			Sprite[] frames = new Sprite[8];
			frames[0] = new Sprite(sheet.GetSection(84, 133, 40, 32), -20, -16);
			frames[1] = new Sprite(sheet.GetSection(174, 133, 32, 40), -16, -20);
			frames[2] = new Sprite(sheet.GetSection(174, 174, 32, 40), -16, -20);
			frames[3] = new Sprite(sheet.GetSection(125, 133, 40, 32), -20, -16);
			frames[4] = new Sprite(sheet.GetSection(84, 133, 8, 32), -4, -16);
			frames[5] = new Sprite(sheet.GetSection(174, 133, 32, 8), -16, -4);
			frames[6] = new Sprite(sheet.GetSection(174, 174, 32, 8), -16, -4);
			frames[7] = new Sprite(sheet.GetSection(125, 133, 8, 32), -4, -16);
			
			for (int i = 0; i < 16; i++)
			{
				List<Sprite> sprs = new List<Sprite>();
				int frame = i;
				switch (frame)
				{
					case 0: // 3 Spikes (Up)
					case 1: // 3 Spikes (Right)
					case 2: // 3 Spikes (Left)
					case 3: // 3 Spikes (Down)
					case 4: // 1 Spike (Up)
					case 5: // 1 Spike (Right)
					case 6: // 1 Spike (Left)
					case 7: // 1 Spike (Down)
						sprs.Add(new Sprite(frames[frame]));
						break;
					
					case 8:  // 3 Spikes (Spaced Out) (Up)
					case 11: // 3 Spikes (Spaced Out) (Down)
						frame -= 4;
						sprs.Add(new Sprite(frames[frame], -24, 0));
						sprs.Add(new Sprite(frames[frame]));
						sprs.Add(new Sprite(frames[frame], 24, 0));
						break;
					
					case 9:  // 3 Spikes (Spaced Out) (Right)
					case 10: // 3 Spikes (Spaced Out) (Left)
						frame -= 4;
						sprs.Add(new Sprite(frames[frame], 0, -24));
						sprs.Add(new Sprite(frames[frame]));
						sprs.Add(new Sprite(frames[frame], 0, 24));
						break;
						
					case 12: // 6 Spikes (Spaced Out) (Up)
					case 15: // 6 Spikes (Spaced Out) (Down)
						frame -= 8;
						for (int j = 0; j < 6; j++)
							sprs.Add(new Sprite(frames[frame], -60 + (j * 24), 0));
						break;
						
					case 13: // 6 Spikes (Spaced Out) (Right)
					case 14: // 6 Spikes (Spaced Out) (Left)
						frame -= 8;
						for (int j = 0; j < 6; j++)
							sprs.Add(new Sprite(frames[frame], 0, -60 + (j * 24)));
						break;
				}
				
				sprites[i] = new Sprite(sprs.ToArray());
				
				// and now, let's do the debug vis for moving spikes
				Rectangle bounds = sprites[i].Bounds;
				BitmapBits overlay = new BitmapBits(bounds.Size);
				overlay.DrawRectangle(6, 0, 0, bounds.Width - 1, bounds.Height - 1); // LevelData.ColorWhite
				debug[i] = new Sprite(overlay, bounds.X, bounds.Y);
				
				switch (i & 3) // Direction
				{
					case 0: debug[i].Offset(0,  32); break; // Up
					case 1: debug[i].Offset(-32, 0); break; // Right
					case 2: debug[i].Offset( 32, 0); break; // Left
					case 3: debug[i].Offset(0, -32); break; // Down
				}
			}
			
			properties[0] = new PropertySpec("Count", typeof(int), "Extended",
				"How many Spikes there will be.", null, new Dictionary<string, int>
				{
					{ "3 Spikes", 0 },
					{ "1 Spike", 4 },
					{ "3 Spikes - Spaced", 8 },
					{ "6 Spikes - Spaced", 12 }
				},
				(obj) => obj.PropertyValue & 12,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~12) | (int)value));
			
			properties[1] = new PropertySpec("Direction", typeof(int), "Extended",
				"Which way the Spikes are facing.", null, new Dictionary<string, int>
				{
					{ "Up", 0 },
					{ "Right", 1 },
					{ "Left", 2 },
					{ "Down", 3 }
				},
				(obj) => obj.PropertyValue & 3,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~3) | (int)value));
			
			properties[2] = new PropertySpec("Moving", typeof(bool), "Extended",
				"If the Spikes should peek in and out.", null,
				(obj) => (obj.PropertyValue > 15),
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~0xf0) | ((bool)value ? 0x80 : 0x00)));
		}
		
		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8a, 0x8b, 0x8c, 0x8d, 0x8e, 0x8f}); }
		}
		
		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
		}

		public override string SubtypeName(byte subtype)
		{
			// i don't really like this, but it's better than nothing ig
			
			string[] counts = {"3 ", "1 ", "3 Spaced ", "6 Spaced "};
			string[] directions = {"Upwards", "Right", "Left", "Downwards"}; // not too major, but should it be "Upward" or "Upwards"? i'm honestly not sure...
			
			string name = counts[(subtype & 12) >> 2];
			name += directions[subtype & 3];
			
			if (subtype > 15) name += " (Moving)";
			
			return name;
		}

		public override Sprite Image
		{
			get { return sprites[0]; }
		}

		public override Sprite SubtypeImage(byte subtype)
		{
			return sprites[subtype & 15];
		}

		public override Sprite GetSprite(ObjectEntry obj)
		{
			return sprites[obj.PropertyValue & 15];
		}
		
		public override Sprite GetDebugOverlay(ObjectEntry obj)
		{
			return (obj.PropertyValue > 15) ? debug[obj.PropertyValue & 15] : null;
		}
	}
}