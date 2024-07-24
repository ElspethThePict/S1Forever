using SonicRetro.SonLVL.API;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1FObjectDefinitions.GHZ
{
	class Bridge : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[1];
		private Sprite sprite;
		
		public override void Init(ObjectData data)
		{
			// #Forever - "BossRush" folder check
			if (LevelData.StageInfo.folder.EndsWith("BossRush")) // not sure if we should keep using EndsWith since Forever is based off of pre-Origins? may as well keep it like this anyways though, in case they update it
				sprite = new Sprite(LevelData.GetSpriteSheet("MBZ/Objects.gif").GetSection(198, 233, 16, 16), -8, -8);
			else
				sprite = new Sprite(LevelData.GetSpriteSheet("GHZ/Objects.gif").GetSection(1, 1, 16, 16), -8, -8);
			
			properties[0] = new PropertySpec("Length", typeof(int), "Extended",
				"How long the Bridge should be.", null,
				(obj) => obj.PropertyValue,
				(obj, value) => obj.PropertyValue = (byte)((int)value));
		}

		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[] {6, 8, 10, 12, 14, 16}); } // it can be any value, but why not give a few starting ones
		}
		
		public override byte DefaultSubtype
		{
			get { return 12; }
		}

		public override string SubtypeName(byte subtype)
		{
			return subtype + " logs";
		}
		
		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
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
			
			int st = -(((obj.PropertyValue) * 16) / 2) + 8;
			List<Sprite> sprs = new List<Sprite>();
			for (int i = 0; i < obj.PropertyValue; i++)
				sprs.Add(new Sprite(sprite, st + (i * 16), 0));
			
			return new Sprite(sprs.ToArray());
		}
	}
}