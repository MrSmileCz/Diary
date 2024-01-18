using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Diar
{
    public class diar
    {
        public List <Udalost> veci = new List <Udalost>();
        
        public void json_ins()
        {
            var FileName = "Udalosti.json";
            string jsonString = JsonConvert.SerializeObject(veci);

            File.WriteAllText(FileName, jsonString);
        }

        public List <Udalost> json_load()
        {
            var FileName = "Udalosti.json";
            string jsonString = File.ReadAllText(FileName);

            List <Udalost> saved_events = JsonConvert.DeserializeObject<List<Udalost>>(jsonString);

            return saved_events;
        }
    }
}
