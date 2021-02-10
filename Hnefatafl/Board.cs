using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hnefatafl
{
    class Board
    {
        public string Fen { get; private set; }
        Figure[,] Figures { get; set; }
        public FiguresType MoveFiguresType { get; private set; }
        public int MoveNumber { get; private set; } // Номер хода

        public Board(string fen)
        {
            this.Fen = fen;
            Figures = new Figure[9, 9];
            Init(); // Инициализатор  начальной позиции fen, расположения всех фигур
        }
        void Init()  // Распарсим fen
        {
            //"3AAA3/4A4/4D4/A3D3A/AADDKDDAA/A3D3A/4D4/4A4/3AAA3 A 1"
            //0                                                  1 2   
            string[] parts = Fen.Split(' ');
            if (parts.Length != 3) return;
            InitFiguresPosition(parts[0]);  // Позиции фигур
            MoveFiguresType = (parts[1] == "A") ? FiguresType.attackingFigures : FiguresType.defendingFigures; // Проверка типа фигуры
            MoveNumber = int.Parse(parts[2]);
        }
        void GenerateFen()  // функция генерации fen после нового хода  (по состоянию доски получаем расположение всех фигур)
        {
            Fen = FenFigures() + " " + (MoveFiguresType == FiguresType.attackingFigures ? "A" : "D") + " " + MoveNumber.ToString();
        }

        string FenFigures()  // fen фигур
        {
            StringBuilder figuresPlacement = new StringBuilder();  // Конструктор строк
            for (int y = 8; y >= 0; y--)
            {
                for (int x = 0; x < 9; x++)
                    figuresPlacement.Append(Figures[x, y] == Figure.none ? '1' : (char)Figures[x, y]);  // Добавляем в  созданную строку либо единицу, либо фигуру, в зависимости от текущего состояния доски
                if (y > 0)
                    figuresPlacement.Append('/'); // Если мы дошли до конца строки и она еще не последняя, то добавляем в конец строки разделитель "/"
            }
            string nine = "111111111"; // Меняем единицы на общее количество пустых клеток
            for (int j = 9; j >= 2; j--) // Меняем только цифры  2-8 
            {
                figuresPlacement.Replace(nine.Substring(0, j), j.ToString()); // Замена количество единиц на значение количества
            }
            return figuresPlacement.ToString();
        }

        //"3AAA3/4A4/4D4/A3D3A/AADDKDDAA/A3D3A/4D4/4A4/3AAA3 S 1"
        void InitFiguresPosition(string data)  // Метод, позволяющий разместить фигуры на на доске
        {
            for (int j = 9; j >= 2; j--)
                data = data.Replace(j.ToString(), (j - 1).ToString() + "1");  // Замена  8 ->  71, 71 -> 6111, 6111 -> 51111 и тд. 11111111  Заменяем все точки на единички
            data = data.Replace("1", "."); // Меняем обратно единицы на точки
            string[] lines = data.Split('/');  // Делим массив на строки
            for (int y = 8; y >= 0; y--)
                for (int x = 0; x < 9; x++)
                    Figures[x, y] = lines[8 - y][x] == '.' ? Figure.none : (Figure)lines[8 - y][x]; // Расставляем фигуры на доске
        }

        public IEnumerable<FigureOnSquare> YieldFigureOnSquare()  // Получить фигуры на квадрате
        {
            foreach (Square square in Square.YieldSquares()) //Переберем все квадраты. При получении каждого элемента в цикле срабатывает оператор yield return, 
                                                             //который возвращает элемент и запоминает текущую позицию.
                if (GetFigureAt(square).GetFiguresType() == MoveFiguresType)  // Если тип фигуры на квадрате будет соответствовать тип фигуры, от которой ожидается ход
                    yield return new FigureOnSquare(GetFigureAt(square), square);   // Вернуть значение фигуры на квадрате
        }

        public Figure GetFigureAt(Square square)  // Метод - Взять фигуру на квадрате
        {
            if (square.IsSquareOnBoard()) // Проверка, находится ли квадрат на доске
                return Figures[square.X, square.Y]; // Возвращаем фигуру с координатами x, y, стоящую на квадрате
            return Figure.none; // Иначе не возвращаем никакую фигуру
        }

        void SetFigureAt(Square square, Figure figure) // Метод - установить фигуру на нужный квадрат. Принимаем вид фигуры и координаты квадрата
        {
            if (square.IsSquareOnBoard())
                Figures[square.X, square.Y] = figure;
        }

        public void DestroyFigureAt(Square square) // Метод - убрать фигуру на  квадрате
        {
            if (square.IsSquareOnBoard())
                Figures[square.X, square.Y] = Figure.none;
        }

        public Board Move(FigureMovement figureMovement) // Используем контейнер FigureMoving для перемещения фигуры
        {
            Board next = new Board(Fen); // Создаем новую доску со старой позицией fen 
            Moves moves = new Moves(next);

            next.SetFigureAt(figureMovement.From, Figure.none); // С клетки с фигурой, которая ходит в текущий момент перемещаем фигуру. Эта клетка становится пустой. 
            next.SetFigureAt(figureMovement.To, figureMovement.Figure); // Ставим взятую фигуру на новую клетку
            moves.DestroyFiguresAround(figureMovement);
            if (MoveFiguresType == FiguresType.defendingFigures)
                next.MoveNumber++;
            next.MoveFiguresType = MoveFiguresType.FlipFiguresType();
            next.GenerateFen();  // Генерация нового fen после совершенного хода и новой созданной доски
            return next;
        }

        public FiguresType WhoIsWin(string move) // Проверка, чья сторона победила, на основании того, кто ходил последним
        {
            string firstChar = move.Substring(0, 1);
            FiguresType MoveFiguresType = (firstChar == "A") ? FiguresType.attackingFigures : FiguresType.defendingFigures; // Проверка типа фигуры
            return MoveFiguresType;
        }
    }
}
