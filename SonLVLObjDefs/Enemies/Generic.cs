using SonicRetro.SonLVL.API;
using System;

// mostly just projeciles and other basic renders which only need MBZ checks
// sorted alphabetically

namespace S1FObjectDefinitions.Enemies
{
	class BallHogBomb : Enemies.Generic
	{
		public override Sprite GetFrame()
		{
			if (LevelData.StageInfo.folder.EndsWith("BossRush"))
				return new Sprite(LevelData.GetSpriteSheet("MBZ/Objects.gif").GetSection(165, 248, 14, 14), -7, -7);
			else if (LevelData.StageInfo.folder.EndsWith("Zone00"))
				return new Sprite(LevelData.GetSpriteSheet("GHZ/Objects3.gif").GetSection(165, 248, 14, 14), -7, -7);
			else
				return new Sprite(LevelData.GetSpriteSheet("SBZ/Objects.gif").GetSection(82, 126, 14, 14), -7, -7);
		}
		
		public override bool Hidden { get { return true; } }
	}
	
	class Batbrain : Enemies.Generic
	{
		public override Sprite GetFrame()
		{
			if (LevelData.StageInfo.folder.EndsWith("BossRush"))
				return new Sprite(LevelData.GetSpriteSheet("MBZ/Objects.gif").GetSection(52, 68, 14, 24), -7, -12);
			else if (LevelData.StageInfo.folder.EndsWith("Zone00"))
				return new Sprite(LevelData.GetSpriteSheet("GHZ/Objects3.gif").GetSection(52, 68, 14, 24), -7, -12);
			else
				return new Sprite(LevelData.GetSpriteSheet("MZ/Objects.gif").GetSection(37, 98, 14, 24), -7, -12);
		}
	}
	
	class BombShrapnel : Enemies.Generic
	{
		public override Sprite GetFrame()
		{
			switch (LevelData.StageInfo.folder[LevelData.StageInfo.folder.Length-1])
			{
				case '5':
				default:
					return new Sprite(LevelData.GetSpriteSheet("SLZ/Objects.gif").GetSection(67, 170, 8, 8), -4, -4);
					break;
				case '6':
					return new Sprite(LevelData.GetSpriteSheet("SBZ/Objects.gif").GetSection(66, 79, 8, 8), -4, -4);
					break;
				case 'h': // "BossRush" (this works but yeah it's dumb, sorry-)
					return new Sprite(LevelData.GetSpriteSheet("MBZ/Objects.gif").GetSection(67, 367, 8, 8), -4, -4);
					break;
				case '0':
					return new Sprite(LevelData.GetSpriteSheet("GHZ/Objects3.gif").GetSection(67, 367, 8, 8), -4, -4);
					break;
			}
		}
		
		public override bool Hidden { get { return true; } }
	}
	
	class Burrobot : Enemies.Generic
	{
		public override Sprite GetFrame()
		{
			if (LevelData.StageInfo.folder.EndsWith("BossRush"))
				return new Sprite(LevelData.GetSpriteSheet("MBZ/Objects.gif").GetSection(88, 217, 24, 46), -12, -24);
			else if (LevelData.StageInfo.folder.EndsWith("BossRush"))
				return new Sprite(LevelData.GetSpriteSheet("GHZ/Objects3.gif").GetSection(88, 217, 24, 46), -12, -24);
			else
				return new Sprite(LevelData.GetSpriteSheet("LZ/Objects.gif").GetSection(92, 68, 24, 46), -12, -24);
		}
	}
	
	class BuzzBomberShot : Enemies.Generic
	{
		public override Sprite GetFrame()
		{
			switch (LevelData.StageInfo.folder[LevelData.StageInfo.folder.Length-1])
			{
				case '1':
				default:
					return new Sprite(LevelData.GetSpriteSheet("GHZ/Objects.gif").GetSection(160, 111, 12, 12), -6, -6);
					break;
				case '2':
					return new Sprite(LevelData.GetSpriteSheet("MZ/Objects.gif").GetSection(37, 179, 12, 12), -6, -6);
					break;
				case '3':
					return new Sprite(LevelData.GetSpriteSheet("SYZ/Objects.gif").GetSection(83, 83, 12, 12), -6, -6);
					break;
				case 'h': // "BossRush" (this works but yeah it's dumb, sorry-)
					return new Sprite(LevelData.GetSpriteSheet("MBZ/Objects.gif").GetSection(35, 51, 16, 16), -8, -8);
					break;
				case '0':
					return new Sprite(LevelData.GetSpriteSheet("GHZ/Objects3.gif").GetSection(35, 51, 16, 16), -8, -8);
					break;
			}
		}
		
		public override bool Hidden { get { return true; } }
	}
	
	class CrabmeatShot : Enemies.Generic
	{
		public override Sprite GetFrame()
		{
			switch (LevelData.StageInfo.folder[LevelData.StageInfo.folder.Length-1])
			{
				case '1':
				default:
					return new Sprite(LevelData.GetSpriteSheet("GHZ/Objects.gif").GetSection(179, 127, 12, 12), -6, -6);
					break;
				case '3':
					return new Sprite(LevelData.GetSpriteSheet("SYZ/Objects.gif").GetSection(227, 1, 12, 12), -6, -6);
					break;
				case 'h': // "BossRush" (this works but yeah it's dumb, sorry-)
					return new Sprite(LevelData.GetSpriteSheet("MBZ/Objects.gif").GetSection(47, 1, 12, 12), -6, -6);
					break;
				case '0':
					return new Sprite(LevelData.GetSpriteSheet("GHZ/Objects3.gif").GetSection(47, 1, 12, 12), -6, -6);
					break;
			}
		}
		
		public override bool Hidden { get { return true; } }
	}
	
	class MotobugExhaust : Enemies.Generic
	{
		public override Sprite GetFrame()
		{
			if (LevelData.StageInfo.folder.EndsWith("BossRush"))
				return new Sprite(LevelData.GetSpriteSheet("MBZ/Objects.gif").GetSection(211, 220, 4, 4), -2, -2);
			else if (LevelData.StageInfo.folder.EndsWith("Zone00"))
				return new Sprite(LevelData.GetSpriteSheet("GHZ/Objects3.gif").GetSection(211, 220, 4, 4), -2, -2);
			else
				return new Sprite(LevelData.GetSpriteSheet("GHZ/Objects2.gif").GetSection(143, 235, 4, 4), -2, -2);
		}
		
		public override bool Hidden { get { return true; } }
	}
	
	class NewtronFly : Enemies.Generic
	{
		public override Sprite GetFrame()
		{
			if (LevelData.StageInfo.folder.EndsWith("BossRush"))
				return new Sprite(LevelData.GetSpriteSheet("MBZ/Objects.gif").GetSection(1, 124, 39, 39), -20, -20);
			else if (LevelData.StageInfo.folder.EndsWith("Zone00"))
				return new Sprite(LevelData.GetSpriteSheet("GHZ/Objects3.gif").GetSection(1, 124, 39, 39), -20, -20);
			else
				return new Sprite(LevelData.GetSpriteSheet("GHZ/Objects2.gif").GetSection(161, 1, 39, 39), -20, -20);
		}
	}
	
	class NewtronShoot : Enemies.Generic
	{
		public override Sprite GetFrame()
		{
			if (LevelData.StageInfo.folder.EndsWith("BossRush"))
				return new Sprite(LevelData.GetSpriteSheet("MBZ/Objects.gif").GetSection(1, 164, 39, 39), -20, -20);
			else if (LevelData.StageInfo.folder.EndsWith("Zone00"))
				return new Sprite(LevelData.GetSpriteSheet("GHZ/Objects3.gif").GetSection(1, 164, 39, 39), -20, -20);
			else
				return new Sprite(LevelData.GetSpriteSheet("GHZ/Objects2.gif").GetSection(1, 1, 39, 39), -20, -20);
		}
	}
	
	class NewtronShot : Enemies.Generic
	{
		public override Sprite GetFrame()
		{
			if (LevelData.StageInfo.folder.EndsWith("BossRush"))
				return new Sprite(LevelData.GetSpriteSheet("MBZ/Objects.gif").GetSection(35, 51, 16, 16), -8, -8);
			else if (LevelData.StageInfo.folder.EndsWith("Zone00"))
				return new Sprite(LevelData.GetSpriteSheet("GHZ/Objects3.gif").GetSection(35, 51, 16, 16), -8, -8);
			else
				return new Sprite(LevelData.GetSpriteSheet("GHZ/Objects.gif").GetSection(160, 111, 12, 12), -6, -6);
		}
		
		public override bool Hidden { get { return true; } }
	}
	
	class Roller : Enemies.Generic
	{
		public override Sprite GetFrame()
		{
			if (LevelData.StageInfo.folder.EndsWith("BossRush"))
				return new Sprite(LevelData.GetSpriteSheet("MBZ/Objects.gif").GetSection(105, 30, 29, 47), -16, -33);
			else if (LevelData.StageInfo.folder.EndsWith("Zone00"))
				return new Sprite(LevelData.GetSpriteSheet("GHZ/Objects3.gif").GetSection(105, 30, 29, 47), -16, -33);
			else
				return new Sprite(LevelData.GetSpriteSheet("SYZ/Objects.gif").GetSection(1, 179, 29, 47), -16, -33);
		}
	}
	
	abstract class Generic : ObjectDefinition
	{
		private Sprite sprite;
		
		public abstract Sprite GetFrame();
		
		public override void Init(ObjectData data)
		{
			sprite = GetFrame();
		}
		
		public override System.Collections.ObjectModel.ReadOnlyCollection<byte> Subtypes
		{
			get { return new System.Collections.ObjectModel.ReadOnlyCollection<byte>(new System.Collections.Generic.List<byte>()); }
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
	}
}