using System;
using System.Linq.Expressions;

namespace Compiler_Project_Tanks___Carl_Meyer
{
    /**
     * This is the visitor class that will be used to traverse the AST in order to perform the
     * contextual analysis.
     * (see page 155 - 157) 
     */
    public interface Visitor
    {
        // Programs:
        public Object visitProgram(Program prog, Object arg);

        // Commands:
        public Object visitCommand(Command command, Object arg);

        public Object visitAssignCommand(AssignCommand command, Object arg);


        public Object visitLetCommand(LetCommand command, Object arg);


        public Object visitIfCommand(IfCommand command, Object arg);


        // Expressions:
        public Object visitExpression(Expression expr, Object arg);

        public Object visitPrimaryExpression(PrimaryExpression expr, Object arg);

        // Primary Expressions
        public Object visitIdentifierPE(IdentifierPE expr, Object arg);

        public Object visitBracketsPE(BracketsPE expr, Object arg);

        // Declaration
        public Object visitDeclaration(Declaration expr, Object arg);
        public Object visitSequentialDeclaration(SequentialDeclaration expr, Object arg);
        public Object visitSingleDeclaration(SingleDeclaration expr, Object arg);

        // Value-or-variable-names:
        public Object visitTerminal(Terminal t, Object arg);
        public Object visitIdentifier(Identifier i, Object arg);
        public Object visitOperator(Operate o, Object arg);
        public Object visitTypeDenoter(TypeDenoter td, Object arg);
        public Object visitVName(VName v, Object arg);
        public Object visitIntLiteral(IntLiteral il, Object arg);
    }
}