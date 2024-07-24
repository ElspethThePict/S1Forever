using SonicRetro.SonLVL.API;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1ObjectDefinitions.SLZ
{
	class Staircase : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[2];
		private Sprite sprite;
		private Sprite[] debug = new Sprite[2];
		
		public override void Init(ObjectData data)
		{
			sprite = new Sprite(LevelData.GetSpriteSheet("SLZ/Objects.gif").GetSection(67, 26, 128, 32), -16, -16);
			
			properties[0] = new PropertySpec("Activate From", typeof(int), "Extended",
				"Where the Staircase should be activated from.", null, new Dictionary<string, int>
				{
					{ "Top", 0 },
					{ "Bottom", 2 }
				},
				(obj) => (obj.PropertyValue == 2) ? 2 : 0,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~2) | (int)value));
			
			properties[1] = new PropertySpec("Direction", typeof(int), "Extended",
				"Which direction the Staircase will expand.", null, new Dictionary<string, int>
				{
					{ "Left", 0 },
					{ "Right", 1 }
				},
				(obj) => ((int)(((V4ObjectEntry)obj).Direction) == 0) ? 0 : 1,
				(obj, value) => ((V4ObjectEntry)obj).Direction = (RSDKv3_4.Tiles128x128.Block.Tile.Directions)value);
			
			BitmapBits bitmap = new BitmapBits(129, 129);
			for (int i = 0; i < 97; i += 32)
				bitmap.DrawRectangle(6, i, i, 31, 31); // LevelData.ColorWhite
			
			// BitmapBits->Sprite constructor doesn't support flips so we need to do this
			Sprite overlay = new Sprite(bitmap);
			
			debug[0] = new Sprite(overlay, 112, 16, true, false); // expand left
			debug[1] = new Sprite(overlay, -16, 16, false, false); // expand right
		}

		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {0, 2}); }
		}
		
		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
		}

		public override string SubtypeName(byte subtype)
		{
			return (subtype == 2) ? "Active From Bottom" : "Activate From Top";
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
			return debug[((int)(((V4ObjectEntry)obj).Direction) == 0) ? 0 : 1];
		}
	}
}