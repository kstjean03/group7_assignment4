using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace group7_assignment4;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    // planet textures
    private Texture2D _mercury;
    private Texture2D _venus;
    private Texture2D _earth;
    private Texture2D _mars;
    // moon + sun textures
    private Texture2D _moonTexture;
    private Texture2D _sunTexture;
    private Sun _sun;
    // audio
    private SoundEffect _twinkle;
    private Song _space;

    private List<PlanetWithMoons> _planets;
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

        // Ensure sun stays fixed at the center after applying window size
        _sunPosition = new Vector2(
            _graphics.PreferredBackBufferWidth / 2f,
            _graphics.PreferredBackBufferHeight / 2f
        );

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Load textures
        _moonTexture = Content.Load<Texture2D>("images/moon");
        _sunTexture = Content.Load<Texture2D>("images/sun");
        _mercury = Content.Load<Texture2D>("images/mercury");
        _venus = Content.Load<Texture2D>("images/venus");
        _earth = Content.Load<Texture2D>("images/earth");
        _mars = Content.Load<Texture2D>("images/mars");
        // Load audio
        _twinkle = Content.Load<SoundEffect>("audio/twinkle");
        _space = Content.Load<Song>("audio/space");
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Volume = 0.25f;
        MediaPlayer.Play(_space);
        // Center of the screen acts as the sun
        _sunPosition = new Vector2(
            GraphicsDevice.Viewport.Width / 2f,
            GraphicsDevice.Viewport.Height / 2f
        );

        _sun = new Sun(
            _sunTexture,
            _sunPosition,
            scale: 0.35f
        );

        _planets = new List<PlanetWithMoons>();

        // Mercury (no moons)
        _planets.Add(new PlanetWithMoons(
            _mercury,
            _moonTexture,
            _sunPosition,
            orbitRadius: 70f,
            orbitSpeed: 1.2f,
            rotationSpeed: 1.0f,
            planetScale: 0.006f,
            planetColor: Color.White,
            moonCount: 0
        ));

        // Venus (no moons)
        _planets.Add(new PlanetWithMoons(
            _venus,
            _moonTexture,
            _sunPosition,
            orbitRadius: 110f,
            orbitSpeed: 1.0f,
            rotationSpeed: 1.0f,
            planetScale: 0.05f,
            planetColor: Color.White,
            moonCount: 0
        ));

        // Earth (1 moon)
        _planets.Add(new PlanetWithMoons(
            _earth,
            _moonTexture,
            _sunPosition,
            orbitRadius: 160f,
            orbitSpeed: 0.6f,
            rotationSpeed: 1.0f,
            planetScale: 0.04f,
            planetColor: Color.White,
            moonCount: 1
        ));

        // Mars (2 moons)
        _planets.Add(new PlanetWithMoons(
            _mars,
            _moonTexture,
            _sunPosition,
            orbitRadius: 230f,
            orbitSpeed: 0.7f,
            rotationSpeed: 1.0f,
            planetScale: 0.03f,
            planetColor: Color.White,
            moonCount: 2
        ));
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // Update sun first so planets can orbit relative to a stable center
        _sun.Update(gameTime);
        foreach (PlanetWithMoons planet in _planets)
        {
            planet.Update(gameTime);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();

        // Draw sun first as the root of the solar system hierarchy
        _sun.Draw(_spriteBatch);
        foreach (PlanetWithMoons planet in _planets)
        {
            planet.Draw(_spriteBatch);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}