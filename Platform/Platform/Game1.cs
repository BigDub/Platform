using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Platform
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>

    enum GameState
    {
        MainMenu,
        Loading,
        Playing,
        Paused,
        Editor
    };

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameState gameState = GameState.Editor;
        Effect e;
        float Elapsed;
        Level currentLevel;
        Vector3 cameraPosition = new Vector3(5,10,10);
        Point cursor;
        KeyboardState ks, pks;
        Model testModel;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            e = Content.Load<Effect>("StandardEffect");
            e.Parameters["World"].SetValue(Matrix.Identity);
            e.Parameters["View"].SetValue(Matrix.CreateLookAt(cameraPosition, cameraPosition - Vector3.UnitZ, Vector3.UnitY));
            e.Parameters["Projection"].SetValue(Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, 4 / 3, 0.1f, 30));

            testModel = Content.Load<Model>("Character");

            global.e = e;
            global.content = Content;

            Tileset installation = Tileset.LoadTileset("Content\\Installation.txt");
            currentLevel = Level.LoadLevel("Content\\Level.txt", installation);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            Elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            pks = ks;
            ks = Keyboard.GetState();
            switch (gameState)
            {
                case(GameState.Editor):
                    updateEDITOR();
                    break;
            }

            base.Update(gameTime);
        }

        private void updateEDITOR()
        {
            #region keys
            if (ks.IsKeyDown(Keys.Left) && pks.IsKeyUp(Keys.Left))
                cursor.X = (cursor.X > 0) ? cursor.X - 1 : 0;
            if (ks.IsKeyDown(Keys.Right) && pks.IsKeyUp(Keys.Right))
                cursor.X = (cursor.X < currentLevel.Width - 1) ? cursor.X + 1 : currentLevel.Width - 1;
            if (ks.IsKeyDown(Keys.Up) && pks.IsKeyUp(Keys.Up))
                cursor.Y = (cursor.Y > 0) ? cursor.Y - 1 : 0;
            if (ks.IsKeyDown(Keys.Down) && pks.IsKeyUp(Keys.Down))
                cursor.Y = (cursor.Y < currentLevel.Height - 1) ? cursor.Y + 1 : currentLevel.Height - 1;
            if (ks.IsKeyDown(Keys.RightAlt) && pks.IsKeyUp(Keys.RightAlt))
                currentLevel.data[cursor.X, cursor.Y] = (Byte)((currentLevel.data[cursor.X, cursor.Y] > 0) ? currentLevel.data[cursor.X, cursor.Y] - 1 : currentLevel.tileset.Length - 1);
            if (ks.IsKeyDown(Keys.RightControl) && pks.IsKeyUp(Keys.RightControl))
                currentLevel.data[cursor.X, cursor.Y] = (Byte)((currentLevel.data[cursor.X, cursor.Y] < currentLevel.tileset.Length - 1) ? currentLevel.data[cursor.X, cursor.Y] + 1 : 0);
            if (ks.IsKeyDown(Keys.LeftControl) && ks.IsKeyDown(Keys.S) && pks.IsKeyUp(Keys.S))
                currentLevel.Save();
            Vector3 movement = new Vector3();
            if (ks.IsKeyDown(Keys.W))
                movement.Y += 1;
            if (ks.IsKeyDown(Keys.S) && ks.IsKeyUp(Keys.LeftControl))
                movement.Y -= 1;
            if (ks.IsKeyDown(Keys.A))
                movement.X -= 1;
            if (ks.IsKeyDown(Keys.D))
                movement.X += 1;
            if (ks.IsKeyDown(Keys.Q))
                movement.Z -= 1;
            if (ks.IsKeyDown(Keys.E))
                movement.Z += 1;
            if (movement.Length() != 0)
            {
                movement.Normalize();
                movement *= 3 * Elapsed;
                cameraPosition += movement;
                e.Parameters["View"].SetValue(Matrix.CreateLookAt(cameraPosition, cameraPosition - Vector3.UnitZ, Vector3.UnitY));
            }
            #endregion
            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            currentLevel.Draw();
            //testModel.Draw(Matrix.CreateRotationX(-MathHelper.PiOver2) * Matrix.CreateTranslation(4, 2, 0), Matrix.CreateLookAt(cameraPosition, cameraPosition - Vector3.UnitZ, Vector3.UnitY), Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, 4 / 3, 0.1f, 10));

            base.Draw(gameTime);
        }
    }
}
