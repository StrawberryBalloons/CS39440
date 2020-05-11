using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class WaterMechanicsTest
    {
        // A Test behaves as an ordinary method



        [Test]
        public void TestDeductWater()
        {
            //ARRANGE
            var waterMech = new WaterMechanics();
            var expected = 0;
            var overlay = waterMech.getWaterOverlay();

            //ACT
            waterMech.deductWater(0, 0, 5);

            //ASSERT
            Assert.That(overlay[0,0], Is.EqualTo(expected));
        }


        [Test]
        public void TestCheckBounds()
        {
            //ARRANGE
            var minBound = 0;
            var maxBound = 100;

            //ACT
            if (minBound < 1)
            {
                minBound = 1;
            }
            else if (maxBound > 99)
            {
                maxBound = 99;
            }
            else if (maxBound < 1)
            {
                maxBound = 1;
            }
            else if (minBound > 99)
            {
                minBound = 99;
            }
            //ASSERT
            Assert.That(minBound, Is.GreaterThanOrEqualTo(1));
            Assert.That(maxBound, Is.LessThanOrEqualTo(99));
        }


        [Test]
        public void TestWaterOptionSelect()
        {
            //ARRANGE
            var waterMech = new WaterMechanics();
            var overlay = waterMech.getWaterOverlay();
            var originX = 0;
            var originY = 0;
            var newX = 1;
            var newY = 1;
            var val1 = 1;
            var val2 = 2;
            var expected = 1;
            var result= 0;


            //ACT
            if (overlay[originX, originY] > 0.0f && overlay[newX, newY] > 0.0f && overlay[originX, originY] >= overlay[newX, newY] && (newX != originX || newY != originY))
            {
                result += 3;
            }
            else if (val1 < val2 && (newX != originX || newY != originY) && newX != 100 && newY != 100)//if the water height of the new is less than the original
            {
                result += 1;
            }
            else if (newX == originX && newY == originY) //water has found a minimum, if sx and sy are the same as x and y then there is no lower area around them
            {
                result += 2;
            }
            else
            {
                Debug.Log("Error");
            }


            //ASSERT
            Assert.That(result, Is.EqualTo(expected));
        }


        [Test]
        public void TestInitOverlay()
        {
            //ARRANGE
            var waterMech = new WaterMechanics();
            var height = 100;
            var width = 100;
            var expected = 0;
            var overlay = waterMech.getWaterOverlay();
            //ACT
            for (int x = 0; x < height; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    overlay[x, y] += 0;
                }
            }
            //ASSERT
            Assert.That(overlay[50, 50], Is.EqualTo(expected));
        }

        [Test]
        public void TestGetWaterOverlay()
        {
            //ARRANGE
            var waterMech = new WaterMechanics();
            var result = false;

            //ACT
            try
            {
                var overlay = waterMech.getWaterOverlay();
            }
            catch(Exception e)
            {
                Debug.Log(e);
                 result = true;
            }
            //ASSERT
            Assert.That(result, Is.True);
        }



        [Test]
        public void TestApplyWater()
        {
            //ARRANGE
            var waterMech = new WaterMechanics();
            var height = 100;
            var width = 100;
            var expected = 1;
            var overlay = waterMech.getWaterOverlay();
            //ACT
            for (int x = 0; x < height; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    overlay[x, y] += 1;
                }
            }
            //ASSERT
            Assert.That(overlay[50, 50], Is.EqualTo(expected));
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        
    }
}
