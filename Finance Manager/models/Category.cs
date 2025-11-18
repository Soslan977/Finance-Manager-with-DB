using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance_Manager.models
{
    internal class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsIncome {  get; set; }//true-доход, false-расход
    }
}
