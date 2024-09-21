using SonicRetro.SonLVL.API;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace S1ObjectDefinitions.SBZ
{
	class Transporter : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[1];
		private Sprite sprite;

		public override void Init(ObjectData data)
		{
			sprite = new Sprite(LevelData.GetSpriteSheet("Global/Display.gif").GetSection(239, 239, 16, 16), -8, -8);
			
			// we can't make debug visualisations on startup because the start is dependant on the transporter's starting position, so
			
			properties[0] = new PropertySpec("Path", typeof(int), "Extended",
				"Which path this Transporter should lead the player.", null, new Dictionary<string, int>
				{
					{ "Straight Down (Long)", 0 },
					{ "Straight Down", 1 },
					{ "Up, Right, Up", 2 },
					{ "Straight Down (Short)", 3 },
					{ "Down, Left, Down", 4 },
					{ "Straight Up", 5 },
					{ "Up, Left, Up", 6 },
					{ "Straight Up (50 Rings)", 7 }
				},
				(obj) => (int)obj.PropertyValue,
				(obj, value) => obj.PropertyValue = (byte)((int)value));
		}

		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {0, 1, 2, 3, 4, 5, 6, 7}); }
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
			switch (subtype)
			{
				case 0:
					return "Straight Down (Long)";
				case 1:
					return "Straight Down";
				case 2:
					return "Up, Right, Up";
				case 3:
					return "Straight Down (Short)";
				case 4:
					return "Down, Left, Down";
				case 5:
					return "Straight Up";
				case 6:
					return "Up, Left, Up";
				case 7:
					return "Straight Up (50 Rings)";
				default:
					return "Unknown";
			}
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
			// this sucks btw
			
			// Copied from the original script
			int[][] movementTables = new int[][] {
				new int[] { 0x7940000, 0x98C0000 },
				new int[] { 0x940000, 0x38C0000 },
				new int[] { 0x7940000, 0x2E80000,
				  0x7A40000, 0x2C00000,
				  0x7D00000, 0x2AC0000,
				  0x8580000, 0x2AC0000,
				  0x8840000, 0x2980000,
				  0x8940000, 0x2700000,
				  0x8940000, 0x1900000 },
				new int[] { 0x8940000, 0x6900000 },
				new int[] { 0x11940000, 0x4700000,
				  0x11840000, 0x4980000,
				  0x11580000, 0x4AC0000,
				  0x0FD00000, 0x4AC0000,
				  0x0FA40000, 0x4C00000,
				  0x0F940000, 0x4E80000,
				  0x0F940000, 0x5900000 },
				new int[] { 0x12940000, 0x4900000 },
				new int[] { 0x15940000, 0x7E80000,
				  0x15840000, 0x7C00000,
				  0x15600000, 0x7AC0000,
				  0x14D00000, 0x7AC0000,
				  0x14A40000, 0x7980000,
				  0x14940000, 0x7700000, // looks weird (goes from the top of the level to the bottom) but idk what to do about it, so..
				  0x14940000, 0x5900000 },
				new int[] { 0x8940000, 0x900000 }
			};
			
			if (obj.PropertyValue >= movementTables.Length)
				return null;
			
			int xmin = obj.X;
			int ymin = obj.Y;
			int xmax = obj.X;
			int ymax = obj.Y;
			
			for (int i = 0; i < movementTables[obj.PropertyValue].Length; i += 2)
			{
				xmin = Math.Min(xmin, movementTables[obj.PropertyValue][i] >> 16);
				ymin = Math.Min(ymin, movementTables[obj.PropertyValue][i+1] >> 16);
				xmax = Math.Max(xmax, movementTables[obj.PropertyValue][i] >> 16);
				ymax = Math.Max(ymax, movementTables[obj.PropertyValue][i+1] >> 16);
			}
			
			BitmapBits bitmap = new BitmapBits(xmax - xmin + 1, ymax - ymin + 1);
			
			bitmap.DrawLine(6, obj.X - xmin, obj.Y - ymin, (movementTables[obj.PropertyValue][0] >> 16) - xmin, (movementTables[obj.PropertyValue][1] >> 16) - ymin); // LevelData.ColorWhite
			
			for (int i = 2; i < movementTables[obj.PropertyValue].Length; i += 2)
				bitmap.DrawLine(6, (movementTables[obj.PropertyValue][i-2] >> 16) - xmin, (movementTables[obj.PropertyValue][i-1] >> 16) - ymin, (movementTables[obj.PropertyValue][i] >> 16) - xmin, (movementTables[obj.PropertyValue][i+1] >> 16) - ymin); // LevelData.ColorWhite
			
			return new Sprite(bitmap, xmin - obj.X, ymin - obj.Y);
		}
	}
}