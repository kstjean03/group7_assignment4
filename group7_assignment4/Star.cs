using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
namespace group7_assignment4;

public class Star
{
    private Texture2D core;
    private Texture2D glow;
    
    public Vector2 Position;    
    public Vector2 Velocity;
    public float Scale;
    public float Brightness;
    public float GlowSpeed;                     // Star twinkle speed
    
    private float phase;                        // Star twinkle state
    
    private Vector2 coreOrigin;
    private Vector2 glowOrigin;

    public bool matrixMovement = false;
    private Matrix move;
    
    public Star(
        Texture2D coreTexture,
        Texture2D glowTexture,
        Vector2 position,
        float scale,
        float brightness,
        float glowSpeed,
        Vector2 velocity,
        bool useMatrixMovement = false)
    {
        core = coreTexture;
        glow = glowTexture;

        Position = position;
        Scale = scale;
        Brightness = brightness;
        GlowSpeed = glowSpeed;
        Velocity = velocity;
        
        matrixMovement = useMatrixMovement;
        phase = 0f;
        
        coreOrigin = new Vector2(core.Width / 2f, core.Height / 2f);
        glowOrigin = new Vector2(glow.Width / 2f, glow.Height / 2f);
        
        move = Matrix.CreateTranslation(Position.X, Position.Y, 0f);
    }
    
    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        phase += GlowSpeed * dt;

        Position += Velocity * dt; 
            
        if (matrixMovement)
            move *= Matrix.CreateTranslation(Velocity.X * dt, Velocity.Y * dt, 0.0f);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Vector2 position;
        
        if (matrixMovement)
            position = Vector2.Transform(Vector2.Zero, move);
        else
            position = Position;
        
        float glowAlpha = (float)Math.Abs(Math.Sin(phase));
        float glowPulse = 1f + glowAlpha * .25f;
        
        // Draw the outer glow
        spriteBatch.Draw(
            glow,
            position,
            null,
            Color.White * glowAlpha,
            0f,
            glowOrigin,
            Scale * glowPulse,
            SpriteEffects.None,
            0f
        );
        
        // Draw the core
        spriteBatch.Draw(
            core,
            position,
            null,
            Color.White * Brightness,
            0f,
            coreOrigin,
            Scale,
            SpriteEffects.None,
            0f
        );
    }
}