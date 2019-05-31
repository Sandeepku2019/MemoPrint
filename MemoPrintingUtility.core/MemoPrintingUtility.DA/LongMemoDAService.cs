using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoPrintingUtility.Entity;

namespace MemoPrintingUtility.DA
{
    public class LongMemoDAService : ILongMemoDAService
    {
        public List<StudentInformation> GetBCA_P_StudentDetailPRES()
        {
            try
            {

                MemoPrintDBDataContext StudentContext = new MemoPrintDBDataContext();
                StudentContext.CommandTimeout = 260;

                var result = StudentContext.Get_PREData_BCA_Previuos_LM().AsQueryable();
                var studentdetails = (from stu in result
                                      select new StudentInformation
                                      {
                                          collegecode = stu.fk_clgcode,
                                          Year = stu.FK_YEAR,
                                          Sem = stu.FK_SEM,

                                          HallTicketNumber = stu.HTNO,
                                          StudentName = stu.FULLNAME,
                                          FatherName = stu.FNAME,
                                          GRACE_MARKS = Convert.ToString(stu.GRACE_MARKS1),
                                          GRACE_MARKS2 = Convert.ToString(stu.GRACE_MARKS2),
                                          Ei = stu.EI,
                                          Order = Convert.ToInt16(stu.ORD.Replace("P", "").Replace("PR", "")),
                                          ExernalMarks = stu.FINAL_VAL_MARKS,
                                          InternalMarks = stu.INT_MARKS,
                                          SubjectExternalMarks = stu.CONVERT_SUB_MAXMARKS,
                                          SubjectInternalMarks = stu.convert_iNT_MAXMARKS,

                                          SubjectName = stu.CONVERT_SUBJECTNAME,
                                          TotalMarks = stu.TOTALMARKS,
                                          SubjectCode = stu.CONVERT_SUBCODE,
                                          FinalResult = stu.FINAL_RESULT,

                                      }).ToList<StudentInformation>();

                return studentdetails;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<ConsDataEntity> GetBCA_P_StudentsConsDetails()
        {
            MemoPrintDBDataContext StudentContext = new MemoPrintDBDataContext();
            var studentConsdetails = (from stu in StudentContext.Get_CONSData_BCA_Previuos_LM().AsQueryable()
                                      select new ConsDataEntity
                                      {

                                          HTNO = stu.HTNO,
                                          SEM = stu.SEM,
                                          P1 = stu.P1,
                                          M1 = stu.M1,
                                          S1 = stu.S1,
                                          A1 = stu.Y1,

                                          P2 = stu.P2,
                                          M2 = stu.M2,
                                          S2 = stu.S2,
                                          A2 = stu.Y2,

                                          P3 = stu.P3,
                                          M3 = stu.M3,
                                          S3 = stu.S3,
                                          A3 = stu.Y3,



                                          P4 = stu.P4,
                                          M4 = stu.M4,
                                          S4 = stu.S4,
                                          A4 = stu.Y4,

                                          P5 = stu.P5,
                                          M5 = stu.M5,
                                          S5 = stu.S5,
                                          A5 = stu.Y5,

                                          P6 = stu.P6,
                                          M6 = stu.M6,
                                          S6 = stu.S6,
                                          A6 = stu.Y6,

                                          P7 = stu.P7,
                                          M7 = stu.M7,
                                          S7 = stu.S7,
                                          A7 = stu.Y7,



                                          RES = stu.RES,
                                          ORES = stu.ORES,


                                      }).ToList<ConsDataEntity>();

            return studentConsdetails;
        }



    }
}
