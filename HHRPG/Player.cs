﻿using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using PolyOne;
using PolyOne.Animation;
using PolyOne.Collision;
using PolyOne.Engine;
using PolyOne.Input;
using PolyOne.Scenes;

namespace HHRPG
{
    public class Player : Entity
    {
        private Level level;

        private Vector2 remainder;

        private Texture2D downTexture;
        private Texture2D upTexture;
        private Texture2D leftTexture;
        private Texture2D rightTexture;

        private AnimationData downAnimationData;
        private AnimationData upAnimationData;
        private AnimationData leftAnimationData;
        private AnimationData rightAnimationData;

        private AnimationPlayer player;

        public PlayerCamera Camera = new PlayerCamera();

        private const float runAccel = 1.0f;
        private const float turnMul = 0.75f;
        private const float normMaxSpeed = 2.0f;

        private Vector2 velocity;

        public Player(Vector2 position)
            :base(position)
        {
            this.Tag((int)GameTags.Player);
            this.Collider = new Hitbox((float)20.0f, (float)32.0f, -10.0f, -16.0f);
            this.Visible = true;

            player = new AnimationPlayer();

            downTexture = Engine.Instance.Content.Load<Texture2D>("Player/DrifterDown");
            upTexture = Engine.Instance.Content.Load<Texture2D>("Player/DrifterUp");
            leftTexture = Engine.Instance.Content.Load<Texture2D>("Player/DrifterLeft");
            rightTexture = Engine.Instance.Content.Load<Texture2D>("Player/DrifterRight");


            downAnimationData = new AnimationData(downTexture, 60, 32, true);
            upAnimationData = new AnimationData(upTexture, 60, 32, true);
            leftAnimationData = new AnimationData(leftTexture, 60, 32, true);
            rightAnimationData = new AnimationData(rightTexture, 60, 32, true);


            player.PlayAnimation(downAnimationData);

            Camera.CameraTrap = new Rectangle((int)this.Right, (int)this.Bottom - 40, 40, 40);
        }

        public override void Added(Scene Scene)
        {
            base.Added(Scene);

            if (base.Scene is Level) {
                this.level = (base.Scene as Level);
            }
        }

        public override void Update()
        {
            base.Update();

            if(PolyInput.Keyboard.Check(Keys.W) || PolyInput.Keyboard.Check(Keys.Up))
            {
                player.PlayAnimation(upAnimationData);
                velocity.Y -= runAccel;
            }
            else if(PolyInput.Keyboard.Check(Keys.S) || PolyInput.Keyboard.Check(Keys.Down))
            {
                player.PlayAnimation(downAnimationData);
                velocity.Y += runAccel;
            }
            else if(PolyInput.Keyboard.Check(Keys.A) || PolyInput.Keyboard.Check(Keys.Left))
            {
                player.PlayAnimation(leftAnimationData);
                velocity.X -= runAccel;
            }
            else if(PolyInput.Keyboard.Check(Keys.D) || PolyInput.Keyboard.Check(Keys.Right))
            {
                player.PlayAnimation(rightAnimationData);
                velocity.X += runAccel;
            }
            else
            {
                velocity = Vector2.Zero;
            }

            player.Update();
            Camera.LockToTarget(this.Rectangle, Engine.VirtualWidth, Engine.VirtualHeight);
            Camera.ClampToArea((int)level.Tile.MapWidthInPixels - Engine.VirtualWidth, (int)level.Tile.MapHeightInPixels - Engine.VirtualHeight);

            velocity.X = MathHelper.Clamp(velocity.X, -normMaxSpeed, normMaxSpeed);
            MovementHorizontal(velocity.X);

            velocity.Y = MathHelper.Clamp(velocity.Y, -normMaxSpeed, normMaxSpeed);
            MovementVerical(velocity.Y);
        }

        private void MovementHorizontal(float amount)
        {
            remainder.X += amount;
            int move = (int)Math.Round((double)remainder.X);

            if (move != 0)
            {
                remainder.X -= move;
                int sign = Math.Sign(move);

                while (move != 0)
                {
                    Vector2 newPosition = Position + new Vector2(sign, 0);
                    if (this.CollideFirst((int)GameTags.Solid, newPosition) != null)
                    {
                        remainder.X = 0;
                        break;
                    }
                    Position.X += sign;
                    move -= sign;
                }
            }
        }

        private void MovementVerical(float amount)
        {
            remainder.Y += amount;
            int move = (int)Math.Round((double)remainder.Y);

            if (move < 0)
            {
                remainder.Y -= move;
                while (move != 0)
                {
                    Vector2 newPosition = Position + new Vector2(0, -1.0f);
                    if (this.CollideFirst((int)GameTags.Solid, newPosition) != null)
                    {
                        remainder.Y = 0;
                        break;
                    }
                    Position.Y += -1.0f;
                    move -= -1;
                }
            }
            else if (move > 0)
            {
                remainder.Y -= move;
                while (move != 0)
                {
                    Vector2 newPosition = Position + new Vector2(0, 1.0f);
                    if (this.CollideFirst((int)GameTags.Solid, newPosition) != null)
                    {
                        remainder.Y = 0;
                        break;
                    }

                    Position.Y += 1.0f;
                    move -= 1;
                }
            }
        }

        public override void Draw()
        {
            base.Draw();
            player.Draw(Position, 0.0f, SpriteEffects.None);
        }
    }
}
