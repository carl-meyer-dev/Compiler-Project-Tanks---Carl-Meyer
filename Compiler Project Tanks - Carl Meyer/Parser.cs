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

        public Parser(String Sentence)
        {
            Scanner S = new Scanner(Sentence);

            TokenList = S.getTokens();
            CurTokenPos = -1;
            FetchNextToken();
            Expression P = parseExpression();
        }

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
                Console.WriteLine("Syntax Error in accept");
        }

        void acceptIt()
        {
            FetchNextToken();
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