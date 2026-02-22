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
    
    // star textures
    private Texture2D _core;
    private Texture2D _glow;
    private Star _star1;
    private Star _star2;
    private Star _shootingStar;
    
    // audio
    private SoundEffect _twinkle;
    private bool _playedTwinkle = false;
    private Song _space;

    private List<PlanetWithMoons> _planets;
    private Vector2 _sunPosition;

    private Texture2D _pixel;

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

        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });

        // Load textures
        _moonTexture = Content.Load<Texture2D>("images/moon");
        _sunTexture = Content.Load<Texture2D>("images/sun");
        _mercury = Content.Load<Texture2D>("images/mercury");
        _venus = Content.Load<Texture2D>("images/venus");
        _earth = Content.Load<Texture2D>("images/earth");
        _mars = Content.Load<Texture2D>("images/mars");
        _core = Content.Load<Texture2D>("images/star_core");
        _glow = Content.Load<Texture2D>("images/star_glow");
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
            orbitRadius: 55f,
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
            orbitRadius: 80f,
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
            orbitRadius: 115f,
            orbitSpeed: 0.7f,
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
            orbitRadius: 150f,
            orbitSpeed: 0.5f,
            rotationSpeed: 1.0f,
            planetScale: 0.03f,
            planetColor: Color.White,
            moonCount: 2
        ));
        
        // Star 1
        _star1 = new Star(
            _core,
            _glow,
            new Vector2(250, 200),
            0.4f,
            0.8f,
            3f,
            Vector2.Zero);
        
        // Star 2
        _star2 = new Star(
            _core,
            _glow,
            new Vector2(900, 750),
            0.3f,
            0.8f,
            3f,
            Vector2.Zero);
        
        
        // Shooting star
        _shootingStar = new Star(
            _core,
            _glow,
            new Vector2(0, 900),
            0.35f,
            1f,
            0f,
            new Vector2(250f, -150f)
        );
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
        
        // Update each star
        _star1.Update(gameTime);
        _star2.Update(gameTime);
        
        // Update shooting star and play twinkle effect when on screen
        _shootingStar.Update(gameTime);
        if (_shootingStar.Position.X > 0f && !_playedTwinkle)
        {
            _twinkle.Play();
            _playedTwinkle = true;
        }
        

        base.Update(gameTime);
    }

    private void DrawOrbit(Vector2 center, float radius, Color color)
    {
        const int segments = 120;
        float increment = MathHelper.TwoPi / segments;

        Vector2 prev = center + new Vector2(radius, 0f);

        for (int i = 1; i <= segments; i++)
        {
            float angle = increment * i;
            Vector2 next = center + new Vector2(
                MathF.Cos(angle) * radius,
                MathF.Sin(angle) * radius
            );

            Vector2 edge = next - prev;
            float length = edge.Length();
            float rotation = MathF.Atan2(edge.Y, edge.X);

            _spriteBatch.Draw(
                _pixel,
                prev,
                null,
                color,
                rotation,
                Vector2.Zero,
                new Vector2(length, 1f),
                SpriteEffects.None,
                0f
            );

            prev = next;
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();
        
        // Draw stars
        _star1.Draw(_spriteBatch);
        _star2.Draw(_spriteBatch);
        _shootingStar.Draw(_spriteBatch);

        // Draw faint orbit paths first (behind planets)
        Color orbitColor = new Color(150, 150, 180) * 0.2f;

        DrawOrbit(_sunPosition, 55f, orbitColor);   // Mercury
        DrawOrbit(_sunPosition, 80f, orbitColor);   // Venus
        DrawOrbit(_sunPosition, 115f, orbitColor);  // Earth
        DrawOrbit(_sunPosition, 150f, orbitColor);  // Mars

        // Draw sun and planets
        _sun.Draw(_spriteBatch);

        foreach (PlanetWithMoons planet in _planets)
        {
            planet.Draw(_spriteBatch);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}