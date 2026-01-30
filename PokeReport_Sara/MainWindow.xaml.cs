using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Windows;
using Path = System.IO.Path;

namespace PokeReport_Sara
{
    /// <summary>
    /// Lògica d'interacció per a la finestra principal (MainWindow).
    /// Gestiona la descàrrega de recursos i la generació de l'informe PDF.
    /// </summary>
    public partial class MainWindow : Window
    {
        // Reutilitzem la instància de HttpClient per evitar esgotar els sòcols (sockets).
        private static readonly HttpClient httpClient = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Esdeveniment que s'executa quan la finestra s'ha carregat completament.
        /// Orquestra tot el procés: configuració, descàrrega i visualització.
        /// La imatge es guarda en una carpeta Images de Environment.CurrentDirectory.
        /// </summary>
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // 0. CONFIGURACIÓN INICIAL de la baarra de càrrega
                int totalPokemons = InvoiceDocumentDataSource.numPokemon;
                BarraProgreso.Maximum = totalPokemons; 
                BarraProgreso.Value = 0;

                for (int i = 0; i < totalPokemons; i++)
                {
                    TxtStatus.Text = $"Capturant Pokémon {i + 1} de {totalPokemons}...";
                    // 1. Assegurem que tenim la imatge necessària abans de generar el PDF
                    string urlPokemon = "https://www.pokemon.com/static-assets/content-assets/cms2/img/pokedex/full/"+(i+1).ToString("D3") +".png";
                    await GestionarImatgeLocalAsync(urlPokemon);
                    BarraProgreso.Value = i + 1;

                }

                TxtStatus.Text = "Generant informe PDF...";
                //await Task.Delay(10);

                // 2. Configurem la llicència de QuestPDF (Community és gratuïta per a ús no comercial/petites empreses)
                QuestPDF.Settings.License = LicenseType.Community;

                // 3. Obtenim les dades i generem el document
                // Nota: Assegura't que tens la classe 'InvoiceDocument' i 'InvoiceDocumentDataSource' definides al projecte
                var model = InvoiceDocumentDataSource.GetInvoiceDetails();
                var document = new InvoiceDocument(model);

                // 4. Generem el fitxer PDF al disc
                string nomFitxer = "invoice.pdf";
                document.GeneratePdf(nomFitxer);

                // 5. Mostrem el PDF dins del component WebView de WPF
                String fullPath = Path.GetFullPath(nomFitxer);
                BrowserView.Source = new Uri("file:///" + fullPath);

                //Ocultar pantalla de carrega
                LoadingScreen.Visibility = Visibility.Collapsed;
                BrowserView.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error generant PDF: {ex.Message}");
                MessageBox.Show($"S'ha produït un error crític: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Descarrega una imatge des d'una URL i la guarda en una carpeta local "Images".
        /// Si la imatge ja existeix localment, no la torna a descarregar (sistema de memòria cau simple).
        /// </summary>
        /// <param name="imageUrl">L'adreça URL de la imatge a descarregar.</param>
        /// <returns>Una tasca asíncrona (Task).</returns>
        private async Task GestionarImatgeLocalAsync(string imageUrl)
        {
            try
            {
                // Extraiem el nom del fitxer de la URL (ex: 001.png)
                string fileName = imageUrl.Substring(imageUrl.LastIndexOf('/') + 1);

                // Creem el directori si no existeix
                DirectoryInfo di = Directory.CreateDirectory(@"Images");
                string localPath = Path.Combine(di.FullName, fileName);

                // Només descarreguem si no tenim el fitxer
                if (!File.Exists(localPath))
                {
                    byte[] imageBytes = await httpClient.GetByteArrayAsync(imageUrl);
                    await File.WriteAllBytesAsync(localPath, imageBytes);
                    Debug.WriteLine($"Imatge descarregada: {localPath}");
                }
                else
                {
                    Debug.WriteLine($"Imatge trobada a la memòria cau: {localPath}");
                }
            }
            catch (Exception ex)
            {
                // Gestionem errors de xarxa o de disc, però no aturem l'aplicació completament
                // Podríem rellançar l'excepció si fos crític
                Debug.WriteLine($"Error descarregant la imatge: {ex.Message}");
                throw; // Rellancem l'error perquè Window_Loaded sàpiga que ha fallat
            }
        }
    }
}