using System;
using System.Collections;
using System.Linq.Expressions;

namespace Compiler_Project_Tanks___Carl_Meyer
{
    public class Parser
    {
        ArrayList TokenList;
        Token CurrentToken;
        int CurTokenPos;

        const int Identifier = 1;
        const int Operator = 2;
        const int LPar = 3;
        const int RPar = 4;
        const int Literal = 5;
        const int If = 6;
        const int Then = 7;
        const int Else = 8;
        const int Let = 9;
        const int In = 10;
        const int End = 11;
        public Parser(String Sentence)
        {
            Scanner S = new Scanner(Sentence);

            TokenList = S.getTokens();
            CurTokenPos = -1;
            FetchNextToken();
            Expression P = parseExpression();
        }
        
        // =============================================================================================================
        // Auxiliary methods 
        void FetchNextToken()
        {
            CurTokenPos++;
            if (CurTokenPos < TokenList.Count)
                CurrentToken = (Token) TokenList[CurTokenPos];
            else
                CurrentToken = null;
        }

        void accept(int Type)
        {
            if (CurrentToken.matchesType(Type))
                FetchNextToken();
            else
                //Switch on type
                switch (Type)
                {
                    case 1:
                        Console.WriteLine("Syntax Error expected identifier");
                        break;
                    case 2:
                        Console.WriteLine("Syntax Error expected operator");
                        break;
                    case 3:
                        Console.WriteLine("Syntax Error expected '('");
                        break;
                    case 4:
                        Console.WriteLine("Syntax Error expected ')'");
                        break;
                    case 5:
                        Console.WriteLine("Syntax Error expected literal");
                        break;
                    case 6:
                        Console.WriteLine("Syntax Error expected 'if'");
                        break;
                    case 7:
                        Console.WriteLine("Syntax Error expected 'then' after if");
                        break;
                    case 8:
                        Console.WriteLine("Syntax Error expected 'else' after then");
                        break;
                    case 9:
                        Console.WriteLine("Syntax Error expected 'Let'");
                        break;
                    case 10:
                        Console.WriteLine("Syntax Error expected 'in' after let");
                        break;
                    case 11:
                        Console.WriteLine("Syntax Error missing end");
                        break;
                    default:
                        Console.WriteLine("Syntax Error in accept");
                        break;
                }
        }

        void acceptIt()
        {
            FetchNextToken();
        }
        
        // =============================================================================================================
        // Parsing Methods

        // Program 		::= 	Command
        Program parseProgram()
        {
            Program ProgramAST = new Program(parseCommand());
            return ProgramAST;

        }
        // =============================================================================================================
        // Handle the following production rules:

        // Command 		::=	if Expression then Command else Command |
        //		| V-name := Expression |
        //		| let Declaration in Command
        Command parseCommand()
        {
            if (CurrentToken == null)
                return null;
            Command command;
            switch (CurrentToken.getType())
            {
                // Command 		::=	if Expression then Command else Command 
                case If:
                    command = HandleIfCommand();
                    break;
                // Command | let Declaration in Command
                case Let:
                    command = handleLetCommand();
                    break; 
                
                // Command | V-name := Expression |
                case Identifier:
                    command = handleAssignCommand();
                    break;
                    
            }
        }

        Command HandleIfCommand()
        {
            // if token found, accept it then continue parsing
            acceptIt();
            Expression expression = parseExpression();
            accept(Then);
            Command ifTrueCommand = parseCommand();
            Command ifFalseCommand = null;
            if (CurrentToken.getType().Equals(Else))
            {
                accept(Else);
                ifFalseCommand = parseCommand();
            }
            accept(End);
            //done parsing if accept an end command
            return new IfCommand(expression, ifTrueCommand, ifFalseCommand);
        }

        Command handleLetCommand()
        {
            acceptIt();
            Declaration declaration = parseDeclaration();
            accept(In);
            Command command = new LetCommand(declaration, parseCommand());
            accept(End);
            return command;
        }

        Command HandleAssignCommand()
        {
            
        }

        Expression parseExpression()
        {
            Expression ExpAST;
            PrimaryExpression P1 = parsePrimary();
            Operate O = parseOperator();
            PrimaryExpression P2 = parsePrimary();
            ExpAST = new Expression(P1, O, P2);
            return ExpAST;
        }


        PrimaryExpression parsePrimary()
        {
            PrimaryExpression PE;
            if (CurrentToken == null)
                return null;
            switch (CurrentToken.getType())
            {
                case Identifier:
                    Identifier I = parseIdentifier();
                    PE = new IdentifierPE(I);
                    break;
                case LPar:
                    acceptIt();
                    PE = new BracketsPE(parseExpression());
                    accept(RPar);
                    break;
                default:
                    Console.WriteLine("Syntax Error in Primary");
                    PE = null;
                    break;
            }

            return PE;
        }

        Declaration parseDeclaration()
        {
           
            switch (CurrentToken.getType())
            {
                
            }
            return 
        }

        Identifier parseIdentifier()
        {
            Identifier I = new Identifier(CurrentToken.getSpelling());
            accept(Identifier);
            return I;
        }

        Operate parseOperator()
        {
            Operate O = new Operate(CurrentToken.getSpelling());
            accept(Operator);
            return O;
        }
    }
}