using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hnefatafl
{
    struct Square
    {
        public static Square none = new Square(-1, 1);  // Значение для некорректного ввода хода playerMove

        public int X { get; private set; }
        public int Y { get; private set; }

        public Square(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Square(string playerMove)  // Конструктор, принимающий команду на ход с клавиатуры в формате e2. Определяет координаты квадрата
        {
            if (playerMove.Length == 2 &&
                playerMove[0] >= 'a' && playerMove[0] <= 'i' &&
                playerMove[1] >= '1' && playerMove[1] <= '9')
            {
                X = playerMove[0] - 'a';
                Y = playerMove[1] - '1';
            }

            else this = none;
        }

        public bool IsSquareOnBoard() // Метод, проверяющий, не вышла ли клетка за пределы доски
        {
            return X >= 0 && X < 9 && Y >= 0 && Y < 9;
        }

        public bool IsSquareAFortress() // Метод, проверяющий, не является ли клетка крепостью
        {
            return ((X == 0 || X == 8) && (Y == 0 || Y == 8)) || (X == 4 && Y == 4);
        }

        public string Name { get { return ((char)('a' + X)).ToString() + (Y + 1).ToString(); } }  // В формате e2

        public static bool operator ==(Square firstSquare, Square secondSquare)  // Сравнивает две клетки на поле
        {
            return firstSquare.X == secondSquare.X && firstSquare.Y == secondSquare.Y;
        }

        public static bool operator !=(Square firstSquare, Square secondSquare)
        {
            return firstSquare.X != secondSquare.X || firstSquare.Y != secondSquare.Y;
        }

        public static IEnumerable<Square> YieldSquares()  // Перебор всех клеток
        {
            for (int y = 0; y < 9; y++)
                for (int x = 0; x < 9; x++)
                    yield return new Square(x, y);
        }
    }
}
