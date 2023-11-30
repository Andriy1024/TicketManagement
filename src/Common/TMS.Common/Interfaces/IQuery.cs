using MediatR;

namespace TMS.Common.Interfaces;

public interface IQuery<TResult> : IRequest<TResult>
{
}