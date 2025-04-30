using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace roguelike.render;

public static class Texture {
	public static Texture2D CreateBlankTexture(Engine engine) {
		Texture2D blankTexture = new Texture2D(engine.Graphics.GraphicsDevice, 1, 1);
		blankTexture.SetData(new[] {Color.White});

		return blankTexture;
	}

	public static Texture2D CreateFontTexture(Engine engine) {
		Texture2D fontTexture;
		using (var stream = File.OpenRead("Content/sprites/ascii_tiles.png")) {
			fontTexture = Texture2D.FromStream(engine.Graphics.GraphicsDevice, stream);
		}
		return fontTexture;
	}
}