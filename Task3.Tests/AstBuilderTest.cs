using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Task3.Tests
{
    [TestClass]
    public class AstBuilderTest
    {
        [TestMethod]
        public void AstBuilder_ComplexExpression_Build()
        {
            var tree = new Ast("+", true);
            tree.Operands.Add(new Ast("4"));
            tree.Operands.Add(new Ast("6"));

            var buffer = new Ast("/", true);
            buffer.Operands.Add(new Ast("81"));
            buffer.Operands.Add(new Ast("-3"));

            var anotherBuffer = new Ast("*", true);
            anotherBuffer.Operands.Add(new Ast("3"));
            anotherBuffer.Operands.Add(new Ast("3"));
            buffer.Operands.Add(anotherBuffer);

            tree.Operands.Add(buffer);

            buffer = new Ast("-", true);
            buffer.Operands.Add(new Ast("10"));
            buffer.Operands.Add(new Ast("12"));

            tree.Operands.Add(buffer);
            var input = "(+ 4 6 (/ 81 -3 (* 3 3 ))( - 10 12 ))";

            var actualTree = new AstBuilder().Build(new Tokenizer().GetTokens(input));
            Assert.IsTrue(actualTree.Equals(tree));
        }
    }
}