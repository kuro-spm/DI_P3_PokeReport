using PokeData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PokeReport.QuestPdf
{
    /// <summary>
    /// Represents an invoice containing a title image, the current date, and a list of Pokémon.
    /// </summary>
    public class InvoiceModel
    {
        public Image? pokeTitle { get; set; }
        public DateTime currentDate { get; set; }
        public List<Pokemon> pokemonList { get; set; }

    }
}
