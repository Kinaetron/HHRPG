using Microsoft.Xna.Framework;
using PolyOne.Engine;
using PolyOne.Scenes;
using PolyOne.Utility;

namespace HHRPG
{
    public class Level : Scene
    {
        private Player player;

        public override void LoadContent()
        {
            base.LoadContent();

            player = new Player(new Vector2(200, 200));
            this.Add(player);
            player.Added(this);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw()
        {
            Engine.Begin(Resolution.GetScaleMatrix);
            base.Draw();
            Engine.End();
        }
    }
}
