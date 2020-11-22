using System;

namespace Compiler_Project_Tanks___Carl_Meyer
{
    public abstract class AST
    {
    }

    public class Expression : AST
    {
        public PrimaryExpression P1;
        public Operate O;
        public PrimaryExpression P2;

        public Expression(PrimaryExpression P1, Operate O, PrimaryExpression P2)
        {
            this.P1 = P1;
            this.O = O;
            this.P2 = P2;
        }
    }

    public class PrimaryExpression : AST
    {
    }

    public class IdentifierPE : PrimaryExpression
    {
        Terminal T;

        public IdentifierPE(Terminal T)
        {
            this.T = T;
        }
    }

    public class BracketsPE : PrimaryExpression
    {
        Expression E;

        public BracketsPE(Expression E)
        {
            this.E = E;
        }
    }

    public class Terminal : AST
    {
        String Spelling;

        public Terminal(String Spelling)
        {
            this.Spelling = Spelling;
        }
    }

    public class Identifier : Terminal
    {
        public Identifier(String Spelling) : base(Spelling)
        {
        }
    }

    public class Operate : Terminal
    {
        public Operate(String Spelling) : base(Spelling)
        {
        }
    }
}