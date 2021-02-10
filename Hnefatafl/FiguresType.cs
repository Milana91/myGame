using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hnefatafl
{
    enum FiguresType
    {
        none,
        defendingFigures,
        attackingFigures
    }

    static class FiguresTypeExtension // Метод расширения для типа фигур, позволяющий осуществлять смену типа после выполнения хода
    {
        public static FiguresType FlipFiguresType(this FiguresType figuresType)  // Метод принимает текущий тип фигур и меняет его на другой
        {
            if (figuresType == FiguresType.defendingFigures) return FiguresType.attackingFigures;
            if (figuresType == FiguresType.attackingFigures) return FiguresType.defendingFigures;
            return FiguresType.none;
        }
    }
}
