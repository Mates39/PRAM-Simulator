using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakalarka
{
    public class HistoryRecord
    {
        public int IP {  get; set; }
        public bool Parallel { get; set; }
        private MemoryGateway Gateway {  get; set; }
        public int Value { get; set; }
        public int StepBack()
        {
            return 0;
        }
    }
}
