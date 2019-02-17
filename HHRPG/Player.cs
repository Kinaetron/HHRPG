using System;
using System.Collections.Generic;
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
    public enum Horizonal
    {
        None,
        Left, 
        Right
    }

    public enum Vertical
    {
        None,
        Up, 
        Down
    }

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
        private List<Keys> keyList = new List<Keys>(new Keys[] { Keys.W, Keys.A, Keys.S, Keys.D, Keys.Up,
                                                                 Keys.Down, Keys.Left, Keys.Right });

        private bool controllerMode;
        private bool keyboardMode;

        private Vector2 velocity;

        private Vertical vertical;
        private Horizonal horizonal;

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

            vertical = Vertical.None;
            horizonal = Horizonal.None;

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

            Input();
            Movement();
            Animation();

            player.Update();
            Camera.LockToTarget(this.Rectangle, Engine.VirtualWidth, Engine.VirtualHeight);
            Camera.ClampToArea((int)level.Tile.MapWidthInPixels - Engine.VirtualWidth, (int)level.Tile.MapHeightInPixels - Engine.VirtualHeight);
        }

        private void InputMode()
        {
            foreach (Keys key in keyList)
            {
                if (PolyInput.Keyboard.Check(key) == true)
                {
                    controllerMode = false;
                    keyboardMode = true;
                }
            }
            if (PolyInput.GamePads[0].ButtonCheck() == true)
            {
                controllerMode = true;
                keyboardMode = false;
            }

            if (controllerMode == false && keyboardMode == false) {
                keyboardMode = true;
            }
        }

        private void Input()
        {
            InputMode();

            if (controllerMode == true)
            {
                if (PolyInput.GamePads[0].LeftStickHorizontal(0.3f) > 0.4f ||
                    PolyInput.GamePads[0].DPadRightCheck == true)
                {
                    horizonal = Horizonal.Right;
                }
                else if (PolyInput.GamePads[0].LeftStickHorizontal(0.3f) < -0.4f ||
                         PolyInput.GamePads[0].DPadLeftCheck == true)
                {
                    horizonal = Horizonal.Left;
                }
                else {
                    horizonal = Horizonal.None;
                }

                if (PolyInput.GamePads[0].LeftStickVertical(0.3f) > 0.4f ||
                    PolyInput.GamePads[0].DPadUpCheck == true) {
                    vertical = Vertical.Up;
                }
                else if (PolyInput.GamePads[0].LeftStickVertical(0.3f) < -0.4f ||
                         PolyInput.GamePads[0].DPadDownCheck == true) {
                    vertical = Vertical.Down;
                }
                else {
                    vertical = Vertical.None;
                }
            }
            else if (keyboardMode == true)
            {
                if (PolyInput.Keyboard.Check(Keys.Right) ||
                    PolyInput.Keyboard.Check(Keys.D))
                {
                    horizonal = Horizonal.Right;
                }
                else if (PolyInput.Keyboard.Check(Keys.Left) ||
                         PolyInput.Keyboard.Check(Keys.A))
                {
                    horizonal = Horizonal.Left;
                }
                else {
                    horizonal = Horizonal.None;
                }

                if (PolyInput.Keyboard.Check(Keys.Up) ||
                    PolyInput.Keyboard.Check(Keys.W))
                {
                    vertical = Vertical.Up;
                }
                else if (PolyInput.Keyboard.Check(Keys.Down) ||
                         PolyInput.Keyboard.Check(Keys.S)) {
                    vertical = Vertical.Down;
                }
                else {
                    vertical = Vertical.None;
                }
            }
        }

        private void Movement()
        {
            if (vertical == Vertical.Up) {
                velocity.Y -= runAccel;
            }
            else if (vertical == Vertical.Down) {
                velocity.Y += runAccel;
            }
            else {
                velocity.Y = 0;
            }

            if (horizonal == Horizonal.Left) {
                velocity.X -= runAccel;
            }
            else if (horizonal == Horizonal.Right) {
                velocity.X += runAccel;
            }
            else {
                velocity.X = 0;
            }

            velocity.X = MathHelper.Clamp(velocity.X, -normMaxSpeed, normMaxSpeed);
            MovementHorizontal(velocity.X);

            velocity.Y = MathHelper.Clamp(velocity.Y, -normMaxSpeed, normMaxSpeed);
            MovementVerical(velocity.Y);
        }

        private void Animation()
        {
            if(vertical == Vertical.Up && horizonal == Horizonal.Left) {
                player.PlayAnimation(upAnimationData);
            }
            else if(vertical == Vertical.Up && horizonal == Horizonal.Right) {
                player.PlayAnimation(upAnimationData);
            }
            else if(vertical == Vertical.Down && horizonal == Horizonal.Left) {
                player.PlayAnimation(downAnimationData);
            }
            else if (vertical == Vertical.Down && horizonal == Horizonal.Right) {
                player.PlayAnimation(downAnimationData);
            }
            else if(vertical == Vertical.Up) {
                player.PlayAnimation(upAnimationData);
            }
            else if (vertical == Vertical.Down) {
                player.PlayAnimation(downAnimationData);
            }
            else if (horizonal == Horizonal.Left) {
                player.PlayAnimation(leftAnimationData);
            }
            else if (horizonal == Horizonal.Right) {
                player.PlayAnimation(rightAnimationData);
            }
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
