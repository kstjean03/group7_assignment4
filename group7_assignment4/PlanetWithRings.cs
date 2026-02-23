using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
namespace group7_assignment4;

public class PlanetWithRings
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

    // Ring children
    private List<Ring> rings;

    public PlanetWithRings(
        Texture2D planetTexture,
        List<(Texture2D texture, float scale)> ringData,
        Vector2 sunPosition,
        float orbitRadius,
        float orbitSpeed, //create variables
        float rotationSpeed,
        float planetScale,
        Color planetColor
    )
    {
        this.planetTexture = planetTexture;
        this.sunPosition = sunPosition;
        this.orbitRadius = orbitRadius;
        this.orbitSpeed = orbitSpeed;
        this.rotationSpeed = rotationSpeed;
        this.planetScale = planetScale;
        this.planetColor = planetColor;

        orbitAngle = (float)(new Random().NextDouble() * Math.PI * 2); //calculate orbit angle
        selfRotation = 0f;

        rings = new List<Ring>(); //create list of rings
        
        foreach (var data in ringData)
        {
            rings.Add(
                new Ring(
                    data.texture, //add rings to planets
                    data.scale,
                    2f,
                    0.08f
                )
            );
        }
    }

    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        orbitAngle += orbitSpeed * dt;
        selfRotation += rotationSpeed * dt;
        foreach (var ring in rings) //update each ring to planet
            ring.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Matrix orbitRotation = Matrix.CreateRotationZ(orbitAngle);
        Matrix orbitTranslation = Matrix.CreateTranslation(orbitRadius, 0f, 0f); //Create matrix
        Matrix moveToSun = Matrix.CreateTranslation(sunPosition.X, sunPosition.Y, 0f);
        Matrix world =
            orbitTranslation *
            orbitRotation *
            moveToSun;
        spriteBatch.End();
        
        spriteBatch.Begin(transformMatrix: world);
        spriteBatch.Draw(
            planetTexture,
            Vector2.Zero, //draw planet
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
        
        foreach (var ring in rings)
        {
            ring.Draw(spriteBatch, selfRotation); //draw ring on top of planet
        }

        spriteBatch.End();
        spriteBatch.Begin();
    }
}