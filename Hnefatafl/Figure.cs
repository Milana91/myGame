using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hnefatafl
{
    enum Figure
    {
        none,
        attacker = 'A',
        defender = 'D',
        king = 'K'
    }

    static class FigureExtension
    {
        public static FiguresType GetFiguresType(this Figure figure) // Метод расширения, показывающий тип фигур
        {
            if (figure == Figure.none)
                return FiguresType.none;
            return (figure == Figure.defender ||
                    figure == Figure.king)
                    ? FiguresType.defendingFigures
                    : FiguresType.attackingFigures;
        }
    }
}
