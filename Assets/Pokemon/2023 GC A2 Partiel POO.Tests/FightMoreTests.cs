using _2023_GC_A2_Partiel_POO.Level_2;
using NUnit.Framework;
using System;

namespace _2023_GC_A2_Partiel_POO.Tests.Level_2
{
    public class FightMoreTests
    {
        // Tu as probablement remarqué qu'il y a encore beaucoup de code qui n'a pas été testé ...
        // À présent c'est à toi de créer les TU sur le reste et de les implémenter

        // Ce que tu peux ajouter:
        // - Ajouter davantage de sécurité sur les tests apportés
        // - un heal ne régénère pas plus que les HP Max
        // - si on abaisse les HPMax les HP courant doivent suivre si c'est au dessus de la nouvelle valeur
        // - ajouter un equipement qui rend les attaques prioritaires puis l'enlever et voir que l'attaque n'est plus prioritaire etc)
        // - Le support des status (sleep et burn) qui font des effets à la fin du tour et/ou empeche le pkmn d'agir
        // - Gérer la notion de force/faiblesse avec les différentes attaques à disposition (skills.cs)
        // - Cumuler les force/faiblesses en ajoutant un type pour l'équipement qui rendrait plus sensible/résistant à un type

        [Test]
        public void BaseStatusCheck()
        {
            var c = new Character(100, 50, 30, 20, TYPE.NORMAL);

            Assert.That(c.BaseStatus, Is.EqualTo(StatusPotential.NONE));
        }

        [Test]
        public void AddStatusCheck()
        {
            var c = new Character(100, 50, 30, 20, TYPE.NORMAL);
            var c2 = new Character(100, 50, 30, 20, TYPE.NORMAL);
            FireBall fb = new FireBall(); //FireBall Has burn Status
            Assert.That(c.CurrentStatus, Is.EqualTo(null));
            c.ReceiveAttack(fb,c2);
            Assert.That(c.CurrentStatus.GetType(), Is.EqualTo(typeof(BurnStatus))); //Get Burn Status
        }

        [Test]
        public void CheckStatusForNullAttack()
        {
            var c = new Character(100, 50, 30, 20, TYPE.NORMAL);
            var c2 = new Character(100, 50, 30, 20, TYPE.NORMAL);
            Punch p = new Punch(); //Punch as no status
            Assert.That(c.CurrentStatus, Is.EqualTo(null));
            c.ReceiveAttack(p, c2);
            Assert.That(c.CurrentStatus, Is.EqualTo(null)); //Still Null
        }

        [Test]
        public void CheckRemainingTurnForStatus()
        {
            var pikachu = new Character(2000, 50, 30, 20, TYPE.NORMAL);
            var salameche = new Character(2000, 50, 30, 200, TYPE.NORMAL);
            Fight f = new Fight(pikachu, salameche);
            //Attacks
            Punch p = new Punch();
            FireBall fb = new FireBall(); //FireBall Has burn Status

            f.ExecuteTurn(p, fb); //Salameche Fight First
            Assert.That(pikachu.CurrentStatus.GetType(), Is.EqualTo(typeof(BurnStatus))); //Pikachu is burned
            Assert.That(pikachu.CurrentStatus.RemainingTurn, Is.EqualTo(5)); //Il reste 5 tour et le "status" n'a pas fait effet puis qu'il vient d'etre appliqué
        }

        [Test]
        public void CheckEndStatus()
        {
            var pikachu = new Character(2000, 50, 30, 20, TYPE.NORMAL);
            var salameche = new Character(2000, 50, 30, 200, TYPE.NORMAL);
            Fight f = new Fight(pikachu, salameche);
            //Attacks
            Punch p = new Punch();
            FireBall fb = new FireBall(); //FireBall Has burn Status

            f.ExecuteTurn(p, fb); //Salameche Fight First 5turn remains
            for(int i = 0; i<4; i++)
            {
                f.ExecuteTurn(p, p);
            }
            Assert.That(pikachu.CurrentStatus.GetType(), Is.EqualTo(typeof(BurnStatus))); //Pikachu still burned 
            Assert.That(pikachu.CurrentStatus.RemainingTurn, Is.EqualTo(1)); //Still 1 turn
            f.ExecuteTurn(p, p);
            Assert.That(pikachu.CurrentStatus, Is.EqualTo(null)); //Pikachu still burned 
        }

        [Test]
        public void CheckDmgStatus()
        {
            var pikachu = new Character(2000, 50, 0, 20, TYPE.NORMAL);
            var salameche = new Character(2000, 50, 30, 200, TYPE.NORMAL);
            Fight f = new Fight(pikachu, salameche);
            //Attacks
            Punch p = new Punch();
            FireBall fb = new FireBall(); //FireBall Has burn Status and deal 50

            f.ExecuteTurn(p, fb); //Salameche Fight First 5turn remains
            Assert.That(pikachu.CurrentHealth, Is.EqualTo(1950)); //Pikachu loose 50hp
            f.ExecuteTurn(p, p);
            Assert.That(pikachu.CurrentHealth, Is.EqualTo(1870)); //Pikachu loose 70hp + 10hp of burn
        }
        /*
        [Test]
        public void CheckCrazyStatus()
        {
            var pikachu = new Character(2000, 50, 0, 20, TYPE.NORMAL);
            var salameche = new Character(2000, 50, 30, 200, TYPE.NORMAL);
            Fight f = new Fight(pikachu, salameche);
            //Attacks
            Punch p = new Punch();
            CrazyAtk ca = new CrazyAtk(); //FireBall Has burn Status and deal 50

            f.ExecuteTurn(p, ca); //Salameche Fight First 5turn remains
            Assert.That(pikachu.CurrentHealth, Is.EqualTo(1950)); //Pikachu loose 50hp
            f.ExecuteTurn(p, p);
            Assert.That(pikachu.CurrentHealth, Is.EqualTo(1870)); //Pikachu loose 70hp + 10hp of burn
        }
        */

        //J'ai codé Le crazy Status et le fait qu'il ne puisse pas attaquer ainsi que le fait qu'il puisse se SELF harm
    }
}
