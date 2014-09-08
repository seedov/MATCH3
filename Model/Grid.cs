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

        public List<Cell[]> FindVerticalMatchInSequence(Cell[] sequence)
        {
            var vertSequences = new List<Cell[]>();
            var vertGroup = sequence.GroupBy(c => c.ColIndex).ToArray(); // FindVerticalMatchInSequence(sequence);
            foreach (var seq in vertGroup)
            {
                var orderedSequence = seq.OrderBy(c=>c.RowIndex).ToArray();
                var isGoodSequence = true;
                for (var i=0;i<orderedSequence.Length-1; ++i)
                {
                    var cell = orderedSequence[i];
                    if (cell.Up != null)
                    {
                        if (cell.Up != orderedSequence[i + 1])
                        {
                            isGoodSequence = false;
                            break;
                        }
                    }
                }
                if (isGoodSequence)
                    vertSequences.Add(orderedSequence);
            }
            return vertSequences;

            //var result = new List<Cell>();
            //result.Add(sequence[0]);
            //for (var i = 0; i < sequence.Length-1; ++i)
            //{
            //    if (sequence[i].Up!=null && sequence[i].Up == sequence[i+1] && sequence[i].Element.State == sequence[i+1].Element.State)
            //    {
            //        result.Add(sequence[i+1]);
            //    }
            //    else
            //    {
            //        if (result.Count < 3)
            //        {
            //            result.Clear();
            //            result.Add(sequence[i+1]);
            //        }
            //    }
            //}

            //if (result.Count > 2)
            //{
            //    return result.ToArray();
            //}
            //return null;
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

        private Cell[] GetCellsInRadius(int radius, Cell c, Cell[] sequence)
        {
            var seq = sequence.ToList();

            var leftColInd = Math.Max(0, c.ColIndex - radius);
            var downRowInd = Math.Max(0, c.RowIndex - radius);
            var rightColInd = Math.Min(Width-1, c.ColIndex + radius);
            var upRowInd = Math.Min(Height-1, c.RowIndex + radius);
            for(var i=leftColInd; i<=rightColInd; ++i)
                for (var j = downRowInd; j <= upRowInd; ++j)
                {
                    if (!seq.Contains(Cells[i, j])) seq.Add(Cells[i, j]);
                }
            return seq.ToArray();
        }

        private Cell[] GetCellsInCross(Cell c)//, Cell[] sequence)
        {
            var seq = new List<Cell>();// sequence.ToList();
            var cell = c;
            while (cell.Up != null)
            {
                if (!seq.Contains(cell.Up))
                    seq.Add(cell.Up);
                cell = cell.Up;
            }
            cell = c;
            while (cell.Down != null)
            {
                if (!seq.Contains(cell.Down)) seq.Add(cell.Down);
                cell = cell.Down;
            }
            cell = c;
            while (cell.Left != null)
            {
                if (!seq.Contains(cell.Left)) seq.Add(cell.Left);
                cell = cell.Left;
            }
            cell = c;
            while (cell.Right != null)
            {
                if (!seq.Contains(cell.Right)) seq.Add(cell.Right);
                cell = cell.Right;
            }
            return seq.ToArray();
        }

        public Cell[] GetCellsInX(Cell c)
        {
            var seq = new List<Cell>();

            //найти самую дальнюю точку левого нижнего луча
            int x0=0, y0=0, x1=Width-1, y1 = Height-1;
            if (c.ColIndex > c.RowIndex)
            {
                x0 = c.ColIndex - c.RowIndex;
                y0 = 0;
            }
            else if(c.RowIndex > c.ColIndex)
            {
                y0 = c.RowIndex - c.ColIndex;
                x0 = 0;
            }
            else
            {
                x0 = 0;
                y0 = 0;
            }

            for (var i = 0; i < Math.Min(c.ColIndex, c.RowIndex); ++i)
                seq.Add(Cells[x0 + i, y0 + i]);

            //найти самую дальнюю точку левого верхнего луча
            if (c.ColIndex > Height - 1 - c.RowIndex)
            {
                x0 = c.ColIndex - (Height - 1 - c.RowIndex);
                y0 = Height-1;
            }
            else if (c.ColIndex < Height - 1 - c.RowIndex)
            {
                y0 = (c.ColIndex + c.RowIndex);
                x0 = 0;
            }
            else
            {
                x0 = 0;
                y0 = Height - 1;
            }
            for (var i = 0; i < Math.Min(c.ColIndex, Height-1-c.RowIndex); ++i)
                seq.Add(Cells[x0 + i, y0 - i]);

            //найти самую дальнюю точку правого верхнего луча
            if (Width - 1 - c.ColIndex > Height - 1 - c.RowIndex)
            {
                x0 = c.ColIndex + (Height-1- c.RowIndex);
                y0 = Height - 1;
            }
            else if (Width - 1 - c.ColIndex < Height - 1 - c.RowIndex)
            {
                x0 = Width - 1;
                y0 = c.RowIndex + ((Width-1) - c.ColIndex);
            }
            else
            {
                x0 = Width - 1;
                y0 = Height - 1;
            }
            for (var i = 0; i < Math.Min(Width -1 -c.ColIndex, Height - 1 - c.RowIndex); ++i)
                seq.Add(Cells[x0 - i, y0 - i]);

            //найтить самую дальнюю точку правого нижнего луча
            if (Width - 1 - c.ColIndex > c.RowIndex)
            {
                x0 = c.ColIndex + c.RowIndex;
                y0 = 0;
            }
            else if (Width - 1 - c.ColIndex < c.RowIndex)
            {
                x0 = Width - 1;
                y0 = Width -1 - c.ColIndex;
            }
            else
            {
                x0 = Width - 1;
                y0 = 0;
            }
            for (var i = 0; i < Math.Min(Width - 1 - c.ColIndex, c.RowIndex); ++i)
                seq.Add(Cells[x0 - i, y0 + i]);

            return seq.ToArray();
        }

        /// <summary>
        /// Найти последовательность для уничтожения с учетом всех дополнительных эффектов у элементов в исходной последовательность <see cref=" sequence"/>. Доп эффект когда какойто элемент разрушает помимо себя еще какието элементы
        /// </summary>
        /// <param name="sequence">Исходная последовательность без учета доп. эффектов</param>
        /// <returns>Последовательность с учетом доп. эффектов</returns>
        public Cell[] FindSequenceToDestroy(Cell[] sequence)
        {
            var seq = sequence.ToList();


            //проверить каждый элемент на наличие доп. эффектов и применить эти эффекты
            foreach (var c in sequence)
            {
                switch (c.Element.Effect)
                {
                    case Effects.radius1:
                        sequence = GetCellsInRadius(1, c, sequence);
                        break;

                    case Effects.radius2:
                        sequence = GetCellsInRadius(2, c, sequence);
                        break;

                    case Effects.cross:
                        var cross = GetCellsInCross(c);//, sequence);
                        sequence = seq.Union(cross).ToArray();
                        break;

                    case Effects.star:
                        var X = GetCellsInX(c);
                        cross = GetCellsInCross(c);
                        foreach (var cell in X)
                            if (!seq.Contains(cell)) seq.Add(cell);
                        sequence = seq.Union(X).Union(cross).ToArray();
                        break;

                    case Effects.all:
                        foreach (var cl in Cells)
                            if (!seq.Contains(cl)) seq.Add(cl);
                        sequence = seq.ToArray();
                        break;
                }
            }
            return sequence;
        }

        /// <summary>
        /// Уничтожить произвольную последовательность и провалить все элементы, которые находились над уничтоженными
        /// </summary>
        /// <param name="sequence">Уничтожаемая последовательность</param>
        public void DestroySequence(Cell[] sequence)
        {
            var seq = FindSequenceToDestroy(sequence);

            //sequence = FindSequenceToDestroy(sequence);
            var sequenceList = seq.ToList();
            var vertSequences = FindVerticalMatchInSequence(seq);

            Cell[] notVertSeq= seq;
            if (seq.Length < 3) return;

            foreach (var vertSeq in vertSequences)
            {
                if (vertSeq != null)
                {
                    DestroyVerticalSequence(vertSeq.ToArray());
                    notVertSeq = notVertSeq.Except(vertSeq).ToArray();
                }
            }

            notVertSeq = notVertSeq.OrderByDescending(c => c.RowIndex).ToArray();

            foreach (var c in  notVertSeq)
            {
                var destroyableElement = c.Element;

                var cell = c;
                while (cell.Up != null && !sequenceList.Contains(cell.Up))
                {
                    cell.Element = cell.Up.Element;
                    cell = cell.Up;
                }
                destroyableElement.Init();
                sequenceList.Remove(c);
                cell.Element = destroyableElement;
            }

            var last = sequence[sequence.Length - 1];
            switch (sequence.Length)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    last.Element.Effect = Effects.no;
                    break;
                case 4:
                    last.Element.IsUniversal = true;//.Effect = Effects.uni;// State.uni;
                    break;
                case 5:
                    last.Element.Effect = Effects.radius1;
                    break;
                case 6:
                    last.Element.Effect = Effects.cross;
                    break;
                case 7:
                default:
                    last.Element.Effect = Effects.star;
                    break;
            }

            OnSequenceDestroyed(seq);
        }



        public event Action<Cell[]> SequenceDestroyed
        {
            add { sequenceDestroyed += value; }
            remove { sequenceDestroyed -= value; }
        }
        private Action<Cell[]> sequenceDestroyed;
        public void OnSequenceDestroyed(Cell[] sequence)
        {


            var h = sequenceDestroyed;
            if (h != null) h(sequence);


        }
    }
}
