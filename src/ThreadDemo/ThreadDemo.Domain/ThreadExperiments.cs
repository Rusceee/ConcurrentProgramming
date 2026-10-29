namespace ThreadDemo.Domain;


public class ThreadExperiments
{
    public void RunSequential()
    {
        Work("A");
        Work("B");
    }

    public void RunThreads()
    {
        //Lambda expression to assign a method to a thread
        Thread threadA = new (() => Work("A"));
        Thread threadB = new (() => Work("B"));
        Console.WriteLine("Starting threads....");
        // non deterministic execution of threads
        threadA.Start();
        threadB.Start();
        Console.WriteLine("This line is written by the main thread");
        // Wait for the threads to finish
        threadA.Join();
        threadB.Join();
        // this is a deterministic way to wait for the threads to finish, but it is not the only way. You can also use threadA.IsAlive and threadB.IsAlive to check if the threads are still running.
        Console.WriteLine("All Finished");

    }

    public void ShowThreadProperties()
    {
        Thread currentThread = Thread.CurrentThread;
        Console.WriteLine($"Thread ID: {currentThread.ManagedThreadId}");
        Console.WriteLine($"Thread Name: {currentThread.Name}");
        Console.WriteLine($"Is Background: {currentThread.IsBackground}");
        Console.WriteLine($"Is Alive: {currentThread.IsAlive}");
        Console.WriteLine($"Priority: {currentThread.Priority}");
       
    }

    // Funzione per eseguire un contatore senza lock o con lock a seconda del parametro useLock
    // Avere una risorsa condivise su cui piu thread possono accedere contemporaneamente senza lock
    // puo' portare a risultati non deterministici e quindi a bug difficili da trovare a causa della variabile condivisa che viene modificata da piu thread contemporaneamente.ù
    // primo test: i thread contano sulla stessa variabile senza lock, quindi il risultato finale non e' deterministico
    // secondo test: i thread contano sulla stessa variabile con lock, quindi il risultato finale e' deterministico
    public (int expected, int actual) RunCounter(bool useLock, int iterations=100_000) 
    {
        int counter = 0;
        object gateLock = new();

        void Increment()
        {
            for (int i = 0; i <= iterations; i++)
            {
                if(useLock)
                {
                    // sezione critica
                    lock(gateLock)
                    {
                        counter += 1;
                    }
                }
                else
                {
                    counter += 1;
                }
                
            }
        }

        Thread Thread1 = new(Increment);
        Thread Thread2 = new(Increment);

        Thread1.Start();
        Thread2.Start();

        Thread1.Join();
        Thread2.Join();

        return (expected: iterations * 2, actual: counter);
    }

   
    // Funzione per simulare del lavoro
    private static void Work(string name)
    {
        for (int i = 0; i <= 5; i++)
        {
            Console.WriteLine($"{name}: Thread: {Thread.CurrentThread.ManagedThreadId}; index {i}");
            Thread.Sleep(100);
        }
    }
}
