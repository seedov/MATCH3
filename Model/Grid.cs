using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{

    public class Grid
    {
        private Cell[,] cells;
        public Cell[,] Cells { get { return cells; } }
        private List<Element> elements = new List<Element>();



        public int Width { get; set; }
        public int Height { get; set; }

        public Grid(int width, int height)
        {
            Width = width;
            Height = height;

            cells = new Cell[Width, Height];
            for (var i = 0; i < Width; ++i)
            {
                for (var j = 0; j < Height; ++j)
                {
                    var cell = new Cell(i, j);

                    cells[i, j] = cell;

                    var element = new Element();
                    element.Init();
                    elements.Add(element);
                    cell.Element = element;
                }
            }

            for (var i = 0; i < Width; ++i)
                for (var j = 0; j < Height; ++j)
                {
                    if (i > 0) cells[i, j].Left = cells[i - 1, j];
                    if (i < Width - 1) cells[i, j].Right = cells[i + 1, j];
                    if (j > 0) cells[i, j].Down = cells[i, j - 1];
                    if (j < Height - 1) cells[i, j].Up = cells[i, j + 1];
                }


            //        CheckRows();
            //        CheckColumns();

        }


        /// <summary>
        /// Прокрутить полностью весь столбец с индексом <see cref="columnIndex"/> на количество ячеек <see cref="length"/>. Элементы скроллятся по кругу.
        /// </summary>
        /// <param name="columnIndex">Индекс столбца для скролла</param>
        /// <param name="length">Количество ячеек на которое происходит скролл. Если больше нуля - скролл вверх, если меньше - вниз</param>
        public void ScrollColumn(int columnIndex, int length)
        {
            while (length >= Height) 
                length -= Height;
            while (length < 0)
                length += Height;
            if (length == 0) return;


            for (var l = 0; l < length; ++l)
            {
                var firstElem = cells[columnIndex, 0].Element;
                for (var j = 0; j < Height - 1; ++j)
                {
                    var rowIndex = j + 1 >= Height ? j + 1 - Height : j + 1;
                    cells[columnIndex, j].Element = cells[columnIndex, rowIndex].Element;
                }
                cells[columnIndex, Height - 1].Element = firstElem;
            }

        }

        /// <summary>
        /// Прокрутить полностью всю строку с индексом <see cref="rowIndex"/> на количество ячеек <see cref="length"/>. Элементы скроллятся по кругу.
        /// </summary>
        /// <param name="columnIndex">Индекс строки для скролла</param>
        /// <param name="length">Количество ячеек на которое происходит скролл. Если больше нуля - скролл вправо, если меньше - влево</param>
        public void ScrollRow(int rowIndex, int length)
        {
            while (length >= Height)
                length -= Height;
            while (length < 0)
                length += Height;
            if (length == 0) return;

            for (var l = 0; l < length; ++l)
            {
                var firstElem = cells[0, rowIndex].Element;


                for (var i = 0; i < Width-1; ++i)
                {
                    var colIndex = i + 1 >= Width ? i + 1 - Width : i + 1;
                    cells[i, rowIndex].Element = cells[colIndex, rowIndex].Element;
                }
                cells[Width - 1, rowIndex].Element = firstElem;
            }
        }

        /// <summary>
        /// Найти вертикальную последовательность ячеек (минимум 3) с одинаковыми элементами
        /// </summary>
        /// <returns>Найденную последовательность или null</returns>
        public Cell[] FindVerticalMatch()
        {
            for (var j = 0; j < Width; ++j)
            {
                var col = new Cell[Height];
                for (var i = 0; i < Height - 1; ++i)
                    col[i] = cells[j, i];

                var sequence = new List<Cell>();
                sequence.Add(col[0]);
                for (var i = 0; i < Height - 1; ++i)
                {
                    if (col[i].Element.State == col[i].Up.Element.State)
                    {
                        sequence.Add(col[i].Up);
                    }
                    else
                    {
                        if (sequence.Count < 3)
                        {
                            sequence.Clear();
                            sequence.Add(col[i].Up);
                        }
                    }
                }

                if (sequence.Count > 2)
                {
                    return sequence.ToArray();
                }


            }
            return null;
        }

        public Cell[] FindVerticalMatchInSequence(Cell[] sequence)
        {

            var result = new List<Cell>();
            result.Add(sequence[0]);
            for (var i = 0; i < sequence.Length-1; ++i)
            {
                if (sequence[i].Up!=null && sequence[i].Up == sequence[i+1] && sequence[i].Element.State == sequence[i+1].Element.State)
                {
                    result.Add(sequence[i+1]);
                }
                else
                {
                    if (result.Count < 3)
                    {
                        result.Clear();
                        result.Add(sequence[i+1]);
                    }
                }
            }

            if (result.Count > 2)
            {
                return result.ToArray();
            }
            return null;
        }

        /// <summary>
        /// Освободить ячейки <see cref="sequence"/>, спустить на их место стоявшие сверху элементы, заполнить освободившееся место новыми элементами
        /// </summary>
        /// <param name="sequence">Последовательность ячеек для освобождения</param>
        public void DestroyVerticalSequence(Cell[] sequence)
        {
            sequence = sequence.OrderBy(c => c.RowIndex).ToArray();
            var destroyableElements = new List<Element>();
            foreach (var c in sequence)
                destroyableElements.Add(c.Element);


            var aboveCell = sequence[sequence.Length - 1].Up;
            var currCell = sequence[0];
            while (aboveCell != null)
            {
                currCell.Element = aboveCell.Element;
                currCell = currCell.Up;
                aboveCell = aboveCell.Up;
            }

            foreach (var e in destroyableElements)
            {
                currCell.Element = e;
                e.Init();
                currCell = currCell.Up;
            }
        }

        /// <summary>
        /// Найти горизонтальную последовательность ячеек (минимум 3) с одинаковыми элементами
        /// </summary>
        /// <returns>Найденную последовательность или null</returns>
        public Cell[] FindHorizontalMatch()
        {
                for (var j = 0; j < Height; ++j)
                {
                    var row = new Cell[Width];
                    for (var i = 0; i < Width - 1; ++i)
                        row[i] = cells[i, j];

                    var sequence = new List<Cell>();
                    sequence.Add(row[0]);
                    for (var i = 0; i < Width - 1; ++i)
                    {
                        if (row[i].Element.State == row[i].Right.Element.State)
                        {
                            sequence.Add(row[i].Right);
                        }
                        else
                        {
                            if (sequence.Count < 3)
                            {
                                sequence.Clear();
                                sequence.Add(row[i].Right);
                            }
                        }

                    }
                    if (sequence.Count > 2)
                    {
                        return sequence.ToArray();
//                        DestroyRow(sequence);
                    }
                }
                return null;
        }

        ///<summary>
        /// Уничтожить горизонтальную секвенцию и переместить все верхние ячейки на их место
        /// </summary>
        public void DestroyHorizontalSequence(Cell[] sequence)
        {
            foreach (var c in sequence)
            {
                var destroyableElement = c.Element;

                var cell = c;
                while (cell.Up != null)
                {
                    cell.Element = cell.Up.Element;
                    cell = cell.Up;
                }
                destroyableElement.Init();
                cell.Element = destroyableElement;
            }
        }

        /// <summary>
        /// Уничтожить произвольную последовательность и провалить все элементы, которые находились над уничтоженными
        /// </summary>
        /// <param name="sequence">Уничтожаемая последовательность</param>
        public void DestroySequence(Cell[] sequence)
        {
            var seq = sequence.ToList();


            //проверить каждый элемент на наличие доп. эффектов и применить эти эффекты
            foreach (var c in sequence)
            {
                switch (c.Element.Effect)
                {
                    case Effects.radius:
                        if (c.Up != null)
                        {
                            if(!seq.Contains(c.Up)) seq.Add(c.Up);
                            if (c.Up.Right != null)
                                if (!seq.Contains(c.Up.Right)) seq.Add(c.Up.Right);
                            if (c.Up.Left != null)
                                if (!seq.Contains(c.Up.Left)) seq.Add(c.Up.Left);
                        }
                        if (c.Down != null)
                        {
                            if (!seq.Contains(c.Down)) seq.Add(c.Down);
                            if (c.Down.Right != null)
                                if (!seq.Contains(c.Down.Right)) seq.Add(c.Down.Right);
                            if (c.Down.Left != null)
                                if (!seq.Contains(c.Down.Left)) seq.Add(c.Down.Left);
                        }
                        if (c.Right != null)
                            if (!seq.Contains(c.Right)) seq.Add(c.Right);
                        if (c.Left != null)
                            if (!seq.Contains(c.Left)) seq.Add(c.Left);

                        sequence = seq.ToArray();

                        break;
                }
            }

            var vertSequences = sequence.GroupBy(c => c.ColIndex); // FindVerticalMatchInSequence(sequence);

            Cell[] notVertSeq= sequence.ToArray(); 
            if (sequence.Length < 3) return;

            foreach (var vertSeq in vertSequences)
            {
                if (vertSeq != null)
                {
                    DestroyVerticalSequence(vertSeq.ToArray());
                    notVertSeq = notVertSeq.Except(vertSeq).ToArray();
                }
            }

            foreach (var c in notVertSeq)
            {
                var destroyableElement = c.Element;

                var cell = c;
                while (cell.Up != null && !sequence.Contains(cell.Up))
                {
                    cell.Element = cell.Up.Element;
                    cell = cell.Up;
                }
                destroyableElement.Init();
                cell.Element = destroyableElement;
            }

            OnSequenceDestroyed(sequence);
        }


        public event Action<Cell[]> SequenceDestroyed
        {
            add { sequenceDestroyed += value; }
            remove { sequenceDestroyed -= value; }
        }
        private Action<Cell[]> sequenceDestroyed;
        public void OnSequenceDestroyed(Cell[] sequence)
        {
            var last = sequence[sequence.Length - 1];
            switch (sequence.Length)
            {
                case 4:
                    last.Element.State = State.uni;
                    break;
                case 5:
                    last.Element.Effect = Effects.radius;
                    break;
                case 6:
                    last.Element.Effect = Effects.cross;
                    break;
                case 7:
                    last.Element.Effect = Effects.all;
                    break;
                default:
                    last.Element.Effect = Effects.no;
                    break;
            }

            var h = sequenceDestroyed;
            if (h != null) h(sequence);


        }
    }
}
