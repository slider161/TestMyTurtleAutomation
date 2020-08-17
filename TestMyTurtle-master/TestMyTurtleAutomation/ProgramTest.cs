using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TestMyTurtle
{
    class ProgramTest
    {
        //Actions actions;
        Board board;
        Turtle turtle;
        Game game;

        [SetUp]
        public void Setup()
        {
            //actions = new Actions { Sequences = new List<List<Action>>{ new List<Action> { Action.Move, Action.Rotate } } }; 

            //board = new Board { Cols = 5, Rows = 4, Mines = new List<(int, int)>{(1,1),(4,3)}, ExitX = 2, ExitY = 2 };

            //turtle = new Turtle { Direction = Direction.South, X = 0, Y = 0 };

            //game = new Game { Turtle = turtle, Sequence = new List<Action> { Action.Move, Action.Rotate }, Board = board };
        }

        [TestCase(Direction.North, ExpectedResult = Direction.East)]
        [TestCase(Direction.East, ExpectedResult = Direction.South)]
        [TestCase(Direction.South, ExpectedResult = Direction.West)]
        [TestCase(Direction.West, ExpectedResult = Direction.North)]
        public Direction TurtleRotateTest(Direction startingDirection)
        {
            turtle = new Turtle { Direction = startingDirection, X = 0, Y = 0 };
            board = new Board { Cols = 5, Rows = 4, Mines = new List<(int, int)> {}, ExitX = 2, ExitY = 2 };

            game = new Game { Turtle = turtle, Sequence = new List<Action> { Action.Rotate }, Board = board };
            game.Execute(Action.Rotate);

            return turtle.Direction;
        }

        [TestCase(Direction.North, 1, 1, ExpectedResult = new[] { 1, 0 })]
        [TestCase(Direction.East, 1, 1, ExpectedResult = new[] { 2, 1 })]
        [TestCase(Direction.South, 1, 1, ExpectedResult = new[] { 1, 2 })]
        [TestCase(Direction.West, 1, 1, ExpectedResult = new[] { 0, 1 })]
        public int[] TurtleMoveTest(Direction startingDirection, int x, int y)
        {
            turtle = new Turtle { Direction = startingDirection, X = x, Y = y };
            board = new Board { Cols = 5, Rows = 4, Mines = new List<(int, int)> {}, ExitX = 3, ExitY = 3 };

            game = new Game { Turtle = turtle, Sequence = new List<Action> { Action.Move }, Board = board };
            game.Execute(Action.Move);

            Console.WriteLine(turtle.X.ToString()+" "+turtle.Y.ToString());

            return new[] { turtle.X, turtle.Y };
        }

        [TestCase(Direction.East, ExpectedResult = false)]
        [TestCase(Direction.West, ExpectedResult = true)]
        public Boolean TurtleDiesTest(Direction startingDirection)
        {
            turtle = new Turtle { Direction = startingDirection, X = 0, Y = 0 };
            board = new Board { Cols = 5, Rows = 4, Mines = new List<(int, int)> { (1, 0) }, ExitX = 2, ExitY = 2 };

            game = new Game { Turtle = turtle, Sequence = new List<Action> { Action.Move }, Board = board };
            game.Execute(Action.Move);

            return turtle.IsAlive;
        }

        [TestCase(Direction.West, ExpectedResult = false)]
        [TestCase(Direction.East, ExpectedResult = true)]
        public Boolean TurtleEscapesTest(Direction startingDirection)
        {
            turtle = new Turtle { Direction = startingDirection, X = 0, Y = 0 };
            board = new Board { Cols = 5, Rows = 4, Mines = new List<(int, int)> { }, ExitX = 1, ExitY = 0 };

            game = new Game { Turtle = turtle, Sequence = new List<Action> { Action.Move }, Board = board };
            game.Execute(Action.Move);

            return game.TurtleExited;
        }

        [Test]
        public void TurtleOutOfBoundsTest()
        {
            turtle = new Turtle { Direction = Direction.East, X = 0, Y = 0 };
            board = new Board { Cols = 0, Rows = 0, Mines = new List<(int, int)> { }, ExitX = 1, ExitY = 1 };

            game = new Game { Turtle = turtle, Sequence = new List<Action> { Action.Move }, Board = board };
            
            Assert.Throws<System.Exception>(() => game.Execute(Action.Move));
        }

        [Test]
        public async Task CheckBoardTest()
        {
            var settings = Settings.Parse(await File.ReadAllTextAsync($"{AppDomain.CurrentDomain.BaseDirectory}\\settings.txt"));

            var board = new Board { Cols = settings.BoardCols, Rows = settings.BoardRows, Mines = settings.Mines, ExitX = settings.ExitX, ExitY = settings.ExitY };

            //If these are Zero you have very limited game! 
            Assert.IsTrue(board.Cols != 0 && board.Rows != 0);

            //Need Mines
            Assert.IsNotEmpty(board.Mines);

            //Need an exit
            Assert.IsNotNull(board.ExitX);
            Assert.IsNotNull(board.ExitY);
        }

        [Test]
        public async Task CheckTurtleTest()
        {
            var settings = Settings.Parse(await File.ReadAllTextAsync($"{AppDomain.CurrentDomain.BaseDirectory}\\settings.txt"));

            var turtle = new Turtle { Direction = settings.InitialDirection, X = settings.InitialX, Y = settings.InitialY };

            Assert.IsNotNull(turtle.Direction);
            Assert.IsNotNull(turtle.X);
            Assert.IsNotNull(turtle.Y);
            Assert.IsTrue(turtle.IsAlive);
        }

        [TearDown]
        public void TearDown()
        {

        }
    }
}
