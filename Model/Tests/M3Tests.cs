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

            var sequence = new Cell[]{grid.Cells[0,1], grid.Cells[1,1], grid.Cells[2,2], grid.Cells[2,3], grid.Cells[2,4]};
            grid.DestroySequence(sequence);

            Assert.AreEqual(gridState[0, 2], grid.Cells[0, 1].Element.State);
            Assert.AreEqual(gridState[1, 2], grid.Cells[1, 1].Element.State);
            Assert.AreEqual(gridState[2, 3], grid.Cells[2, 2].Element.State);
            Assert.AreEqual(gridState[2, 4], grid.Cells[2, 3].Element.State);
            Assert.AreEqual(gridState[2, 5], grid.Cells[2, 4].Element.State);

            
        }
    }
}
