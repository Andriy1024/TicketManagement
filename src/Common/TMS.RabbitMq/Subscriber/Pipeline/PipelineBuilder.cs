namespace TMS.RabbitMq.Pipeline;

internal partial class Pipe
{
    public sealed class Builder<T>
    {
        private readonly List<Line<T>> _pipes = new List<Line<T>>();

        public Builder<T> Add(Line<T> pipe)
        {
            _pipes.Add(pipe); return this;
        }

        public Builder<T> Add(IPipeLine<T> pipe)
        {
            _pipes.Add(pipe.Handle); return this;
        }

        public Handler<T> Build(Handler<T> lastPipe)
        {
            if (_pipes.Count == 0)
            {
                return lastPipe;
            }

            return Build(0, lastPipe);
        }

        private Handler<T> Build(int index, Handler<T> lastPipe)
        {
            if (index < _pipes.Count - 1)
            {
                return (T request) => _pipes[index](request, Build(index + 1, lastPipe));
            }
            else
            {
                return (T request) => _pipes[index](request, lastPipe);
            }
        }
    }
}
