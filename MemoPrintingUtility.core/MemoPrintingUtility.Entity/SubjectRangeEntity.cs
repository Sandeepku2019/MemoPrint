using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoPrintingUtility.Entity
{
    public class SubjectRangeEntity
    {
        public string SubjectCode { get; set; }

        public string SubjectName { get; set; }


        public int Count { get; set; }

        public Int32 RangeStart { get; set; }

        public Int32 RangeEnd { get; set; }
    }
}
