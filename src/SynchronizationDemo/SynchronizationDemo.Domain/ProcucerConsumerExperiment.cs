using Microsoft.VisualBasic;

namespace SynchronizationDemo.Domain
{
    public class ProcucerConsumerExperiment
    {
        private const int BufferSize = 4; // grandezza della tavola in cui mettere i piatti con il cibo
        private const int NProcucers = 2; // numero di produttori
        private const int NConsumers = 2; // numero di consumatori

        // mi serve una struttura dati per posizionare i prodotti creati e consumati dai thread

        Queue<int> buffer = new();
       
        public void Run()
        {

            var spaziDisponibili = new SemaphoreSlim(BufferSize, BufferSize);
            var elementiDisponibili = new SemaphoreSlim(0, BufferSize);
            var accessoTavola = new SemaphoreSlim(1, 1);

            Thread[] produttori = new Thread[NProcucers];
            Thread[] consumatori = new Thread[NConsumers];

            int nextId = 0;

            //TODO: implementazione dei metodi per produrre e consumare
            void ConsumeSomeProduct(int consumerId)
            {
                while (true)
                {
                    int foodItem = Interlocked.Increment(ref nextId); //per evitare che piu thread agiscano sullo stesso item devo creare syncronization
                    spaziDisponibili.Wait();
                    try
                    {
                        try
                        {
                            accessoTavola.Wait();
                            buffer.Enqueue(foodItem);
                            Console.WriteLine($"Produttore {consumerId} ha prodotto l'item {foodItem}. Buffer size: {buffer.Count}");
                        }
                        finally
                        {
                            accessoTavola.Release();
                        }
                    }
                    catch
                    {
                        spaziDisponibili.Release();
                    }
                    elementiDisponibili.Release();
                    Thread.Sleep(Random.Shared.Next(100, 250));
                }   
                  // Simula il tempo di produzione
            }
            
            void MakeSomeProduct(int consumerId)
            {

            }
            for (int i = 1; i <= NProcucers; i++)
            {
                // i corrisponde all'id del thread corrente
                produttori[i] = new Thread(() => MakeSomeProduct(i));
                produttori[i].Start();
            }
            for (int i = 1; i <= NConsumers; i++)
            {
                // i corrisponde all'id del thread corrente
                consumatori[i] = new Thread(() => ConsumeSomeProduct(i));
                consumatori[i].Start();
            }

            foreach (Thread consumatore in consumatori)
            {
                consumatore.Join();
            }
            foreach (Thread produttore in produttori)
            {
                produttore.Join();
            }
        }
    }
}
