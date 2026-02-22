using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace group7_assignment4;

public class PlanetWithMoonsAndRings
{
    // Core planet fields
    private Texture2D planetTexture;
    private Vector2 sunPosition;

    private float orbitRadius;
    private float orbitSpeed;
    private float rotationSpeed;

    private float orbitAngle;
    private float selfRotation;

    private float planetScale;
    private Color planetColor;

    // Moon hierarchy
    private List<Moon> moons;
    private List<float> moonAngleOffsets;
    private float moonBaseAngle;
    private float moonSpacing;

    // Ring hierarchy
    // private Ring ring;

    public PlanetWithMoonsAndRings(
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

        orbitAngle = 0f;
        selfRotation = 0f;

        moons = new List<Moon>();
        moonAngleOffsets = new List<float>();

        moonBaseAngle = 0f;
        moonSpacing = MathHelper.TwoPi / Math.Max(1, moonCount);

        for (int i = 0; i < moonCount; i++)
        {
            moons.Add(new Moon(moonTexture));
            moonAngleOffsets.Add(i * moonSpacing);
        }

        // ring = new Ring(...);
    }

    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        orbitAngle += orbitSpeed * dt;
        selfRotation += rotationSpeed * dt;
        moonBaseAngle += 1.2f * dt;

        foreach (var moon in moons)
            moon.Update(gameTime);

        // ring?.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Matrix orbitTransform =
            Matrix.CreateRotationZ(orbitAngle) *
            Matrix.CreateTranslation(orbitRadius, 0f, 0f) *
            Matrix.CreateTranslation(sunPosition.X, sunPosition.Y, 0f);

        spriteBatch.End();
        spriteBatch.Begin(transformMatrix: orbitTransform);

        spriteBatch.Draw(
            planetTexture,
            Vector2.Zero,
            null,
            planetColor,
            selfRotation,
            new Vector2(planetTexture.Width / 2f, planetTexture.Height / 2f),
            planetScale,
            SpriteEffects.None,
            0f
        );

        for (int i = 0; i < moons.Count; i++)
        {
            float moonOrbitRadius = planetTexture.Width * planetScale * 0.35f;
            float angle = moonBaseAngle + moonAngleOffsets[i];

            Matrix moonTransform =
                Matrix.CreateRotationZ(angle) *
                Matrix.CreateTranslation(moonOrbitRadius, 0f, 0f);

            spriteBatch.End();
            spriteBatch.Begin(transformMatrix: moonTransform * orbitTransform);

            moons[i].Draw(spriteBatch, Vector2.Zero);

            spriteBatch.End();
            spriteBatch.Begin(transformMatrix: orbitTransform);
        }

        // ring?.Draw(spriteBatch);

        spriteBatch.End();
        spriteBatch.Begin();
    }
}