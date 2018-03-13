using System;
using Gomoku;

class AI : Interface
{
	const int MAX_BOARD = 100;
	Random rand = new Random();
	Core core = new Core();
	public override void AI_init()
	{
		if (width < 5 || height < 5)
		{
			Console.WriteLine("ERROR size of the board");
			return;
		}
		if (width > MAX_BOARD || height > MAX_BOARD)
		{
			Console.WriteLine("ERROR Maximal board size is " + MAX_BOARD);
			return;
		}
		Console.WriteLine(core.cmdStart(height));
	}

	public override void AI_board(int x, int y, int player)
	{
		core.AddToBoard(x, y, player);
	}

	public override void AI_board2()
	{
		Console.WriteLine(core.cmdBoard());
	}
	
	public override void AI_turn(int x, int y)
	{

		Console.WriteLine(core.cmdTurn(x, y));
	}

	public override void AI_end()
	{
	}
	
	public override void AI_start()
	{
		Console.WriteLine(core.cmdBegin());
	}

}
