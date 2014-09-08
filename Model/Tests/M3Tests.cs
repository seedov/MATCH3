using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace Tests
{
    [TestClass]
    public class M3Tests
    {
        [TestMethod]
        public void TestGenerateGrid()
        {
            var w=6;
            var h=6;
            var grid = new Grid(w,h);

            for (var i = 0; i < w; ++i)
            {
                Assert.IsNull(grid.Cells[i, 0].Down);
                Assert.IsNull(grid.Cells[i, h-1].Up);
                for (var j = 1; j < h - 1; ++j)
                {
                    Assert.AreEqual(grid.Cells[i, j - 1], grid.Cells[i, j].Down);
                    Assert.AreEqual(grid.Cells[i, j + 1], grid.Cells[i, j].Up);
                }
            }

            for (var j = 0; j < h; ++j)
            {
                Assert.IsNull(grid.Cells[0, j].Left);
                Assert.IsNull(grid.Cells[w - 1, j].Right);
                for (var i = 1; i < w - 1; ++i)
                {
                    Assert.AreEqual(grid.Cells[i+1, j], grid.Cells[i, j].Right);
                    Assert.AreEqual(grid.Cells[i-1, j], grid.Cells[i, j].Left);
                }
            }

            foreach (var c in grid.Cells)
            {
                Assert.IsNotNull(c.Element);
                Assert.AreNotEqual(State.empty, c.Element.State);
            }
        }

        [TestMethod]
        public void TestScrollColumn()
        {
            var w=6;
            var h=6;

            var grid = new Grid(w, h);

            var columnState = new State[h];
            for (var stepsCnt = -h; stepsCnt < h; ++stepsCnt)
            {
                for (var i = 0; i < w; ++i)
                {
                    //Сохранить состояние столбца до скролла
                    for (var j = 0; j < h; ++j)
                        columnState[j] = grid.Cells[i, j].Element.State;

                    //Проскроллить столбец
                    grid.ScrollColumn(i, stepsCnt);

                    for (var j = 0; j < h; ++j)
                    {
                        if (stepsCnt < 0) stepsCnt += h;
                        var rowIndex = j + stepsCnt >= h ? j + stepsCnt - h : j + stepsCnt;

                        Assert.AreEqual(columnState[rowIndex], grid.Cells[i, j].Element.State);
                    }
                    //Assert.AreEqual(columnState[0], grid.Cells[i, h - 1].Element.State);
                }
            }
        }

        [TestMethod]
        public void TestScrollRow()
        {
            var w = 6;
            var h = 6;

            var grid = new Grid(w, h);

            var rowState = new State[w];
            for (var stepsCnt = -h; stepsCnt < h; ++stepsCnt)
            {
                for (var i = 0; i < h; ++i)
                {

                    for (var j = 0; j < w; ++j)
                    {
                        rowState[j] = grid.Cells[j, i].Element.State;
                    }

                    grid.ScrollRow(i, stepsCnt);

                    for (var j = 0; j < w; ++j)
                    {
                        if (stepsCnt < 0) stepsCnt += w;
                        var colIndex = j + stepsCnt >= w ? (j + stepsCnt) - w : j + stepsCnt;

                        Assert.AreEqual(rowState[colIndex], grid.Cells[j, i].Element.State);
                    }
                    //Assert.AreEqual(columnState[0], grid.Cells[i, h - 1].Element.State);
                }
            }
        }

        [TestMethod]
        public void TestMatchColumns()
        {
            var w = 4;
            var h = 6;

            var grid = new Grid(w, h);

            //назначить всем элементам в столбцах разные значения
            for (var j = 0; j < h; ++j)
            {
                for (var i = 0; i < w; ++i )
                    grid.Cells[i, j].Element.State = (State)(j + 1);
            }

            //сделать вертикальную последовательность из трех элементов
            for (var j = 0; j < 3; ++j)
            {
                grid.Cells[0, j].Element.State = State.s1;
                grid.Cells[2, j].Element.State = State.s2;
            }

            //найти вертикальную последовательность
            var verticalSequense = grid.FindVerticalMatch();

            Assert.IsNotNull(verticalSequense);

            for (var j = 0; j < 3; ++j)
                Assert.AreEqual(verticalSequense[j], grid.Cells[0, j]);

            grid.DestroyVerticalSequence(verticalSequense);

            //на место уничтоженных элементов опустились верхние
            for (var j = 0; j < 3; ++j)
                Assert.AreEqual((State)(j+1+3), grid.Cells[0, j].Element.State);

            //освободившееся место - пусто
//            for (var j = 3; j < h; ++j)
//                Assert.AreEqual(State.empty, grid.Cells[0, j].Element.State);

            for (var j = 3; j < h; ++j)
                grid.Cells[0, j].Element.State = (State)j;

            //найти еще вертикальную последовательность
            verticalSequense = grid.FindVerticalMatch();

            Assert.IsNotNull(verticalSequense);

            grid.DestroyVerticalSequence(verticalSequense);
        }

        [TestMethod]
        public void TestMatchRows()
        {
            var w = 4;
            var h = 6;

            var grid = new Grid(w, h);

            //назначить всем элементам в строках разные значения
            for (var i = 0; i < w; ++i)
            {
                for (var j = 0; j < h; ++j)
                    grid.Cells[i, j].Element.State = (State)(j + 1);
            }

            //сделать горизонтальную последовательность из трех элементов
            for (var j = 0; j < 3; ++j)
            {
//                grid.Cells[j, 0].Element.State = State.s1;
                grid.Cells[j, 2].Element.State = State.s2;
            }

            //найти горизонтальную последовательность
            var horizontalSequense = grid.FindHorizontalMatch();

            Assert.IsNotNull(horizontalSequense);

            //убедиться что в последовательности правильные элементы
            for (var j = 0; j < 3; ++j)
                Assert.AreEqual(horizontalSequense[j], grid.Cells[j, 2]);

            grid.DestroyHorizontalSequence(horizontalSequense);

            //на место уничтоженных элементов опустились верхние
            for (var j = 0; j < 3; ++j)
                Assert.AreEqual((State)(j + 1 + 3), grid.Cells[j, 2].Element.State);

            //освободившееся место - пусто
            //            for (var j = 3; j < h; ++j)
            //                Assert.AreEqual(State.empty, grid.Cells[0, j].Element.State);

            for (var j = 3; j < h; ++j)
                grid.Cells[0, j].Element.State = (State)j;

            //найти еще вертикальную последовательность
            horizontalSequense = grid.FindHorizontalMatch();

            Assert.IsNotNull(horizontalSequense);

            grid.DestroyHorizontalSequence(horizontalSequense);
        }

        [TestMethod]
        public void TestDestroySequence()
        {
            var w = 6;
            var h = 6;

            var grid = new Grid(w, h);

            var gridState = new State[w, h];
            for (var i = 0; i < w; ++i)
                for (var j = 0; j < h; ++j)
                    gridState[i, j] = grid.Cells[i, j].Element.State;


            grid.Cells[0, 1].Element.State = grid.Cells[1, 1].Element.State = grid.Cells[2, 2].Element.State = grid.Cells[2, 3].Element.State = grid.Cells[2, 4].Element.State = State.s4;
            var sequence = new Cell[]{grid.Cells[0,1], grid.Cells[1,1], grid.Cells[2,2], grid.Cells[2,3], grid.Cells[2,4]};
            grid.DestroySequence(sequence);

            Assert.AreEqual(gridState[0, 2], grid.Cells[0, 1].Element.State);
            Assert.AreEqual(gridState[1, 2], grid.Cells[1, 1].Element.State);
            Assert.AreEqual(gridState[2, 5], grid.Cells[2, 2].Element.State);
        }

        [TestMethod]
        public void TestDestroySequenceWithHole()
        {
            var w = 6;
            var h = 6;

            var grid = new Grid(w, h);

            var gridState = new State[w, h];
            for (var i = 0; i < w; ++i)
                for (var j = 0; j < h; ++j)
                    gridState[i, j] = grid.Cells[i, j].Element.State;

            var sequence = new Cell[] {
            grid.Cells[1, 1],  
            grid.Cells[2, 1],  
            grid.Cells[3, 1],  
            grid.Cells[3, 2], 
            grid.Cells[3, 3], 
            grid.Cells[2, 3], 
            grid.Cells[1, 3], 
            };
            foreach(var c in sequence)
                c.Element.State = State.s4;

            grid.DestroySequence(sequence);

            Assert.AreEqual(gridState[1, 2], grid.Cells[1, 1].Element.State);
            Assert.AreEqual(gridState[2, 2], grid.Cells[2, 1].Element.State);
            Assert.AreEqual(gridState[3, 4], grid.Cells[3, 1].Element.State);
            Assert.AreEqual(gridState[1, 4], grid.Cells[1, 2].Element.State);
            Assert.AreEqual(gridState[2, 4], grid.Cells[2, 2].Element.State);
        }

        [TestMethod]
        public void TestFindVerticalMatchInSequence()
        {
            var w = 6;
            var h = 6;

            var grid = new Grid(w, h);
            grid.Cells[1,1].Element.State = State.s3;
            grid.Cells[1,2].Element.State = State.s3;
            grid.Cells[1,3].Element.State = State.s3;
            grid.Cells[2,3].Element.State = State.s3;
            var seq = grid.FindVerticalMatchInSequence(new[] { grid.Cells[1, 1], grid.Cells[1, 2], grid.Cells[1, 3], grid.Cells[2, 3] });

            Assert.IsNotNull(seq);
        }

        [TestMethod]
        public void TestDestroySequenceWithRadius1Effect()
        {
            var w = 6;
            var h = 6;

            var grid = new Grid(w, h);

            //сохранить состояние сетки
            var gridState = new State[w, h];
            for (var i = 0; i < w; ++i)
                for (var j = 0; j < h; ++j)
                    gridState[i, j] = grid.Cells[i, j].Element.State;

            var seq = new[] { grid.Cells[1, 1], grid.Cells[1, 2], grid.Cells[1, 3], grid.Cells[1,4] };
            foreach (var c in seq)
                c.Element.State = State.s2;

            grid.Cells[1, 3].Element.Effect = Effects.radius1;

            grid.DestroySequence(seq);

            Assert.AreEqual(gridState[1, 5], grid.Cells[1, 1].Element.State);
            Assert.AreEqual(gridState[0, 5], grid.Cells[0, 2].Element.State);
            Assert.AreEqual(gridState[2, 5], grid.Cells[2, 2].Element.State);

            //убедиться что не передалось эффектов никому
            foreach (var c in grid.Cells)
                Assert.AreEqual(Effects.no, c.Element.Effect);

        }

        [TestMethod]
        public void TestDestroySequenceWithRadius2Effect()
        {
            var w = 6;
            var h = 6;

            var grid = new Grid(w, h);

            //сохранить состояние сетки
            var gridState = new State[w, h];
            for (var i = 0; i < w; ++i)
                for (var j = 0; j < h; ++j)
                    gridState[i, j] = grid.Cells[i, j].Element.State;

            grid.Cells[2, 0].Element.State = grid.Cells[2, 1].Element.State = grid.Cells[2, 2].Element.State = State.s2;
            grid.Cells[2, 2].Element.Effect = Effects.radius2;

            grid.DestroySequence(new[] { grid.Cells[2, 0], grid.Cells[2, 1], grid.Cells[2, 2] });

            Assert.AreEqual(gridState[0, 5], grid.Cells[0, 0].Element.State);
            Assert.AreEqual(gridState[1, 5], grid.Cells[1, 0].Element.State);
            Assert.AreEqual(gridState[2, 5], grid.Cells[2, 0].Element.State);
            Assert.AreEqual(gridState[3, 5], grid.Cells[3, 0].Element.State);
            Assert.AreEqual(gridState[4, 5], grid.Cells[4, 0].Element.State);

            //убедиться что не передалось эффектов никому
            foreach (var c in grid.Cells)
                Assert.AreEqual(Effects.no, c.Element.Effect);

        }

        [TestMethod]
        public void TestDestroySequenceWithCrossEffect()
        {
            var w = 6;
            var h = 6;

            var grid = new Grid(w, h);


            grid.Cells[1, 1].Element.State = grid.Cells[1, 2].Element.State = grid.Cells[1, 3].Element.State = State.s2;
            grid.Cells[1, 3].Element.Effect = Effects.cross;

            //сохранить состояние сетки
            var gridState = new State[w, h];
            for (var i = 0; i < w; ++i)
                for (var j = 0; j < h; ++j)
                    gridState[i, j] = grid.Cells[i, j].Element.State;

            grid.DestroySequence(new[] { grid.Cells[1, 1], grid.Cells[1, 2], grid.Cells[1, 3] });



            Assert.AreEqual(gridState[0, 4], grid.Cells[0, 3].Element.State);
            Assert.AreEqual(gridState[2, 4], grid.Cells[2, 3].Element.State);
            Assert.AreEqual(gridState[3, 4], grid.Cells[3, 3].Element.State);
            Assert.AreEqual(gridState[4, 4], grid.Cells[4, 3].Element.State);
            Assert.AreEqual(gridState[5, 4], grid.Cells[5, 3].Element.State);

            //убедиться что не передалось эффектов никому
            foreach (var c in grid.Cells)
                Assert.AreEqual(Effects.no, c.Element.Effect);

        }

        [TestMethod]
        public void TestGetCellsInX()
        {
            var w = 6;
            var h = 6;

            var grid = new Grid(w, h);

            var cells = grid.GetCellsInX(grid.Cells[3, 1]);
            Assert.AreEqual(7, cells.Length);
            Assert.AreEqual(grid.Cells[2, 0], cells[0]);

            Assert.AreEqual(grid.Cells[2, 2], cells[3]);
            Assert.AreEqual(grid.Cells[1, 3], cells[2]);
            Assert.AreEqual(grid.Cells[0, 4], cells[1]);

            Assert.AreEqual(grid.Cells[5, 3], cells[4]);
            Assert.AreEqual(grid.Cells[4, 2], cells[5]);

            Assert.AreEqual(grid.Cells[4, 0], cells[6]);

            //от другой точки
            cells = grid.GetCellsInX(grid.Cells[2, 1]);
            Assert.AreEqual(7, cells.Length);
            Assert.AreEqual(grid.Cells[1, 0], cells[0]);

            Assert.AreEqual(grid.Cells[0, 3], cells[1]);
            Assert.AreEqual(grid.Cells[1, 2], cells[2]);

            Assert.AreEqual(grid.Cells[5, 4], cells[3]);
            Assert.AreEqual(grid.Cells[4, 3], cells[4]);
            Assert.AreEqual(grid.Cells[3, 2], cells[5]);

            Assert.AreEqual(grid.Cells[3, 0], cells[6]);

            //от другой точки
            cells = grid.GetCellsInX(grid.Cells[2, 3]);
            Assert.AreEqual(9, cells.Length);
            Assert.AreEqual(grid.Cells[0, 1], cells[0]);
            Assert.AreEqual(grid.Cells[1, 2], cells[1]);
            Assert.AreEqual(grid.Cells[0, 5], cells[2]);
            Assert.AreEqual(grid.Cells[1, 4], cells[3]);
            Assert.AreEqual(grid.Cells[4, 5], cells[4]);
            Assert.AreEqual(grid.Cells[3, 4], cells[5]);
            Assert.AreEqual(grid.Cells[5, 0], cells[6]);
            Assert.AreEqual(grid.Cells[4, 1], cells[7]);
            Assert.AreEqual(grid.Cells[3, 2], cells[8]);

            //от другой точки
            cells = grid.GetCellsInX(grid.Cells[3, 4]);
            Assert.AreEqual(7, cells.Length);
            Assert.AreEqual(grid.Cells[0, 1], cells[0]);
            Assert.AreEqual(grid.Cells[1, 2], cells[1]);
            Assert.AreEqual(grid.Cells[2, 3], cells[2]);
            Assert.AreEqual(grid.Cells[2, 5], cells[3]);
            Assert.AreEqual(grid.Cells[4, 5], cells[4]);
            Assert.AreEqual(grid.Cells[5, 2], cells[5]);
            Assert.AreEqual(grid.Cells[4, 3], cells[6]);

            //из левого нижнего угла
            cells = grid.GetCellsInX(grid.Cells[0, 0]);
            Assert.AreEqual(5, cells.Length);
            Assert.AreEqual(grid.Cells[5, 5], cells[0]);
            Assert.AreEqual(grid.Cells[4, 4], cells[1]);
            Assert.AreEqual(grid.Cells[3, 3], cells[2]);
            Assert.AreEqual(grid.Cells[2, 2], cells[3]);
            Assert.AreEqual(grid.Cells[1, 1], cells[4]);

            //из левого верхнего угла
            cells = grid.GetCellsInX(grid.Cells[0, 5]);
            Assert.AreEqual(5, cells.Length);
            Assert.AreEqual(grid.Cells[5, 0], cells[0]);
            Assert.AreEqual(grid.Cells[4, 1], cells[1]);
            Assert.AreEqual(grid.Cells[3, 2], cells[2]);
            Assert.AreEqual(grid.Cells[2, 3], cells[3]);
            Assert.AreEqual(grid.Cells[1, 4], cells[4]);

            //из правого нижнего угла
            cells = grid.GetCellsInX(grid.Cells[5, 0]);
            Assert.AreEqual(5, cells.Length);
            Assert.AreEqual(grid.Cells[0, 5], cells[0]);
            Assert.AreEqual(grid.Cells[1, 4], cells[1]);
            Assert.AreEqual(grid.Cells[2, 3], cells[2]);
            Assert.AreEqual(grid.Cells[3, 2], cells[3]);
            Assert.AreEqual(grid.Cells[4, 1], cells[4]);

            //из правого верхнего угла
            cells = grid.GetCellsInX(grid.Cells[5, 5]);
            Assert.AreEqual(5, cells.Length);
            Assert.AreEqual(grid.Cells[0, 0], cells[0]);
            Assert.AreEqual(grid.Cells[1, 1], cells[1]);
            Assert.AreEqual(grid.Cells[2, 2], cells[2]);
            Assert.AreEqual(grid.Cells[3, 3], cells[3]);
            Assert.AreEqual(grid.Cells[4, 4], cells[4]);

            //пробуем с другой (неквадратной) сеткой
            grid = new Grid(6, 4);
            cells = grid.GetCellsInX(grid.Cells[3, 1]);

            Assert.AreEqual(6, cells.Length);
            Assert.AreEqual(grid.Cells[2, 0], cells[0]);
            Assert.AreEqual(grid.Cells[1, 3], cells[1]);
            Assert.AreEqual(grid.Cells[2, 2], cells[2]);
            Assert.AreEqual(grid.Cells[5, 3], cells[3]);
            Assert.AreEqual(grid.Cells[4, 2], cells[4]);
            Assert.AreEqual(grid.Cells[4, 0], cells[5]);


            //cells = grid.GetCellsInX(grid.Cells[3, 1]);
        }

        [TestMethod]
        public void TestDestroySequenceWithStarEffect()
        {
            var w = 7;
            var h = 7;

            var grid = new Grid(w, h);

            grid.Cells[1, 2].Element.State = grid.Cells[2, 2].Element.State = grid.Cells[3, 2].Element.State = State.s2;
            grid.Cells[3, 2].Element.Effect = Effects.star;

            //сохранить состояние сетки
            var gridState = new State[w, h];
            for (var i = 0; i < w; ++i)
                for (var j = 0; j < h; ++j)
                    gridState[i, j] = grid.Cells[i, j].Element.State;

            grid.DestroySequence(new[] { grid.Cells[1, 2], grid.Cells[2, 2], grid.Cells[3, 2] });

            Assert.AreEqual(gridState[0, 0], grid.Cells[0, 0].Element.State);
            Assert.AreEqual(gridState[1, 0], grid.Cells[1, 1].Element.State);
            Assert.AreEqual(gridState[2, 0], grid.Cells[2, 0].Element.State);
            //Assert.AreEqual(gridState[0, 3], grid.Cells[0, 0].Element.State);
            Assert.AreEqual(gridState[4, 0], grid.Cells[4, 0].Element.State);
            Assert.AreEqual(gridState[5, 0], grid.Cells[5, 1].Element.State);
            Assert.AreEqual(gridState[6, 0], grid.Cells[6, 0].Element.State);

        }
    }
}
