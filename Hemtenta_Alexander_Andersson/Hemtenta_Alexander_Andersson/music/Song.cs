using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemtentaTdd2017.music
{
    public class Song : ISong
    {
        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
    }
}
