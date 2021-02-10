using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hnefatafl
{
    public class Tablut
    {
        public string Fen { get; private set; }
        Board board;
        Moves moves;

        List<FigureMovement> allMoves;  // Список всех возможных в текущей позиции ходов

        public Tablut(string fen = "1DA1AA3/2DA5/9/A1D4A1/A5A2/A7A/1D5DA/3DA3K/3A1A3 D 0")  // Доска со стартовым положением фигур
        {
            Fen = fen;
            board = new Board(Fen);  // Генерация доски по fen 
            moves = new Moves(board);
        }
        Tablut(Board board)  // Генерируем новую доску после нового хода и передаем
        {
            this.board = board;
            Fen = board.Fen;
            moves = new Moves(board);
        }

        /*   public void DestroyFigures() 
           {
               moves.FindFiguresAround();
           }
        */

        public Tablut Move(string move)  // Ae2e3 Dd2d7
        {
            if (move.Length != 5)
                return this;
            FigureMovement figureMovement = new FigureMovement(move); // Выполняем новый ход
            if (!moves.CanMove(figureMovement)) // Перед осуществлением хода делаем проверку возможности его совершить
                return this;
            Board nextBoard = board.Move(figureMovement);  // Создаем новую доску после выполнения хода. На этом этапе в действитетнльности создается новый объект доски с новыми позициями фигур
            Tablut nextTablut = new Tablut(nextBoard);  // При передвижении фигуры каждый раз создается новый объект игры на основании новой сгенерированной доски
            return nextTablut;
        }

        public char GetFigureAt(int x, int y)  // Упрощает отладку программы, позволяет узнать, где какая фигура находится по данным координатам. Выводит символ фигуры в зависимости от квадрата на доске.
        {
            Square square = new Square(x, y);
            Figure figure = board.GetFigureAt(square);
            return figure == Figure.none ? '.' : (char)figure;
        }


        public bool IsGameFinished() // Проверка на окончание игры
        {
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    if (GetFigureAt(x, y) == 'K')
                    {
                        if (((x == 0) && (y == 0)) || ((x == 8) && (y == 0)) || ((y == 8) && (x == 0)) || ((y == 8) && (x == 8)))
                        {
                            Console.WriteLine("Выиграли защитники");
                            return true;
                        }
                        return false;
                    }
                    else continue;
                }
            }
            Console.WriteLine("Выиграли атакующие");
            return true;
        }

        public IEnumerable<string> YieldValidMoves()
        {
            foreach (FigureOnSquare fs in board.YieldFigureOnSquare())
                foreach (Square to in Square.YieldSquares())
                {
                    FigureMovement fm = new FigureMovement(fs, to);
                    if (moves.CanMove(fm))
                        yield return fm.ToString();
                }
        }

        void FindAllMoves()  // Функция, кторая находит все возможные в текущей позиции фигур ходы
        {
            allMoves = new List<FigureMovement>();
            foreach (FigureOnSquare fs in board.YieldFigureOnSquare()) // Найдем все фигуры на квадратах того типа, чей сейчас ход.  *Пересечение множеств фигур на клетках и клеток, куда они могут пойти* 
                foreach (Square to in Square.YieldSquares()) // Перебор всех клеток на доске, куда мы можем пойти
                {
                    FigureMovement fm = new FigureMovement(fs, to); // Передаем фигуры на клетках, которые могут куда-то пойти. fs  - это каждая фигура того типа, что сейчас ходит, to - это все квадраты поля
                    if (moves.CanMove(fm)) // Если ход на конкретный квадрат может быть сделан, тогда
                            allMoves.Add(fm); // добавляем его в список всех возможных в текущей позиции ходов
                }
        }

        public List<string> GetAllMoves()  // Переводим список возмождных ходов в строчный формат для вывода на экран
        {
            FindAllMoves();
            List<string> list = new List<string>(); // Создаем строчный список ходов
            foreach (FigureMovement fm in allMoves)
                list.Add(fm.ToString());
            return list;
        }

        public int Who(string move) // Проверка на окончание игры
        {
            if (board.WhoIsWin(move) == FiguresType.defendingFigures)
            {
                return 1;
            }
            else return 0;
        }
    }
}
