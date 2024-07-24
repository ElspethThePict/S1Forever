using SonicRetro.SonLVL.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace S1FObjectDefinitions.LZ
{
	class WaterPlane : ObjectDefinition
	{
		private PropertySpec[] properties = new PropertySpec[3];
		private Sprite sprite;
		
		public override void Init(ObjectData data)
		{
			// the in-game vis uses a chain post icon, but let's stick with this for the editor
			sprite = new Sprite(LevelData.GetSpriteSheet("Global/Display.gif").GetSection(239, 239, 16, 16), -8, -8);
			
			properties[0] = new PropertySpec("Size", typeof(int), "Extended",
				"How long the Water Plane should be.", null,
				(obj) => obj.PropertyValue & 0x1e,
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~0x1e) | Math.Min(Math.Max((int)value & ~1, 2), 0x1e)));
			
			// TODO: finish this, or don't depending on if they change it
			
			int[] leftPlanes = {1, 0, 1, 0};
			int[] rightPlanes = {1, 0, 0, 1};
			
			properties[1] = new PropertySpec("Left Draw Order", typeof(int), "Extended",
				"Which draw layer is to the left.", null, new Dictionary<string, int>
				{
					{ "Low Layer", 0 },
					{ "High Layer", 1 }
				},
				(obj) => leftPlanes[(int)GetIndex(obj)],
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & 0x1e) | (int)value));
			
			properties[2] = new PropertySpec("Right Draw Order", typeof(int), "Extended",
				"Which draw layer is to the right.", null, new Dictionary<string, int>
				{
					{ "Low Layer", 0 },
					{ "High Layer", 1 }
				},
				(obj) => rightPlanes[(int)GetIndex(obj)],
				(obj, value) => obj.PropertyValue = (byte)((obj.PropertyValue & ~0x1e) | (int)value));
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
			return subtype + " nodes";
		}
		
		public override PropertySpec[] CustomProperties
		{
			get { return properties; }
		}
		
		// bit 0 - direction type
		// bits 1-4 - (& 0x1e) - size
		// top 3 bits - (& 0xe0) - should there be checks
		
		// left plane: low if bottom set
		// right plane: low if bottom set 
		
		// top empty & bottom empty - high right/left
		// top empty & bottom set   - low right/left
		
		// top set & bottom empty - low right, high left
		// top set & bottom set   - high right, low left
		
		static object GetIndex(ObjectEntry obj)
		{
			return (obj.PropertyValue & 1) | ((obj.PropertyValue > 0xe0) ? 2 : 0);
		}
		
		static void SetSpeed(ObjectEntry obj, object value)
		{
			
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
			int length = Math.Max(obj.PropertyValue & 0x1e, 1); // yeah a 1 length is impossible but also we don't want a blank icon
			if (length == 1)
				return sprite;
			
			List<Sprite> sprs = new List<Sprite>();
			for (int i = 0; i < length; i++)
				sprs.Add(new Sprite(sprite, 0, i * -16));
			
			return new Sprite(sprs.ToArray());
		}
	}
}