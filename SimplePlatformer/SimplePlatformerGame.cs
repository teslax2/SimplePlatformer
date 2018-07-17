using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SimplePlatformer
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class SimplePlatformerGame : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        private Jumper _jumper;
        private Board _board;
        private SpriteFont _debugFont;

        public SimplePlatformerGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.PreferredBackBufferWidth = 15 * 64;
            _graphics.PreferredBackBufferHeight = 10 * 64;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            // My Objects
            _jumper = new Jumper(Content.Load<Texture2D>("Jumper"), new Vector2(80, 80),_spriteBatch);
            var _tileTextures = new Texture2D[] {
                Content.Load<Texture2D>("Tile"),
                Content.Load<Texture2D>("TileTop"),
                Content.Load<Texture2D>("TileLeft"),
                Content.Load<Texture2D>("TileRight") };
            _board = new Board(15, 10, _tileTextures, _spriteBatch);
            _debugFont = Content.Load<SpriteFont>("DebugFont");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _jumper.Update(gameTime);
            CheckKeyboardAndReact();
            if (_board.CreatorOn) { CheckMouseAndReact(); }
        }

        private void CheckMouseAndReact()
        {
            MouseState state = Mouse.GetState();
            if(state.LeftButton == ButtonState.Pressed) { Board.CurrentBoard.DrawTileByMouse(new Vector2(state.X, state.Y),true); }
            if (state.RightButton == ButtonState.Pressed) { Board.CurrentBoard.DrawTileByMouse(new Vector2(state.X, state.Y), false); }
        }

        private void CheckKeyboardAndReact()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.F5)) { RestartGame(); }
            if (state.IsKeyDown(Keys.F6)) { TurnOnCreator(); }
            if (state.IsKeyDown(Keys.F7)) { TurnOffCreator(); }
            if (state.IsKeyDown(Keys.Escape)) { Exit(); }
        }

        private void TurnOnCreator()
        {
            _board.ClearAllInnerTiles();
            _board.CreatorOn = true;
            IsMouseVisible = true;
        }

        private void TurnOffCreator()
        {
            _board.CreatorOn = false;
            IsMouseVisible = false;
        }

        private void RestartGame()
        {
            Board.CurrentBoard.CreateNewBoard();
            PutJumperInLeftCorner();
        }

        private void PutJumperInLeftCorner()
        {
            _jumper.Position = Vector2.One * 80;
            _jumper.Movement = Vector2.Zero;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            string positionInText = string.Format("Position of Jumper: ({0:0.0},{1:0.0})", _jumper.Position.X, _jumper.Position.Y);
            string movementInText = string.Format("Current Movement: ({0:0.0},{1:0.0})", _jumper.Movement.X, _jumper.Movement.Y);
            GraphicsDevice.Clear(Color.WhiteSmoke);
            _spriteBatch.Begin();
            base.Draw(gameTime);
            _board.Draw();
            _spriteBatch.DrawString(_debugFont, positionInText, new Vector2(10, 0), Color.White);
            _spriteBatch.DrawString(_debugFont, movementInText, new Vector2(10, 20), Color.White);
            _jumper.Draw();
            _spriteBatch.End();
        }
    }
}
