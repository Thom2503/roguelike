using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace roguelike.render;

public class Map {
    private const int _tileWidth = 16;
    private const int _tileHeight = 16;
    private readonly AsciiTile[,] _tiles;

    private Texture2D _blankTexture;
    private Texture2D _fontTexture;

    private bool _texturesLoaded = false;

    public Map() {}
    public Map(AsciiTile[,] tiles) {
        _tiles = tiles;
    }

    public void LoadContent(Engine engine) {
        if (!_texturesLoaded) {
            _blankTexture = Texture.CreateBlankTexture(engine);
            _fontTexture = Texture.CreateFontTexture(engine);
            _texturesLoaded = true;
        }
    }

    public void Draw(GameTime gameTime, Engine engine) {
        engine.Graphics.GraphicsDevice.Clear(Color.Black);

        engine.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

        for (int y = 0; y < _tiles.GetLength(1); y++) {
            for (int x = 0; x < _tiles.GetLength(0); x++) {
                AsciiTile tile = _tiles[x, y];

                Rectangle destRect = new Rectangle(x * _tileWidth, y * _tileHeight, _tileWidth, _tileHeight);
                Rectangle srcRect = GetCharSourceRect(tile.Character);

                engine.SpriteBatch.Draw(_blankTexture, destRect, tile.Background);
                engine.SpriteBatch.Draw(_fontTexture, destRect, srcRect, tile.Foreground);
            }
        }

        engine.SpriteBatch.End();
    }

    private Rectangle GetCharSourceRect(char c) {
        int index = c;
        int cols = _fontTexture.Width / _tileWidth;
        int x = index % cols * _tileWidth;
        int y = index / cols * _tileHeight;
        return new Rectangle(x, y, _tileWidth, _tileHeight);
    }
}
