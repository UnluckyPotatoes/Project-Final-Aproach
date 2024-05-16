﻿using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

public class AddMoves : AnimationSprite
{
    MyGame myGame;
    TiledObject tiledObject;
    public Vec2 position;
    Code code;
    public AddMoves(string fileName, int cols, int rows, TiledObject obj = null) : base(fileName, cols, rows)
    {
        myGame = (MyGame)game;
        tiledObject = obj;
        float width = tiledObject.Width;
        float height = tiledObject.Height;
        position = new Vec2(tiledObject.X, tiledObject.Y);
    }
    void Update()
    {
    }
    public void Bye()
    {
        position = new Vec2(-500, -500);
        this.Remove();
    }
}