using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoPrintingUtility.Entity;

namespace MemoPrintingUtility.DA
{
    public class SDLTablarDA : ISDLTablarDA
    {
        public List<SDLCEntityCON> GetConDataforSDLC(string Course)
        {
            try
            {
                KUPostDBDataContext KUPOContext = new KUPostDBDataContext();
                KUPOContext.CommandTimeout = 260;

                var result = KUPOContext.sp_GetSDLCConData_TRReport(Course).AsQueryable();
                var studentdetails = (from stu in result
                                      select new SDLCEntityCON
                                      {

                                          Code = stu.CODE,

                                          HTNO = stu.HTNO,

                                          P11 = stu.P11,
                                          P12 = stu.P12,
                                          P13 = stu.P13,
                                          P14 = stu.P14,
                                          P15 = stu.P15,
                                          P16 = stu.P16,

                                          P21 = stu.P21,
                                          P22 = stu.P22,
                                          P23 = stu.P23,
                                          P24 = stu.P24,
                                          P25 = stu.P25,
                                          P26 = stu.P26,
                                          P27 = stu.P27,

                                          P31 = stu.P31,
                                          P32 = stu.P32,
                                          P33 = stu.P33,
                                          P34 = stu.P34,
                                          P35 = stu.P35,
                                          P36 = stu.P36,
                                          P37 = stu.P37,

                                          M11 = stu.M11,
                                          M12 = stu.M12,
                                          M13 = stu.M13,
                                          M14 = stu.M14,
                                          M15 = stu.M15,
                                          M16 = stu.M16,

                                          M21 = stu.M21,
                                          M22 = stu.M22,
                                          M23 = stu.M23,
                                          M24 = stu.M24,
                                          M25 = stu.M25,
                                          M26 = stu.M26,
                                          M27 = stu.M27,

                                          M31 = stu.M31,
                                          M32 = stu.M32,
                                          M33 = stu.M33,
                                          M34 = stu.M34,
                                          M35 = stu.M35,
                                          M36 = stu.M36,
                                          M37 = stu.M37,

                                          Y11 = stu.Y11,
                                          Y12 = stu.Y12,
                                          Y13 = stu.Y13,
                                          Y14 = stu.Y14,
                                          Y15 = stu.Y15,
                                          Y16 = stu.Y16,

                                          Y21 = stu.Y21,
                                          Y22 = stu.Y22,
                                          Y23 = stu.Y23,
                                          Y24 = stu.Y24,
                                          Y25 = stu.Y25,
                                          Y26 = stu.Y26,
                                          Y27 = stu.Y27,

                                          Y31 = stu.Y31,
                                          Y32 = stu.Y32,
                                          Y33 = stu.Y33,
                                          Y34 = stu.Y34,
                                          Y35 = stu.Y35,
                                          Y36 = stu.Y36,
                                          Y37 = stu.Y37
                                      }).ToList();


                return studentdetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<SDLCEntityPRES> GetPRESDataforSDLC(string Course)
        {
            try
            {
                KUPostDBDataContext KUPOContext = new KUPostDBDataContext();
                KUPOContext.CommandTimeout = 260;

                var result = KUPOContext.sp_GetSDLCResultData_TRReport(Course).AsQueryable();

                var studentdetails = (from stu in result
                                      select new SDLCEntityPRES
                                      {
                                          HTNO = stu.HTNO,
                                          FullName = stu.NAME,
                                          FName = stu.FNAME,
                                          EI = stu.EI,
                                          Year = stu.FK_YEAR,
                                          ColCode = stu.FK_CLGCODE,

                                          CLM = stu.CLM,
                                          SubjectCode = stu.CONS_SUB,
                                          order = Convert.ToInt32(stu.CLM.Substring(stu.CLM.Length - 1)),
                                          Course = stu.CRS,


                                          Marks = stu.CC_FINAL_VALMARKS,
                                          TotalMarks = stu.TOTAL_MARKS,

                                          Result = stu.RESULT,
                                          FResult = stu.FINALRES,

                                      }).ToList();

                return studentdetails;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
