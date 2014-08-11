using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pokerGame
{
    class MainViewModel
    {
        public MainViewModel()
        {
            pokerGame = new pokerViewModel();
        }
        public pokerViewModel pokerGame { get; set; }
    }
}
