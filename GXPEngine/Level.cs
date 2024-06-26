using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;
using GXPEngine;
using GXPEngine.Core;
using GXPEngine.Managers;
using GXPEngine.OpenGL;


public class Level : GameObject
{
    MyGame myGame;
    private PlayerBall player;
    readonly TiledLoader loader;
    public Level(string filename)
    {
        loader = new TiledLoader(filename);
        CreateLevel();
    }
    void CreateLevel(bool IncludeImageLayer = true)
    {
        loader.autoInstance = true;
        loader.rootObject = this;

        loader.addColliders = false;
        loader.LoadImageLayers();

        loader.LoadTileLayers(); // background (skybox/ scenery)
        loader.LoadTileLayers(1); // background and props
        loader.addColliders = true;
        loader.LoadTileLayers(2); // platforms and walls (everything that is collidable)
        loader.LoadObjectGroups();
        CreateCollisions();
    }
    void CreateCollisions()
    {
        CollisionLine[] lines = FindObjectsOfType<CollisionLine>();
        foreach (var line in lines)
        {
            CollisionLine cLine = line as CollisionLine;
            var corners = cLine.GetExtents();

            for (int i = 0; 0 <= corners.Length; i++)
            {
                if (i != corners.Length)
                {
                    // top line
                    myGame.AddChild(new LineSegment(new Vec2(corners[i].x, corners[i].y), new Vec2(corners[i + 1].x, corners[i + 1].y)));
                }
                else
                {
                    myGame.AddChild(new LineSegment(new Vec2(corners[i].x, corners[i].y), new Vec2(corners[0].x, corners[0].y)));
                }
            }
        }

    }
    void HandleScroll()
    {
        if (player == null) return;
        int boundarySizex = 300;
        int boundarySizey = 200;
        if (player.x + x < boundarySizex)
        {
            x = boundarySizex - player.x;
        }
        if (player.x + x > game.width - boundarySizex)
        {
            x = game.width - boundarySizex - player.x;
        }
        if (player.y + y < boundarySizey)
        {
            y = boundarySizey - player.y;
        }
        if (player.y + y > game.height - boundarySizey)
        {
            y = game.height - boundarySizey - player.y;
        }
    }
    void GameBoundary()
    {
        if (player == null) return;
        if (x > 0) x = 0;
        if (-x >= game.width * 2) x = (game.width * -2);
        if (-y >= game.height) y = (game.height) * -1;
        if (y > 0) y = 0;
    }
    void Update()
    {
        HandleScroll();
        GameBoundary();
        Console.WriteLine(game.height);
    }
}
