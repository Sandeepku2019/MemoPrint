using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoPrintingUtility.Entity;

namespace MemoPrintingUtility.DA
{
    public class TabularReportDAService : ITabularReportDAService
    {
        public List<StudentInformation> GetStudentDetail(string Course, int Semister, int sem, int year)
        {
            MemoPrintDBDataContext StudentContext = new MemoPrintDBDataContext();

            var studentdetails = (from stu in StudentContext.SP_GetStudentDetails(Course, Semister, year, sem).AsQueryable()
                                  select new StudentInformation
                                  {
                                      collegecode = stu.FK_CLGCODE,
                                      HallTicketNumber = stu.HTNO,
                                      StudentName = stu.FULLNAME,
                                      FatherName = stu.FNAME,
                                      Ei = stu.EI,
                                      Order = Convert.ToInt16(stu.ORD.Replace("P", "").Replace("PR", "")),
                                      ExernalMarks = stu.FINAL_VAL_MARKS,
                                      InternalMarks = stu.INT_MARKS,
                                      SubjectName = stu.SUBJECTNAME,
                                      Credits = stu.CREDITS,
                                      LeterGrade = stu.LETERGRADE,
                                      SubjectCode = stu.CONVERT_SUBCODE,
                                      Status = stu.RESULT,
                                      FinalResult = stu.FINALRESULT,
                                      SGPA = stu.SGPA
                                  }).ToList<StudentInformation>();

            return studentdetails;
        }

        public List<ConsDataEntity> GetStudentsConsDetails(string Course, int Semister)
        {
            MemoPrintDBDataContext StudentContext = new MemoPrintDBDataContext();


            var studentConsdetails = (from stu in StudentContext.get_ConsDataforTR(Course, Semister).AsQueryable()
                                      select new ConsDataEntity
                                      {
                                          CODE = stu.CODE,
                                          HTNO = stu.HTNO,
                                          SEM = stu.SEM,
                                          P1 = stu.P1,
                                          M1 = stu.M1,
                                          S1 = stu.S1,
                                          R1 = stu.R1,
                                          A1 = stu.A1,
                                          P2 = stu.P2,
                                          M2 = stu.M2,
                                          S2 = stu.S2,
                                          R2 = stu.R2,
                                          A2 = stu.A2,
                                          P3 = stu.P3,
                                          M3 = stu.M3,
                                          S3 = stu.S3,
                                          R3 = stu.R3,
                                          A3 = stu.A3,
                                          P4 = stu.P4,
                                          M4 = stu.M4,
                                          S4 = stu.S4,
                                          R4 = stu.R4,
                                          A4 = stu.A4,
                                          P5 = stu.P5,
                                          M5 = stu.M5,
                                          S5 = stu.S5,
                                          R5 = stu.R5,
                                          A5 = stu.A5,
                                          P6 = stu.P6,
                                          M6 = stu.M6,
                                          S6 = stu.S6,
                                          R6 = stu.R6,
                                          A6 = stu.A6,
                                          P7 = stu.P7,
                                          M7 = stu.M7,
                                          S7 = stu.S7,
                                          R7 = stu.R7,
                                          A7 = stu.A7,
                                          P8 = stu.P8,
                                          M8 = stu.M8,
                                          S8 = stu.S8,
                                          R8 = stu.R8,
                                          A8 = stu.A8,
                                          P9 = stu.P9,
                                          M9 = stu.M9,
                                          S9 = stu.S9,
                                          R9 = stu.R9,
                                          A9 = stu.A9,
                                          P10 = stu.P10,
                                          M10 = stu.M10,
                                          S10 = stu.S10,
                                          R10 = stu.R10,
                                          A10 = stu.A10,
                                          RES = stu.RES,
                                          SGPA = stu.SGPA,
                                          EXAMMONT = stu.EXAMMONT

                                      }).ToList<ConsDataEntity>();

            return studentConsdetails;
            throw new NotImplementedException();
        }

        public List<StudentInformation> GetMallPractHtno(string Course, int Semister, int sem, int year)
        {
            MemoPrintDBDataContext StudentContext = new MemoPrintDBDataContext();


            var studentConsdetails = (from stu in StudentContext.SP_MallPracDetails(Course,Semister,year,sem).AsQueryable()
                                      select new StudentInformation
                                      {                                                                                  
                                          HallTicketNumber = stu.HTNO,
                                      }).ToList<StudentInformation>();
            return studentConsdetails;
        }
    }
}
