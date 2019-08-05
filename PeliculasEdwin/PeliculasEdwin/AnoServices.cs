using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PeliculasEdwin
{
    public class AnoServices
    {
        public List<SelectListItem> ObtenerAno()
        {
            List<int> years = new List<int>();
            int AnoActual = DateTime.Now.Year;

            for (int i = 1900; i <= AnoActual; i++)
            {
                years.Add(i);
            }


            List<SelectListItem> anos = new List<SelectListItem>();
            foreach (var ano in years)
            {

                var ano1 = new SelectListItem()
                {

                    Text = ano.ToString(),
                    Value = ano.ToString()


                };
                anos.Add(ano1);



            }
            return anos;

        }
    }
}