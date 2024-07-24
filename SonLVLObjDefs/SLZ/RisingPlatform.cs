using SonicRetro.SonLVL.API;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System;

namespace S1ObjectDefinitions.SLZ
{
	class RisingPlatform : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[2];
		private Sprite sprite;
		private Sprite[] debug = new Sprite[15];
		
		public override void Init(ObjectData data)
		{
			sprite = new Sprite(LevelData.GetSpriteSheet("SLZ/Objects.gif").GetSection(84, 188, 80, 32), -40, -8);
			
			// The default values from RisingPlatform_distanceTable ("table28") in the object's script
			// If you've modified those, you can simply copy them over.
			int[] RisingPlatform_distanceTable = new int[15] {0x400000, 0x800000, 0xD00000, 0x400000, 0x800000, 0xD00000, 0x500000, 0x900000, 0xB00000, 0x500000, 0x900000, 0xB00000, 0x800000, 0x800000, 0xC00000};
			
			BitmapBits overlay = new BitmapBits(80, 32);
			overlay.DrawRectangle(6, 0, 0, 80 - 1, 32 - 1); // LevelData.ColorWhite
			
			for (int i = 0; i < 15; i++)
			{
				switch (i)
				{
					default:
						// Vertical
						int sign = (i % 6 < 3) ? -1 : 1;
						debug[i] = new Sprite(overlay, -40, -8 + (sign * (RisingPlatform_distanceTable[i] / 0x10000 * 2)));
						break;
						
					case 12:
					case 13:
						// Diagonal
						sign = (i == 13) ? -1 : 1;
						debug[i] = new Sprite(overlay, -40 + (sign * (RisingPlatform_distanceTable[i] / 0x10000 * 2)), -8 + (-sign * (RisingPlatform_distanceTable[i] / 0x10000)));
						break;
						
					case 14:
						// Spawner
						debug[i] = new Sprite(overlay, -40, -8 - (RisingPlatform_distanceTable[i] / 0x10000 * 2));
						break;
				}
			}
			
			// it's kinda iffy - with the top bit set, the rest of the prop val is interpreted completely differently
			// because of that, things are a little weird here...
			properties[0] = new PropertySpec("Movement", typeof(int), "Extended",
				"The Platform's movement.", null, new Dictionary<string, int>
				{
					// instead of going with numerical value, let's arrange them in order of direction/distance
					{ "Up (128px)", 0 },
					{ "Up (160px)", 6 },
					{ "Up (256px)", 1 },
					{ "Up (288px)", 7 },
					{ "Up (352px)", 8 },
					{ "Up (416px)", 2 },
					
					{ "Down (128px)", 3 },
					{ "Down (160px)", 9 },
					{ "Down (256px)", 4 },
					{ "Down (288px)", 10 },
					{ "Down (352px)", 11 },
					{ "Down (416px)", 5 },
					
					{ "Up-Right", 12 },
					{ "Down-Left", 13 },
					
					// 14 is for a platform spawned by a spawner, but those shouldn't be placed in the scene
					// 15 is technically static, but it unloads right as soon as it gets off screen, so
					
					{ "Spawner", 0x80 }
				},
				(obj) => (obj.PropertyValue < 0x80) ? (obj.PropertyValue & 15) : 0x80, // if the top bit is set then it's a spawner, otherwise look at bottom 4 bits
				(obj, value) => {
						byte val = (byte)((int)value);
						
						if (obj.PropertyValue < 0x80) // are we not a spawner?
						{
							obj.PropertyValue = val; // reset the entire prop val, including interval
							
							if (obj.PropertyValue == 0x80)
								obj.PropertyValue |= 0x0A; // let's have a starting interval of 10 frames (0 frames doesn't really play nice, even if it technically works)
						}
						else // we're a spawner
						{
							if (val < 0x80)
								obj.PropertyValue = val; // no longer a spawner, let's reset interval
						}							
					}
				);
			
			properties[1] = new PropertySpec("Interval", typeof(int), "Extended",
				"Used for Platform Spawners only. The interval, in frames, at which new platforms should spawn.", null,
				(obj) => (obj.PropertyValue > 0x80) ? ((obj.PropertyValue & 0x7f) * 6) : -1, // only show it for Spawners, otherwise make it 0
				(obj, value) => {
						if (obj.PropertyValue >= 0x80) // only set it for spawners
							obj.PropertyValue = (byte)(0x80 | ((int)value / 6));
					}
				);
		}

		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {0, 6, 1, 7, 8, 2, 3, 9, 4, 10, 11, 5, 12, 13, 0x85, 0x8A, 0x8F, 0x94, 0x99}); }
		}
		
		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
		}

		public override string SubtypeName(byte subtype)
		{
			if (subtype < 0x80)
			{
				switch (subtype & 15)
				{
					case 0: return "Up (128px)";
					case 1: return "Up (256px)";
					case 2: return "Up (416px)";
					case 3: return "Down (128px)";
					case 4: return "Down (256px)";
					case 5: return "Down (416px)";
					case 6: return "Up (160px)";
					case 7: return "Up (288px)";
					case 8: return "Up (352px)";
					case 9: return "Down (160px)";
					case 10: return "Down (288px)";
					case 11: return "Down (352px)";
					case 12: return "Up-Right";
					case 13: return "Down-Left";
					case 14: return "Disappear"; // this should never be reached
					
					default: return "Static"; // well yeah it's technically this but it dies upon going offscreen, so
				}
			}
			else
				return "Spawner (" + ((subtype & 0x7f) * 6) + " Frame Interval)";
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
			int index = (obj.PropertyValue & 15);
			if (obj.PropertyValue >= 0x80) index = 14;
			else if (index > 13) return null;
			return debug[index];
		}
	}
}