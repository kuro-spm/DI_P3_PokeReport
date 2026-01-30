using Microsoft.EntityFrameworkCore;
using PokeData.Models;
using PokeReport.QuestPdf;
using System.Windows;

public static class InvoiceDocumentDataSource
{
    private static Random Random = new Random();
    private static AppDbContext context = new AppDbContext();
    public readonly static int numPokemon = 9;

    /// <summary>
    /// Retrieves invoice details including a list of Pokemons with their base stats and types, along with the current
    /// date.
    /// </summary>
    /// <returns>An InvoiceModel containing the selected Pokemons and the current date, or null if an error occurs.</returns>
    public static InvoiceModel GetInvoiceDetails()
    {
        try
        {
            return new InvoiceModel
            {
                pokemonList = context.Pokemons
                     .Include(p => p.BaseStat)
                     .Include(p => p.PokemonTypes).ThenInclude(pt => pt.Type)
                     .Take(numPokemon)
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