using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakalarka
{
    public class Memory : INotifyPropertyChanged
    {
        public ObservableCollection<MemCell> memory { get; set; }
        public List<MemoryAccess> MemoryAccesses { get; set; }
        public Memory(ObservableCollection<MemCell> mem) { this.memory = mem; }
        public Memory() { 
            this.memory = new ObservableCollection<MemCell>();
            this.MemoryAccesses = new List<MemoryAccess>();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void TryWrite(MemoryAccess ma)
        {
            if(ma.gateway is LocalMemoryGateway)
            {
                var item = memory.FirstOrDefault(i => i.Index == ma.memoryIndex);
                if (item != null) { item.Value = ma.value; }
            }
            else
                MemoryAccesses.Add(ma);
        }
        public bool MemoryAccessCheck(int variant)
        {
            if (variant == PRAM_ACCESS_TYPE.CRCW_C)
            {
                var accesses = MemoryAccesses.GroupBy(x => x.memoryIndex);
                foreach (var item in accesses)
                {
                    if (item.Count() > 1) { MemoryAccesses.Clear(); return false; }
                    var it = item.First();
                    var mem = memory.FirstOrDefault(x => x.Index == it.memoryIndex);
                    if (mem != null) { mem.Value = it.value; }
                }
                MemoryAccesses.Clear();
                return true;
            }
            else if (variant == PRAM_ACCESS_TYPE.CRCW_A)
            {
                var accesses = MemoryAccesses.GroupBy(x => x.memoryIndex);
                foreach (var item in accesses)
                {
                    int random = new Random().Next(0, item.Count());
                    var it = item.Where(x => x.processorID == random).First();
                    var mem = memory.FirstOrDefault(x => x.Index == it.memoryIndex);
                    if (mem != null) { mem.Value = it.value; }
                }
                MemoryAccesses.Clear();
                return true;
            }
            else if (variant == PRAM_ACCESS_TYPE.CRCW_P)
            {
                var accesses = MemoryAccesses.GroupBy(x => x.memoryIndex);
                foreach (var item in accesses)
                {
                    var it = item.OrderBy(x => x.processorID).First();
                    var mem = memory.FirstOrDefault(x => x.Index == it.memoryIndex);
                    if (mem != null) { mem.Value = it.value; }
                }
                MemoryAccesses.Clear();
                return true;
            }
            else if (variant == PRAM_ACCESS_TYPE.CREW)
            {
                return true;
            }
            else if (variant == PRAM_ACCESS_TYPE.EREW)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
