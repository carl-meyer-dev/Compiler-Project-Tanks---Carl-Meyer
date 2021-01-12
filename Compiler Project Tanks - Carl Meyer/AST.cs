using System;

namespace Compiler_Project_Tanks___Carl_Meyer
{
    // Abstract Class AST that will allow us to build a tree structure
    public abstract class AST
    {
        // visitor method used to traverse the nodes in the AST
        public abstract Object visit(Visitor v, Object arg);
    }


    // =================================================================================================================
    // Program 		::= 	Command        ~ Program
    // This is also the root of the AST
    public class Program : AST
    {
        public Command command;

        public Program(Command c)
        {
            command = c;
        }

        public override object visit(Visitor v, object arg)
        {
            return v.visitProgram(this, arg);
        }
    }
    //==================================================================================================================
    // Command 		::=	if Expression then Command else Command |       ~ IfCommand
    // | V-name := Expression |                                         ~ AssignCommand
    // | let Declaration in Command                                     ~ LetCommand


    public class Command : AST
    {
        public override object visit(Visitor v, object arg)
        {
            return v.visitCommand(this, arg);
        }
    }

    public class IfCommand : Command
    {
        public Expression _expression;
        public Command _command1;

        public Command _command2;

        // Getters
        public IfCommand(Expression e, Command c1, Command c2)
        {
            _expression = e;
            _command1 = c1;
            _command2 = c2;
        }

        public override object visit(Visitor v, object arg)
        {
            return v.visitIfCommand(this, arg);
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
        // visit command for contextual analysis
        public override object visit(Visitor v, object arg)
        {
            return v.visitLetCommand(this, arg);
        }
    }

    public class AssignCommand : Command
    {
        public VName vname;
        public Expression exp;

        public AssignCommand(VName vname, Expression exp)
        {
            this.vname = vname;
            this.exp = exp;
        }

        public override object visit(Visitor v, object arg)
        {
            return v.visitAssignCommand(this, arg);
        }
    }

    // =================================================================================================================
    // Expression 		::=	PrimaryExpression Operator PrimaryExpression

    public class Expression : AST
    {
        public PrimaryExpression P1;
        public Operate O;

        public PrimaryExpression P2;

        // for contextual analysis
        public Type type;

        public Expression(PrimaryExpression P1, Operate O, PrimaryExpression P2)
        {
            this.P1 = P1;
            this.O = O;
            this.P2 = P2;
        }

        private void inferReturnType()
        {
            switch (O.Spelling)
            {
                case ">":
                case "<":
                case "=":
                    if ((P1.Type.kind == 1 && P2.Type.kind == 2) || (P1.Type.kind == 2 && P2.Type.kind == 1))
                    {
                        UI.Error("Type Error: can not  check an int and a double against each other");
                        type = new Type(-1);
                        break;
                    }

                    type = new Type(3);
                    break;
                case "+":
                case "-":
                case "*":
                case "/":
                    // int + - * / int => int
                    if (P1.Type.kind == 1 && P2.Type.kind == 1)
                    {
                        type = new Type(1);
                    }

                    // double + - * / double => double
                    if (P1.Type.kind == 2 && P2.Type.kind == 2)
                    {
                        type = new Type(2);
                    }

                    // int + - * / double => double
                    if (P1.Type.kind == 1 && P2.Type.kind == 2)
                    {
                        type = new Type(2);
                    }

                    // double + - * / int => double
                    if (P1.Type.kind == 2 && P2.Type.kind == 1)
                    {
                        type = new Type(2);
                    }

                    // if either the Left Primary or Right Primary is a bool then its an error
                    if (P1.Type.kind == 3 || P2.Type.kind == 3)
                    {
                        UI.Error("Contextual Error: Expected an int or a double but found a bool.");
                        type = new Type(-1);
                    }

                    // if either primary's type is 0 it means there was a syntax error
                    if (P1.Type.kind == 3 || P2.Type.kind == 3)
                    {
                        UI.Error("Contextual Error: Expected an int or a double but found a syntax error.");
                        type = new Type(-1);
                    }

                    break;
            }
        }

        public override object visit(Visitor v, object arg)
        {
            return v.visitExpression(this, arg);
        }
    }
    // PrimaryExpression       ::=	V-name                ~ IdentifierPE
    //                         ::=	| ( Expression )      ~ BracketsPE
    //                         | Int-literal              ~ IdentifierPE

    public class PrimaryExpression : AST
    {
        public Type Type;
        public override object visit(Visitor v, object arg)
        {
            return v.visitPrimaryExpression(this, arg);
        }
    }

    public class IdentifierPE : PrimaryExpression
    {
        public Terminal T;

        public IdentifierPE(Terminal T)
        {
            this.T = T;
        }

        public override object visit(Visitor v, object arg)
        {
            return v.visitIdentifierPE(this, arg);
        }
    }

    public class BracketsPE : PrimaryExpression
    {
        public Expression E;

        public BracketsPE(Expression E)
        {
            this.E = E;
        }

        public override object visit(Visitor v, object arg)
        {
            return v.visitBracketsPE(this, arg);
        }
    }
    // ================================================================================================================
    // Declaration 		::= 	single-Declaration                ~ Declaration
    //                         | Declaration ; Declaration        ~ SequentialDeclaration
    // Single-Declaration 	::= 	var V-name : Type-denoter     ~ SingleDeclaration

    public class Declaration: AST
    {
        public override object visit(Visitor v, object arg)
        {
            return v.visitDeclaration(this, arg);
        }
    }

    public class SequentialDeclaration : Declaration
    {
        public Declaration _declaration1;
        public Declaration _declaration2;

        public SequentialDeclaration(Declaration d1, Declaration d2)
        {
            _declaration1 = d1;
            _declaration2 = d2;
        }

        public override object visit(Visitor v, object arg)
        {
            return v.visitSequentialDeclaration(this, arg);
        }
    }


    public class SingleDeclaration : Declaration
    {
        public Identifier Identifier;

        public TypeDenoter TypeDenoter;

        // for contextual analysis
        public Type Type;

        public SingleDeclaration(Identifier i, TypeDenoter t)
        {
            Identifier = i;
            TypeDenoter = t;
            inferType(t.Spelling);
        }

        private static Type inferType(String type)
        {
            switch (type)
            {
                case "int":
                    return new Type(1);
                case "double":
                    return new Type(2);
                case "boolean":
                    return new Type(3);
                default:
                    Console.WriteLine("Syntax Error: Expect type to be one of int | double | boolean but received {0}",
                        type);
                    return new Type(0);
            }
        }

        public override object visit(Visitor v, object arg)
        {
            return v.visitSingleDeclaration(this, arg);
        }
    }


    // ================================================================================================================
    // All Terminal Symbols
    public class Terminal : AST
    {
        public String Spelling;

        public Terminal(String spelling)
        {
            Spelling = spelling;
        }

        public override object visit(Visitor v, object arg)
        {
            return v.visitTerminal(this, arg);
        }
    }

    // V-name 		::=	a | b | c | d | e        ~ Identifier
    public class Identifier : Terminal
    {
        // a reference to where this Identifier was declared
        public Declaration Declaration;

        public Identifier(String spelling) : base(spelling)
        {
        }

        public override object visit(Visitor v, object arg)
        {
            return v.visitIdentifier(this, arg);
        }
    }

    // Operator 		::=	+ | - | * | / | < | > | =
    public class Operate : Terminal
    {
        public OperatorDeclaration OperatorDeclaration;

        public Operate(String spelling) : base(spelling)
        {
        }

        public override object visit(Visitor v, object arg)
        {
            return v.visitOperator(this, arg);
        }
    }

    public class OperatorDeclaration
    // TODO:
    {
    }

    public class TypeDenoter : Terminal
    {
        public TypeDenoter(String spelling) : base(spelling)
        {
        }

        public override object visit(Visitor v, object arg)
        {
            return v.visitTypeDenoter(this, arg);
        }
    }

    // V-name 		::=	a | b | c | d | e
    public class VName : Terminal
    {
        public Type Type;
        public bool Variable;

        public VName(String spelling) : base(spelling)
        {
        }

        public override object visit(Visitor v, object arg)
        {
            return v.visitVName(this, arg);
        }
    }

    // Int-literal 		::=	1 | 2 | 3
    public class IntLiteral : PrimaryExpression
    {
        public String Spelling;

        public IntLiteral(String s)
        {
            Spelling = s;
            this.Type = new Type(1);
        }

        public override object visit(Visitor v, object arg)
        {
            return v.visitIntLiteral(this, arg);
        }
    }

    // Type Class for Contextual Analysis
    public class Type
    {
        const int Integer = 1;
        const int Double = 2;
        const int Boolean = 3;

        const int Error = -1; // indicates a TypeError
        /*
         * Type Rules:
         * int + - * / int => int
         * int + - * / double => double
         * double + - * / double => double
         * int > < >= <= = int => boolean
         * double > < >= <= = double => boolean
         * double > < >= <= = int : TYPE ERROR (can not check int & double against each other)
         */

        public int kind;

        public Type(int kind)
        {
            this.kind = kind;
        }

        public Type(TypeDenoter T)
        {
            this.kind = InferType(T.Spelling);
        }

        public bool _Equals(Object other)
        {
            Type otherType = (Type) other;
            return (kind == otherType.kind || this.kind == Error || otherType.kind == Error);
        }
        // using underscores to not confuse with actual C# bool, int, double data types
        public static Type _bool = new Type(Boolean);
        public static Type _int = new Type(Integer);
        public static Type _double = new Type(Double);
        public static Type _error = new Type(Error);

        private static int InferType(String type)
        {
            switch (type)
            {
                case "int":
                    return Integer;
                case "double":
                    return Double;
                case "boolean":
                    return Boolean;
                default:
                    Console.WriteLine("Syntax Error: Expect type to be one of int | double | boolean but received {0}",
                        type);
                    return Error;
            }
        }
    }
}