using Microsoft.Xna.Framework;

using PolyOne.Engine;

namespace HHRPG
{
    public class Game : Engine
    { 
        private Level level = new Level();

        public Game()
            : base(480, 270, "HHRPG", 2.0f, false)
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            level.LoadContent();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            level.Update();
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            level.Draw();
        }
    }

    static class Program
    {
        static void Main(string[] args)
        {
            using (Game game = new Game())
            {
                game.Run();
            }
        }
    }
}
