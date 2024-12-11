namespace E_Commerce_VS.Models.Database.Paginación
{
    public class Paginacion <T>
     where T: class
    {    
            // Todas las cosas que debe tener la paginacion para que funcione
            public IEnumerable<T> Resultados{ get; set; } = Enumerable.Empty<T>();
            public int NumeroFilas { get; set; }
            public int TotalPaginas { get; set; }
            public int ElementosPorPagina { get; set; }
            public int PaginaActual { get; set; }
            
            
        }

    }

