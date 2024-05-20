﻿namespace Arc.UniInk.NUnitTest
{

    // ReSharper disable PartialTypeWithSinglePart
    using Arc.UniInk;
    using NUnit.Framework;


    [TestFixture]
    public sealed partial class NUnit_UniInkSpeed
    {
        [Repeat(10000)]
        [TestCase("+123456789             ", ExpectedResult = +123456789)]
        [TestCase("+123456789+987654321   ", ExpectedResult = +123456789 + 987654321)]
        [TestCase("111 * 111 * 3 /3*3/3   ", ExpectedResult = 111 * 111 * 3 / 3 * 3 / 3)]
        [TestCase("3333333-3+3+  3 - 3    ", ExpectedResult = 3333333 - 3 + 3 + 3 - 3)]
        [TestCase("   999999 + 999999     ", ExpectedResult = 999999              + 999999)]
        [TestCase("9*((1+(1+1)+(1+1))+1+1)", ExpectedResult = 9 * ((1 + (1 + 1) + (1 + 1)) + 1 + 1))]
        [TestCase("9*(1+1 + 1 + 1 + 1+1+1)", ExpectedResult = 9 * (1                       + 1 + 1 + 1 + 1 + 1 + 1))]
        [TestCase("9 * ( ( 1 + 2 * 3 ) /2)", ExpectedResult = 9 * ((1 + 2 * 3) / 2))]
        public static int Test_Arithmetic_Int(string input)
        {
            var res    = (UniInk_Speed.InkValue)UniInk_Speed.Evaluate(input);
            var result = res!.Value_int;
            UniInk_Speed.InkValue.Release(res);

            return result;
        }

        [Repeat(10000)]
        [TestCase("+123456789.987654321f  ",                         ExpectedResult = 123456789.987654321f)]
        [TestCase("+123456789.987654321f + 987654321.123456789f",    ExpectedResult = 123456789.987654321f + 987654321.123456789f)]
        [TestCase("111.111f * 111.111f * 3.3f /3.3f*3.3f/3.3f",      ExpectedResult = 111.111f * 111.111f * 3.3f / 3.3f * 3.3f / 3.3f)]
        [TestCase("3333333.3333333f-3.3f+3.3f+  3.3f - 3.3f",        ExpectedResult = 3333333.3333333f                               - 3.3f + 3.3f + 3.3f - 3.3f)]
        [TestCase("9.9f*((1.1f+(1.1f+1.1f)+(1.1f+1.1f))+1.1f+1.1f)", ExpectedResult = 9.9f * ((1.1f + (1.1f + 1.1f) + (1.1f + 1.1f)) + 1.1f + 1.1f))]
        [TestCase("   999999.9999999f + 999999.9999999f     ",       ExpectedResult = 999999.9999999f + 999999.9999999f)]
        public static float Test_Arithmetic_Float(string input)
        {
            var res    = (UniInk_Speed.InkValue)UniInk_Speed.Evaluate(input);
            var result = res!.Value_float;
            UniInk_Speed.InkValue.Release(res);

            return result;
        }
    }

}


// private static object ProcessQueue_Internal(InkSyntaxList keys, int startIndex, int endIndex)
// {
//     object      cacheLeft     = null;
//     InkOperator cacheOperator = null;
//
//     for (var i = startIndex ; i < endIndex ; i++)
//     {
//         object process;
//
//         if (keys.IndexDirty[i])
//         {
//             if (keys.CastOther[i] != null)
//             {
//                 process           = keys.CastOther[i];
//                 keys.CastOther[i] = null;
//             }
//             else
//             {
//                 continue;
//             }
//         }
//         else
//         {
//             process = keys[i];
//         }
//
//
//         if (process is InkOperator @operator)
//         {
//             cacheOperator = @operator;
//         }
//         else if (cacheLeft == null)
//         {
//             cacheLeft = process;
//         }
//         else
//         {
//             if (dic_OperatorsFunc.TryGetValue(cacheOperator, out var func))
//             {
//                 cacheLeft = func(cacheLeft, process);
//             }
//             else
//             {
//                 InkSyntaxException.Throw($"Unknown Operator : {cacheOperator}");
//             }
//         }
//     }
//
//     return cacheLeft;
// }