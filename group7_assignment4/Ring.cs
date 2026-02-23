using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace group7_assignment4;

public class Ring
{
    private Texture2D texture;
    private float baseScale;
    private float pulseSpeed;
    private float pulseAmount;
    private float pulseTimer;

    public Ring(Texture2D texture, float baseScale, float pulseSpeed, float pulseAmount)
    {
        this.texture = texture;
        this.baseScale = baseScale;
        this.pulseSpeed = pulseSpeed;
        this.pulseAmount = pulseAmount;
        pulseTimer = 0f;
    }

    public void Update(GameTime gameTime)
    {
        pulseTimer += pulseSpeed *
                      (float)gameTime.ElapsedGameTime.TotalSeconds; //update pulse timer
    }

    public void Draw(SpriteBatch spriteBatch, float parentRotation)
    {
        float pulse = 1f + MathF.Sin(pulseTimer) * pulseAmount; //pulse in sinusoidal motion

        spriteBatch.Draw( //draw rings around planet
            texture,
            Vector2.Zero, 
            null,
            Color.White,
            parentRotation,
            new Vector2(
                texture.Width / 2f,
                texture.Height / 2f
            ),
            baseScale * pulse,
            SpriteEffects.None,
            0f
        );
    }
}