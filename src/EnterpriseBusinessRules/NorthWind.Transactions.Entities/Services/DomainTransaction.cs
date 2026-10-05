using NorthWind.Transactions.Entities.Interfaces;

namespace NorthWind.Transactions.Entities.Services;

public class DomainTransaction : IDomainTransaction, IDisposable
{
    private TransactionScope? transactionScope;
    private bool disposedValue;

    public void BeginTransaction()
    {
#pragma warning disable CA1416
        TransactionManager.ImplicitDistributedTransactions = true;
#pragma warning restore CA1416

        var options = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
            Timeout = TransactionManager.DefaultTimeout
        };

        transactionScope = new TransactionScope(
            TransactionScopeOption.Required,
            options,
            TransactionScopeAsyncFlowOption.Enabled
        );
    }

    public void CommitTransaction()
    {
        transactionScope?.Complete();
    }

    public void RollbackTransaction()
    {
        transactionScope?.Dispose();
        transactionScope = null;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                transactionScope?.Dispose();
                transactionScope = null;
            }
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
