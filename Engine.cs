using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using roguelike.render;
using roguelike.player;
using System.Collections.Generic;
using roguelike.action;
using roguelike.ai;
using System;

namespace roguelike;

public class Engine : Game
{
    public GraphicsDeviceManager graphics { get; }
    public SpriteBatch spriteBatch { get; private set; }
    public Queue<GameAction> aiActions;
    public Queue<GameAction> playerActions;

    private Map map;
    private Player player;
    private List<Monster> monsters;
    private double actionTimer;
    private double actionInterval = 0.1;

    public Engine()
    {
        this.Window.AllowUserResizing = true;
        this.Window.ClientSizeChanged += new EventHandler<EventArgs>(WindowClientSizeChanged);
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        player = new Player("Gamer", 100, 1, 1);
        playerActions = new Queue<GameAction>();
        aiActions = new Queue<GameAction>();
        monsters = new List<Monster>() {
            new Monster("Mummy", 20, 4, 5),
        };
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

        actionTimer += gameTime.ElapsedGameTime.TotalSeconds;
        if (actionTimer < actionInterval) return;

        var keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.Up)) {
            this.playerActions.Enqueue(new MoveAction(0, -1, player));
        }
        else if (keyboardState.IsKeyDown(Keys.Down)) {
            this.playerActions.Enqueue(new MoveAction(0, 1, player));
        }
        else if (keyboardState.IsKeyDown(Keys.Left)) {
            this.playerActions.Enqueue(new MoveAction(-1, 0, player));
        }
        else if (keyboardState.IsKeyDown(Keys.Right)) {
            this.playerActions.Enqueue(new MoveAction(1, 0, player));
        }

        foreach (var monster in monsters) {
            if (monster.hasMoved == true) continue;
            (int x, int y) = monster.GetRandomMovement();
            this.aiActions.Enqueue(new MoveAction(x, y, monster));
            monster.hasMoved = false;
        }

        actionTimer = 0;

        if (this.playerActions.Count != 0) {
            GameAction action = this.playerActions.Dequeue();
            action.Execute();
        }
        if (this.aiActions.Count != 0) {
            GameAction action = this.aiActions.Dequeue();
            action.Execute();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
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
            monster.hasMoved = false;
        }

        map = new Map(tiles);
        map.LoadContent(this);
        map.Draw(gameTime, this);
    }

    void WindowClientSizeChanged(object sender, EventArgs e) {}
}
