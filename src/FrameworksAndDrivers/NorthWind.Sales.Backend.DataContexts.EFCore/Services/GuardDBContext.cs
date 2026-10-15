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
            throw new ConcurrencyException("El registro o producto ha sido modificado por otra transacción simultánea. Debe esperar a que termine la transacción anterior antes de reintentar.", ex, entries);
        }
        catch (DbUpdateException ex)
        {
            var entries = ex.Entries.Select(e => e.Entity.GetType().Name).ToList();
            throw new UpdateException("Ocurrió un error al persistir los cambios en la base de datos.", ex, entries);
        }
    }
}
