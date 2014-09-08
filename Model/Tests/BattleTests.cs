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

    }
}
