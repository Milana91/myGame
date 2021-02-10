using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hnefatafl
{
    class Moves
    {
        FigureMovement FigureMovement { get; set; }
        Board Board { get; set; }

        public Moves(Board board)
        {
            this.Board = board;
        }

        public bool CanMove(FigureMovement figureMovement)  // Проверка возможности сделать ход
        {
            FigureMovement = figureMovement;
            return
                CanMoveFrom() && // Можем ли походить с клетки
                CanMoveTo() && // Можем ли походить на клетку
                CanFigureMove(); // Можем ли данная фигура сделать ход на нужную клетку
        }
        bool CanMoveFrom() // Проверка возможности сделать ход c определенной клетки
        {
            return FigureMovement.From.IsSquareOnBoard() &&  // Клетка, с которой ходим, должна быть на доске
                   FigureMovement.Figure.GetFiguresType() == Board.MoveFiguresType && // Тип  фигуры должен соответствовать типу активных фигур
                   Board.GetFigureAt(FigureMovement.From) != Figure.none; // Клетка, откуда должен быть сделан ход не должна быть пустой
        }

        bool CanMoveTo() // Проверка возможности сделать ход на определенную клетку
        {
            return FigureMovement.To.IsSquareOnBoard() && // Клетка, куда идем должна быть на доске
                FigureMovement.From != FigureMovement.To && // Клетки, откуд и куда ходим не должны быть равны
                Board.GetFigureAt(FigureMovement.To) == Figure.none; // Проверка клетки на пустоту. Нельзя походить, если там стоит фигура
        }

        bool CanFigureMove() // Проверка возможности сделать ход данной фигурой на определенную клетку
        {
            Square fromSquare = FigureMovement.From;
            // Проверяем, является ли целевая клетка крепостью. Только король ходит на клетку крепости
            if (FigureMovement.Figure != Figure.king && FigureMovement.To.IsSquareAFortress())
                return false;
            else
            {
                while (fromSquare.IsSquareOnBoard() && (FigureMovement.SignX == 0 || FigureMovement.SignY == 0)) // Проверяем верность направления движения фигур
                {
                    fromSquare = new Square(fromSquare.X + FigureMovement.SignX, fromSquare.Y + FigureMovement.SignY); // Изменение местоположения фигура на шаг
                    if (fromSquare == FigureMovement.To) // Фигура достигла целевой клетки
                    {
                        return true;
                    }
                    if (Board.GetFigureAt(fromSquare) == Figure.none) // Если клетка не пуста, дальше движение не возможно
                    {
                        continue;
                    }
                    return false;
                }
            }
            return false;
        }


        // Уничтожение фигур
        public void DestroyFiguresAround(FigureMovement figureMovement)
        {
            FigureMovement = figureMovement;
            Square squareLeft = new Square(FigureMovement.To.X - 1, FigureMovement.To.Y);
            Square squareRight = new Square(FigureMovement.To.X + 1, FigureMovement.To.Y);
            Square squareBottom = new Square(FigureMovement.To.X, FigureMovement.To.Y - 1);
            Square squareTop = new Square(FigureMovement.To.X, FigureMovement.To.Y + 1);

            Square mainFortress = new Square(4, 4);
            Square squareFortressLeft = new Square(mainFortress.X - 1, mainFortress.Y);
            Square squareFortressRight = new Square(mainFortress.X + 1, mainFortress.Y);
            Square squareFortressBottom = new Square(mainFortress.X, mainFortress.Y - 1);
            Square squareFortressTop = new Square(mainFortress.X, mainFortress.Y + 1);


            // Окружение короля на троне с четырех сторон
            if (Board.GetFigureAt(mainFortress) == Figure.king)
            {
                if ((Board.GetFigureAt(squareFortressLeft) == Figure.attacker) && (Board.GetFigureAt(squareFortressRight) == Figure.attacker) && (Board.GetFigureAt(squareFortressTop) == Figure.attacker) && (Board.GetFigureAt(squareFortressBottom) == Figure.attacker))
                {
                    Board.DestroyFigureAt(mainFortress); // Убираем с поля короля. Победа атакующих
                }
            }

            Square squareLeftMore = new Square(squareLeft.X - 1, squareLeft.Y);
            Square squareRightMore = new Square(squareRight.X + 1, squareLeft.Y);
            Square squareTopMore = new Square(squareTop.X, squareTop.Y + 1);
            Square squareBottomMore = new Square(squareBottom.X, squareBottom.Y - 1);

            Square squareLeftTop = new Square(squareLeft.X, squareLeft.Y + 1);
            Square squareLeftBottom = new Square(squareLeft.X, squareLeft.Y - 1);

            Square squareRightTop = new Square(squareRight.X, squareRight.Y + 1);
            Square squareRightBottom = new Square(squareRight.X, squareRight.Y - 1);

            Square squareTopRight = new Square(squareTop.X + 1, squareTop.Y);
            Square squareTopLeft = new Square(squareTop.X - 1, squareTop.Y);

            Square squareBottomRight = new Square(squareBottom.X + 1, squareBottom.Y);
            Square squareBottomLeft = new Square(squareBottom.X - 1, squareBottom.Y);


            // Проверка и уничтожение фигуры слева
            if ((Board.GetFigureAt(squareLeftMore).GetFiguresType() == FigureMovement.Figure.GetFiguresType() && Board.GetFigureAt(squareLeft) != Figure.king) ||
                (Board.GetFigureAt(squareLeftMore).GetFiguresType() == FigureMovement.Figure.GetFiguresType() && Board.GetFigureAt(squareLeft) == Figure.king &&
                (squareLeftTop != mainFortress) && (squareLeftBottom != mainFortress)) ||
                (Board.GetFigureAt(squareLeft) == Figure.king && squareLeftMore.IsSquareAFortress() && (Board.GetFigureAt(squareLeftTop) == Figure.attacker) && (Board.GetFigureAt(squareLeftBottom) == Figure.attacker)) ||
                squareLeftMore.IsSquareAFortress() && (Board.GetFigureAt(squareLeft) != Figure.king) &&
                (Board.GetFigureAt(squareFortressLeft) != Figure.king) &&
                (Board.GetFigureAt(squareFortressRight) != Figure.king) &&
                (Board.GetFigureAt(squareFortressTop) != Figure.king) &&
                (Board.GetFigureAt(squareFortressBottom) != Figure.king))
            {
                Board.DestroyFigureAt(squareLeft); // Убираем с доски фигуру
            }

            // Проверка и уничтожение фигуры справа
            if ((Board.GetFigureAt(squareRightMore).GetFiguresType() == FigureMovement.Figure.GetFiguresType() && Board.GetFigureAt(squareRight) != Figure.king) ||
                (Board.GetFigureAt(squareRightMore).GetFiguresType() == FigureMovement.Figure.GetFiguresType() && Board.GetFigureAt(squareRight) == Figure.king &&
                (squareRightTop != mainFortress) && (squareRightBottom != mainFortress)) ||
                (Board.GetFigureAt(squareRight) == Figure.king && squareRightMore.IsSquareAFortress() && (Board.GetFigureAt(squareRightTop) == Figure.attacker) && (Board.GetFigureAt(squareRightBottom) == Figure.attacker)) ||
                squareRightMore.IsSquareAFortress() && (Board.GetFigureAt(squareRight) != Figure.king) &&
                (Board.GetFigureAt(squareFortressLeft) != Figure.king) &&
                (Board.GetFigureAt(squareFortressRight) != Figure.king) &&
                (Board.GetFigureAt(squareFortressTop) != Figure.king) &&
                (Board.GetFigureAt(squareFortressBottom) != Figure.king))
            {
                Board.DestroyFigureAt(squareRight);
            }

            // Проверка и уничтожение фигуры сверху
            if ((Board.GetFigureAt(squareTopMore).GetFiguresType() == FigureMovement.Figure.GetFiguresType() && Board.GetFigureAt(squareTop) != Figure.king) ||
                (Board.GetFigureAt(squareTopMore).GetFiguresType() == FigureMovement.Figure.GetFiguresType() && Board.GetFigureAt(squareTop) == Figure.king &&
                (squareLeftTop != mainFortress) && (squareRightTop != mainFortress)) ||
                (Board.GetFigureAt(squareTop) == Figure.king && squareTopMore.IsSquareAFortress() && (Board.GetFigureAt(squareTopLeft) == Figure.attacker) && (Board.GetFigureAt(squareTopRight) == Figure.attacker)) ||
                squareTopMore.IsSquareAFortress() && (Board.GetFigureAt(squareTop) != Figure.king) &&
                (Board.GetFigureAt(squareFortressLeft) != Figure.king) &&
                (Board.GetFigureAt(squareFortressRight) != Figure.king) &&
                (Board.GetFigureAt(squareFortressTop) != Figure.king) &&
                (Board.GetFigureAt(squareFortressBottom) != Figure.king))
            {
                Board.DestroyFigureAt(squareTop);
            }

            // Проверка и уничтожение фигуры снизу
            if ((Board.GetFigureAt(squareBottomMore).GetFiguresType() == FigureMovement.Figure.GetFiguresType() && Board.GetFigureAt(squareBottom) != Figure.king) ||
                (Board.GetFigureAt(squareBottomMore).GetFiguresType() == FigureMovement.Figure.GetFiguresType() && Board.GetFigureAt(squareBottom) == Figure.king &&
                (squareLeftBottom != mainFortress) && (squareRightBottom != mainFortress)) ||
                (Board.GetFigureAt(squareBottom) == Figure.king && squareBottomMore.IsSquareAFortress() && (Board.GetFigureAt(squareBottomLeft) == Figure.attacker) && (Board.GetFigureAt(squareBottomRight) == Figure.attacker)) ||
                squareBottomMore.IsSquareAFortress() && (Board.GetFigureAt(squareBottom) != Figure.king) &&
                (Board.GetFigureAt(squareFortressLeft) != Figure.king) &&
                (Board.GetFigureAt(squareFortressRight) != Figure.king) &&
                (Board.GetFigureAt(squareFortressTop) != Figure.king) &&
                (Board.GetFigureAt(squareFortressBottom) != Figure.king))
            {
                Board.DestroyFigureAt(squareBottom);
            }

            // короля мождно окружить  с трех сторон между ним и крепостью
            if ((Board.GetFigureAt(squareFortressLeft) == Figure.king) ||
                (Board.GetFigureAt(squareFortressRight) == Figure.king) ||
                (Board.GetFigureAt(squareFortressBottom) == Figure.king) ||
                (Board.GetFigureAt(squareFortressTop) == Figure.king))
            {

                if (Board.GetFigureAt(squareLeft) == Figure.king ||
                    Board.GetFigureAt(squareRight) == Figure.king ||
                    Board.GetFigureAt(squareBottom) == Figure.king ||
                    Board.GetFigureAt(squareTop) == Figure.king)
                {
                    // Проверка и уничтожение короля слева
                    if ((FigureMovement.Figure.GetFiguresType() == FiguresType.attackingFigures) && (Board.GetFigureAt(squareLeft) == Figure.king))
                    {
                        if (((Board.GetFigureAt(squareLeftTop) == Figure.attacker) || (squareLeftTop == mainFortress))
                            && (Board.GetFigureAt(squareLeftBottom) == Figure.attacker) || (squareLeftBottom == mainFortress)
                            && ((Board.GetFigureAt(squareLeftMore) == Figure.attacker) || (squareLeftBottom == mainFortress)))

                        {
                            Board.DestroyFigureAt(squareLeft);
                        }
                    }

                    // Проверка и уничтожение короля справа
                    if ((FigureMovement.Figure.GetFiguresType() == FiguresType.attackingFigures) && (Board.GetFigureAt(squareRight) == Figure.king))
                    {
                        if (((Board.GetFigureAt(squareRightTop) == Figure.attacker) || (squareRightTop == mainFortress))
                            && ((Board.GetFigureAt(squareRightBottom) == Figure.attacker) || (squareRightBottom == mainFortress))
                            && ((Board.GetFigureAt(squareRightMore) == Figure.attacker)))
                        {
                            Board.DestroyFigureAt(squareRight);
                        }
                    }

                    // Проверка и уничтожение короля сверху
                    if ((FigureMovement.Figure.GetFiguresType() == FiguresType.attackingFigures) && (Board.GetFigureAt(squareTop) == Figure.king))
                    {
                        if (((Board.GetFigureAt(squareTopRight) == Figure.attacker) || (squareTopRight == mainFortress))
                            && ((Board.GetFigureAt(squareTopLeft) == Figure.attacker) || (squareTopLeft == mainFortress))
                            && ((Board.GetFigureAt(squareTopMore) == Figure.attacker)))
                        {
                            Board.DestroyFigureAt(squareTop);
                        }
                    }

                    // Проверка и уничтожение короля снизу
                    if ((FigureMovement.Figure.GetFiguresType() == FiguresType.attackingFigures) && (Board.GetFigureAt(squareBottom) == Figure.king))
                    {
                        if (((Board.GetFigureAt(squareBottomRight) == Figure.attacker) || (squareBottomRight == mainFortress))
                            && ((Board.GetFigureAt(squareBottomLeft) == Figure.attacker) || (squareBottomLeft == mainFortress))
                            && ((Board.GetFigureAt(squareBottomMore) == Figure.attacker)))
                        {
                            Board.DestroyFigureAt(squareBottom);
                        }
                    }
                }


            }
        }

    }
}
