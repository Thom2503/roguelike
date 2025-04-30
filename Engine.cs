using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using roguelike.render;
using roguelike.player;
using System.Collections.Generic;
using roguelike.action;
using roguelike.ai;
using System;
using roguelike.core;

namespace roguelike;

public class Engine : Game {
    public GraphicsDeviceManager Graphics { get; }
    public SpriteBatch SpriteBatch { get; private set; }
    public Queue<GameAction> aiActions;
    public Queue<GameAction> playerActions;

    private Map map;
    private readonly Player player;
    private readonly List<Monster> monsters;
    private double actionTimer;
    private readonly double actionInterval = 0.1;
    private GameLoop gameLoop;
    private KeyboardState _prevKeyboardState;

    public Engine() {
        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += new EventHandler<EventArgs>(WindowClientSizeChanged);
        Graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        player = new Player("Gamer", 100, 2, 2);
        playerActions = new Queue<GameAction>();
        aiActions = new Queue<GameAction>();
        monsters = new List<Monster>() {
            new Monster("Mummy", 20, 4, 5),
            new Monster("Mummy2", 20, 6, 8),
        };
    }

    protected override void Initialize() {
        map = new Map();
        gameLoop = new GameLoop();
        gameLoop.AddActor(player);
        gameLoop.AddActor(new Pienus("Pienus", 100, player.x - 1, player.y - 1));
        foreach (Actor monster in monsters) {
            gameLoop.AddActor(monster);
        }
        base.Initialize();
    }

    protected override void LoadContent() {
        SpriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime) {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        actionTimer += gameTime.ElapsedGameTime.TotalSeconds;
        if (actionTimer < actionInterval) return;

        var keyboardState = Keyboard.GetState();
        player.SetInput(keyboardState, _prevKeyboardState);
        _prevKeyboardState = keyboardState;

        actionTimer = 0;
        gameLoop.Process();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        base.Draw(gameTime);

        AsciiTile[,] tiles = new AsciiTile[75, 30];

        for (int y = 0; y < 30; y++) {
            for (int x = 0; x < 75; x++) {
                bool isBorder = x == 0 || y == 0 || x == 75 || y == 30;

                tiles[x, y] = new AsciiTile {
                    Character = isBorder ? '#' : '.',
                    Foreground = isBorder ? Color.White : Color.LightGray,
                    Background = isBorder ? Color.DarkSlateGray : Color.Black
                };
            }
        }
        if (player.x >= 0 && player.x < 75 && player.y >= 0 && player.y < 30)
            tiles[player.x, player.y] = player.tile;
        foreach (var monster in monsters) {
            if (monster.x >= 0 && monster.x < 50 && monster.y >= 0 && monster.y < 20)
                tiles[monster.x, monster.y] = monster.tile;
        }

        map = new Map(tiles);
        map.LoadContent(this);
        map.Draw(gameTime, this);
    }

    void WindowClientSizeChanged(object sender, EventArgs e) {}
}
