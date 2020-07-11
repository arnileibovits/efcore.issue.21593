using System;
using System.Collections.Generic;
using System.Text;

namespace EFTest
{
    public class Parent
    {
        public int Id { get; set; }
        public virtual AbstractChild Child { get; set; }
    }

    public abstract class AbstractChild
    {
        public int Id { get; set; }
        public virtual List<GrandChild> GrandChildren { get; set; }
    }

    public class ConcreteChild : AbstractChild
    {
    }

    public class GrandChild
    {
        public int Id { get; set; }
    }
}
