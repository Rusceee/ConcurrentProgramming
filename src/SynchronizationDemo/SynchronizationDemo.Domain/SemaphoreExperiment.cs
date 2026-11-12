namespace SynchronizationDemo.Domain;

public class SemaphoreExperiment
{

    public void Run()
    {
        Console.WriteLine("Esecuzione con semaforo");
        // risorsa condivisa tra i thread.
        //nel primo caso avremo un solo permesso di entrta.
        //1° parametro:initalCount: quanti inizalmente hanno accesso
        //2° parametro:maxCount : quanti al massimo possono avere accesso contemporaneamente
        var stampante = new SemaphoreSlim(1, 1); // Limite di 2 thread contemporanei

        // Creazione di 5 thread che rappresentano i lavoratori
        Thread[] lavoratori = new Thread[4];

        //accesso alla risorsa condivisa dei vari lavoratori
        for (int i = 0; i < lavoratori.Length; i++)
        {
            int idLavoratore = i + 1; // ID del lavoratore (1, 2, 3, ...)
            // Creazione del thread per il lavoratore che chiederà l'accesso alla risorsa condivisa
            lavoratori[i] = new Thread(() =>
            {
                Console.WriteLine($"Lavoratore {idLavoratore} sta aspettando l'accesso alla stampante...");
                stampante.Wait(); // Il lavoratore richiede l'accesso alla risorsa condivisa

                Console.WriteLine($"Lavoratore {idLavoratore} ha ottenuto l'accesso alla stampante.");
                Thread.Sleep(2000);

                Console.WriteLine($"Lavoratore {idLavoratore} rilascia la risorsa condivisa");
                stampante.Release(); // Il lavoratore rilascia la risorsa condivisa

            });

        }


        // Creazione e avvio dei thread
        foreach (Thread lavoratore in lavoratori)
        {
            lavoratore.Start();
        }

        // Attesa che tutti i thread terminino
        foreach (Thread lavoratore in lavoratori)
        {
            lavoratore.Join();
        }

        Console.WriteLine("tutti i lavoratori hanno terminato l'accesso alla stampante");

    }

}