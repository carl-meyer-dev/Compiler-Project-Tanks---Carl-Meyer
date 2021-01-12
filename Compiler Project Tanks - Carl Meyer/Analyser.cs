namespace Compiler_Project_Tanks___Carl_Meyer
{
    public class Analyser : Visitor
    {
        private IdentificationTable idTable;

        public Analyser()
        {
            idTable = new IdentificationTable();
        }

        public void Check(Program program)
        {
            program.visit(this, null);
        }

        public object visitProgram(Program prog, object arg)
        {
            prog.command.visit(this, null);
            return null;
        }

        public object visitCommand(Command command, object arg)
        {
            command.visit(this, null);
            return null;
        }

        public object visitAssignCommand(AssignCommand command, object arg)
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

        public object visitLetCommand(LetCommand command, object arg)
        {
            idTable.openScope();
            command.declaration.visit(this, null);
            command.command.visit(this, null);
            idTable.closeScope();
            return null;
        }

        public object visitIfCommand(IfCommand command, object arg)
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

        public object visitExpression(Expression expr, object arg)
        {
            Type oType = (Type) expr.O.visit(this, null);
            Type p1Type = (Type) expr.P1.visit(this, null);
            Type p2Type = (Type) expr.P2.visit(this, null);
            return null;

        }

        public object visitPrimaryExpression(PrimaryExpression expr, object arg)
        {
            Type eType = (Type) expr.visit(this, null);
            return eType;
        }

        public object visitIdentifierPE(IdentifierPE expr, object arg)
      {
          expr.Type = null;
          TypeDenoter vType = (TypeDenoter) expr.visit(this, null);
          return vType;
      }

        public object visitBracketsPE(BracketsPE expr, object arg)
        {
            Type eType = (Type) expr.E.visit(this, null);
            return eType;
        }

        public object visitDeclaration(Declaration declaration, object arg)
        {
            declaration.visit(this, null);
            return null;
        }

        public object visitSequentialDeclaration(SequentialDeclaration declaration, object arg)
       {
           declaration._declaration1.visit(this, null);
           declaration._declaration2.visit(this, null);
           return null;
       }

        public object visitSingleDeclaration(SingleDeclaration expr, object arg)
        {
            idTable.enter(expr.Identifier.Spelling, expr);
            return null;
        }

        public object visitTerminal(Terminal t, object arg)
        {
            return t.Spelling;
        }

        public object visitIdentifier(Identifier i, object arg)
        {
            Declaration binding = idTable.retrieve(i.Spelling);
            if (binding != null)
                i.Declaration = binding;
            return binding;
        }

        public object visitOperator(Operate o, object arg)
        {
            Declaration binding = idTable.retrieve(o.Spelling);
            if (binding != null)
                o.OperatorDeclaration = binding;
            return binding;
        }

        public object visitTypeDenoter(TypeDenoter td, object arg)
        {
            return Type.InferType(td.Spelling);
        }

        public object visitVName(VName v, object arg)
        {
            return v.Spelling;
        }

        public object visitIntLiteral(IntLiteral il, object arg)
        {
            return Type._int;
        }
    }
}