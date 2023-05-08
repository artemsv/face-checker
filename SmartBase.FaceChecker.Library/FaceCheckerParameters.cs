using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBase.FaceChecker.Library
{
    public class FaceCheckerParameters
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Action<string> LogCallback { get; set; }
    }
}
