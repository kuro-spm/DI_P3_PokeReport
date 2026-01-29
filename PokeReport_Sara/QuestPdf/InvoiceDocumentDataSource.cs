using Microsoft.EntityFrameworkCore;
using PokeData.Models;
using PokeReport.QuestPdf;
using QuestPDF.Helpers;
using System.Net;
using System.Windows;

public static class InvoiceDocumentDataSource
{
    private static Random Random = new Random();
    private static AppDbContext context = new AppDbContext();


    public static InvoiceModel GetInvoiceDetails()
    {
        try
        {
            return new InvoiceModel
            {
                pokemonList = context.Pokemons
                     .Include(p => p.BaseStat)
                     .Include(p => p.PokemonTypes).ThenInclude(pt => pt.Type)
                     .Take(100)
                     .ToList(),
                currentDate = DateTime.Now
            };

        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return null;
        }
    }




}