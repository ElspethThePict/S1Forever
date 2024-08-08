using SonicRetro.SonLVL.API;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace S1ObjectDefinitions.LZ
{
	class CurrentTunnel : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[1];
		private Sprite sprite;
		private Sprite debug;
		
		public override void Init(ObjectData data)
		{
			sprite = new Sprite(LevelData.GetSpriteSheet("Global/Display.gif").GetSection(165, 141, 16, 16), -8, -8);
			
			BitmapBits bitmap = new BitmapBits(2, 193);
			bitmap.DrawLine(6, 0, 0, 0, 192);
			debug = new Sprite(bitmap, 0, -96);
			
			properties[0] = new PropertySpec("Marker", typeof(int), "Extended",
				"Which type of marker this object is.", null, new Dictionary<string, int>
				{
					{ "Enterance", 0 },
					{ "Exit", 1 }
				},
				(obj) => (obj.PropertyValue > 0) ? 1 : 0,
				(obj, value) => obj.PropertyValue = (byte)((int)value));
		}
		
		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {0, 1}); }
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
			return (subtype > 0) ? "Exit Marker" : "Enterance Marker";
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
		
		bool updateOverlay = false;
		
		public override Sprite GetDebugOverlay(ObjectEntry obj)
		{
			try
			{
				// this object's kind of weird, this representation isn't really the most accurate thing ever but it's the neatest way of showing it
				// the vis kind of makes it look like it'll take the player vertically, but nope it won't do that
				// considering drawing an arrow (like R4's water currents) but i think that i'll just stick with a plain box, for now at least
				
				ObjectEntry other = LevelData.Objects[LevelData.Objects.IndexOf(obj) + ((obj.PropertyValue == 0) ? 1 : -1)];
				
				short xmin = Math.Min(obj.X, other.X);
				short ymin = Math.Min(obj.Y, other.Y);
				short xmax = Math.Max(obj.X, other.X);
				short ymax = Math.Max(obj.Y, other.Y);
				
				BitmapBits bitmap = new BitmapBits(xmax - xmin + 1, ymax - ymin + 1);
				bitmap.DrawLine(6, obj.X - xmin, obj.Y - ymin, other.X - xmin, other.Y - ymin); // LevelData.ColorWhite
				
				Sprite overlay = new Sprite(bitmap, xmin - obj.X, ymin - obj.Y - 96);
				overlay = new Sprite(overlay, new Sprite(overlay, 0, 192), debug, new Sprite(debug, other.X - obj.X, other.Y - obj.Y));
				
				// yeah this is kinda iffy, but it's the best i can think of for now..
				// (we don't want an unlimited loop where the two objects keep calling each other's UpdateDebugOverlay())
				if ((other.Type == obj.Type) && !updateOverlay)
				{
					if (other.DebugOverlay == null)
						updateOverlay = true;
					else if (!other.DebugOverlay.Size.Equals(overlay.Width))
						updateOverlay = true;
					
					if (updateOverlay)
						other.UpdateDebugOverlay();
				}
				
				updateOverlay = false;
				
				return overlay;
			}
			catch
			{
			}
			
			updateOverlay = false;
			return null;
		}
	}
}