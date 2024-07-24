using SonicRetro.SonLVL.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1ObjectDefinitions.LZ
{
	class HorizontalDoor : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[2];
		private Sprite sprite;
		private Sprite[] debug = new Sprite[2];
		
		public override void Init(ObjectData data)
		{
			sprite = new Sprite(LevelData.GetSpriteSheet("LZ/Objects.gif").GetSection(84, 223, 128, 32), -64, -16);
			
			BitmapBits bitmap = new BitmapBits(129, 33);
			bitmap.DrawRectangle(6, 0, 0, 128, 32); // LevelData.ColorWhite
			debug[0] = new Sprite(bitmap, -192, -16);
			debug[1] = new Sprite(bitmap, 64, -16);
			
			properties[0] = new PropertySpec("Activate By", typeof(int), "Extended",
				"How this Door will be activated. If Button, the object[+1] will be looked at.", null, new Dictionary<string, int>
				{
					{ "Button", 0 },
					{ "Water", 1 }
				},
				(obj) => (obj.PropertyValue == 1) ? 1 : 0,
				(obj, value) => obj.PropertyValue = (byte)((int)value));
			
			properties[1] = new PropertySpec("Direction", typeof(int), "Extended",
				"Which way the Door will slide.", null, new Dictionary<string, int>
				{
					{ "Left", 0 },
					{ "Right", 1 }
				},
				(obj) => ((((V4ObjectEntry)obj).Direction == RSDKv3_4.Tiles128x128.Block.Tile.Directions.FlipNone) ? 0 : 1),
				(obj, value) => ((V4ObjectEntry)obj).Direction = (RSDKv3_4.Tiles128x128.Block.Tile.Directions)value);
		}

		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {0, 1}); }
		}

		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
		}

		public override string SubtypeName(byte subtype)
		{
			return (subtype == 1) ? "Activated By Water" : "Activated By Button[+1]";
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
			return debug[(((V4ObjectEntry)obj).Direction == RSDKv3_4.Tiles128x128.Block.Tile.Directions.FlipNone) ? 0 : 1];
		}
	}
}