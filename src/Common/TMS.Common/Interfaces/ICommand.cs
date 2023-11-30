using MediatR;

namespace TMS.Common.Interfaces;

public interface ICommand<TResult> : IRequest<TResult>
{
}

public interface ICommand : IRequest<Unit>
{
}
