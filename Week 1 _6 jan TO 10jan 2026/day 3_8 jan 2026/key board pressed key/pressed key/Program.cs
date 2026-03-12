namespace pressed_key
{
    internal class Program
    {
        static void Main(string[] args)
        {
            char pressedKey;
            Console.Write("Press key from the keyboard :: ");
            pressedKey = Convert.ToChar(Console.ReadLine());

            if (pressedKey >= '0' && pressedKey <= '9')
            {
                Console.WriteLine("Pressed key is " + pressedKey);
            }
            else
            {
                Console.WriteLine("Not Allowed");
            }
            Console.ReadLine();

        }
    }
}
