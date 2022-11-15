using static dtp15_todolist.Todo;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace dtp15_todolist
{
    public class Todo
    {
        public static List<TodoItem> list = new List<TodoItem>();

        public const int Active = 1;
        public const int Waiting = 2;
        public const int Ready = 3;
        public const int Done = 4;
        public static string StatusToString(int status)
        {
            switch (status)
            {
                case Active: return "aktiv";
                case Waiting: return "väntande";
                case Ready: return "avklarad";
                case Done: return "klar";
                default: return "(felaktig)";
            }
        }
        public class TodoItem
        {
            public int status;
            public int priority;
            public string task;
            public string taskDescription;
            public TodoItem(int priority, string task, string taskDescription)
            {
                this.status = Active;
                this.priority = priority;
                this.task = task;
                this.taskDescription = taskDescription;
            }
            public TodoItem(string todoLine)
            {
                string[] field = todoLine.Split('|');
                status = Int32.Parse(field[0]);
                priority = Int32.Parse(field[1]);
                task = field[2];
                taskDescription = field[3];
            }
            public void Print(bool verbose = false)
            {
                string statusString = StatusToString(status);
                Console.Write($"|{statusString,-12}|{priority,-6}|{task,-20}|");
                if (verbose)
                    Console.WriteLine($"{taskDescription,-40}|");
                else
                    Console.WriteLine();
            } 
           
        }
        public static void ReadListFromFile()
        {
            string todoFileName = "todo.lis";
            Console.Write($"Läser från fil {todoFileName} ... ");
            StreamReader sr = new StreamReader(todoFileName);
            int numRead = 0;

            string line;
            while ((line = sr.ReadLine()) != null)
            {
                TodoItem item = new TodoItem(line);
                list.Add(item);
                numRead++;
            }
            sr.Close();
            Console.WriteLine($"Läste {numRead} rader.");
        }
        private static void PrintHeadOrFoot(bool head, bool verbose)
        {
            if (head)
            {
                Console.Write("|status      |prio  |namn                |");
                if (verbose) Console.WriteLine("beskrivning                             |");
                else Console.WriteLine();
            }
            Console.Write("|------------|------|--------------------|");
            if (verbose) Console.WriteLine("----------------------------------------|");
            else Console.WriteLine();
        }
        private static void PrintHead(bool verbose)
        {
            PrintHeadOrFoot(head: true, verbose);
        }
        private static void PrintFoot(bool verbose)
        {
            PrintHeadOrFoot(head: false, verbose);

        }
        public static void PrintTodoList(bool verbose = false)
        {
            PrintHead(verbose);
            foreach (TodoItem item in list)
            {
                item.Print(verbose);
            }
            PrintFoot(verbose);
        }
        public static void PrintTodoList2(bool verbose = false, int S = 0)
        {
            PrintHead(verbose);
            foreach (TodoItem item in list)
            {
                if(S == 0) { item.Print(verbose); }
                else if(item.status == S) { item.Print(verbose); }
                
            }
            PrintFoot(verbose);
        }

        public static void PrintHelp()
        {
            Console.WriteLine("Kommandon:");
            Console.WriteLine("hjälp        lista denna hjälp");
            Console.WriteLine("ny           skapa en ny lista");
            Console.WriteLine("lista        lista att-göra-listan");
            Console.WriteLine("Lista allt   lista alla att-göra-listan");
            Console.WriteLine("beskriv      lista alla active att-göra-listan");
            Console.WriteLine("klar         lista klar uppgifte");
            Console.WriteLine("sluta        spara att-göra-listan och sluta");
        }
    }
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Välkommen till att-göra-listan!");
            Todo.ReadListFromFile();
            Todo.PrintHelp();
            string command;
            do
            {
                command = MyIO.ReadCommand("> ");
                if (MyIO.Equals(command, "hjälp"))
                {
                    Todo.PrintHelp();
                }
                else if (MyIO.Equals(command, "ny"))
                {
                    Console.Write("Uppgiftens namn: ");
                    string task = Console.ReadLine();
                    Console.Write("Prioritet: ");
                    int prio = Int32.Parse(Console.ReadLine());
                    Console.Write("Beskrivning: ");
                    string beskrivning = Console.ReadLine();
                    TodoItem item = new TodoItem(prio, task, beskrivning);
                    list.Add(item);
                    Console.WriteLine($"Sparad!");

                }
                else if(MyIO.Equals(command, "beskriv"))
                {
                    Todo.PrintTodoList2(verbose: true, 1);
                    
                }
                else if(MyIO.Equals(command, "klar"))
                {
                    string status = Todo.StatusToString(0);
                    TodoItem item = new TodoItem(status);
                    list.Add(item);
                    //File.WriteAllText(@"todo.lis", status);
                    PrintTodoList2(verbose: true, 4);

                }
                else if (MyIO.Equals(command, "sluta"))
                {
                    Console.WriteLine("Hej då!");
                    break;
                }
                else if (MyIO.Equals(command, "lista"))
                {
                    if (MyIO.HasArgument(command, "allt"))
                        Todo.PrintTodoList(verbose: false);
                    else
                        Todo.PrintTodoList2(verbose: false, 1);
                }
                else
                {
                    Console.WriteLine($"Okänt kommando: {command}");
                }
            }
            while (true);
        }
    }
    class MyIO
    {
        static public string ReadCommand(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
        static public bool Equals(string rawCommand, string expected)
        {
            string command = rawCommand.Trim();
            if (command == "") return false;
            else
            {
                string[] cwords = command.Split(' ');
                if (cwords[0] == expected) return true;
            }
            return false;
        }
        static public bool HasArgument(string rawCommand, string expected)
        {
            string command = rawCommand.Trim();
            if (command == "") return false;
            else
            {
                string[] cwords = command.Split(' ');
                if (cwords.Length < 2) return false;
                if (cwords[1] == expected) return true;
            }
            return false;
        }
    }
}
