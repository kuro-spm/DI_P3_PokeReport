using PokeData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PokeReport.QuestPdf
{
    public class InvoiceModel
    {
        public Image? pokeTitle { get; set; }
        public DateTime currentDate { get; set; }
        public List<Pokemon> pokemonList { get; set; }

    }
}
