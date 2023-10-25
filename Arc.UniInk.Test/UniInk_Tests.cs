﻿/************************************************************************************************************************
 * 📰 Title    : UniInk_NunitTest (https://github.com/Arc-huangjingtong/UniInk-CSharpInterpreter4Unity)                 *
 * 🔖 Version  : 1.0.0                                                                                                  *
 * 👩‍💻 Author   : Arc                                                                                                    *
 * 🤝 Support  : Assembly: nunit.framework, Version=3.5.0.0                                                             *
 * 📝 Desc     : the UniInk's unitTest                                                                                  *
 * 📚 TestNum  : 41                                                                                                     *
 * ⏱️ Speed    : 2'551 s                                                                                                *
/************************************************************************************************************************/

namespace Arc.UniInk.NunitTest
{
    using Arc.UniInk;
    using NUnit.Framework;
    using System;
    using System.Diagnostics;
    using System.Collections.Generic;

    //🔴🟠🟡🟢🔵🟣🟤⚫⚪
    [TestFixture]
    public class UniInk_NunitTest
    {
        private static readonly UniInk Ink = new();
        private static readonly Random random = new();

        private static long Memory;

        [OneTimeSetUp]
        public void Test_SetUp()
        {
            TestContext.Progress.WriteLine("UniInk_NunitTest : test start");

            Memory = Process.GetCurrentProcess().WorkingSet64;

            var test = new HelperClass();
            Ink.Context = test;
            Ink.Types.Add(typeof(TestEnum));
            Ink.Types.Add(typeof(HelperClass));
            Ink.StaticTypesForExtensionsMethods.Add(typeof(ExtensionClass));
        }

        [OneTimeTearDown]
        public void Test_Teardown()
        {
            var _Current = Process.GetCurrentProcess().WorkingSet64;
            var _Delta = _Current - Memory;
            TestContext.Progress.WriteLine("UniInk_NunitTest : test complete");
            TestContext.Progress.WriteLine("UniInk_NunitTest : total memory used : " + _Delta / 1024 / 1024 + " MB");
        }


        /// <summary> Test SimpleMath Calculate </summary>
        /// <remark > create random two  number and operator : [+] [-] [*] [/] </remark>
        [Test, Repeat(100)]
        public void Test01_SimpleMath_Int()
        {
            var operand1 = random.Next(1, int.MaxValue);
            var operand2 = random.Next(1, int.MaxValue);
            char[] operators = { '+', '-', '*', '/' };
            var randomOperator = operators[random.Next(0, operators.Length)];

            string expression = $"{operand1} {randomOperator} {operand2}";

            var expectedResult = CalculateExpectedResult(operand1, operand2, randomOperator);

            var actualResult = Ink.Evaluate<int>(expression);

            Assert.AreEqual(expectedResult, actualResult);
            TestContext.Progress.WriteLine($"✅:{expression}=" + $"{actualResult}");
        }

        [TestCase("(1+2)+3+4+5+6+7+8+9+10", 55)]
        [TestCase("(2+4)+6+(8+10)+(6/2)+6+(6-100)", -55)]
        [TestCase(" !TRUE&&false    ", false)] //Evaluate Boolean
        [TestCase("(45 * 2) + 3     ", 93)] //Evaluate Expression must be surrounded by parentheses
        [TestCase("-45 * 2 + 3      ", -87)] //Evaluate Expression must be surrounded by parentheses
        [TestCase("65>7?3:2         ", 3)] //Evaluate Ternary Conditional Operator
        [TestCase("HelperClass      ", typeof(HelperClass))] //Evaluate Type
        [TestCase("PI+E             ", null)] //Evaluate Custom Property
        public void Test02_SimpleExpression(string script, object answer)
        {
            var ans = Ink.Evaluate(script);
            if (answer is null)
            {
                Assert.NotNull(ans);
            }
            else
            {
                Assert.AreEqual(answer, ans);
            }

            TestContext.Progress.WriteLine($"✅:{script}=" + $"{ans}");
        }


        [TestCase("Avg (1,2,3,4,5,6,7,8,9,10   )", 5.5)]
        [TestCase("Max (1,2,3,4,5,6,7,8,9,10   )", 10)]
        [TestCase("Min (1,1,2,3,4,5,6,7,8,9,-10)", -10)]
        [TestCase("List(1,2,3,4,5,6,7,8,9,10   )", null)]
        [TestCase("List(1,2,3,4,5,6,7,8,9,10   ).Count", 10)]
        [TestCase("List(\"aaa\",\"bbb\"   )", null)]
        public void Test03_MultipleArgsFunction(string script, object answer)
        {
            var ans = Ink.Evaluate(script);
            if (answer is null)
            {
                Assert.NotNull(ans);
            }
            else
            {
                Assert.AreEqual(answer, ans);
            }

            TestContext.Progress.WriteLine($"✅:{script}=" + $"{ans}");
        }


        [TestCase("if(3>5){return 3;}else{return 5;}")] //Test [if] [else]
        [TestCase("if(3>5) return 3; else  return 5;")] //Test [if] [else]
        [TestCase("if(3>5)   {return 3;}   else if   (3==5)   {return   3 ; } else {return 5;}")] //Test [if] [else] [else if]
        [TestCase("if(3>5){return \"aaa\";}else if(3==5){return \"bbb\";}else{return \"ccc\";}")] //Test [if] [else] [else if]
        [TestCase("var sum = 0;  for  (var i = 0 ; i < 100 ; i++)    { sum += i; } return sum;")] //Test [for]
        public void Test04_blockKeyword(string script)
        {
            var ans = Ink.ScriptEvaluate(script);
            TestContext.Progress.WriteLine($"✅:{ans}");
            Assert.NotNull(ans);

            if (ans is string str)
            {
                TestContext.Progress.WriteLine($"✅:{script}={ans}" + " ---  " + str.Length);
            }
        }

        //[TestCase("List<int> list = D; ")]
        public void Test05_Type(string script)
        {
            var ans = Ink.ScriptEvaluate(script);
            TestContext.Progress.WriteLine($"✅:{ans}");
            Assert.NotNull(ans);

            if (ans is string str)
            {
                TestContext.Progress.WriteLine($"✅:{script}={ans}" + " ---  " + str.Length);
            }
        }

        [TestCase("this.Test3();")]
        [TestCase("this.Test3(\"aaa\");")]
        public void Test06_ExtensionMethod(string script)
        {
            var ans = Ink.ScriptEvaluate(script);
            TestContext.Progress.WriteLine($"✅:{ans}");
            Assert.NotNull(ans);
        }

        [TestCase("ParamsTest(1,2,3,4,5);")]
        [TestCase("ParamsTest();")]
        [TestCase("ParamsTest(\"aaa\");")]
        public void Test07_ParamKeyword(string script)
        {
            var ans = Ink.ScriptEvaluate(script);
            TestContext.Progress.WriteLine($"✅:{ans}");
            Assert.NotNull(ans);
        }


        [TestCase("var w = 2+4;        return w;")]
        [TestCase("var w = 4-2;               w;")]
        [TestCase("int w = 4-2; w +=3; return w;")]
        [TestCase("var w = 4-2;")]
        [TestCase("4/2;")]
        [TestCase("Test();")]
        [TestCase("var ccc= new(HelperClass); return ccc.Id;")]
        [TestCase("Test(\"aaa\"+\"aaaaa\");")]
        [TestCase("Test(Test(aaa));")]
        [TestCase("Test(Test(aaa)+\"aaaaa\");")]
        [TestCase("TestA(x => (x > 0) && (int)TestEnum.A == 1);")] //测试lambda表达式
        [TestCase("TestEnum.A;")] //测试枚举
        [TestCase("TestA(x => (x > 0) && (A == B));")]
        [TestCase("TestA(x => x == D);")]
        [TestCase("TestC<TestEnum>(A);")]
        [TestCase("TestD<TestEnum,TestEnum>(A);")] //测试多泛型参数
        [TestCase(" TestC<List<int>>(D);  ")] //测试多泛型参数
        public void Test04_Scripts(string script)
        {
            var ans = Ink.ScriptEvaluate(script);
            Assert.NotNull(ans);
            TestContext.Progress.WriteLine($"✅:{script}={ans}" + " ---  " + ans.GetType());

            if (ans is string str)
            {
                TestContext.Progress.WriteLine($"✅:{script}={ans}" + " ---  " + str.Length);
            }
        }

        [TestCase("")] [TestCase(" ")]
        public void Test05_Boundary(string script)
        {
            try
            {
                Ink.Evaluate(script);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        // [Test]
        public void Test_Custom()
        {
            var stack = new Stack<object>();
            stack.Push(null);
            var regex = new System.Text.RegularExpressions.Regex(@"^['](?<char>([\\][\\'0abfnrtv]|[^']|))[']");
            var match = regex.Match("''");
            Assert.IsTrue(match.Success);
            Assert.IsTrue(match.Groups["char"].Success);
            Assert.AreEqual("", match.Groups["char"].Value);
        }

        private static int CalculateExpectedResult(int operand1, int operand2, char @operator)
        {
            return @operator switch
            {
                '+' => operand1 + operand2, // 
                '-' => operand1 - operand2, // 
                '*' => operand1 * operand2, // 
                '/' => operand1 / operand2, // 
                _ => throw new ArgumentException("Invalid operator") //
            };
        }
    }


    public enum TestEnum { A, B, C, D }


    public class HelperClass
    {
        public int Id = 233;

        public string aaa = "2222";

        public TestEnum A => TestEnum.A;
        public TestEnum B => TestEnum.B;
        public TestEnum C => TestEnum.C;

        public static List<int> D = new List<int>();


        ///无参数的测试函数
        public static int Test()
        {
            Console.WriteLine("test");


            return 1;
        }

        ///有参数的测试函数
        public static string Test(string str)
        {
            Console.WriteLine(str);
            return str;
        }

        public static string TestA(Predicate<int> predicate)
        {
            Console.WriteLine("predicate");
            return "action";
        }

        public static string TestA(Predicate<List<int>> predicate)
        {
            Console.WriteLine("predicate");
            return "action";
        }

        public static void TestB()
        {
            Console.WriteLine("actionB");
        }

        public static T TestC<T>(T t)
        {
            Console.WriteLine("actionB");
            return t;
        }

        public static T2 TestD<T1, T2>(T2 t)
        {
            Console.WriteLine("actionB");
            return t;
        }


        public int ParamsTest(params object[] args)
        {
            Console.WriteLine(args.Length);
            return args.Length;
        }

        public int ParamsTest(string a, params object[] args)
        {
            Console.WriteLine(a + args.Length);
            return args.Length + 1000;
        }
    }

    public static class MyStaticClass
    {
        public static string StaticTest()
        {
            return "test1";
        }
    }

    public static class ExtensionClass
    {
        public static string Test3(this HelperClass str)
        {
            Console.WriteLine(str);
            return str.ToString();
        }

        public static string Test3(this HelperClass str, string str2)
        {
            Console.WriteLine(str + str2);
            return str.ToString();
        }
    }
}
//2.425