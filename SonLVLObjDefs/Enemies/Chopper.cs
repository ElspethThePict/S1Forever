using SonicRetro.SonLVL.API;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1FObjectDefinitions.Enemies
{
	class Chopper : ObjectDefinition
	{
		private Sprite sprite;
		private Sprite debug;
		
		public override void Init(ObjectData data)
		{
			if (LevelData.StageInfo.folder.EndsWith("BossRush"))
				sprite = new Sprite(LevelData.GetSpriteSheet("MBZ/Objects.gif").GetSection(106, 81, 30, 32), -14, -15);
			else if (LevelData.StageInfo.folder.EndsWith("Zone00"))
				sprite = new Sprite(LevelData.GetSpriteSheet("GHZ/Objects3.gif").GetSection(106, 81, 30, 32), -14, -15);
			else
				sprite = new Sprite(LevelData.GetSpriteSheet("GHZ/Objects.gif").GetSection(98, 94, 30, 32), -14, -15);
			
			// it's kind of hard to guess how high the Chopper will jump, so let's draw a debug vis for it
			BitmapBits bitmap = new BitmapBits(2, 265);
			bitmap.DrawLine(6, 0, 0, 0, 265); // LevelData.ColorWhite
			debug = new Sprite(bitmap, 0, -265);
		}

		public override ReadOnlyCollection<byte> Subtypes
		{
			get { return new ReadOnlyCollection<byte>(new byte[0]); }
		}
		
		public override string SubtypeName(byte subtype)
		{
			return null;
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
			return debug;
		}
	}
}