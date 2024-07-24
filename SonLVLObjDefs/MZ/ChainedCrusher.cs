using SonicRetro.SonLVL.API;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1ObjectDefinitions.MZ
{
	class ChainedCrusher : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[3];
		private Sprite[,,] sprites = new Sprite[3,7,2]; // size, length, triggered
		private Sprite[,,] debug = new Sprite[3,7,2]; // size, length, triggered
		
		public override void Init(ObjectData data)
		{
			BitmapBits sheet = LevelData.GetSpriteSheet("MZ/Objects.gif");
			
			Sprite crusherbase = new Sprite(sheet.GetSection(256, 76, 32, 32), -16, -52);
			Sprite chain; // set in the loop
			
			Sprite[] crushers = new Sprite[3];
			crushers[0] = new Sprite(new Sprite(sheet.GetSection(143, 180, 112, 32), -56, -20), new Sprite(sheet.GetSection(199, 256, 87, 32), -44, 12)); // large
			crushers[1] = new Sprite(new Sprite(sheet.GetSection(159, 147, 96, 32), -48, -20), new Sprite(sheet.GetSection(199, 256, 87, 32), -44, 12)); // medium
			crushers[2] = new Sprite(sheet.GetSection(256, 109, 32, 32), -16, -20); // small (block)
			
			Sprite[] outlines = new Sprite[crushers.Length];
			for (int i = 0; i < crushers.Length; i++)
			{
				Rectangle bounds = crushers[i].Bounds;
				BitmapBits overlay = new BitmapBits(bounds.Size);
				overlay.DrawRectangle(6, 0, 0, bounds.Width - 1, bounds.Height - 1); // LevelData.ColorWhite
				outlines[i] = new Sprite(overlay, bounds.X, bounds.Y);
			}
			
			int[] lengths = {112, 160, 80, 120, 56, 88, 184};
			
			for (int i = 0; i < lengths.Length; i++)
			{
				for (int j = 0; j < crushers.Length; j++) // size
				{
					// normal ver
					chain = new Sprite(sheet.GetSection(308, 257 - 4, 8, 4), -4, -(18 + 4));
					sprites[j,i,0] = new Sprite(chain, crusherbase, crushers[j]);
					
					// extended ver
					chain = new Sprite(sheet.GetSection(308, 257 - (lengths[i] + 4), 8, (lengths[i] + 4)), -4, -(18 + lengths[i] + 4));
					sprites[j,i,1] = new Sprite(new Sprite(chain, 0, lengths[i]), crusherbase, new Sprite(crushers[j], 0, lengths[i]));
					
					// debug vis
					debug[j,i,0] = new Sprite(outlines[j], 0, lengths[i]); // for normal crushers let's draw the box at where they'll be when extended
					debug[j,i,1] = new Sprite(outlines[j]); // for extended crushers let's make the box their base pos, we don't draw this but we use it for sel bounds
				}
			}
			
			properties[0] = new PropertySpec("Distance", typeof(int), "Extended",
				"How far the crusher should drop.", null, new Dictionary<string, int>
				{
					{ "56 px", 4 },
					{ "80 px", 2 },
					{ "88 px", 5 },
					{ "112 px", 0 },
					{ "120 px", 3 },
					{ "160 px", 1 },
					{ "184 px", 6 }
				}, GetDistance,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~7) | (int)value));
			
			properties[1] = new PropertySpec("Size", typeof(int), "Extended",
				"How large the crusher will be.", null, new Dictionary<string, int>
				{
					{ "Large Crusher", 0 },
					{ "Medium Crusher", 1 },
					/*
					// RE2 has these listed like this.. but the get method doesn't even support the Drop On Visible value? so i'm not really sure...
					{ "Small (Interval)", 2 },
					{ "Small (Drop On Visible)", 3 }
					*/
					{ "Small Block", 2 }
				}, GetSize,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~0x7f) | ((int)value << 4)));
			
			properties[2] = new PropertySpec("Triggered", typeof(int), "Extended", // not a bool because we use the get method for getting array index
				"If the crusher should be extended by default and retracted with a button, as opposed to moving automatically.", null, new Dictionary<string, int>
				{
					{ "False", 0 },
					{ "True", 1 }
				}, GetTriggered,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~0x80) | ((int)value << 7)));
		}

		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {4, 2, 5, 0, 3, 1, 6, 0x14, 0x12, 0x15, 0x10, 0x13, 0x11, 0x16, 0x24, 0x22, 0x25, 0x20, 0x23, 0x21, 0x26, 0x24, 0x82, 0x85, 0x80, 0x83, 0x81, 0x86, 0x94, 0x92, 0x95, 0x90, 0x93, 0x91, 0x96, 0xa4, 0xa2, 0xa5, 0xa0, 0xa3, 0xa1, 0xa6}); } // just chuckin' everything
		}
		
		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
		}
		
		static object GetDistance(ObjectEntry obj)
		{
			return (((obj.PropertyValue & 7) > 6) ? 0 : obj.PropertyValue & 7);
		}
		
		static object GetSize(ObjectEntry obj)
		{
			return (((obj.PropertyValue & 0x7f) >> 4) % 3);
		}
		
		static object GetTriggered(ObjectEntry obj)
		{
			return (obj.PropertyValue >> 7);
		}
		
		public override string SubtypeName(byte subtype)
		{
			string[] sizes = {"Large", "Medium", "Small"};
			int[] lengths = {112, 160, 80, 120, 56, 88, 184};
			
			string name = sizes[((subtype & 0x7f) >> 4) % 3] + ", " + lengths[((subtype & 7) > 6) ? 0 : subtype & 7] + " px"; // "px" alone is kind of weird.. but "pixels downwards" sounds even weirder imo, so
			
			if ((subtype & 0x80) == 0x80)
				name += " (Button Triggered)";
			
			return name;
		}

		public override Sprite Image
		{
			get { return sprites[0,0,0]; }
		}
		
		public override Sprite SubtypeImage(byte subtype)
		{
			return sprites[((subtype & 0x7f) >> 4) % 3,((subtype & 7) > 6) ? 0 : subtype & 7,subtype >> 7];
		}

		public override Sprite GetSprite(ObjectEntry obj)
		{
			return sprites[(int)GetSize(obj),(int)GetDistance(obj),(int)GetTriggered(obj)];
		}

		public override Sprite GetDebugOverlay(ObjectEntry obj)
		{
			if ((obj.PropertyValue & 0x80) == 0x80) return null; // since triggered ones are already shown as extended, let's not show anything
			return debug[(int)GetSize(obj),(int)GetDistance(obj),(int)GetTriggered(obj)]; // (still leaving in the triggered bit here in case i want to revert this in the future)
		}
		
		public override Rectangle GetBounds(ObjectEntry obj)
		{
			// let's make the selection bounds just be the crusher part, ignoring the rod and the base
			Rectangle bounds = debug[(int)GetSize(obj),(int)GetDistance(obj),(int)GetTriggered(obj)^1].Bounds; // note that we flip the triggered part
			bounds.Offset(obj.X, obj.Y);
			return bounds;
		}
	}
}