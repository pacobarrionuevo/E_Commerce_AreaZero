﻿using E_Commerce_VS.Models.Database.Repositories;
using E_Commerce_VS.Models.Database;

public class UnitOfWork
{
    //El unitofwork es muy parecido a lo que tiene Jose (evidentemente adaptado a lo nuestro)
    private readonly ProyectoDbContext _context;

    //Todas las cosas que necesitamos guardar
    public RepositorioProducto RepoProd { get; init; }
    public RepositorioReview RepoRev { get; init; }
    public RepositorioCarrito RepoCar { get; init; }
    public RepositorioOrdenTemporal RepoOT { get; init; }


    // Exponer el DbContext
    public ProyectoDbContext Context => _context;

    public UnitOfWork(ProyectoDbContext context, RepositorioProducto repoProd, RepositorioReview repoRev, RepositorioCarrito repoCar, RepositorioOrdenTemporal repoOT)
    {
        _context = context;

        RepoProd = repoProd;
        RepoRev = repoRev;
        RepoCar = repoCar;
        RepoOT = repoOT;

    }

    //Método para guardar
    public async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
