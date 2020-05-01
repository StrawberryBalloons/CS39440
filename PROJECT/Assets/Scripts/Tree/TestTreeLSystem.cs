using System.Collections;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TestTreeLSystem
    {

        // A Test behaves as an ordinary method
        [Test]
        public void TestTreeLSystemSimplePasses()
        {


        }

        [Test]
        public void TestAwake()
        {
            //ARRANGE
            Dictionary<char, string> rules = new Dictionary<char, string>();
            rules.Add('X', "[-FX][+FX][FX]");
            rules.Add('F', "FF");
            string currentPath = " ";
            string axiom = "X";
            currentPath = axiom;
            StringBuilder stringBuilder = new StringBuilder();
            char[] currentPathChars = currentPath.ToCharArray();

            //ACT
            stringBuilder.Append(rules.ContainsKey(currentPathChars[0]) ? rules[currentPathChars[0]] : currentPathChars[0].ToString());

            //ASSERT
            Assert.That(currentPath[0], Is.EqualTo('F'));

        }
        [Test]
        public void TestSetTreeProgenitor()
        {
            //ARRANGE
            var Lsystem = new TreeLSystem();
            GameObject expected = new GameObject();

            //ACT
            Lsystem.setTreeProgenitor(expected);

            //ASSERT
            Assert.That(Lsystem.patriarch, Is.EqualTo(expected));
        }

        [Test]
        public void TestGenerate()
        {
            //No way to test line renderer from editor and mechanical aspects tested in Awake
            //ARRANGE

            //ACT

            //ASSERT
            Assert.That(1, Is.Negative);
        }

    }
}
