using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoPrintingUtility.Entity
{
    public class BALPresEntity : BALSubjectInformation
    {
        public string TP { get; set; }
        public string FK_CLGCODE { get; set; }
        public string HTNO { get; set; }
        public string FK_COURSEID { get; set; }
        public string FK_YEAR { get; set; }
        public string EI { get; set; }
        public string SUB { get; set; }
        public string MARKS { get; set; }
        public string RESULT { get; set; }
        public string TOTAL_MARKS { get; set; }

        public string Academic { get; set; }

        public string FullName { get; set; }


        public string SubjectName { get; set; }

        public string FName { get; set; }

    }

    public class BALSubjectInformation
    {

        public string SubjectName { get; set; }

        public string SubjectCode { get; set; }

        public string Year { get; set; }


        public string MaxMark { get; set; }

        public string MinMark { get; set; }

        public string CourseName { get; set; }

    }




    public class BCAPSubjectINformation
    {

        public string SubjectName { get; set; }

        public string SubjectCode { get; set; }

        public string Year { get; set; }

        public string Sem { get; set; }

        public string MaxMark { get; set; }

        public string MinMark { get; set; }

        public string InternalMark { get; set; }

        public string CourseName { get; set; }

    }
}
