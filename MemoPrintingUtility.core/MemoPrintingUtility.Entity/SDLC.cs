using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoPrintingUtility.Entity
{
    public class SDLC
    {
        public string HTNO { get; set; }
        public string Type { get; set; }
        public string Year { get; set; }

        public string subjectCode { get; set; }

        public string Marks { get; set; }

        public string Result { get; set; }

        public string Academic { get; set; }

         public string EI { get; set; }

        public int Order { get; set; }

    }
    public class SDLCSUB
    {
        public string YR { get; set; }
        public string SCODE { get; set; }
        public string F { get; set; }
        public string OCODE { get; set; }
        public string MXMR { get; set; }
        public string MNMR { get; set; }
        public string PMXMR { get; set; }
        public string PMNMR { get; set; }
        public string DEPT { get; set; }
        public string ELEC { get; set; }
        public string INTM { get; set; }
        public string CRS { get; set; }

    }


    public class BALEntity
    {
        public string HTNO { get; set; }
        public string Type { get; set; }
        public string Year { get; set; }

        public string subjectCode { get; set; }

        public string Marks { get; set; }

        public string Result { get; set; }

        public string Academic { get; set; }

        public string EI { get; set; }

        public int Order { get; set; }

        public string PTotal { get; set; }

        public bool Practical { get; set; }

        public string PMarks { get; set; }
        public string PYear { get; set; }


    }
}
