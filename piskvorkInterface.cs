using System;
using System.Threading;

abstract class Interface
{
	public int width, height;
	public int info_timeout_turn = 50000;
	public int info_timeout_match = 1000000000;
	public int info_time_left = 1000000000;
	public int info_max_memory = 0;
	public int info_game_type = 1;
	public bool info_exact5 = false;
	public bool info_renju = false;
	public bool info_continuous = false;
	public int terminate;
	public int start_time;
	public string dataFolder;
	
	public int xx;
	public int yy;

	abstract public void AI_init();
	abstract public void AI_start();
	abstract public void AI_turn(int x, int y);
	abstract public void AI_board2();
	abstract public void AI_board(int x, int y, int player);
	abstract public void AI_end();


	private string cmd;
	private AutoResetEvent event1;
	private ManualResetEvent event2;

	private void get_line()
	{
		cmd = Console.ReadLine();
		if (cmd == null) 
			Environment.Exit(0);
	}

	private bool parse_coord2(string param, out int x, out int y)
	{
		string[] p = param.Split(',');
		if (p.Length == 2 && int.TryParse(p[0], out x) && int.TryParse(p[1], out y) && x >= 0 && y >= 0)
			return true;
		x = y = 0;
		return false;
	}

	private bool parse_coord(string param, out int x, out int y)
	{
		return parse_coord2(param, out x, out y) && x < width && y < height;
	}

	private void parse_3int_chk(string param, out int x, out int y, out int z)
	{
		string[] p = param.Split(',');
		if (!(p.Length == 3 && int.TryParse(p[0], out x) && int.TryParse(p[1], out y) && int.TryParse(p[2], out z)
			&& x >= 0 && y >= 0 && x < width && y < height))
			x = y = z = 0;
	}

	private static string get_cmd_param(string command, out string param)
	{
		param = "";
		int pos = command.IndexOf(' ');
		if (pos >= 0)
		{
			param = command.Substring(pos + 1).TrimStart(' ');
			command = command.Substring(0, pos);
		}
		return command.ToLower();
	}

	private void threadLoop()
	{
		while (true)
		{
			event1.WaitOne();
			AI_turn(xx, yy);
			event2.Set();
		}
	}

	private void turn()
	{
		terminate = 0;
		event2.Reset();
		event1.Set();
	}

	private void stop()
	{
		terminate = 1;
		event2.WaitOne();
	}

	private void start()
	{
		stop();
		if (width == 0)
		{
			width = height = 20;
			AI_init();
		}
	}

	private void do_command()
	{
		string param, info;
		int x, y, who, e;

		switch (get_cmd_param(cmd, out param))
		{
			case "start":
				if (!int.TryParse(param, out width) || width < 5)
				{
					width = 0;
					Console.WriteLine("ERROR bad START parameter");
				}
				else
				{
					height = width;
					start();
					AI_init();
				}
				break;
			case "turn":
				start();
				if (!parse_coord(param, out x, out y))
				{
					Console.WriteLine("ERROR bad coordinates");
				}
				else
				{
					xx = x;
					yy = y;
					turn();
				}
				break;

			case "begin":
				start();
				AI_start();
				break;
			case "about":
				Console.WriteLine("name=\"Didilebest\", version=\"1.0\", author=\"Didier - Loïc - Valentin\", country=\"France\"");
				break;
			case "end":
				stop();
				AI_end();
				Environment.Exit(0);
				break;
			case "info":
				switch (get_cmd_param(param, out info))
				{
					case "max_memory":
						int.TryParse(info, out info_max_memory);
						break;
					case "timeout_match":
						int.TryParse(info, out info_timeout_match);
						break;
					case "timeout_turn":
						int.TryParse(info, out info_timeout_turn);
						break;
					case "time_left":
						int.TryParse(info, out info_time_left);
						break;
					case "game_type":
						int.TryParse(info, out info_game_type);
						break;
					case "rule":
						if (int.TryParse(info, out e))
						{
							info_exact5 = (e & 1) != 0;
							info_continuous = (e & 2) != 0;
							info_renju = (e & 4) != 0;
						}
						break;
					case "folder":
						dataFolder = info;
						break;
				}
				break;
			case "board":
				start();
				for (; ; )
				{
					get_line();
					parse_3int_chk(cmd, out x, out y, out who);
					AI_board(x, y, who);
				}
				AI_board2();
				break;
			default:
				Console.WriteLine("UNKNOWN command");
				break;
		}
	}


	public void main()
	{
		try
		{
			int dummy = Console.WindowHeight;
		}
		catch (System.IO.IOException)
		{}

		event1 = new AutoResetEvent(false);
		new Thread(threadLoop).Start();
		event2 = new ManualResetEvent(true);
		for (; ; )
		{
			get_line();
			do_command();
		}
	}

	static void Main(string[] args)
	{
		new AI().main();
	}
}
