using SonicRetro.SonLVL.API;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1ObjectDefinitions.MZ
{
	class HCrusher : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[2];
		private Sprite[,,] sprites = new Sprite[2,4,2]; // direction, length, triggered
		private Sprite[,,] debug = new Sprite[2,4,2]; // direction, length, triggered
		
		public override void Init(ObjectData data)
		{
			// this whole section is a bit busy.. hope that's alright...
			
			// note - even if right facing crushers don't work in the base game by default, we're still going to respect them, so they still have a proper render and debug vis and everything
			// (and honestly i think this object is one of the coolest ones in the game, i'd love to see someone fix that bug up and use this obj in a custom level-)
			
			BitmapBits sheet = LevelData.GetSpriteSheet("MZ/Objects.gif");
			
			Sprite[] frames = new Sprite[3];
			frames[0] = new Sprite(sheet.GetSection(295, 388, 21, 16), 16, -8); // rod
			frames[1] = new Sprite(sheet.GetSection(289, 158, 8, 32), 36, -16); // base
			Sprite crusher = frames[2] = new Sprite(new Sprite(sheet.GetSection(256, 142, 32, 64), -12, -32), new Sprite(sheet.GetSection(256, 207, 31, 48), -43, -24)); // Crusher
			
			sprites[0,0,0] = new Sprite(frames);
			
			int[] lengths = {56, 159, 80, 56};
			
			// left-facing
			for (int i = 0; i < lengths.Length; i++)
			{
				frames[0] = new Sprite(sheet.GetSection(299 - (lengths[i] + 4), 388, lengths[i] + 4 + 17, 16), 16 - lengths[i], -8);
				frames[2] = new Sprite(crusher, -lengths[i], 0);
				sprites[0,i,1] = new Sprite(frames);
				
				sprites[0,i,0] = sprites[0,0,0];
			}
			
			frames[1].Flip(true, false);
			crusher.Flip(true, false);
			frames[2] = crusher;
			
			frames[0] = new Sprite(sheet.GetSection(295, 388, 21, 16), -38, -8);
			
			sprites[1,0,0] = new Sprite(frames);
			
			// right-facing
			for (int i = 0; i < lengths.Length; i++)
			{
				frames[0] = new Sprite(sheet.GetSection(299 - (lengths[i] + 4), 388, lengths[i] + 4 + 17, 16), -34, -8);
				frames[2] = new Sprite(crusher, lengths[i], 0);
				sprites[1,i,1] = new Sprite(frames);
				
				sprites[1,i,0] = sprites[1,0,0];
			}
			
			BitmapBits bitmap = new BitmapBits(54, 64);
			bitmap.DrawRectangle(6, 0, 0, 53, 63); // LevelData.ColorWhite
			
			// for non-triggered ones, let's show where the crusher will be when it's extended
			for (int i = 0; i < lengths.Length; i++)
			{
				debug[0,i,0] = new Sprite(bitmap, -42 - lengths[i], -32); // facing left
				debug[1,i,0] = new Sprite(bitmap, -12 + lengths[i], -32); // facing right
			}
			
			// for triggered ones, draw a box for where it'll be when it's retracted
			// it's not that useful of a vis so it's not drawn, but we're doing this anyways for bounds calculating
			debug[0,0,1] = new Sprite(bitmap, -42, -32); // facing left
			debug[1,0,1] = new Sprite(bitmap, -12, -32); // facing right
			
			for (int i = 1; i < lengths.Length; i++)
			{
				debug[0,i,1] = debug[0,0,1];
				debug[1,i,1] = debug[1,0,1];
			}
			
			properties[0] = new PropertySpec("Distance", typeof(int), "Extended",
				"How far the crusher should extend.", null, new Dictionary<string, int>
				{
					{ "56 px", 0 },
					{ "80 px", 2 },
					{ "159 px", 1 }
					// { "56 px (Use Button)", 3 } // RE2 has this but i don't think this is actually any different from the normal 56px?
				},
				(obj) => obj.PropertyValue & 3,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~3) | (int)value));
			
			properties[1] = new PropertySpec("Triggered", typeof(bool), "Extended",
				"If the crusher should be extended by default and retracted with a button, as opposed to moving automatically.", null,
				(obj) => (obj.PropertyValue & 0x80) == 0x80,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~0x80) | ((bool)value ? 0x80 : 0x00)));
			
			/*
			// Well.. it *should* work like this, but right-facing crushers aren't programmed correctly in the actual game, so...
			// To restore this property, just remove the block comment markers and change the "PropertySpec[2]" at the top of the script to be a 3 instead
			properties[2] = new PropertySpec("Direction", typeof(int), "Extended",
				"Which direction this crusher should extend in.", null, new Dictionary<string, int>
				{
					{ "Left", 0 },
					{ "Right", 0x40 }
				},
				(obj) => obj.PropertyValue & 0x40,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~0x40) | (int)value));
			*/
		}

		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {0, 2, 1, 0x80, 0x82, 0x81}); }
		}
		
		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
		}

		public override string SubtypeName(byte subtype)
		{
			int[] lengths = {56, 159, 80, 56};
			string name = lengths[subtype & 3] + " Pixels Long";
			// [nothing for direction here]
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
			return sprites[(subtype & 0x40) >> 6,subtype & 3,(subtype & 0x80) >> 7];
		}

		public override Sprite GetSprite(ObjectEntry obj)
		{
			return sprites[(obj.PropertyValue & 0x40) >> 6,obj.PropertyValue & 3,(obj.PropertyValue & 0x80) >> 7];
		}

		public override Sprite GetDebugOverlay(ObjectEntry obj)
		{
			if ((obj.PropertyValue & 0x80) == 0x80) return null; // since triggered ones are already shown as extended, let's not show anything
			return debug[(obj.PropertyValue & 0x40) >> 6,obj.PropertyValue & 3,(obj.PropertyValue & 0x80) >> 7]; // (still leaving in the triggered bit here in case i want to revert this in the future)
		}
		
		public override Rectangle GetBounds(ObjectEntry obj)
		{
			// let's make the selection bounds just be the crusher part, ignoring the rod and the base
			Rectangle bounds = debug[(obj.PropertyValue & 0x40) >> 6,obj.PropertyValue & 3,((obj.PropertyValue & 0x80) >> 7) ^ 1].Bounds; // note that we flip the triggered part
			bounds.Offset(obj.X, obj.Y);
			return bounds;
		}
	}
}