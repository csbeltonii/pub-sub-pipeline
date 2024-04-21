using ConcurrentPipeline.Batching.Interfaces;
using System.Threading.Channels;

namespace ConcurrentPipeline.Batching.Services
{
    public class DataQueueService<TModel> : IDataQueueService<TModel>
    {
        private readonly Channel<TModel> _queue;

        public static DataQueueService<TModel> Create(int capacity) => new(capacity);

        private DataQueueService(int capacity)
        {
            var options = new BoundedChannelOptions(capacity)
            {
                FullMode = BoundedChannelFullMode.Wait
            };

            _queue = Channel.CreateBounded<TModel>(options);
        }

        public async ValueTask QueueAsync(TModel entity)
            => await _queue.Writer.WriteAsync(entity);

        public async ValueTask<TModel> DequeueAsync(CancellationToken cancellationToken)
            => await _queue.Reader.ReadAsync(cancellationToken);

        public IAsyncEnumerable<TModel> DequeueAllAsync(CancellationToken cancellationToken)
            => _queue.Reader.ReadAllAsync(cancellationToken);
    }
}
