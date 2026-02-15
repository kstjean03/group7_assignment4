using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace group7_assignment4;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Texture2D _gasPlanetTexture;
    private Texture2D _rockyPlanetTexture;
    private Texture2D _moonTexture;
    private Texture2D _sunTexture;
    private Sun _sun;

    private PlanetWithMoons _testPlanet;
    private Vector2 _sunPosition;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 1400;
        _graphics.PreferredBackBufferHeight = 900;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        _graphics.ApplyChanges();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Load textures
        _gasPlanetTexture = Content.Load<Texture2D>("images/gasPlanet");
        _rockyPlanetTexture = Content.Load<Texture2D>("images/rockyPlanet");
        _moonTexture = Content.Load<Texture2D>("images/moon");
        _sunTexture = Content.Load<Texture2D>("images/sun");

        // Center of the screen acts as the sun
        _sunPosition = new Vector2(
            GraphicsDevice.Viewport.Width / 2f,
            GraphicsDevice.Viewport.Height / 2f
        );

        _sun = new Sun(
            _sunTexture,
            _sunPosition,
            scale: 0.55f
        );

        // Create a test planet with moons
        _testPlanet = new PlanetWithMoons(
            _rockyPlanetTexture,
            _moonTexture,
            _sunPosition,
            orbitRadius: 160f,
            orbitSpeed: 0.6f,
            rotationSpeed: 1.2f,
            planetScale: 0.2f,
            planetColor: Color.White,
            moonCount: 2
        );
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _sun.Update(gameTime);
        _testPlanet.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();

        _sun.Draw(_spriteBatch);
        _testPlanet.Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}