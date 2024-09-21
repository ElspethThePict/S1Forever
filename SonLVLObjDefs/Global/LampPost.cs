using SonicRetro.SonLVL.API;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace S1FObjectDefinitions.Global
{
	class LampPost : ObjectDefinition
	{
		private Sprite[] sprites = new Sprite[2];
		private PropertySpec[] properties = new PropertySpec[1];
		
		public override void Init(ObjectData data)
		{
			BitmapBits sheet = LevelData.GetSpriteSheet("Global/KnuxGreyShield.gif"); // don't mind the sheet name, i'm kind of confused too..
			sprites[0] = new Sprite(sheet.GetSection(1, 137, 16, 64), -8, -44);  // normal
			sprites[1] = new Sprite(sheet.GetSection(35, 206, 26, 46), -8, -26); // broken
			
			// worth noting here, in the base game (not S1F stuff) lamp posts use their prop val as an "ID" type of thing,
			// the value's used in the original game i think (as in the 1991 MD ver) but the 2013 remake just sorts them automatically
			// based on entity pos instead, and that carried over to S1F too
			
			properties[0] = new PropertySpec("Broken", typeof(bool), "Extended",
				"If this Lamp Post is broken or not.", null,
				(obj) => (obj.PropertyValue == 50), // idk why they chose 50 tbh but whatever works!-
				(obj, value) => obj.PropertyValue = (byte)((bool)value ? 50 : 0));
		}
		
		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {0, 50}); }
		}
		
		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
		}

		public override string SubtypeName(byte subtype)
		{
			return (subtype == 50) ? "Broken" : "Normal";
		}

		public override Sprite Image
		{
			get { return sprites[0]; }
		}

		public override Sprite SubtypeImage(byte subtype)
		{
			return sprites[(subtype == 50) ? 1 : 0];
		}

		public override Sprite GetSprite(ObjectEntry obj)
		{
			return sprites[(obj.PropertyValue == 50) ? 1 : 0];
		}
	}
}