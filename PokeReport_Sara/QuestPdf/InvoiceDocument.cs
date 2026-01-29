using PokeReport.QuestPdf;
using QuestPDF.Drawing;
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



    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Margin(50);

                page.Background().Element(ComposeBackground);

                page.Header().Element(ComposeHeader);
                //page.Content().Element(ComposeContent);


                page.Footer().Element(ComposeFooter);
                //page.Footer().AlignCenter().Text(x =>
                //{

                //    x.CurrentPageNumber();
                //    x.Span(" / ");
                //    x.TotalPages();
                //});
            });
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
            .Height(250)
            .Background(Colors.Grey.Lighten3)
            .AlignCenter()
            .AlignMiddle()
            .Text("Content").FontSize(16);
    }

    void ComposeBackground(IContainer container)
    {
        string backgroundImage = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Media", "background.png");
        if (File.Exists(backgroundImage))
        {
            container.Image(backgroundImage).FitUnproportionally();
        }
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
