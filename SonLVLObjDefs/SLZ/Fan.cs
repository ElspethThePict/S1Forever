using SonicRetro.SonLVL.API;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1FObjectDefinitions.SLZ
{
	class Fan : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[4];
		private readonly Sprite[,] sprites = new Sprite[2,4]; // wind dir, sprite dir
		private readonly Sprite[,] debug = new Sprite[2,4]; // hitbox size, direction
		
		public override void Init(ObjectData data)
		{
			// #Forever - "BossRush" folder check
			if (!LevelData.StageInfo.folder.EndsWith("BossRush")) // not sure if we should keep using EndsWith since Forever is based off of pre-Origins? may as well keep it like this anyways though, in case they update it
			{
				// (also yeah, in-game it does a reverse check like this, don't ask me why-
				BitmapBits sheet = LevelData.GetSpriteSheet("SLZ/Objects.gif");
				sprites[0, 0] = new Sprite(sheet.GetSection(6, 223, 27, 32), -11, -16);
				sprites[1, 0] = new Sprite(sheet.GetSection(72, 225, 27, 30), -11, -14);
			}
			else
			{
				BitmapBits sheet = LevelData.GetSpriteSheet("MBZ/Objects.gif");
				sprites[0, 0] = new Sprite(sheet.GetSection(1, 471, 27, 32), -11, -16);
				sprites[1, 0] = new Sprite(sheet.GetSection(58, 471, 27, 30), -11, -14);
			}
			
			for (int i = 1; i < 4; i++) // set up flipped sprites
			{
				// it would be cool if we could cast to RSDKv3_4.Tiles128x128.Block.Tile.Directions for flip dir but it turns out only BitmapBits has that, not Sprite
				sprites[0, i] = new Sprite(sprites[0, 0], (i & 1) == 1, (i & 2) == 2);
				sprites[1, i] = new Sprite(sprites[1, 0], (i & 1) == 1, (i & 2) == 2);
			}
			
			// (mandatory LevelData.ColorWhite tag)
			BitmapBits[] bitmap = { new BitmapBits(240, 112), new BitmapBits(240, 144) };
			bitmap[0].DrawRectangle(6, 0, 0, 239, 111); // smaller hitbox 
			bitmap[1].DrawRectangle(6, 0, 0, 239, 143); // taller hitbox
			
			// hitbox size, direction
			Point[,] offsets = { {new Point(-160, -97), new Point(-160, -97)}, // Left
			                     {new Point(-80, -97),  new Point(-80, -97)},  // Right
								 {new Point(-160, -16), new Point(-160, -48)}, // Left (Roof)
								 {new Point(-80, -16), new Point(-80, -48)} }; // Right (Roof)
			for (int i = 0; i < 4; i++)
			{
				debug[0, i] = new Sprite(bitmap[0], offsets[i, 0]);
				debug[1, i] = new Sprite(bitmap[1], offsets[i, 1]);
			}
			
			properties[0] = new PropertySpec("Wind Direction", typeof(int), "Extended",
				"Where the wind blows.", null, new Dictionary<string, int>
				{
					{ "Right", 0 },
					{ "Left", 1 }
				},
				(obj) => obj.PropertyValue & 1,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~1) | (int)value));
			
			properties[1] = new PropertySpec("Direction", typeof(int), "Extended",
				"Which way the sprite should face.", null, new Dictionary<string, int>
				{
					{ "Left", 0 },
					{ "Right", 1 },
					{ "Left (Roof)", 2 },
					{ "Right (Roof)", 3 },
				},
				(obj) => (int)(((V4ObjectEntry)obj).Direction),
				(obj, value) => ((V4ObjectEntry)obj).Direction = (RSDKv3_4.Tiles128x128.Block.Tile.Directions)value);
			
			/*
			// was thinking about combining Wind Dir and Sprite Dir, but then decided against it
			properties[0] = new PropertySpec("Direction", typeof(int), "Extended",
				"Where the wind blows.", null, new Dictionary<string, int>
				{
					{ "Right", 0 },
					{ "Left", 1 }
				},
				(obj) => ((V4ObjectEntry)obj).Direction, // base it off of Sprite dir
				(obj, value) => {
						obj.PropertyValue = (byte)((obj.PropertyValue & ~1) | (int)value); // wind dir
						((V4ObjectEntry)obj).Direction = (RSDKv3_4.Tiles128x128.Block.Tile.Directions)value; // sprite dir
					}
				);
			*/
			
			properties[2] = new PropertySpec("Always Active", typeof(bool), "Extended",
				"If the Fan should always be active, as opposed to being powered in cycles.", null,
				(obj) => (obj.PropertyValue & 2) == 2,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~2) | ((bool)value ? 2 : 0)));
			
			// i kind of want to make it clearer that the range increase is toward's the fan's base and not away, but i'm not sure how to say that exactly..
			// i hope the debug vis shows that good enough anyways?
			properties[3] = new PropertySpec("Range", typeof(int), "Extended",
				"The vertical range of the Fan's hitbox.", null, new Dictionary<string, int>
				{
					{ "Short", 0 },
					{ "Tall", 4 }
				},
				(obj) => obj.PropertyValue & 4,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~4) | (int)value));
		}
		
		// default subtypes can't really set dir so i'm not 100% sure on this.. but its' better than nothing ig?
		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {0, 1, 2, 3, 4, 5, 6, 7}); }
		}
		
		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
		}
		
		public override string SubtypeName(byte subtype)
		{
			string name = ((subtype & 1) == 0) ? "Blow Right" : "Blow Left";
			if ((subtype & 2) == 2) name += " (Always Active)";
			if ((subtype & 4) == 4) name += " (Taller Box)"; // sounds kinda weird but hope it's fine enough
			return name;
		}

		public override Sprite Image
		{
			get { return sprites[0, 0]; }
		}

		public override Sprite SubtypeImage(byte subtype)
		{
			return sprites[subtype & 1, 0]; // can't get dir, so let's stick with normal left
		}

		public override Sprite GetSprite(ObjectEntry obj)
		{
			return sprites[obj.PropertyValue & 1, (int)(((V4ObjectEntry)obj).Direction)];
		}
		
		public override Sprite GetDebugOverlay(ObjectEntry obj)
		{
			return debug[(obj.PropertyValue & 4) >> 2, (int)(((V4ObjectEntry)obj).Direction)];
		}
	}
}