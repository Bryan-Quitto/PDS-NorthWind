using NorthWind.Exceptions.Entities.Exceptions;

namespace NorthWind.Sales.Backend.DataContexts.EFCore.Services;

public static class GuardDBContext
{
    public static async Task SaveChangesAsync(DbContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            var entries = ex.Entries.Select(e => e.Entity.GetType().Name).ToList();
            throw new ConcurrencyException("A concurrency conflict occurred while saving changes to the database.", ex, entries);
        }
        catch (DbUpdateException ex)
        {
            var entries = ex.Entries.Select(e => e.Entity.GetType().Name).ToList();
            throw new UpdateException("Ocurrió un error al persistir los cambios en la base de datos.", ex, entries);
        }
    }
}
