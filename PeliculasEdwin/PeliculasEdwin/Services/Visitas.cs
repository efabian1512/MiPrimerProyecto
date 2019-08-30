using PeliculasEdwin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PeliculasEdwin.Services
{
    public class Visita
    {
        public int Id { get; set; }
        public int NumeroVisitas { get; set; }
        public DateTime FechaVisita { get; set; }
        public Pelicula Pelicula { get; set; }
    }
}