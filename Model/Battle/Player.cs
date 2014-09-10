using System;
using System.Collections.Generic;
using System.Linq;
using Model;

namespace Battle
{
    public class Player:Creature
    {
        public Storage Storage = new Storage();
        Element[] collectedElements;

        /// <summary>
        /// Оставшееся оличество ходов перед ходом противника
        /// </summary>
        public int TurnsToEnemyAttack=3;

        float absorbMultiplier=1;

        public override void ApplyDamage(float Damage, float multiplier=1)
        {
            TurnsToEnemyAttack = 3;
            base.ApplyDamage(Damage*absorbMultiplier);
            absorbMultiplier = 1;
        }

        public void AttackMonsterWithSpecialElement()
        {
            foreach (var e in collectedElements)
            {
                e.DamageMultiplier = 1;
                Storage.Remove(e);
            }
            ((Monster)Enemy).ApplyDamage(collectedElements);

        }

        public override void AttackEnemy()
        {
            TurnsToEnemyAttack--;
            var uniElements = collectedElements.Where(e => e.IsUniversal);

            var uniFireElements = uniElements.Where(e => e.State == State.s1);
            var chargedFireElements = collectedElements.Where(e => e.Effect == Effects.radius1).Where(e => e.State == State.s1);
            var crossFireElements = collectedElements.Where(e => e.Effect == Effects.cross).Where(e => e.State == State.s1);
            var starFireElements = collectedElements.Where(e => e.Effect == Effects.star).Where(e => e.State == State.s1);

            var damageMultiplier = uniFireElements.Count() == 0 ? 1 : 1.5f* uniFireElements.Count();
            damageMultiplier *= chargedFireElements.Count() == 0 ? 1 : 2 * chargedFireElements.Count();
            damageMultiplier *= crossFireElements.Count() == 0 ? 1 : 2 * crossFireElements.Count();
            damageMultiplier *= starFireElements.Count() == 0 ? 1 : 3 * starFireElements.Count();

            var uniWaterElements = collectedElements.Where(e => e.IsUniversal).Where(e => e.State == State.s2);
            var chargedWaterElements = collectedElements.Where(e => e.Effect == Effects.radius1).Where(e => e.State == State.s2);
            var crossWaterElements = collectedElements.Where(e => e.Effect == Effects.cross).Where(e => e.State == State.s2);
            var starWaterElements = collectedElements.Where(e => e.Effect == Effects.star).Where(e => e.State == State.s2);

            var energyMultiplier = uniWaterElements.Count() == 0 ? 1 : 1.5f * uniWaterElements.Count();
            energyMultiplier *= chargedWaterElements.Count() == 0 ? 1 : 2f * chargedWaterElements.Count();
            energyMultiplier *= crossWaterElements.Count() == 0 ? 1 : 2f * crossWaterElements.Count();
            energyMultiplier *= starWaterElements.Count() == 0 ? 1 : 2f * starWaterElements.Count();

            var uniNatureElements = collectedElements.Where(e => e.IsUniversal).Where(e => e.State == State.s3);
            var chargedNatureElements = collectedElements.Where(e => e.Effect == Effects.radius1).Where(e => e.State == State.s3);
            var crossNatureElements = collectedElements.Where(e => e.Effect == Effects.cross).Where(e => e.State == State.s3);
            var starNatureElements = collectedElements.Where(e => e.Effect == Effects.star).Where(e => e.State == State.s3);

            var healMultiplier = uniNatureElements.Count() == 0 ? 0 : 0.1f * uniNatureElements.Count();
            healMultiplier += chargedNatureElements.Count() == 0 ? 0 : 0.25f * chargedNatureElements.Count();
            healMultiplier += crossNatureElements.Count() == 0 ? 0 : 0.5f * crossNatureElements.Count();
            healMultiplier += starNatureElements.Count() == 0 ? 0 : 1f * starNatureElements.Count();

            var uniLightElements = collectedElements.Where(e => e.IsUniversal).Where(e => e.State == State.s4);
            var chargedLightElements = collectedElements.Where(e => e.Effect == Effects.radius1).Where(e => e.State == State.s4);
            var crossLightElements = collectedElements.Where(e => e.Effect == Effects.cross).Where(e => e.State == State.s4);
            var starLightElements = collectedElements.Where(e => e.Effect == Effects.star).Where(e => e.State == State.s4);

            absorbMultiplier = uniLightElements.Count() == 0 ? 1 : 0.1f * uniLightElements.Count();
            absorbMultiplier *= chargedLightElements.Count() == 0 ? 1 : 0.2f * chargedLightElements.Count();
            absorbMultiplier *= crossLightElements.Count() == 0 ? 1 : 0.25f * crossLightElements.Count();
            absorbMultiplier *= starLightElements.Count() == 0 ? 1 : 1f * starLightElements.Count();

            var uniDarkElements = collectedElements.Where(e => e.IsUniversal).Where(e => e.State == State.s5);
            var chargedDarkElements = collectedElements.Where(e => e.Effect == Effects.radius1).Where(e => e.State == State.s5);
            var crossDarkElements = collectedElements.Where(e => e.Effect == Effects.cross).Where(e => e.State == State.s5);
            var starDarkElements = collectedElements.Where(e => e.Effect == Effects.star).Where(e => e.State == State.s5);

            var vampMultiplier = uniDarkElements.Count() == 0 ? 0 : 0.05f * uniDarkElements.Count();
            vampMultiplier += chargedDarkElements.Count() == 0 ? 0 : 0.1f * chargedDarkElements.Count();
            vampMultiplier += crossDarkElements.Count() == 0 ? 0 : 0.15f * crossDarkElements.Count();
            vampMultiplier += starDarkElements.Count() == 0 ? 0 : 0.25f * starDarkElements.Count();

            var seq = Grid.GetElementsToDestroy(collectedElements); 
            
            //применить мультипликатор дамага ко всем элементам
            foreach (var e in seq)// collectedElements)
            {
                e.Element.DamageMultiplier = damageMultiplier;
//                Storage.Add(e, energyMultiplier);
            }


            var elements = seq.Select(c => c.Element).ToArray();
            foreach (var e in elements)
            {
//                e.DamageMultiplier = damageMultiplier;
                Storage.Add(e, energyMultiplier);
            }
            Health += elements.Where(e => e.State == State.s3).Count();
            var damageApplied = ((Monster)Enemy).ApplyDamage(elements.Where(e=>e.State!=State.s3).ToArray());
     //       Health += damageApplied * healMultiplier;
            Health += damageApplied * vampMultiplier;
            ((Monster)Enemy).ApplyDamage(damageApplied*vampMultiplier);

        }
            
        public void CollectElements(Element[] elements)
        {
            collectedElements = elements;
        }
    }

    public class Storage
    {
        public void Clear()
        {
            FireCnt = WaterCnt = NatureCnt = LightCnt = DarknessCnt = 0;
        }
        public void Add(Element e, float multiplier=1)
        {
            switch (e.State)
            {
                case State.s1:
                    FireCnt += multiplier;
                    break;
                case State.s2:
                    WaterCnt += multiplier;
                    break;
                case State.s3:
                    NatureCnt += multiplier;
                    break;
                case State.s4:
                    LightCnt += multiplier;
                    break;
                case State.s5:
                    DarknessCnt += multiplier;
                    break;
            }
        }

        public void Remove(Element e)
        {
            switch (e.State)
            {
                case State.s1:
                    FireCnt--;
                    break;
                case State.s2:
                    WaterCnt--;
                    break;
                case State.s3:
                    NatureCnt--;
                    break;
                case State.s4:
                    LightCnt--;
                    break;
                case State.s5:
                    DarknessCnt--;
                    break;
            }
        }
        public float FireCnt;
        public float WaterCnt;
        public float NatureCnt;
        public float LightCnt;
        public float DarknessCnt;
    }
}
