using SonicRetro.SonLVL.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1FObjectDefinitions.LZ
{
	class RotatingSpike : ObjectDefinition
	{
		private readonly Sprite[] sprites = new Sprite[3];
		private PropertySpec[] properties = new PropertySpec[3];
		
		public override void Init(ObjectData data)
		{
			// #Forever - "BossRush" folder check
			if (LevelData.StageInfo.folder.EndsWith("BossRush")) // not sure if we should keep using EndsWith since Forever is based off of pre-Origins? may as well keep it like this anyways though, in case they update it
			{
				BitmapBits sheet = LevelData.GetSpriteSheet("MBZ/Objects.gif");
				sprites[0] = new Sprite(sheet.GetSection(76, 330, 16, 16), -8, -8); // post
				sprites[1] = new Sprite(sheet.GetSection(93, 330, 16, 16), -8, -8); // chain
				sprites[2] = new Sprite(sheet.GetSection(222, 396, 32, 32), -16, -16); // spike ball
			}
			else
			{
				BitmapBits sheet = LevelData.GetSpriteSheet("LZ/Objects.gif");
				sprites[0] = new Sprite(sheet.GetSection(84, 173, 16, 16), -8, -8); // post
				sprites[1] = new Sprite(sheet.GetSection(101, 173, 16, 16), -8, -8); // chain
				sprites[2] = new Sprite(sheet.GetSection(84, 190, 32, 32), -16, -16); // spike ball
			}
			
			properties[0] = new PropertySpec("Length", typeof(int), "Extended",
				"How many chains the Spike should hang off of.", null,
				(obj) => obj.PropertyValue & 0x0f,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~0x0f) | (int)value));
			
			properties[1] = new PropertySpec("Speed", typeof(int), "Extended",
				"The speed of this Spike Ball. Positive values are clockwise, negative values are counter-clockwise.", null,
				(obj) => {
						int speed = (obj.PropertyValue & 0xf0) >> 4;
						if (speed >= 8)
							speed -= 16;
						return speed;
					},
				(obj, value) => {
						int speed = Math.Min(Math.Max((int)value, -8), 7);
						if (speed < 0)
							speed += 16;
						
						obj.PropertyValue = (byte)((obj.PropertyValue & ~0xf0) | (speed << 4));
					}
				);
			
			properties[2] = new PropertySpec("Start From", typeof(int), "Extended",
				"Which direction this Rotating Spike should start from.", null, new Dictionary<string, int>
				{
					{ "Right", 0 },
					{ "Bottom", 1 },
					{ "Left", 2 },
					{ "Top", 3 }
				},
				(obj) => (int)(((V4ObjectEntry)obj).Direction),
				(obj, value) => ((V4ObjectEntry)obj).Direction = (RSDKv3_4.Tiles128x128.Block.Tile.Directions)value);
		}
		
		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {0x34, 0x35, 0xd4, 0xd5}); }
		}
		
		public override byte DefaultSubtype
		{
			get { return 0x34; } // length: 4, speed: 3
		}
		
		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
		}

		public override string SubtypeName(byte subtype)
		{
			return (subtype & 0x0f) + " chains" + ((subtype < 0x80) ? " (Clockwise)" : " (Counter-Clockwise)");
		}

		public override Sprite Image
		{
			get { return sprites[2]; }
		}

		public override Sprite SubtypeImage(byte subtype)
		{
			return sprites[1];
		}

		public override Sprite GetSprite(ObjectEntry obj)
		{
			List<Sprite> sprs = new List<Sprite>();
			
			int length = (obj.PropertyValue & 0x0f);
			double angle = (int)(((V4ObjectEntry)obj).Direction) * (Math.PI / 2.0);
			
			for (int i = 0; i < length + 1; i++)
			{
				int frame = (i == 0) ? 0 : (i == length) ? 2 : 1;
				sprs.Add(new Sprite(sprites[frame], (int)(Math.Cos(angle) * (i * 16)), (int)(Math.Sin(angle) * (i * 16))));
			}
			
			return new Sprite(sprs.ToArray());
		}
		
		public override Sprite GetDebugOverlay(ObjectEntry obj)
		{
			int length = (obj.PropertyValue & 0x0f) * 16;
			
			var overlay = new BitmapBits(2 * length + 1, 2 * length + 1);
			overlay.DrawCircle(6, length, length, length); // LevelData.ColorWhite
			return new Sprite(overlay, -length, -length);
		}
		
		public override Rectangle GetBounds(ObjectEntry obj)
		{
			int length = obj.PropertyValue & 0x0f;
			double angle = (int)(((V4ObjectEntry)obj).Direction) * (Math.PI / 2.0);
			
			Rectangle bounds = sprites[2].Bounds;
			bounds.Offset(obj.X + (int)(Math.Cos(angle) * (length * 16)), obj.Y + (int)(Math.Sin(angle) * (length * 16)));
			return bounds;
		}
	}
}