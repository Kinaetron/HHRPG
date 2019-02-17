using PolyOne.Engine;
using PolyOne.LevelProcessor;
using PolyOne.Scenes;

namespace HHRPG
{
    public enum GameTags
    {
        Player = 1,
        Solid = 2
    }

    public class Level : Scene
    {
        private Player player;
        private LevelData levelData;

        private LevelTilesSolid tiles;
        private bool[,] collisionInfo;
        public LevelTiler Tile { get; private set; }

        public override void LoadContent()
        {
            base.LoadContent();

            Tile = new LevelTiler();

            levelData = LevelData.Load("Content/testMap.json");
            Tile.LoadContent(levelData);

            collisionInfo = LevelTiler.TileConverison(Tile.CollisionLayer, 2);
            tiles = new LevelTilesSolid(collisionInfo);
            this.Add(tiles);

            player = new Player(Tile.PlayerPosition[0]);
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
            Engine.Begin(player.Camera.TransformMatrix);
            Tile.DrawImageBackground();
            Tile.DrawBackground();
            base.Draw();
            Engine.End();
        }
    }
}
