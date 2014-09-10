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
        [TestMethod]
        public void TestAttackMonster()
        {
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


            player.CollectElements(new[] { new Element() { State = State.s1 }, new Element() { State = State.s1 }, new Element() { State = State.s2 } });
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
            var player = new Player();
            var monster = new Monster();
            player.Enemy = monster;
            monster.Enemy = player;

            Assert.AreEqual(100, monster.Health);

            player.CollectElements(new[] { new Element() { State = State.s1 }, new Element() { State = State.s1, IsUniversal=true }, new Element() { State = State.s1 } });
            player.AttackEnemy();

            Assert.AreEqual(100 - 3 * 1.5f, monster.Health);

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

            grid.Cells[2, 1].Element.Effect = Effects.radius1;
            
            player.CollectElements( new[] {grid.Cells[1,1].Element, grid.Cells[2,1].Element, grid.Cells[3,1].Element});
            player.AttackEnemy();

            Assert.AreEqual(100 - 3 * 2f, monster.Health);

        }

        [TestMethod]
        public void TestAttackMonsterWithCrossFireSpecialElements()
        {
            var player = new Player();
            var monster = new Monster();
            player.Enemy = monster;
            monster.Enemy = player;

            Assert.AreEqual(100, monster.Health);

            player.CollectElements(new[] { new Element() { State = State.s1 }, new Element() { State = State.s1, Effect = Effects.cross }, new Element() { State = State.s1 } });
            player.AttackEnemy();

            Assert.AreEqual(100 - 3 * 2f, monster.Health);

        }
        [TestMethod]
        public void TestAttackMonsterWithStarFireSpecialElements()
        {
            var player = new Player();
            var monster = new Monster();
            player.Enemy = monster;
            monster.Enemy = player;

            Assert.AreEqual(100, monster.Health);

            Assert.AreEqual(0, player.Storage.FireCnt);
            Assert.AreEqual(0, player.Storage.WaterCnt);
            Assert.AreEqual(0, player.Storage.NatureCnt);
            Assert.AreEqual(0, player.Storage.LightCnt);
            Assert.AreEqual(0, player.Storage.DarknessCnt);

            player.CollectElements(new[] { new Element() { State = State.s1 }, new Element() { State = State.s1, Effect = Effects.star }, new Element() { State = State.s1 } });
            player.AttackEnemy();

            Assert.AreEqual(100 - 3 * 3f, monster.Health);

            Assert.AreEqual(3, player.Storage.FireCnt);
            Assert.AreEqual(0, player.Storage.WaterCnt);
            Assert.AreEqual(0, player.Storage.NatureCnt);
            Assert.AreEqual(0, player.Storage.LightCnt);
            Assert.AreEqual(0, player.Storage.DarknessCnt);

        }

        [TestMethod]
        public void TestAttackMonsterWithMultipleFireEffects()
        {
            var player = new Player();
            var monster = new Monster();
            player.Enemy = monster;
            monster.Enemy = player;

            Assert.AreEqual(100, monster.Health);

            player.CollectElements(new[] { 
                new Element() { State = State.s1, IsUniversal = true }, 
                new Element() { State = State.s1, IsUniversal = true }, 
                new Element() { State = State.s1 }, 
                new Element() { State = State.s1 } 
            });

            player.AttackEnemy();

            Assert.AreEqual(100-9, monster.Health);
        }

        [TestMethod]
        public void TestAttackMonsterWithUniversalWaterSpecialElements()
        {
            var player = new Player();
            var monster = new Monster();
            player.Enemy = monster;
            monster.Enemy = player;

            Assert.AreEqual(100, monster.Health);
            Assert.AreEqual(0, player.Storage.FireCnt);
            Assert.AreEqual(0, player.Storage.WaterCnt);
            Assert.AreEqual(0, player.Storage.NatureCnt);
            Assert.AreEqual(0, player.Storage.LightCnt);
            Assert.AreEqual(0, player.Storage.DarknessCnt);

            player.CollectElements(new[] { new Element() { State = State.s2 }, new Element() { State = State.s2, IsUniversal = true }, new Element() { State = State.s2 } });
            player.AttackEnemy();

            Assert.AreEqual(100 - 3, monster.Health);
            Assert.AreEqual(0, player.Storage.FireCnt);
            Assert.AreEqual(4.5f, player.Storage.WaterCnt);
            Assert.AreEqual(0, player.Storage.NatureCnt);
            Assert.AreEqual(0, player.Storage.LightCnt);
            Assert.AreEqual(0, player.Storage.DarknessCnt);
        }

        [TestMethod]
        public void TestAttackMonsterWithUniversalNatureSpecialElements()
        {
            var player = new Player();
            var monster = new Monster();
            player.Enemy = monster;
            monster.Enemy = player;

            Assert.AreEqual(100, monster.Health);
            Assert.AreEqual(100, player.Health);

            player.CollectElements(new[] { new Element() { State = State.s3 }, new Element() { State = State.s3, IsUniversal = true }, new Element() { State = State.s3 } });
            player.AttackEnemy();

            Assert.AreEqual(100 - 3, monster.Health);
            Assert.AreEqual(100+.3f, player.Health);
        }

        [TestMethod]
        public void TestAttackMonsterWithUniversalLightSpecialElements()
        {
            var player = new Player();
            var monster = new Monster();
            player.Enemy = monster;
            monster.Enemy = player;

            Assert.AreEqual(100, monster.Health);
            Assert.AreEqual(100, player.Health);

            player.CollectElements(new[] { new Element() { State = State.s4 }, new Element() { State = State.s4, IsUniversal = true }, new Element() { State = State.s4 } });
            player.AttackEnemy();

            Assert.AreEqual(100 - 3, monster.Health);

            monster.AttackEnemy();
        }

        [TestMethod]
        public void TestAttackMonsterWithUniversalDarkSpecialElements()
        {
            var player = new Player();
            var monster = new Monster();
            player.Enemy = monster;
            monster.Enemy = player;

            Assert.AreEqual(100, monster.Health);
            Assert.AreEqual(100, player.Health);

            player.CollectElements(new[] { new Element() { State = State.s5 }, new Element() { State = State.s5, IsUniversal = true }, new Element() { State = State.s5 } });
            player.AttackEnemy();

            Assert.AreEqual(Math.Round(100 - 3 - 0.15, 2), Math.Round(monster.Health, 2));
            Assert.AreEqual(Math.Round(100+0.15,2), Math.Round(player.Health,2));
            
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
