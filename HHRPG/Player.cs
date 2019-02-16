using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using PolyOne;
using PolyOne.Animation;
using PolyOne.Engine;
using PolyOne.Input;

namespace HHRPG
{
    public class Player : Entity
    {
        private Texture2D downTexture;
        private Texture2D upTexture;
        private Texture2D leftTexture;
        private Texture2D rightTexture;

        private AnimationData downAnimationData;
        private AnimationData upAnimationData;
        private AnimationData leftAnimationData;
        private AnimationData rightAnimationData;

        private AnimationPlayer player;

        public Player(Vector2 position)
            :base(position)
        {
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
        }

        public override void Update()
        {
            base.Update();

            if(PolyInput.Keyboard.Check(Keys.W))
            {
                player.PlayAnimation(upAnimationData);
                Position.Y -= 2.0f;
            }

            if(PolyInput.Keyboard.Check(Keys.S))
            {
                player.PlayAnimation(downAnimationData);
                Position.Y += 2.0f;
            }

            if(PolyInput.Keyboard.Check(Keys.A))
            {
                player.PlayAnimation(leftAnimationData);
                Position.X -= 2.0f;
            }

            if(PolyInput.Keyboard.Check(Keys.D))
            {
                player.PlayAnimation(rightAnimationData);
                Position.X += 2.0f;
            }

            player.Update();
        }

        public override void Draw()
        {
            base.Draw();
            player.Draw(Position, 0.0f, SpriteEffects.None);
        }
    }
}
