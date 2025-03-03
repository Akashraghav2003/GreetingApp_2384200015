using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Model
{
    public class DictionaryForMethod
    {
        private Dictionary<string, long> countryPopulation = new Dictionary<string, long>();

        public bool AddCountry(string country, long population)
        {
            if (countryPopulation.ContainsKey(country))
                return false; // Country already exists

            countryPopulation[country] = population;
            return true;
        }

        public bool UpdatePopulation(string country, long newPopulation)
        {
            if (!countryPopulation.ContainsKey(country))
                return false;

            countryPopulation[country] = newPopulation;
            return true;
        }

        public bool DeleteCountry(string country)
        {
            return countryPopulation.Remove(country);
        }

        public bool IsContain(string key)
        {
            if (countryPopulation.ContainsKey(key)) return true;
            return false;
        }
        



    }
}
