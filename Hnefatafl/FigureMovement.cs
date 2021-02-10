using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hnefatafl
{
    class FigureMovement
    {
        public Figure Figure { get; private set; }
        public Square From { get; private set; }
        public Square To { get; private set; }

        public FigureMovement(FigureOnSquare figureOnSquare, Square to)
        {
            Figure = figureOnSquare.Figure;  // Какая была фигура на квадрате
            From = figureOnSquare.Square; // Откуда, с какого квадрата эта фигура будет ходить
            To = to;
        }

        public FigureMovement(string move) // Ae2e4  Конструктор, принимающий ход в формате е2 с клавиатуры.
        {
            Figure = (Figure)move[0];
            From = new Square(move.Substring(1, 2));
            To = new Square(move.Substring(3, 2));
        }

        public int DeltaX { get { return To.X - From.X; } }  // Вспомогательное свойство для вычисления разницы координат по x . Необходимо для проверок возможности совершать ходы 
        public int DeltaY { get { return To.Y - From.Y; } }  // Вспомогательное свойство для вычисления разницы координат по y. Необходимо для проверок возможности совершать ходы 

        public int AbsDeltaX { get { return Math.Abs(DeltaX); } } // Свойство для вычисления модуля значений DeltaX
        public int AbsDeltaY { get { return Math.Abs(DeltaY); } } // Свойство для вычисления модуля значений DeltaY

        public int SignX { get { return Math.Sign(DeltaX); } } // Знак числа, чтобы понять в какую сторону совершается ход (-1 или 1)
        public int SignY { get { return Math.Sign(DeltaY); } } // Знак числа, чтобы понять в какую сторону совершается ход (-1 или 1)

        public override string ToString()
        {
            string text = (char)Figure + From.Name + To.Name;
            return text;
        }
    }
}
