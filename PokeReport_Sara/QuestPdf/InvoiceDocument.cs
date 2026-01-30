using PokeData.Models;
using PokeReport.QuestPdf;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;


public class InvoiceDocument : IDocument
{
    string baseDir = AppDomain.CurrentDomain.BaseDirectory;

    public InvoiceModel Model { get; }

    public InvoiceDocument(InvoiceModel model)
    {
        Model = model;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
    public DocumentSettings GetSettings() => DocumentSettings.Default;


    /// <summary>
    /// Mètode principal que compon l'estructur de la pàgina del pdf.
    /// Defineix marges, fons, capçalera, contingut principal i peu de pàgina.
    /// </summary>
    /// <param name="container"></param>
    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Margin(50);
                page.Background().Element(ComposeBackground);
                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().Element(ComposeFooter);
            });
    }

    /// <summary>
    /// Defineix una imatge de fons si existeix al disc
    /// </summary>
    /// <param name="container"></param>
    void ComposeBackground(IContainer container)
    {
        string backgroundImage = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Media", "background.png");
        if (File.Exists(backgroundImage))
        {
            container.Image(backgroundImage).FitUnproportionally();
        }
    }

    /// <summary>
    /// Composa la capçalera amb el logo i la data actual
    /// </summary>
    /// <param name="container"></param>
    void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                string imagePath = Path.Combine(baseDir, "Media", "Ball-Pokedex.png");
                if (File.Exists(imagePath))
                {
                    column.Item().Image(imagePath);
                }
                else
                {
                    column.Item().Height(50).Width(50).Placeholder();
                    Console.WriteLine("DEBUG: No s'ha trobat la imatge: " + imagePath);
                }

                row.RelativeItem().AlignRight().AlignMiddle().Text(text =>
                {
                    text.Span("Data: ").SemiBold();
                    text.Span($"{Model.currentDate:d}");
                });
            });
        });
    }

    /// <summary>
    /// Defineix el cos principal del document. Itera sobre la llista de pokemon per crear cadascuna de les files del pdf
    /// </summary>
    /// <param name="container"></param>
    void ComposeContent(IContainer container)
    {
        container
            .PaddingVertical(40)
            .AlignCenter()
            .AlignMiddle()
            .Column(col =>
            {
                col.Spacing(10);
                foreach (var pokemon in Model.pokemonList.ToList())
                {
                    col.Item().Element(subContainer => ComposePokerow(subContainer, pokemon));
                }
            });
    }

    /// <summary>
    /// Composes a row in the container displaying a Pokémon's image, name, types, and base stats.
    /// </summary>
    /// <param name="container">The container to which the Pokémon row is added.</param>
    /// <param name="pokemon">The Pokémon whose information is displayed in the row.</param>
    void ComposePokerow(IContainer container, Pokemon pokemon)
    {
        string imagePath = Path.Combine(baseDir, "Images", pokemon.PokId.ToString("D3") + ".png");

        container.Row(row =>
        {
            if (File.Exists(imagePath))
                row.RelativeItem().Image(imagePath);
            else
                row.RelativeItem().Placeholder();

            row.RelativeItem().Padding(5).Background("#E0E0E0").Column(col =>
            {
                col.Item().Padding(25).AlignCenter().AlignMiddle().Text($"N.°:{pokemon.PokId:D4}").FontColor(Colors.Grey.Darken2);
                col.Item().Padding(5).AlignCenter().AlignMiddle().Text(pokemon.PokName.ToUpper()).Bold().FontSize(14);
                col.Item().Padding(25).PaddingTop(5).Row(typeRow =>
                {
                    typeRow.Spacing(5);
                    if (pokemon.PokemonTypes != null)
                    {
                        foreach (var pokType in pokemon.PokemonTypes)
                        {
                            string color = pokType.Type.Color ?? Colors.Grey.Medium;

                            typeRow.RelativeItem()
                                   .Background(color)
                                   .Padding(4)
                                   .PaddingHorizontal(5)
                                   .AlignCenter()
                                   .Text(pokType.Type.TypeName.ToUpper())
                                   .FontSize(8)
                                   .FontColor(Colors.White);
                        }
                    }
                });
            });
            row.RelativeItem().Padding(5).Background("#E0E0E0").Column(col =>
            {
                col.Item().Padding(25).AlignCenter().Text("STATS");
                col.Item().PaddingHorizontal(5).PaddingBottom(25).Height(75).Row(stats =>
                {
                    stats.Spacing(2);
                    if (pokemon.BaseStat != null)
                    {
                        DrawStatBar(stats, "HP", pokemon.BaseStat.BHp);
                        DrawStatBar(stats, "ATK", pokemon.BaseStat.BAtk);
                        DrawStatBar(stats, "DEF", pokemon.BaseStat.BDef);
                        DrawStatBar(stats, "SPA", pokemon.BaseStat.BSpAtk);
                        DrawStatBar(stats, "SPD", pokemon.BaseStat.BSpDef);
                        DrawStatBar(stats, "VEL", pokemon.BaseStat.BSpeed);
                    }
                });
            });
        });
    }

    /// <summary>
    /// Renders a statistical bar with a specified label and value within the given row.
    /// </summary>
    /// <param name="row">The row descriptor where the stat bar will be drawn.</param>
    /// <param name="label">The label to display below the stat bar.</param>
    /// <param name="value">The value determining the height of the stat bar; if null, defaults to 0.</param>
    void DrawStatBar(RowDescriptor row, string label, int? value)
    {
        row.RelativeItem().AlignBottom().Column(c =>
        {
            float height = (value ?? 0) / 2.5f;
            c.Item().Height(height).PaddingHorizontal(4).Background(Colors.Blue.Lighten2);

            c.Item().AlignCenter().Text(label).FontSize(6);
        });
    }

    /// <summary>
    /// Adds a footer to the specified container with a centered page number and images on both sides.
    /// </summary>
    /// <param name="container">The container to which the footer is added.</param>
    void ComposeFooter(IContainer container)
    {
        string pathImage = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Media", "pokeball2.png");

        container.Row(row =>
        {

            row.RelativeItem().AlignRight().Width(10).Image(pathImage);
            row.RelativeItem().AlignCenter().Text(x =>
            {
                x.Span("Pàgina ").FontSize(10);
                x.CurrentPageNumber().FontSize(10);
                x.Span(" / ").FontSize(10);
                x.TotalPages().FontSize(10);
            });
            row.RelativeItem().AlignLeft().Width(10).Image(pathImage);


        });
    }
}
