using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using roguelike.render;

namespace roguelike;

public class Engine : Game
{
    public GraphicsDeviceManager graphics { get; }
    public SpriteBatch spriteBatch { get; private set; }

    private Map map;

    public Engine()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        map = new Map();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        AsciiTile[,] tiles = new AsciiTile[50, 25];

        for (int y = 0; y < 25; y++) {
            for (int x = 0; x < 50; x++) {
                bool isBorder = x == 0 || y == 0 || x == 9 || y == 4;

                tiles[x, y] = new AsciiTile {
                    Character = isBorder ? '#' : '.',
                    Foreground = isBorder ? Color.White : Color.LightGray,
                    Background = isBorder ? Color.DarkSlateGray : Color.Black
                };
            }
        }

        map = new Map(tiles);
        map.LoadContent(this);
        map.Draw(gameTime, this);
    }
}
