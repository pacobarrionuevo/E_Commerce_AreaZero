namespace E_Commerce_VS.Models.Database.Filtros
{
    public class Enumerables
    {
        //En vez de hacer dos enumerables como Jose tenia pensado, nos salia mas rentable hacer este conjunto y a funcionar.
        public enum Ordenacion
        {
            AscendentePrecio = 0,
            DescendentePrecio = 1,
            AscendenteNombre = 2,
            DescendenteNombre = 3
        }
    }
}
