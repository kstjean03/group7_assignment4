using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace group7_assignment4;

public class Moon
{
    private Texture2D texture;

    private float orbitAngle;
    private float orbitSpeed;
    private float orbitRadius;
    private float scale;

    public Moon(Texture2D texture)
    {
        this.texture = texture;

        orbitAngle = 0f;
        orbitSpeed = 1.5f;
        orbitRadius = 70f;
        scale = 0.08f;
    }

    public void Update(GameTime gameTime)
    {
        orbitAngle += orbitSpeed *
                      (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 planetPosition)
    {
        Vector2 moonPosition = planetPosition +
                               new Vector2(
                                   MathF.Cos(orbitAngle),
                                   MathF.Sin(orbitAngle)
                               ) * orbitRadius;

        spriteBatch.Draw(
            texture,
            moonPosition,
            null,
            Color.White,
            0f,
            new Vector2(
                texture.Width / 2f,
                texture.Height / 2f
            ),
            scale,
            SpriteEffects.None,
            0f
        );
    }
}