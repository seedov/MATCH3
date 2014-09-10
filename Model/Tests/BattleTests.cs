using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Battle;

namespace Tests
{
    [TestClass]
    public class BattleTests
    {
        Grid grid;
        Player player;
        Monster monster;

        [TestInitialize]
        public void InitTests()
        {
            grid = new Grid(6, 6);
            player = new Player();
            monster = new Monster();
            player.Enemy = monster;
            monster.Enemy = player;

        }

        [TestMethod]
        public void TestAttackMonster()
        {
            var grid = new Grid(6, 6);
            var player = new Player();
            var monster = new Monster();
            player.Enemy = monster;
            monster.Enemy = player;

            Assert.AreEqual(100, monster.Health);
            Assert.AreEqual(100, player.Health);

            Assert.AreEqual(0, player.Storage.FireCnt);
            Assert.AreEqual(0, player.Storage.WaterCnt);
            Assert.AreEqual(0, player.Storage.NatureCnt);
            Assert.AreEqual(0, player.Storage.LightCnt);
            Assert.AreEqual(0, player.Storage.DarknessCnt);

            grid.Cells[0, 0].Element.State = State.s1;
            grid.Cells[1, 0].Element.State = State.s1;
            grid.Cells[2, 0].Element.State = State.s2;

            player.CollectElements(new[] { grid.Cells[0, 0].Element, grid.Cells[1, 0].Element, grid.Cells[2, 0].Element});
            player.AttackEnemy();

            Assert.AreEqual(2, player.Storage.FireCnt);
            Assert.AreEqual(1, player.Storage.WaterCnt);
            Assert.AreEqual(0, player.Storage.NatureCnt);
            Assert.AreEqual(0, player.Storage.LightCnt);
            Assert.AreEqual(0, player.Storage.DarknessCnt);

            Assert.AreEqual(97, monster.Health);
            Assert.AreEqual(100, player.Health);
        }

        [TestMethod]
        public void TestAttackPlayer()
        {
            var player = new Player();
            var monster = new Monster();
            player.Enemy = monster;
            monster.Enemy = player;

            Assert.AreEqual(100, player.Health);
            monster.AttackEnemy();
            Assert.AreNotEqual(100, player.Health);
        }

        [TestMethod]
        public void TestAttackMonsterWithUniversalFireSpecialElements()
        {
            var grid = new Grid(6, 6);
            var player = new Player();
            var monster = new Monster();
            player.Enemy = monster;
            monster.Enemy = player;

            Assert.AreEqual(100, monster.Health);

            var seq = new [] { grid.Cells[1, 1].Element, grid.Cells[2, 1].Element, grid.Cells[3, 1].Element };
            foreach (var e in seq)
                e.State = State.s1;
            grid.Cells[2, 1].Element.IsUniversal = true;

            player.CollectElements(seq);
            player.AttackEnemy();

            Assert.AreEqual(Math.Round(100 - 3 * 1.5f), monster.Health);

        }
        [TestMethod]
        public void TestAttackMonsterWithChargedFireSpecialElements()
        {
            var grid = new Grid(6, 6);
            var player = new Player();
            var monster = new Monster();
            player.Enemy = monster;
            monster.Enemy = player;

            Assert.AreEqual(100, monster.Health);

            foreach (var c in grid.Cells)
                c.Element.State = State.s2;
                

            var seq = new[] { grid.Cells[1, 1].Element, grid.Cells[2, 1].Element, grid.Cells[3, 1].Element };
            foreach (var e in seq)
                e.State = State.s1;

            grid.Cells[2, 1].Element.Effect = Effects.radius1;
            
            player.CollectElements( seq);
            player.AttackEnemy();

            Assert.AreEqual(100 - (3 +3 + 3)* 2, monster.Health);

        }

        [TestMethod]
        public void TestAttackMonsterWithCrossFireSpecialElements()
        {
            var grid = new Grid(6, 6);
            var player = new Player();
            var monster = new Monster();
            player.Enemy = monster;
            monster.Enemy = player;

            Assert.AreEqual(100, monster.Health);

            foreach (var c in grid.Cells)
                c.Element.State = State.s2;

            var seq = new[] { grid.Cells[1, 1].Element, grid.Cells[2, 1].Element, grid.Cells[3, 1].Element };
            foreach (var e in seq)
                e.State = State.s1;

            grid.Cells[2, 1].Element.Effect = Effects.cross;

            player.CollectElements(seq);
            player.AttackEnemy();

            Assert.AreEqual(Math.Round(100 - (6+6-1) * 2f), monster.Health);

        }
        [TestMethod]
        public void TestAttackMonsterWithStarFireSpecialElements()
        {
            var grid = new Grid(6, 6);
            var player = new Player();
            var monster = new Monster();
            player.Enemy = monster;
            monster.Enemy = player;

            foreach (var c in grid.Cells)
                c.Element.State = State.s2;

            var seq = new[] { grid.Cells[1, 1].Element, grid.Cells[2, 1].Element, grid.Cells[3, 1].Element};
            foreach (var e in seq)
                e.State = State.s1;
            grid.Cells[1, 1].Element.Effect = Effects.star;

            Assert.AreEqual(100, monster.Health);

            Assert.AreEqual(0, player.Storage.FireCnt);
            Assert.AreEqual(0, player.Storage.WaterCnt);
            Assert.AreEqual(0, player.Storage.NatureCnt);
            Assert.AreEqual(0, player.Storage.LightCnt);
            Assert.AreEqual(0, player.Storage.DarknessCnt);

            player.CollectElements(seq);
            player.AttackEnemy();

            Assert.AreEqual(100 - 18 * 3f, monster.Health);

            Assert.AreEqual(3, player.Storage.FireCnt);
            Assert.AreEqual(15, player.Storage.WaterCnt);
            Assert.AreEqual(0, player.Storage.NatureCnt);
            Assert.AreEqual(0, player.Storage.LightCnt);
            Assert.AreEqual(0, player.Storage.DarknessCnt);

        }

        [TestMethod]
        public void TestAttackMonsterWithMultipleFireEffects()
        {
            var grid = new Grid(6, 6);
            var player = new Player();
            var monster = new Monster();
            player.Enemy = monster;
            monster.Enemy = player;

            Assert.AreEqual(100, monster.Health);
            var seq = new[] { grid.Cells[1, 1].Element, grid.Cells[2, 1].Element, grid.Cells[3, 1].Element, grid.Cells[4,1].Element };
            foreach (var e in seq)
                e.State = State.s1;
            grid.Cells[1, 1].Element.IsUniversal = true;
            grid.Cells[3, 1].Element.IsUniversal = true;

            player.CollectElements(seq);
            player.AttackEnemy();

            Assert.AreEqual(Math.Round(100-(4*1.5+4*1.5)), monster.Health);
        }

        [TestMethod]
        public void TestAttackMonsterWithUniversalWaterSpecialElements()
        {

            Assert.AreEqual(100, monster.Health);
            Assert.AreEqual(0, player.Storage.FireCnt);
            Assert.AreEqual(0, player.Storage.WaterCnt);
            Assert.AreEqual(0, player.Storage.NatureCnt);
            Assert.AreEqual(0, player.Storage.LightCnt);
            Assert.AreEqual(0, player.Storage.DarknessCnt);

            var seq = new[] { grid.Cells[1, 1].Element, grid.Cells[2, 2].Element, grid.Cells[3, 3].Element };
            foreach (var c in seq)
                c.State = State.s2;

            grid.Cells[2, 2].Element.IsUniversal = true;

            player.CollectElements(seq);
            player.AttackEnemy();

            Assert.AreEqual(100 - 3, monster.Health);
        //    Assert.AreEqual(0, player.Storage.FireCnt);
            Assert.AreEqual(4.5f, player.Storage.WaterCnt);
        //    Assert.AreEqual(0, player.Storage.NatureCnt);
        //    Assert.AreEqual(0, player.Storage.LightCnt);
        //    Assert.AreEqual(0, player.Storage.DarknessCnt);
        }

        [TestMethod]
        public void TestAttackMonsterWithUniversalNatureSpecialElements()
        {

            var seq = new[] { grid.Cells[1, 1].Element, grid.Cells[2, 2].Element, grid.Cells[3, 3].Element };
            foreach (var c in seq)
                c.State = State.s3;

            grid.Cells[1, 1].Element.IsUniversal = true;


            Assert.AreEqual(100, monster.Health);
            Assert.AreEqual(100, player.Health);

            player.CollectElements(seq);
            player.AttackEnemy();

            Assert.AreEqual(100 , monster.Health);
            Assert.AreEqual(100+3, player.Health);
        }

        [TestMethod]
        public void TestAttackMonsterWithUniversalLightSpecialElements()
        {
            var seq = new[] { grid.Cells[1, 1].Element, grid.Cells[2, 2].Element, grid.Cells[3, 3].Element };
            foreach (var c in seq)
                c.State = State.s4;

            grid.Cells[1, 1].Element.IsUniversal = true;

            Assert.AreEqual(100, monster.Health);
            Assert.AreEqual(100, player.Health);

            player.CollectElements(seq);
            player.AttackEnemy();

            Assert.AreEqual(100 - 3, monster.Health);

            monster.AttackEnemy();
        }

        [TestMethod]
        public void TestAttackMonsterWithUniversalDarkSpecialElements()
        {
            var seq = new[] { grid.Cells[1, 1].Element, grid.Cells[2, 2].Element, grid.Cells[3, 3].Element };
            foreach (var c in seq)
                c.State = State.s5;

            grid.Cells[1, 1].Element.IsUniversal = true;

            Assert.AreEqual(100, monster.Health);
            Assert.AreEqual(100, player.Health);

            player.CollectElements(seq);
            player.AttackEnemy();

            Assert.AreEqual(Math.Round(100 - 3 - 0.15), Math.Round(monster.Health));
            Assert.AreEqual(Math.Round(100+0.15), Math.Round(player.Health));
            
            monster.AttackEnemy();
        }

        [TestMethod]
        public void TestAttackMonsterWithSpecialElement()
        {
            var player = new Player();
            var monster = new Monster();
            player.Enemy = monster;
            monster.Enemy = player;

            Assert.AreEqual(100, monster.Health);
            Assert.AreEqual(100, player.Health);

            player.CollectElements(new[] { new Element() { State = State.s5 }, new Element() { State = State.s5 }, new Element() { State = State.s5 } });
            player.AttackMonsterWithSpecialElement();

            Assert.AreEqual(97, monster.Health);
            Assert.AreEqual(100, player.Health);
        }

    }
}
