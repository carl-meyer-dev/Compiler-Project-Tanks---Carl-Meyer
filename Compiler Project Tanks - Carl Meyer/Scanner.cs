using System;
using System.Collections;

namespace Compiler_Project_Tanks___Carl_Meyer
{
    public class Scanner
    {
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

        ArrayList TokenList = new ArrayList();
        String Sentence;     int curPos;

        public string _Sentence
        {
            get => Sentence;
            set => Sentence = value;
        }

        public Scanner(String S)
        {   Sentence = S;
            BuildTokenList();
            curPos = 0;
        }

        public void DisplayTokens()
        {   for (int x = 0; x <= TokenList.Count - 1; x++)
            ((Token)TokenList[x]).showSpelling();
        }

        public ArrayList getTokens()
        {   return TokenList;
        }

        String BuildNextToken()
        {   String Token = "";
            while (Sentence[curPos] == ' ')   curPos++;
            while ((curPos < Sentence.Length) && (Sentence[curPos] != ' '))  
            {   Token = Token + Sentence[curPos];
                curPos++;
            }
            return Token;
        }

        int FindType(String Spelling)
        {   
            if (Spelling.Equals("(")) return LPar;
            if (Spelling.Equals(")")) return RPar;
            if (Spelling.Equals("+")) return Operator;
            if (Spelling.Equals("-")) return Operator;
            if (Spelling.Equals("*")) return Operator;
            if (Spelling.Equals("/")) return Operator;
            if (Spelling.Equals("=")) return Operator;
            if (Spelling.Equals("==")) return Operator;
            if (Spelling.Equals("=>")) return Operator;
            if (Spelling.Equals("<=")) return Operator;
            if (Spelling.Equals("<")) return Operator;
            if (Spelling.Equals(">")) return Operator;
            if (Spelling.Equals("if")) return If;
            if (Spelling.Equals("then")) return Then;
            if (Spelling.Equals("else")) return Else;
            if (Spelling.Equals("end")) return End;
            if (Spelling.Equals("let")) return Let;
            if (Spelling.Equals("in")) return In;
            if (int.TryParse(Spelling, out int n)) return Literal; //See if spelling is a literal
            else return Identifier;
        }
                    
        void BuildTokenList()
        {   Token newOne = null;
            while (curPos < Sentence.Length)
            {
                {   String nextToken = BuildNextToken();
                    newOne = new Token(nextToken, FindType(nextToken));
                }
                TokenList.Add(newOne);
            }
        }

    }
}