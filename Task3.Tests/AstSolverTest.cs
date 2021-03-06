﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Task3.Tests
{
    [TestClass]
    public class AstSolverTest
    {
        [TestMethod]
        public void AstSolver_BigExpression_Solve()
        {
            var actualResult =
                (new AstSolver(new Analyzer())).Solve(
                    (new AstBuilder()).Build(new Tokenizer().GetTokens("(+ 4 6 (/ 81 -3 (* 3 3)) (- 10 12 ))")));
            Assert.AreEqual(5.0, actualResult);
        }
    }
}