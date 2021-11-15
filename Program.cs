using System;
using System.Threading;
using System.Threading.Tasks;
namespace Hack
{
    class Program
    {
        static object locker=new object();
        static Random rnd = new Random();
        static int height = 0;
        static int width = 0;
        static void Main(string[] args)
        {
            
            Console.CursorVisible = false;
            height = Console.WindowHeight;
            width = Console.WindowWidth;
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            Task task = new Task(WindowSize);
            task.Start();
            
            for (int i = 0; i < width; i++)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(ColumnDraw));
                thread.Start(i);
            }

        }
        static void WindowSize()
        {
            while(true)
            {
                lock (locker)
                {
                    if (Console.WindowWidth != width) Console.WindowWidth = width;
                    if (Console.WindowHeight != height) Console.WindowHeight = height;
                }
            }
        }

        static void ColumnDraw(object x)
        {
            int column = (int)x;
            while(true)
            {
                int sleep= rnd.Next(200, 500);
                int size = rnd.Next(4, height-13);


                char[] symbols = new char[size+1];
                int count = 0;
                int countSpace = 0;
                symbols[count++]=(char)rnd.Next(0,1);

                while(count!=0)
                {
                    int y = 0;
                    Thread.Sleep(sleep);

                    if (countSpace + count >= height-2)
                    {
                        count--;
                    }

                    lock (locker)
                    { 
                        if (size == 0)
                        {
                            Console.SetCursorPosition(column, countSpace++);
                            Console.WriteLine(' ');
                            y = countSpace+1;
                        }

                        if (count==1)
                        {
                            Console.SetCursorPosition(column, height-2);
                            Console.WriteLine(' ');
                        }

                        for (int i = 0; i < count; i++)
                        {
                            symbols[i] = (char)rnd.Next(33, 126);
                            Console.SetCursorPosition(column, y++);
                            if (i + 1 == count) Console.ForegroundColor = ConsoleColor.White;
                            if (i + 2 == count) Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(symbols[i]);
                            Console.ForegroundColor = ConsoleColor.DarkGreen;   

                        }
                    }

                    if (size!=0)
                    {
                        symbols[count++]= (char)rnd.Next(33, 126);
                        size--;
                    }
                }

            }
        }
    }
}
