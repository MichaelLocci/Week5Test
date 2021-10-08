using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademyG.Week5.Test.Model
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Descrizione { get; set; }

        public IList<Spesa> Spese { get; set; }

    }
}
