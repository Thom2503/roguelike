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
using roguelike.level;

namespace roguelike;

public class Engine : Game {
	public GraphicsDeviceManager Graphics { get; }
	public SpriteBatch SpriteBatch { get; private set; }
	public Queue<GameAction> aiActions;
	public Queue<GameAction> playerActions;
	public AsciiTile[,] tiles = new AsciiTile[150, 60];
	private AsciiTile[,] baseMapTiles;

	public static AsciiTile[,] tileMap;

	private Map map;
	private MapGenerator generator;
	private readonly Player player;
	private readonly List<Monster> monsters;
	private readonly List<Villager> villagers;
	private readonly Pienus pienus;
	private GameLoop gameLoop;
	private KeyboardState _prevKeyboardState;

	private Point cameraPosition = Point.Zero;
	private const int ViewportWidthInTiles = 60;
	private const int ViewportHeightInTiles = 40;

	public Engine() {
		Window.AllowUserResizing = true;
		Window.ClientSizeChanged += new EventHandler<EventArgs>(WindowClientSizeChanged);
		Graphics = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
		IsMouseVisible = true;

		player = new Player("Gamer", 100, 2, 2);
		pienus = new Pienus("Pienus", 100, player.x - 1, player.y - 1);
		playerActions = new Queue<GameAction>();
		aiActions = new Queue<GameAction>();
		monsters = new List<Monster>() {
			new Monster("Mummy", 20, 4, 5),
			new Monster("Mummy2", 20, 6, 8),
		};
		villagers = new List<Villager>() {
			new Villager("Mummy", 20, 20, 10),
			new Villager("Mummy2", 20, 22, 15),
		};
	}

	protected override void Initialize() {
		generator = new MapGenerator();
		tiles = generator.GenerateMap();
		baseMapTiles = Map.CloneTiles(tiles);
		tileMap = baseMapTiles;

		gameLoop = new GameLoop();
		GameLoop.instance = gameLoop;
		gameLoop.AddActor(player);
		gameLoop.AddActor(pienus);
		foreach (Actor monster in monsters) {
			gameLoop.AddActor(monster);
		}
		foreach (Actor villager in villagers) {
			gameLoop.AddActor(villager);
		}

		base.Initialize();
	}

	protected override void LoadContent() {
		SpriteBatch = new SpriteBatch(GraphicsDevice);
	}

	protected override void Update(GameTime gameTime) {
		if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
			Exit();

		int halfWidth = ViewportWidthInTiles / 2;
		int halfHeight = ViewportHeightInTiles / 2;
		cameraPosition.X = Math.Clamp(player.x - halfWidth, 0, 150 - ViewportWidthInTiles);
		cameraPosition.Y = Math.Clamp(player.y - halfHeight, 0, 60 - ViewportHeightInTiles);

		var keyboardState = Keyboard.GetState();
		player.SetInput(keyboardState, _prevKeyboardState);
		_prevKeyboardState = keyboardState;

		gameLoop.Process();

		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime) {
		base.Draw(gameTime);

		tiles = Map.CloneTiles(baseMapTiles);

		DrawActors();

		map = new Map(tiles);
		map.LoadContent(this);
		map.Draw(gameTime, this, cameraPosition, ViewportWidthInTiles, ViewportHeightInTiles);
	}

	private void DrawActors() {
		foreach (Actor actor in gameLoop.GetActors()) {
			 if (actor.x >= 0 && actor.x < 150 && actor.y >= 0 && actor.y < 60) {
				tiles[actor.x, actor.y] = actor.tile;
			 }
		}
	}

	private void WindowClientSizeChanged(object sender, EventArgs e) {}
}
