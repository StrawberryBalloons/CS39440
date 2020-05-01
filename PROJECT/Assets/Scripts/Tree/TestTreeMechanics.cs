using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TestTreeMechanics
    {
        // A Test behaves as an ordinary method
        [Test]
        public void TestTreeMechanicsSimplePasses()
        {
            //ARRANGE

            //ACT

            //ASSERT

        } //Not in use

        [Test]
        public void TestOnCollisionEnter()
        {
            //ARRANGE

            //ACT

            //ASSERT

        }//Not Testable

        [Test]
        public void TestUpdate()
        {
            //ARRANGE
            float increment = 6.0f;
            float time = 6.0f;
            float expected = 0;

            //ACT
            if (Time.time > time)
            {
                expected = Time.time + increment;
            }

            //ASSERT
            Assert.That(time + increment, Is.EqualTo(expected));
            }

        [Test]
        public void TestReproductionChance()
        {
            //ARRANGE
            var treeMech = new TreeMechanics();
            //ACT
            var result = treeMech.reproductionChance();
            //ASSERT
            Assert.That(result, Is.GreaterThan(-1));
            Assert.That(result, Is.LessThan(101));
        }

        [Test]
        public void TestWaterStarved()
        {
            //ARRANGE
            var treeMech = new TreeMechanics();

            //ACT
            var result = treeMech.waterStarved();

            //ASSERT
            Assert.That(result, Is.True);
        }

        [Test]
        public void TestDestroy()
        {
            //ARRANGE
            var treeMech = new TreeMechanics();
            GameObject expected = new GameObject();

            //ACT
            treeMech.TestDestroy(expected);

            //ASSERT
            Assert.That(expected, Is.Null);
        }

        [Test]
        public void TestWaterLogged()
        {
            //ARRANGE
            var treeMech = new TreeMechanics();

            //ACT
            var result = treeMech.waterLogged();

            //ASSERT
            Assert.That(result, Is.True);

        }

        [Test]
        public void TestGetOverlay()
        {
            //ARRANGE
            var treeMech = new TreeMechanics();
            var expected = GameObject.Find("WaterMeshy").GetComponent<WaterMechanics>().waterOverlay;
            //ACT

            //ASSERT
            Assert.That(expected[0,0], Is.EqualTo(0));
        }

        [Test]
        public void TestGetXY()
        {
            //ARRANGE
            var expected = 10;
            var number = 104;

            //ACT
            var result = (int)Mathf.Round(number) / 10;

            //ASSERT
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void TestChuckSeed()
        {
            //ARRANGE

            //ACT

            //ASSERT
        }//Not testable

        [Test]
        public void TestGetNewSpawnPos()
        {
            //ARRANGE
            var expected = 30;
            var rangeMultiplier = 3;

            //ACT
            int x, y;
            x = 0;
            y = 0;
            while (x == 0 && y == 0) //&& should be fine because only 1 needs to be different
            {
                x = UnityEngine.Random.Range(-(rangeMultiplier * 10), rangeMultiplier * 10);
                y = UnityEngine.Random.Range(-(rangeMultiplier * 10), rangeMultiplier * 10);
            }
            //ASSERT
            Assert.That(x, Is.GreaterThan(-expected));
            Assert.That(x, Is.LessThan(expected));
            Assert.That(y, Is.GreaterThan(-expected));
            Assert.That(y, Is.LessThan(expected));
        }

        [Test]
        public void TestGetHeight()
        {
            //ARRANGE

            //ACT

            //ASSERT
        }//can't be tested
        [Test]
        public void TestNearbyTree()
        {
            //ARRANGE
            var treeMech = new TreeMechanics();

            //ACT
            var result = treeMech.nearbyTree();

            //ASSERT
            Assert.That(result, Is.False);
        }
        [Test]
        public void TestGetClosest()
        {
            //ARRANGE

            //ACT

            //ASSERT
        }// same as test nearby tree, can't be tested
    }
}
