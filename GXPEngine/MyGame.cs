using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Collections.Generic;
using System.Drawing;                           // System.Drawing contains drawing tools such as Color definitions
using System.Threading;

public class MyGame : Game
{
    PlayerBall player;
    bool _stepped = false;
    Vec2 MousePos;
    bool _paused = false;
    int _stepIndex = 0;
    int _startSceneNumber = 0;
	public int currentLevel = 0; // levels start from level 0
	public string[] levels = new string[1];	// amount of levels is 1

	public bool nextLevel = false;

    public EasyDraw colliderHolder;

	public int GetCurrentLevel()
	{
		return currentLevel;
    }
    public void SetCurrentLevel(int _value)
    {
        currentLevel = _value;
        if (currentLevel >= 4) { currentLevel = 0; }
        LoadLevel(levels[currentLevel]);
    }

    public int GetNumberOfLines()
    {
        return _lines.Count;
    }

    public LineSegment GetLine(int index)
    {
        if (index >= 0 && index < _lines.Count)
        {
            return _lines[index];
        }
        return null;
    }
    public void AddLine(Vec2 start, Vec2 end, bool specialCol = false, bool twosided = false, bool addcaps = false)
    {
        if (colliderHolder == null)
        {
            colliderHolder = new EasyDraw(10, 10, false);
        }
        LineSegment line = new LineSegment(start, end, specialCol ? 0xffff00ff : 0xff00ff00, 4);
        AddChild(line);
        _lines.Add(line);

        if (twosided) AddLine(end, start, true, false); // :-)
    }

    Canvas _lineContainer = null;
	public MyGame() : base(800, 800, false, false, 800, 800, false)     
	{
        _movers = new List<Ball>();
        _lines = new List<LineSegment>();
        levels[0] = "Assets/lvl_design_v1.tmx";
        targetFps = 60;
        LoadLevel(levels[0]);
        player = FindObjectOfType<PlayerBall>();
        Ball.acceleration.SetXY(0, 0.75f);
        
    }

    public List<Ball> _movers;
    public List<LineSegment> _lines;
    EasyDraw congrats;
	Font rowdies;
    void DestroyAllLevels()
    {
        List<GameObject> children = GetChildren();
        foreach (GameObject child in children)
        {
            child.Destroy();
        }
    }
    public void DrawLine(Vec2 start, Vec2 end)
    {
        _lineContainer.graphics.DrawLine(Pens.White, start.x, start.y, end.x, end.y);
    }

    public void AddMover(Ball bullet)
    {
        _movers.Add(bullet);
        AddChild(bullet);
    }
    public void AddPlayer(PlayerBall bullet)
    {
        _movers.Add(bullet);
        AddChild(bullet);
    }

    private void LoadLevel(string name)
	{
		nextLevel = false;
		DestroyAllLevels();
		Level level = new Level(name);
		AddChild(level);
		//player = FindObjectOfType<Player>(); /*for when we add a player*/
	}

	// For every game object, Update is called every frame, by the engine:
	void Update()
    {
        if (!_paused)
        {
            StepThroughMovers();
        }
        if (Input.GetMouseButtonDown(1) && !player.pressed)
        {
            MousePos.SetXY(Input.mouseX, Input.mouseY);
            AddMover(new Rock(5, MousePos, 0, default, 5, new Vec2(0, 0.5f)));
            Console.WriteLine("ball made");
        }
    }

    public void RemoveMover(Ball ball)
    {
        _movers.Remove(ball);
    }

    public int GetNumberOfMovers()
    {
        return _movers.Count;
    }

    public Ball GetMover(int index)
    {
        if (index >= 0 && index < _movers.Count)
        {
            return _movers[index];
        }
        return null;
    }
    void StepThroughMovers()
    {
        if (_stepped)
        { // move everything step-by-step: in one frame, only one mover moves
            _stepIndex++;
            if (_stepIndex >= _movers.Count)
            {
                _stepIndex = 0;
            }
            if (_movers[_stepIndex].moving)
            {
                _movers[_stepIndex].Step();
            }
        }
        else
        { // move all movers every frame
            for (int i = _movers.Count - 1; i >= 0; i--)
            {
                if (_movers[i].moving)
                {
                    _movers[i].Step();
                }
            }
        }
    }
    // For every game object, Update is called every frame, by the engine:

	static void Main()                          // Main() is the first method that's called when the program is run
	{
		new MyGame().Start();                   
	}
}