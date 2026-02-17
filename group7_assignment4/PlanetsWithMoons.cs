using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
namespace group7_assignment4;

public class PlanetWithMoons
{
    private Texture2D planetTexture;
    private Vector2 sunPosition;

    private float orbitRadius;
    private float orbitSpeed;
    private float rotationSpeed;

    private float orbitAngle;
    private float selfRotation;

    private float planetScale;
    private Color planetColor;


    // Moon children
    private List<Moon> moons;
    private List<float> moonAngleOffsets;

    private float moonBaseAngle;
    private float moonPairSeparation;
    
    public PlanetWithMoons(
        Texture2D planetTexture,
        Texture2D moonTexture,
        Vector2 sunPosition,
        float orbitRadius,
        float orbitSpeed,
        float rotationSpeed,
        float planetScale,
        Color planetColor,
        int moonCount
    )
    {
        this.planetTexture = planetTexture;
        this.sunPosition = sunPosition;
        this.orbitRadius = orbitRadius;
        this.orbitSpeed = orbitSpeed;
        this.rotationSpeed = rotationSpeed;
        this.planetScale = planetScale;
        this.planetColor = planetColor;

        orbitAngle = (float)(new Random().NextDouble() * Math.PI * 2);
        selfRotation = 0f;

        moonBaseAngle = 0f;
        moonPairSeparation = 0.7f; // radians; keeps 2 moons traveling as a visible pair

        moons = new List<Moon>();
        moonAngleOffsets = new List<float>();

        for (int i = 0; i < moonCount; i++)
        {
            moons.Add(new Moon(moonTexture));

            moonAngleOffsets.Add(i * moonPairSeparation);
        }
    }
    
    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        orbitAngle += orbitSpeed * dt;
        selfRotation += rotationSpeed * dt;

        moonBaseAngle += 1.2f * dt;

        for (int i = 0; i < moons.Count; i++)
        {
            moons[i].Update(gameTime);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Matrix orbitRotation = Matrix.CreateRotationZ(orbitAngle);
        Matrix orbitTranslation = Matrix.CreateTranslation(orbitRadius, 0f, 0f);
        Matrix moveToSun = Matrix.CreateTranslation(sunPosition.X, sunPosition.Y, 0f);

        // Order matters: local → orbit → world
        Matrix world =
            orbitTranslation *
            orbitRotation *
            moveToSun;

        // End any existing batch before applying transform
        spriteBatch.End();

        // Begin with transform matrix
        spriteBatch.Begin(transformMatrix: world);

        // Draw planet at local origin (0,0) because matrix handles positioning
        spriteBatch.Draw(
            planetTexture,
            Vector2.Zero,
            null,
            planetColor,
            selfRotation,
            new Vector2(
                planetTexture.Width / 2f,
                planetTexture.Height / 2f
            ),
            planetScale,
            SpriteEffects.None,
            0f
        );

        for (int i = 0; i < moons.Count; i++)
        {
            // Keep moons close to the planet by basing orbit distance on the planet's rendered size
            float moonOrbitRadius = (planetTexture.Width * planetScale * 0.01f);
            float moonAngle = moonBaseAngle + moonAngleOffsets[i];

            Matrix moonOrbit =
                Matrix.CreateRotationZ(moonAngle) *
                Matrix.CreateTranslation(moonOrbitRadius, 0f, 0f);

            // Draw this moon using a child transform under the planet's world transform
            spriteBatch.End();
            spriteBatch.Begin(transformMatrix: moonOrbit * world);

            moons[i].Draw(spriteBatch, Vector2.Zero);

            // Restore the planet transform for the next moon / any additional drawing
            spriteBatch.End();
            spriteBatch.Begin(transformMatrix: world);
        }

        spriteBatch.End();

        // Restart normal batch so other objects render correctly
        spriteBatch.Begin();
    }
}