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
                Assert.IsNotNull(c.Element);
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

                    for (var j = 0; j < h; ++j)
                    {
                        columnState[j] = grid.Cells[i, j].Element.State;
                    }

                    grid.ScrollColumn(i, stepsCnt);

                    for (var j = 0; j < h; ++j)
                    {
                        if (stepsCnt < 0) stepsCnt += h;
                        var rowIndex = j + stepsCnt >= h ? (j + stepsCnt)-h : j + stepsCnt;

                        Assert.AreEqual(columnState[rowIndex], grid.Cells[i, j].Element.State);
                    }
                    //Assert.AreEqual(columnState[0], grid.Cells[i, h - 1].Element.State);
                }
            }


            
        }
    }
}
