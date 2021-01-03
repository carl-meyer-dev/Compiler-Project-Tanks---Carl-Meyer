using System;
using System.Collections;
using System.Linq.Expressions;
using System.Text.Json;

namespace Compiler_Project_Tanks___Carl_Meyer
{
    public class Parser
    {
        private UI _ui = new UI();
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
        const int Is = 12;
        private const int Var = 13;
        private const int SemiColon = 14;
        public Parser(String Sentence)
        {
            Scanner S = new Scanner(Sentence);

            TokenList = S.getTokens();
            CurTokenPos = -1;
            FetchNextToken();
            
            Program P = parseProgram();
            _ui.Info("Finished Parsing.");
            _ui.Dump(P);
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
                        _ui.Error("Syntax Error expected identifier");
                        break;
                    case 2:
                        _ui.Error("Syntax Error expected operator");
                        break;
                    case 3:
                        _ui.Error("Syntax Error expected '('");
                        break;
                    case 4:
                        _ui.Error("Syntax Error expected ')'");
                        break;
                    case 5:
                        _ui.Error("Syntax Error expected literal");
                        break;
                    case 6:
                        _ui.Error("Syntax Error expected 'if'");
                        break;
                    case 7:
                        _ui.Error("Syntax Error expected 'then' after if");
                        break;
                    case 8:
                        _ui.Error("Syntax Error expected 'else' after then");
                        break;
                    case 9:
                        _ui.Error("Syntax Error expected 'Let'");
                        break;
                    case 10:
                        _ui.Error("Syntax Error expected 'in' after let");
                        break;
                    case 11:
                        _ui.Error("Syntax Error missing end");
                        break;
                    case 12:
                        _ui.Error("Syntax Error missing : after variable declaration");
                        break;
                    case 13:
                        _ui.Error("Syntax Error missing var before declaring variable");
                        break;
                    default:
                        _ui.Error("Syntax Error in accept");
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
            Command command = null;
            switch (CurrentToken.getType())
            {
                // Command 		::=	if Expression then Command else Command 
                case If:
                    command = parseIfCommand();
                    break;
                // Command | let Declaration in Command
                case Let:
                    command = parseLetCommand();
                    break; 
                
                // Command | V-name := Expression |
                case Identifier:
                    command = parseAssignCommand();
                    break;
                default:
                    Console.WriteLine("Syntax Error in Command");
                    break;
                    
            }

            return command;
        }
        /**
         * Parse the IfCommand that can be called from the parseCommand method
         * Command 		::=	if Expression then Command else Command 
         */
        Command parseIfCommand()
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
        
        /**
         * Parse the LetCommand that can be called from the parseCommand method
         * Command := let Declaration in Command
         */
        Command parseLetCommand()
        {
            acceptIt();
            Declaration declaration = parseSingleDeclaration();
            /*
             Here I am following an example from the Textbook
             Example 4.12 shows how to handle the parseSingleDeclaration of the rule Command := singleDeclaration(;singleDeclartion)*
             I am adapting that solution for my use case but instead using an if statement here since we can only have 
             2 Declarations declared after one another (Declaration;Declaration).
             So here I am calling the parseDeclaration method which will resolve the declaration in to its singleDeclaration rule
             but it will also handle the sequentialDeclaration rule as you can see below. 
             */
            if (CurrentToken.getType().Equals(SemiColon))
            {
                // we know its a semi colon so we just accept it
                acceptIt();
                declaration = new SequentialDeclaration(declaration, parseDeclaration());
            }
            // expect the next token to be an In
            accept(In);
            Command command = new LetCommand(declaration, parseCommand());
            // make sure it ends with the End Token
            accept(End);
            return command;
        }
        /**
         * Parse the AssignCommand that can be called from the parseCommand method
         * Command := V-name := Expression 
         */
        Command parseAssignCommand()
        {
            VName VName = new VName(CurrentToken.getSpelling());
            acceptIt(); // Accept VName
            // then we expect the next token to be an operator (which should be an '=' sign)
            accept(Operator);
            return new AssignCommand(VName, parseExpression());
        }
        // ============================================================================================================
        // Expression 		::=	PrimaryExpression Operator PrimaryExpression
        Expression parseExpression()
        {
            Expression expression;
            if (CurrentToken == null)
                return null;
            PrimaryExpression P1 = parsePrimary();
            Operate O = parseOperator();
            PrimaryExpression P2 = parsePrimary();
            expression = new Expression(P1, O, P2);
            return expression;
        }

        // PrimaryExpression	   ::=	V-name
        //                         ::=	| ( Expression )
        //                              | Int-literal

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
    
        // =============================================================================================================
       // V-name 		::=	a | b | c | d | e
       Identifier parseIdentifier()
        {
            Identifier I = new Identifier(CurrentToken.getSpelling());
            accept(Identifier);
            return I;
        }
       // Operator 		::=	+ | - | * | / | < | > | =
        Operate parseOperator()
        {
            Operate O = new Operate(CurrentToken.getSpelling());
            accept(Operator);
            return O;
        }
        // ============================================================================================================
        // Declaration 		::= 	single-Declaration
        //                         | Declaration ; Declaration

        Declaration parseDeclaration()
        {
            Declaration declaration = parseSingleDeclaration();
            /*
             Here I am following an example from the Textbook
             Example 4.12 shows how to handle the parseSingleDeclaration of the rule Command := singleDeclaration(;singleDeclartion)*
             I am adapting that solution for my use case but instead using an if statement here since we can only have 
             2 Declarations declared after one another (Declaration;Declaration).
             So here I am calling the parseDeclaration method which will resolve the declaration in to its singleDeclaration rule
             but it will also handle the sequentialDeclaration rule as you can see below. 
             */
            if (CurrentToken.getType().Equals(SemiColon))
            {
                // we know its a semi colon so we just accept it
                acceptIt();
                declaration = new SequentialDeclaration(declaration, parseDeclaration());
            }
            return declaration;
        }
        
        // Single-Declaration 	::= 	var V-name : Type-denoter
        SingleDeclaration parseSingleDeclaration()
        {
            SingleDeclaration singleDeclaration = null;
            acceptIt () ; 
            Identifier I =  parseIdentifier();
            accept(Is); 
            TypeDenoter kind =  parseTypeDenoter();
            singleDeclaration = new SingleDeclaration(I, kind);
            return singleDeclaration;
        }

        // =============================================================================================================
        // Type-denoter		::= 	int | double | boolean
        TypeDenoter parseTypeDenoter()
        {
            TypeDenoter typeDenoter = new TypeDenoter(CurrentToken.getSpelling());
            accept(Literal);
            return typeDenoter;
        }

        IntLiteral parseIntLiteral()
        {
            IntLiteral intLiteral = new IntLiteral(CurrentToken.getSpelling());
            accept(Literal);
            return intLiteral;
        }
    }
}