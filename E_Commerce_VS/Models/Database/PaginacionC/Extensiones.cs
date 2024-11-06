using Microsoft.EntityFrameworkCore;

namespace E_Commerce_VS.Models.Database.Paginación
{
    public static class Extensiones
    {
        public static async Task<Paginacion<TEntity>> GetPagedResultAsync<TEntity>(this IQueryable<TEntity> source, int ElementosPorPagina, int PaginaActual)
       where TEntity : class
        {
            var rows = source.Count();
            var results = await source
                .Skip(ElementosPorPagina * (PaginaActual - 1))
                .Take(ElementosPorPagina)
                .ToListAsync();

            return new Paginacion<TEntity>
            {
                PaginaActual = PaginaActual,
                TotalPaginas = (int)Math.Ceiling((double)rows / ElementosPorPagina),
                ElementosPorPagina = ElementosPorPagina,
                Resultados = results,
                NumeroFilas = rows
            };
        }
    }
}
