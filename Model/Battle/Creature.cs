using System;
using System.Collections.Generic;
using System.Linq;

namespace Battle
{
    public abstract class Creature
    {
        public Creature Enemy { get; set; }
        private float health;
        public float Health
        {
            get
            {
                return (float)Math.Round(health);
            }
            set { health = value; }
        }
        public virtual void ApplyDamage(float Damage, float multiplier=1)
        {
            health -= Damage;
            if (Health <= 0)
                OnDied();

        }

        public Creature()
        {
            health = 100;
        }

        public abstract void AttackEnemy();

        #region Events and invokations
        public event Action Died
        {
            add { died += value; }
            remove { died -= value; }
        }
        private Action died;
        public void OnDied()
        {
            var h = died;
            if (h != null) died();
        }
        #endregion
        
    }
}
