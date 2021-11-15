using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;



namespace Hack
{
    class Program
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);

        public static int GWL_STYLE = -16;
        public static uint WS_THICKFRAME = 0x00040000;

        static object locker=new object();
        static Random rnd = new Random();
        static int height = 0;
        static int width = 0;
        static void Main(string[] args)
        {

            IntPtr hwnd = FindWindow("ConsoleWindowClass", null);
            if (hwnd != null)
            {
                SetWindowLong(
                    hwnd,
                    GWL_STYLE,
                    GetWindowLong(hwnd, GWL_STYLE) ^ WS_THICKFRAME
                );
            }
            Console.CursorVisible = false;
            height = Console.WindowHeight;
            width = Console.WindowWidth;    
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            for (int i = 0; i < width; i++)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(ColumnDraw));
                thread.Start(i);
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
