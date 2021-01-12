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

        public Object visitAssignCommand(AssignCommand command, Object arg)
        {
            Type vType = (Type) command.vname.visit(this, null);
            Type eType = (Type) command.exp.visit(this, null);
            if (!command.vname.Variable)
            {
                UI.Error("Contextual Error\nRule V:= Expr\nV is not a variable!");
            }

            if (!eType.Equals(vType))
            {
                    UI.Error("Contextual Error\nRule V:= Expr\n V and Expr are not of equivalent Types!");
            }

            return null;
        }

        public Object visitLetCommand(LetCommand command, Object arg)
        {
            //idTable.openScope();
            command.declaration.visit(this, null);
            command.command.visit(this, null);
            //idTable.closeScope();
            return null;
        }

        public Object visitIfCommand(IfCommand command, Object arg)
        {
            Type eType = (Type) command._expression.visit(this, null);
            if(!eType._Equals(Type._bool)){
                UI.Error("Contextual Errorr" + "\n" +
                         "For Rule: if Expression then Command else Command" + "\n" +
                         "Expression is not a boolean");
            }
            command._command1.visit(this, null);
            command._command2.visit(this, null);
            return null;
        }
        

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