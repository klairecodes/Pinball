using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pinball;

public class Game1 : Game
{
    private float fixedUpdateDelta = (int)(1000 / (float)60); // 60 FPS
    Texture2D ballTexture;
    Vector2 ballPosition;
    float ballSpeed;
    float launchAmount;
    bool launched;
    
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 480;
        _graphics.PreferredBackBufferHeight = 800;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        ballPosition = new Vector2(_graphics.PreferredBackBufferWidth,
            _graphics.PreferredBackBufferHeight);
        ballSpeed = 1000f;
        launchAmount = 1;
        launched = false;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        ballTexture = Content.Load<Texture2D>("ball");
    }

    /// <summary>
    /// Exists to limit the running speed of the game to actual time, not the machine
    /// Thanks Lily <3
    /// </summary>
    /// <param name="gameTime"></param>
    protected void FixedUpdate(GameTime gameTime)
    {
        // TODO: Add your update logic here
        // Arrow Key Controls
        var kstate = Keyboard.GetState();
        if (kstate.IsKeyDown(Keys.Up))
        {
            ballPosition.Y -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        if (kstate.IsKeyDown(Keys.Down))
        {
            ballPosition.Y += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        if (kstate.IsKeyDown(Keys.Left))
        {
            ballPosition.X -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        if(kstate.IsKeyDown(Keys.Right))
        {
            ballPosition.X += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        
        // Confine ball to window
        if(ballPosition.X > _graphics.PreferredBackBufferWidth - ballTexture.Width / 2)
        {
            ballPosition.X = _graphics.PreferredBackBufferWidth - ballTexture.Width / 2;
        }
        else if(ballPosition.X < ballTexture.Width / 2)
        {
            ballPosition.X = ballTexture.Width / 2;
        }

        if(ballPosition.Y > _graphics.PreferredBackBufferHeight - ballTexture.Height / 2)
        {
            ballPosition.Y = _graphics.PreferredBackBufferHeight - ballTexture.Height / 2;
        }
        else if(ballPosition.Y < ballTexture.Height / 2)
        {
            ballPosition.Y = ballTexture.Height / 2;
            // if you hit a wall, stop the moving of the ball
            launchAmount = 1;
        }
        
        /* How this is going to work:
         Press Z for n time
         n = amount of time pressed
         Release Z
         Ball launches to position
         Ball stops*
         Z is now ignored)         
         */
        
        Keyboard.GetState();
        Console.WriteLine(launchAmount);
        Console.WriteLine(ballSpeed);
        if (Keyboard.IsPressed(Keys.Z))
        {
            launchAmount *= 1.10f;
        }

        var delta = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        if (!Keyboard.IsPressed(Keys.Z))
        {
            launched = true;
        }

        if (!Keyboard.IsPressed(Keys.Z) && launched && launchAmount > 1)
        {
            ballPosition.Y-= 10;
            launchAmount--;
        }
        
        if (kstate.IsKeyDown(Keys.R))
        {
            Console.WriteLine("RESET");
            ballPosition.Y = _graphics.PreferredBackBufferHeight;
            launchAmount = 1;
        }

        base.Update(gameTime);
    }
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // Check time to create a reset
        if (gameTime.ElapsedGameTime.Milliseconds - gameTime.TotalGameTime.Milliseconds < fixedUpdateDelta)
        {
            FixedUpdate(gameTime);
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        _spriteBatch.Draw(
            ballTexture,
            ballPosition,
            null,
            Color.White,
            0f,
            new Vector2(ballTexture.Width / 2, ballTexture.Height / 2),
            Vector2.One,
            SpriteEffects.None,
            0f
        );
            _spriteBatch.End();

        base.Draw(gameTime);
    }
}
