//namespace interface;

internal class Program
{
    interface Inter1
    {
        void F1();
    }

    interface Inter2
    {
        void F1();
    }

    class C3 : Inter1, Inter2
    {
        void Inter1.F1()
        {
            Console.WriteLine("This is the implementation of Inter1.F1()");
        }

        void Inter2.F1()
        {
            Console.WriteLine("This is the implementation of Inter2.F1()");
        }
    }

    class ClsInterface1
    {
        static void Main(string[] args)
        {
            C3 obj1 = new C3();

            Inter1 obj2 = (Inter1)obj1;
            Inter2 obj3 = (Inter2)obj1;

            obj2.F1();
            obj3.F1();

            Console.ReadLine();
        }
    }
} 

