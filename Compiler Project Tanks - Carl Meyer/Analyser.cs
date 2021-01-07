namespace Compiler_Project_Tanks___Carl_Meyer
{
    public class Analyser
    {
        public Program P;
        
        // Due to our production Rules we will be working with a nested block structure
        
        public Analyser(Program program)
        {
            P = program;
        }

        /*
         * We will run a depth first traversal through the AST to do identification & type checking
         */
        public void performContextualAnalysis()
        {
            
            
            
        }

        public void performIdentificationCheck()
        {
            // Each occurrence of an Identifier needs a corresponding declaration
            // Since we are working with a Nested Block Structure we have the following rules:
            //  No Identifier may be declared more than once in the same block
            // For every applied occurrence of an identifier I in block B, there must be a corresponding declaration of I
            // in block B itself or in any block enclosing B
            

        }
    }
}