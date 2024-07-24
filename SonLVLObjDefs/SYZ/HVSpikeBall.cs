using SonicRetro.SonLVL.API;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System;

namespace S1ObjectDefinitions.SYZ
{
	class HVSpikeBall : ObjectDefinition
	{
		private Sprite[] sprites = new Sprite[5];
		private Sprite[] debug = new Sprite[4];
		private PropertySpec[] properties = new PropertySpec[1];
		
		public override void Init(ObjectData data)
		{
			sprites[4] = new Sprite(LevelData.GetSpriteSheet("SYZ/Objects.gif").GetSection(61, 178, 48, 48), -24, -24);
			sprites[0] = new Sprite(sprites[4], -48 + 48, 0); // start from right
			sprites[1] = new Sprite(sprites[4], -48 - 48, 0); // start from left
			sprites[2] = new Sprite(sprites[4], 0, -48 + 48); // start from bottom
			sprites[3] = new Sprite(sprites[4], 0, -80 - 48); // start from top
			
			BitmapBits bitmap = new BitmapBits(97, 2);
			bitmap.DrawLine(6, 0, 0, 96, 0); // LevelData.ColorWhite
			debug[0] = debug[1] = new Sprite(bitmap, -96, 0);
			
			bitmap = new BitmapBits(2, 97);
			bitmap.DrawLine(6, 0, 0, 0, 96); // LevelData.ColorWhite
			debug[2] = new Sprite(bitmap, 0, -96);
			
			debug[3] = new Sprite(debug[2], 0, -32);
			
			properties[0] = new PropertySpec("Start From", typeof(int), "Extended",
				"Which side this Spike Ball should start from.", null, new Dictionary<string, int>
				{
					{ "Right", 0 },
					{ "Left", 1 },
					{ "Bottom", 2 },
					{ "Top", 3 }
				},
				(obj) => (int)obj.PropertyValue,
				(obj, value) => obj.PropertyValue = (byte)((int)value));
		}
		
		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {0, 1, 2, 3}); }
		}
		
		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
		}

		public override string SubtypeName(byte subtype)
		{
			switch (subtype)
			{
				case 0: return "Start From Right";
				case 1: return "Start From Left";
				case 2: return "Start From Bottom";
				case 3: return "Start From Top";
				default: return "Static";
			}
		}

		public override Sprite Image
		{
			get { return sprites[4]; }
		}

		public override Sprite SubtypeImage(byte subtype)
		{
			return sprites[4];
		}

		public override Sprite GetSprite(ObjectEntry obj)
		{
			return sprites[(obj.PropertyValue < 4) ? obj.PropertyValue : 4];
		}
		
		public override Sprite GetDebugOverlay(ObjectEntry obj)
		{
			return (obj.PropertyValue < 5) ? debug[obj.PropertyValue] : null;
		}
	}
}