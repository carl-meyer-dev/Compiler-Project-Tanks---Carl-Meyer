using System;

namespace Compiler_Project_Tanks___Carl_Meyer
{
    // Abstract Class AST that will allow us to build a tree structure
    public abstract class AST
    {
    }
    // =================================================================================================================
    // Program 		::= 	Command        ~ Program
    // This is also the root of the AST
    public class Program : AST
    {
        private Command command;

        public Program(Command c)
        {
            command = c;
        }
    }
    //==================================================================================================================
    // Command 		::=	if Expression then Command else Command |       ~ IfCommand
    // | V-name := Expression |                                         ~ AssignCommand
    // | let Declaration in Command                                     ~ LetCommand


    public class Command : AST
    {
    }
    
    public class IfCommand: Command
    {
        private Expression _expression;
        private Command _command1;
        private Command _command2;

        public IfCommand(Expression e, Command c1, Command c2)
        {
            _expression = e;
            _command1 = c1;
            _command2 = c2;
        }
    }

    public class LetCommand : Command
    {
        public Declaration declaration;
        public Command command;

        public LetCommand(Declaration declaration, Command command)
        {
            this.declaration = declaration;
            this.command = command;
        }
    }

    public class AssignCommand : Command
    {
        public VName vname;
        public Expression exp;

        public AssignCommand(VName vname,Expression exp)
        {
            this.vname = vname;
            this.exp = exp;
        }
    }
    
    // =================================================================================================================
    // Expression 		::=	PrimaryExpression Operator PrimaryExpression
    
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
    // PrimaryExpression       ::=	V-name                ~ IdentifierPE
    //                         ::=	| ( Expression )      ~ BracketsPE
    //                         | Int-literal              ~ IdentifierPE

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
    // ================================================================================================================
    // Declaration 		::= 	single-Declaration                ~ Declaration
    //                         | Declaration ; Declaration        ~ SequentialDeclaration
    // Single-Declaration 	::= 	var V-name : Type-denoter     ~ SingleDeclaration

    public class Declaration
    {
    }

    public class SequentialDeclaration: Declaration
    {
        private Declaration _declaration1;
        private Declaration _declaration2;

        public SequentialDeclaration(Declaration d1, Declaration d2)
        {
            _declaration1 = d1;
            _declaration2 = d2;
        }
    }
    

    public class SingleDeclaration: Declaration
    {
        private Identifier identifier;
        private TypeDenoter _typeDenoter;

        public SingleDeclaration(Identifier i, TypeDenoter t)
        {
            identifier = i;
            _typeDenoter = t;
        }
    }


    // ================================================================================================================
    // All Terminal Symbols
    public class Terminal : AST
    {
        String Spelling;

        public Terminal(String Spelling)
        {
            this.Spelling = Spelling;
        }
    }
    // V-name 		::=	a | b | c | d | e        ~ Identifier
    public class Identifier : Terminal
    {
        public Identifier(String Spelling) : base(Spelling)
        {
        }
    }
    // Operator 		::=	+ | - | * | / | < | > | =
    public class Operate : Terminal
    {
        public Operate(String Spelling) : base(Spelling)
        {
        }
    }

    public class TypeDenoter : Terminal
    {
        public TypeDenoter(String Spelling) : base(Spelling)
        {
        }
    }

    // V-name 		::=	a | b | c | d | e
    public class VName: Terminal
    {
        public VName(String Spelling) : base(Spelling)
        {
        }
    }
    
    // Int-literal 		::=	1 | 2 | 3
    public class IntLiteral: Terminal
    {
        public IntLiteral(String Spelling) : base(Spelling)
        {
        }
    }
}