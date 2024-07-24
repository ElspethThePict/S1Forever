using SonicRetro.SonLVL.API;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1ObjectDefinitions.SYZ
{
	class HBlock : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[2];
		private Sprite[] sprites = new Sprite[5];
		private Sprite[] debug = new Sprite[2];
		
		public override void Init(ObjectData data)
		{
			sprites[4] = new Sprite(LevelData.GetSpriteSheet("SYZ/Objects.gif").GetSection(119, 34, 32, 32), -16, -16);
			
			int[] offsets = {64, 32, -32, -64};
			for (int i = 0; i < offsets.Length; i++)
			{
				sprites[i] = new Sprite(sprites[4], offsets[i], 0);
			}
			
			// tagging this area withLevelData.ColorWhite
			
			BitmapBits bitmap = new BitmapBits(129, 2);
			bitmap.DrawLine(6, 0, 0, 128, 0);
			debug[0] = new Sprite(bitmap, -64, 0);
			
			bitmap = new BitmapBits(65, 2);
			bitmap.DrawLine(6, 0, 0, 64, 0);
			debug[1] = new Sprite(bitmap, -32, 0);
			
			// i'm sure there's a better way to do this but i've been on this for like an hour and still can't think of anything better, so
			// (i give up-)
			
			Dictionary<byte, byte> startfrom = new Dictionary<byte, byte>
			{
				{0, 3},
				{1, 2}
			};
			
			properties[0] = new PropertySpec("Start From", typeof(int), "Extended", // well technically yeah it doesn't really start from here but this is still cleaner imo
				"Which side this Block should start from.", null, new Dictionary<string, int>
				{
					{ "Right", 0 },
					{ "Left", 1 }
				},
				(obj) => (startfrom.ContainsKey(obj.PropertyValue)) ? 0 : 1,
				(obj, value) => {
						int val = (int)value;
						if (obj.PropertyValue > 3)
							obj.PropertyValue = 0;
						if (val == 0)
						{
							if (!startfrom.ContainsKey(obj.PropertyValue))
								obj.PropertyValue = startfrom.GetKey(obj.PropertyValue);
						}
						else
						{
							if (startfrom.ContainsKey(obj.PropertyValue))
								obj.PropertyValue = startfrom[obj.PropertyValue];
						}
					}
				);
			
			Dictionary<byte, byte> distance = new Dictionary<byte, byte>
			{
				{0, 1},
				{3, 2}
			};
			
			properties[1] = new PropertySpec("Distance", typeof(int), "Extended",
				"The distance that this Block should travel.", null, new Dictionary<string, int>
				{
					{ "Far", 0 },
					{ "Close", 1 }
				},
				(obj) => (distance.ContainsKey(obj.PropertyValue)) ? 0 : 1,
				(obj, value) => {
						int val = (int)value;
						if (obj.PropertyValue > 3)
							obj.PropertyValue = 0;
						if (val == 0)
						{
							if (!distance.ContainsKey(obj.PropertyValue))
								obj.PropertyValue = distance.GetKey(obj.PropertyValue);
						}
						else
						{
							if (distance.ContainsKey(obj.PropertyValue))
								obj.PropertyValue = distance[obj.PropertyValue];
						}
					}
				);
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
			// names sound weird but i can't think of much better
			switch (subtype)
			{
				case 0: return "Start From Right (Far)";
				case 1: return "Start From Right";
				case 2: return "Start From Left";
				case 3: return "Start From Left (Far)";
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
			return sprites[(obj.PropertyValue <= 3) ? obj.PropertyValue : 4];
		}
		
		public override Sprite GetDebugOverlay(ObjectEntry obj)
		{
			switch (obj.PropertyValue)
			{
				case 0:
				case 3: return debug[0];
				case 1:
				case 2: return debug[1];
				default: return null;
			}
		}
	}
}