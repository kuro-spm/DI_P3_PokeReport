using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PokeReport.QuestPdf;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;


public class InvoiceDocument : IDocument
{
    string baseDir = AppDomain.CurrentDomain.BaseDirectory;
    public static int NUMPOKEMON = 1;

    public InvoiceModel Model { get; }

    public InvoiceDocument(InvoiceModel model)
    {
        Model = model;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
    public DocumentSettings GetSettings() => DocumentSettings.Default;



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

    void ComposeBackground(IContainer container)
    {
        string backgroundImage = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Media", "background.png");
        if (File.Exists(backgroundImage))
        {
            container.Image(backgroundImage).FitUnproportionally();
        }
    }

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
                //column.Item().Text(text =>
                //{
                //    text.Span("Data: ").SemiBold();
                //    text.Span($"{Model.currentDate:d}");
                //});


            });

            //row.ConstantItem(100).Height(50).Placeholder();
        });
    }

    void ComposeContent(IContainer container)
    {
        container
            .PaddingVertical(40)
            .AlignCenter()
            .AlignMiddle()
            .Column(col =>
            {
                col.Spacing(10);
                for (int i = 0; i < NUMPOKEMON; i++)
                {
                    col.Item().Element(subContainer => ComposePokerow(subContainer, i+1));
                }
            });
    }

    void ComposePokerow(IContainer container, int i)
    {
        string imagePath = Path.Combine(baseDir, "Images", i.ToString("D3")+".png");

        container.Row(row =>
        {
            row.RelativeItem().Image(imagePath);
            row.RelativeItem().Padding(5).Background("#E0E0E0").Column(col => { 
                            
            });
            row.RelativeItem().Padding(5).Background("#E0E0E0").Column(col => {
                col.Item().AlignCenter().Text("STATS");
                col.Item().Row(stats =>
                {

                });
            });
        });
    }
    

    void ComposeFooter(IContainer container)
    {
        string pathImage = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Media", "pokeball2.png");

        container.Row(row =>
        {

            row.RelativeItem().AlignRight().Width(10).Image(pathImage);
            row.RelativeItem().AlignCenter().Text(x =>
            {
                x.Span("Página ").FontSize(10);
                x.CurrentPageNumber().FontSize(10);
                x.Span(" / ").FontSize(10);
                x.TotalPages().FontSize(10);
            });
            row.RelativeItem().AlignLeft().Width(10).Image(pathImage);


        });
    }
}
