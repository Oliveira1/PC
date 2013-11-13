using System;

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
