using Microsoft.Xna.Framework;

using PolyOne.Engine;

namespace HHRPG
{
    public class Game : Engine
    { 
        public Game()
            : base(480, 270, "HHRPG", 4.0f, false)
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
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
