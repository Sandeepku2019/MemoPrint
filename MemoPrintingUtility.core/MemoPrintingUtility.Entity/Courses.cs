using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoPrintingUtility.Entity
{
    public class Courses
    {

        public Courses()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public int Id { get; set; }
        public int PageId { get; set; }
        public string Ids { get; set; }
        public int Ur_ID { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public string StatusName { get { return (Status == 1 ? "Active" : "Inactive"); } }
        public string CourseName { get; set; }
        public int SubBranch { get; set; }
        public int EnExam { get; set; }
        public int Sem { get; set; }
        public int Years { get; set; }
        public int Months { get; set; }
        public int TotalPapers { get; set; }
        public int Theory { get; set; }
        public int Practical { get; set; }
        public int Category { get; set; }
        public int CoursePattern { get; set; }
        public int Credit { get; set; }
        public int University { get; set; }
        public int College { get; set; }
        public string Description { get; set; }
        public string ShortName { get; set; }
        public int MaxYears { get; set; }
        public int MaxMonths { get; set; }
        public int CourseType { get; set; }
        public int CycleExists { get; set; }
        public string Cycle1 { get; set; }
        public string Cycle2 { get; set; }
        public int Marksheet { get; set; }
        public int Attempts { get; set; }
        public int CutofAttendance { get; set; }
        public int Graceattendance { get; set; }
        public int Leet { get; set; }
        public int StartRollNo { get; set; }
        public int EndRollNo { get; set; }

    }
}
