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
        // BCA lon memo information
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


        // BA L long memo information
        public List<BALConEntity> GetBALConInformaionUpdated()
        {
            MemoPrintDBDataContext StudentContext = new MemoPrintDBDataContext();
            StudentContext.CommandTimeout = 260;

            var result = StudentContext.GetBALConUpdate_LongMemo().AsQueryable();
            var studentdetails = (from stu in result
                                  select new BALConEntity
                                  {
                                      HTNO = stu.HTNO,
                                      Code = stu.CODE,
                                      StudentName = stu.NAME,
                                      FatherName = stu.FNAME,
                                      P11 = stu.P11,
                                      PA11 = stu.PA11,
                                      PM11 = stu.PM11,
                                      P12 = stu.P12,
                                      P13 = stu.P13,
                                      P14 = stu.P14,
                                      P15 = stu.P15,
                                      P16 = stu.P16,
                                      PA16 = stu.PA16,
                                      PM16 = stu.PM16,


                                      P21 = stu.P21,
                                      PA21 = stu.PA21,
                                      PM21 = stu.PM21,
                                      P22 = stu.P22,
                                      P23 = stu.P23,
                                      P24 = stu.P24,
                                      P25 = stu.P25,
                                      P26 = stu.P26,
                                      PA26 = stu.PA26,
                                      PM26 = stu.PM26,


                                      P31 = stu.P31,
                                      P32 = stu.P32,
                                      P33 = stu.P33,
                                      P34 = stu.P34,
                                      P35 = stu.P35,
                                      P36 = stu.P36,
                                      P37 = stu.P37,
                                      PM36 = stu.PM36,
                                      PA36 = stu.PA36,

                                      M11 = stu.M11,
                                      M12 = stu.M12,
                                      M13 = stu.M13,
                                      M14 = stu.M14,
                                      M15 = stu.M15,
                                      M16 = stu.M16,

                                      Y11 = stu.Y11,
                                      Y12 = stu.Y12,
                                      Y13 = stu.Y13,
                                      Y14 = stu.Y14,
                                      Y15 = stu.Y15,
                                      Y16 = stu.Y16,

                                      M21 = stu.M21,
                                      M22 = stu.M22,
                                      M23 = stu.M23,
                                      M24 = stu.M24,
                                      M25 = stu.M25,
                                      M26 = stu.M26,


                                      Y21 = stu.Y21,
                                      Y22 = stu.Y22,
                                      Y23 = stu.Y23,
                                      Y24 = stu.Y24,
                                      Y25 = stu.Y25,
                                      Y26 = stu.Y26,

                                      M31 = stu.M31,
                                      M32 = stu.M32,
                                      M33 = stu.M33,
                                      M34 = stu.M34,
                                      M35 = stu.M35,
                                      M36 = stu.M36,
                                      M37 = stu.M37,

                                      Y31 = stu.Y31,
                                      Y32 = stu.Y32,
                                      Y33 = stu.Y33,
                                      Y34 = stu.Y34,
                                      Y35 = stu.Y35,
                                      Y36 = stu.Y36,
                                      Y37 = stu.Y37,


                                      Total = stu.PRT2,
                                      FinalResult = stu.FinResult




                                  }).ToList<BALConEntity>();

            return studentdetails;


        }


    }
}
