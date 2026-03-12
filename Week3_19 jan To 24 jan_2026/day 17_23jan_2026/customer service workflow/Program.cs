namespace customer_service_workflow
{
    class CustomerServiceApp
    {
        static void Main()
        {
            Queue<string> ticketQueue = new Queue<string>();
            Stack<string> actionHistory = new Stack<string>();

            ticketQueue.Enqueue("Ticket-101");
            ticketQueue.Enqueue("Ticket-102");
            ticketQueue.Enqueue("Ticket-103");

            string currentTicket = ticketQueue.Dequeue();
            Console.WriteLine($"Processing: {currentTicket}");

            actionHistory.Push("Opened ticket");
            actionHistory.Push("Contacted customer");
            actionHistory.Push("Resolved issue");

            Console.WriteLine($"Undo action: {actionHistory.Pop()}");

            Console.WriteLine("\nRemaining Actions:");
            foreach (var action in actionHistory)
                Console.WriteLine(action);

            Console.WriteLine("\nRemaining Tickets:");
            foreach (var ticket in ticketQueue)
                Console.WriteLine(ticket);
        }
    }
}
