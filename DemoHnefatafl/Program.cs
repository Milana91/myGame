using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hnefatafl;

namespace DemoHnefatafl
{
    class Program
    {
        static void Main(string[] args)
        {
            //3AAA3/4A4/4D4/A3D3A/AADDKDDAA/A3D3A/4D4/4A4/3AAA3 A 1
            // 1DA1AA3/2DA5/9/A1D4A1/A5A2/A7A/1D5DA/3DA3K/3A1A3 D 
            Tablut tablut = new Tablut("1DA1AA3/2DA5/9/A4D1A1/A4K3/A2A1DA2/1D6A/3D5/3A1A3 A 1"); // Создаем новый экземпляр игры
            List<string> list;
            while (true)
            {
                list = tablut.GetAllMoves();
                Console.WriteLine(tablut.Fen); // Передаем начальную позицию fen новой партии игры на консоль. Отслеживаем состояние игры.
                Console.WriteLine(TablutToAscii(tablut)); // Передаем нарисованное поле на консоль
                foreach (string moves in list)   // Вывод на экран всех возможных ходов
                    Console.Write(moves + "\t");
                string move = Console.ReadLine(); // Считываем введенный в консоль ход
                if (move == "") break; // Если ничего не введено, то выходим из консоли
                tablut = tablut.Move(move); // После введения желаемого кода, передвигаем фигуру (присваиваем созданной доске новое значение хода) На этом этапе в действитетнльности совершается ход на доске
                /*tablut.DestroyFigures*/
                if (tablut.IsGameFinished())
                {
                    break;
                }
            }
        }
        static string TablutToAscii(Tablut tablut)
        {
            string text = "  +-------------------+\n";
            for (int y = 8; y >= 0; y--)
            {
                text += y + 1;
                text += " | ";
                for (int x = 0; x < 9; x++)
                    text += tablut.GetFigureAt(x, y) + " "; // Цикл позволяет заполнить строчки с фигурами
                text += "|\n";
            }
            text += "  +-------------------+\n";
            text += "    a b c d e f g h i\n";
            return text;
        }
    }
}
