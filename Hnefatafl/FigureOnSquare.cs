using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hnefatafl
{
    class FigureOnSquare  // Контейнер для хранения фигур (какая фигура на каком квадрате находится)
    {
        public Figure Figure { get; private set; }
        public Square Square { get; private set; }

        public FigureOnSquare(Figure figure, Square square)
        {
            Figure = figure;
            Square = square;
        }
    }
}
