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

        public int Year { get; set; }

        public int Sem { get; set; }

        public string CourseName { get; set; }

        public Int32 CourseID { get; set; }

        public Int32 BranchID { get; set; }

        public int Count { get; set; }

        public Int32 RangeStart { get; set; }

        public Int32 RangeEnd { get; set; }

        public string IPractical { get; set; }
    }



    public class SubJectInformation
    {
        public int YCourseID { get; set; }


        public int Yyr { get; set; }

        public string Sem { get; set; }
        public string Year { get; set; }
        
        public string CourseID { get; set; }
        
        public string ShortCode { get; set; }        
        public string SubjectName { get; set; }

        public string IsPractical { get; set; }

        public int BranchID { get; set; }

        public string CourseName { get; set; }

    }


   
}
