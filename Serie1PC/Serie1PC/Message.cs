using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie1PC
{
    public class Message<T>
    {
        public Predicate<uint> pred;
        public uint type;
        public T content;
        public Boolean taken;
    }
}
