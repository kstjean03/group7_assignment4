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

    // Constructor
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

        orbitAngle = 0f;
        selfRotation = 0f;

        moons = new List<Moon>();
        for (int i = 0; i < moonCount; i++)
        {
            moons.Add(new Moon(moonTexture));
        }
    }

    // Update
    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        orbitAngle += orbitSpeed * dt;
        selfRotation += rotationSpeed * dt;

        foreach (Moon moon in moons)
        {
            moon.Update(gameTime);
        }
    }

    // Draw
    public void Draw(SpriteBatch spriteBatch)
    {
        // Planet position relative to the sun
        Vector2 planetPosition = sunPosition +
            new Vector2(
                MathF.Cos(orbitAngle),
                MathF.Sin(orbitAngle)
            ) * orbitRadius;

        // Draw planet
        spriteBatch.Draw(
            planetTexture,
            planetPosition,
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

        // Draw moons (hierarchical children)
        foreach (Moon moon in moons)
        {
            moon.Draw(spriteBatch, planetPosition);
        }
    }
}