using SonicRetro.SonLVL.API;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1ObjectDefinitions.SYZ
{
	class RSpikeChain : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[1];
		private Sprite[] sprites = new Sprite[2];
		private Sprite[] debug = new Sprite[2];
		
		public override void Init(ObjectData data)
		{
			Sprite sprite = new Sprite(LevelData.GetSpriteSheet("SYZ/Objects.gif").GetSection(88, 138, 16, 16), -8, -8);
			
			int[] lengths = {4, 2};
			for (int i = 0; i < lengths.Length; i++)
			{
				Sprite frame = new Sprite(sprite);
				for (int j = 0; j < lengths[i]; j++)
					frame = new Sprite(frame, new Sprite(sprite, 0, -(j+1) * 16));
				
				sprites[i] = frame;
				
				int l = lengths[i] * 16;
				
				BitmapBits bitmap = new BitmapBits(2 * l + 1, 2 * l + 1);
				bitmap.DrawCircle(6, l, l, l); // LevelData.ColorWhite
				debug[i] = new Sprite(bitmap, -l, -l);
			}
			
			// dir attr in the scene doesn't matter, even if it's set sometimes
			
			properties[0] = new PropertySpec("Behaviour", typeof(int), "Extended",
				"How this Spike Chain should behave.", null, new Dictionary<string, int>
				{
					{ "Speed: Fast, Length: Long", 0 },
					{ "Speed: Slow, Length: Long", 1 },
					{ "Speed: Slow, Length: Short", 2 },
					{ "Speed: Slow (Reverse), Length: Short", 3 },
				},
				(obj) => (int)obj.PropertyValue,
				(obj, value) => obj.PropertyValue =  (byte)((int)value));
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
				case 0: return "Long, Fast";
				case 1: return "Long, Slow";
				case 2: return "Short, Slow";
				case 3: return "Short, Slow (Reverse)";
				default: return "Unknown";
			}
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
			return sprites[(obj.PropertyValue <= 1) ? 0 : 1]; // well yeah this doesn't represent invalid ones correctly.. but the "correct" way would just have them invisible-
		}
		
		public override Sprite GetDebugOverlay(ObjectEntry obj)
		{
			return (obj.PropertyValue <= 1) ? debug[0] : (obj.PropertyValue <= 3) ? debug[1] : null;
		}
	}
}