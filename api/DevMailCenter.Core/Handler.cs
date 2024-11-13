namespace DevMailCenter.Core;

public interface ICommandHandler<in T>
{
    ValueTask Handle(T command, CancellationToken token);
}

public interface IQueryHandler<in T, TResult>
{
    ValueTask<TResult> Handle(T query, CancellationToken ct);
}