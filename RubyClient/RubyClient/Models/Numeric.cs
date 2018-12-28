using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubyClient.Models
{
    public class Numeric
    {
        [JsonProperty("a")]
        public int A { get; set; }

        [JsonProperty("b")]
        public int B { get; set; }

        public Numeric(int a, int b)
        {
            this.A = a;
            this.B = b;
        }
    }
}
