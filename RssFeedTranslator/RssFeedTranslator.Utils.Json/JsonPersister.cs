
using System.Collections.Concurrent;
using System.Text.Json;

namespace RssFeedTranslator.Utils.Json
{
    public class JsonPersister : IPersister, IDisposable
    {
        private readonly string fileName;
        private readonly object fileLock = new object();
        private readonly ConcurrentQueue<object> queue = new();
        private Task? task;
        private bool isDisposed;

        public JsonPersister(string fileName)
        {
            this.fileName = fileName;
        }

        public void Store(object data)
        {
            lock(queue)
            {
                queue.Enqueue(data);

                if (task is null)
                {
                    task = Task.Run(ProcessQueue);
                }
            }
        }

        public TValue? Load<TValue>()
        {
            lock (fileLock)
            {
                try
                {
                    using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                    {
                        return JsonSerializer.Deserialize<TValue>(fs);
                    }
                }
                catch (FileNotFoundException)
                {
                    return default;
                }
            }
        }

        private void ProcessQueue()
        {
            while (!isDisposed)
            {
                object? lastData = null;
                lock (queue)
                {
                    while (queue.TryDequeue(out object? data))
                    {
                        lastData = data;
                    }

                    if (lastData is null)
                    {
                        // queue is empty, to set state to not running
                        task = null;
                        return;
                    }
                }

                try
                {
                    lock (fileLock)
                    {
                        using (var fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        {
                            JsonSerializer.Serialize(fs, lastData, new JsonSerializerOptions { WriteIndented = true });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    queue.Clear();
                }

                isDisposed = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

    }
}
