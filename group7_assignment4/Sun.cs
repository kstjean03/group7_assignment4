using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace group7_assignment4;

public class Sun
{
    private Texture2D _texture;
    private Vector2 _position;
    private float _scale;
    private float _rotation;
    private float _pulseTime;

    public Sun(Texture2D texture, Vector2 position, float scale)
    {
        _texture = texture;
        _position = position;
        _scale = scale;
        _rotation = 0f;
        _pulseTime = 0f;
    }

    public void Update(GameTime gameTime)
    {
        float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Very slow rotation
        _rotation += delta * 0.2f;

        // Pulse timer for glow effect
        _pulseTime += delta;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        float pulseScale = 1.0f + 0.05f * (float)Math.Sin(_pulseTime * 2.0f);

        Vector2 origin = new Vector2(
            _texture.Width / 2f,
            _texture.Height / 2f
        );

        spriteBatch.Draw(
            _texture,
            _position,
            null,
            Color.White,
            _rotation,
            origin,
            _scale * pulseScale,
            SpriteEffects.None,
            0f
        );
    }
}