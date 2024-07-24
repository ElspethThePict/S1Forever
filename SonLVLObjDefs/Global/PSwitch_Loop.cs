using SonicRetro.SonLVL.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1ObjectDefinitions.Global
{
	class PSwitch_Loop : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[1];
		private Sprite sprite;
		
		public override void Init(ObjectData data)
		{
			sprite = new Sprite(LevelData.GetSpriteSheet("Global/Display.gif").GetSection(222, 239, 16, 16), -8, -8);
			
			properties[0] = new PropertySpec("Size", typeof(int), "Extended",
				"How tall the Plane Switch should be.", null,
				(obj) => (obj.PropertyValue + 1),
				(obj, value) => obj.PropertyValue = (byte)(Math.Min((int)value - 1, 0)));
		}
		
		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {3, 5, 7, 9, 11}); }
		}
		
		public override byte DefaultSubtype
		{
			get { return 3; }
		}
		
		// this obj is unused and almost directly inferior to PSwitch_V's anyways, so..
		public override bool Hidden
		{
			get { return true; }
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
			return (subtype + 1) + " Nodes";
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
			if (obj.PropertyValue <= 1)
				return sprite;
			
			int count = obj.PropertyValue + 1;
			int sy = -(count * 8) + 8;
			List<Sprite> sprs = new List<Sprite>();
			for (int i = 0; i < count; i++)
				sprs.Add(new Sprite(sprite, 0, sy + (i * 16)));
			
			return new Sprite(sprs.ToArray());
		}
	}
}