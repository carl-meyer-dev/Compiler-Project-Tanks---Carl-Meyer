using System;

namespace Compiler_Project_Tanks___Carl_Meyer
{
    public class IdEntry
    {
        public String id;
        public Declaration attr;
        public int level;
        public IdEntry previous;

        public IdEntry(String id, Declaration attr, int level, IdEntry previous)
        {
            this.id = id;
            this.attr = attr;
            this.level = level;
            this.previous = previous;
        }
    }
}