using SonicRetro.SonLVL.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1ObjectDefinitions.Global
{
	class PSwitch_H : ObjectDefinition
	{
		private PropertySpec[] properties;
		private readonly Sprite[] sprites = new Sprite[17];
		
		public override void Init(ObjectData data)
		{
			BitmapBits sheet = LevelData.GetSpriteSheet("Global/Display.gif");
			
			int index = 0;
			for (int i = 0; i < 8; i++)
			{
				sprites[index++] = new Sprite(sheet.GetSection(46 + (i * 17), 158, 16, 16), -8, -8);
			}
			
			for (int i = 0; i < 8; i++)
			{
				sprites[index++] = new Sprite(sheet.GetSection(46 + (i * 17), 175, 16, 16), -8, -8);
			}
			
			sprites[index++] = new Sprite(sheet.GetSection(182, 141, 16, 16), -8, -8);
			
			properties = new PropertySpec[6];
			properties[0] = new PropertySpec("Size", typeof(int), "Extended",
				"How wide the Plane Switch will be.", null, new Dictionary<string, int>
				{
					{ "4 Nodes", 0 },
					{ "8 Nodes", 1 },
					{ "16 Nodes", 2 },
					{ "32 Nodes", 3 }
				},
				(obj) => obj.PropertyValue & 3,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~3) | (int)value));
			
			properties[1] = new PropertySpec("Top Collision Plane", typeof(int), "Extended",
				"Which plane is above.", null, new Dictionary<string, int>
				{
					{ "Plane A", 0 },
					{ "Plane B", 4 }
				},
				(obj) => obj.PropertyValue & 4,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~4) | (int)value));
			
			properties[2] = new PropertySpec("Bottom Collision Plane", typeof(int), "Extended",
				"Which plane is below.", null, new Dictionary<string, int>
				{
					{ "Plane A", 0 },
					{ "Plane B", 8 }
				},
				(obj) => obj.PropertyValue & 8,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~8) | (int)value));
			
			properties[3] = new PropertySpec("Top Draw Order", typeof(int), "Extended",
				"Which draw layer is above.", null, new Dictionary<string, int>
				{
					{ "Low Layer", 0 },
					{ "High Layer", 16 }
				},
				(obj) => obj.PropertyValue & 16,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~16) | (int)value));
			
			properties[4] = new PropertySpec("Bottom Draw Order", typeof(int), "Extended",
				"Which draw layer is below.", null, new Dictionary<string, int>
				{
					{ "Low Layer", 0 },
					{ "High Layer", 32 }
				},
				(obj) => obj.PropertyValue & 32,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~32) | (int)value));
			
			properties[5] = new PropertySpec("Grounded", typeof(int), "Extended",
				"If only grounded players should be affected.", null, new Dictionary<string, int>
				{
					{ "False", 0 },
					{ "True", 128 }
				},
				(obj) => obj.PropertyValue & 128,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~128) | (int)value));
		}
		
		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[0]); }
		}
		
		public override bool Debug
		{
			get { return true; }
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
			get { return sprites[0]; }
		}

		public override Sprite SubtypeImage(byte subtype)
		{
			return sprites[0];
		}

		public override Sprite GetSprite(ObjectEntry obj)
		{
			int count = Math.Max((1 << ((obj.PropertyValue & 3) + 2)), 1);
			int sx = -(((count) * 16) / 2) + 8;
			
			Sprite frame = new Sprite(sprites[(obj.PropertyValue >> 2) & 15]);
			if (obj.PropertyValue > 0x7f) // Grounded, add back sprite
				frame = new Sprite(sprites[16], frame);
			
			List<Sprite> sprs = new List<Sprite>();
			for (int i = 0; i < count; i++)
			{
				sprs.Add(new Sprite(frame, sx + (i * 16), 0));
			}
			
			return new Sprite(sprs.ToArray());
		}
	}
}