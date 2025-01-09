using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaRubica
{
    public class Persona
    {
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Indirizzo { get; set; }
        public string Telefono { get; set; }
        public int Eta { get; set; }

        // Constructor to initialize the properties
        public Persona(string nome, string cognome, string indirizzo, string telefono, int eta)
        {
            Nome = nome;
            Cognome = cognome;
            Indirizzo = indirizzo;
            Telefono = telefono;
            Eta = eta;
        }

        
       
    }
}
