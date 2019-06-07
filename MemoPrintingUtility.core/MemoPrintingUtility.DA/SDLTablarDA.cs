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
                MemoPrintDBDataContext KUPOContext = new MemoPrintDBDataContext();
                KUPOContext.CommandTimeout = 260;
                var studentdetails = new List<SDLCEntityCON>();
                if (Course == "ART")
                {
                    var result = KUPOContext.sp_GetSDLCConData_TRReport_ART().AsQueryable();
                    studentdetails = (from stu in result
                                      select new SDLCEntityCON
                                      {
                                          Code = stu.CODE,

                                          HTNO = stu.HTNO
  ,
                                          P11 = stu.P11
  ,
                                          P12 = stu.P12
  ,
                                          P13 = stu.P13
  ,
                                          P14 = stu.P14
  ,
                                          P15 = stu.P15
  ,
                                          P16 = stu.P16
  ,
                                          P21 = stu.P21
  ,
                                          P22 = stu.P22
  ,
                                          P23 = stu.P23
  ,
                                          P24 = stu.P24
  ,
                                          P25 = stu.P25
  ,
                                          P26 = stu.P26
  ,
                                          P27 = stu.P27
  ,
                                          P31 = stu.P31
  ,
                                          P32 = stu.P32
  ,
                                          P33 = stu.P33
  ,
                                          P34 = stu.P34
  ,
                                          P35 = stu.P35
  ,
                                          P36 = stu.P36
  ,
                                          P37 = stu.P37
  ,
                                          M11 = stu.M11
  ,
                                          Y11 = stu.Y11
  ,
                                          M12 = stu.M12
  ,
                                          Y12 = stu.Y12
  ,
                                          M13 = stu.M13
  ,
                                          Y13 = stu.Y13
  ,
                                          M14 = stu.M14
  ,
                                          Y14 = stu.Y14
  ,
                                          M15 = stu.M15
  ,
                                          Y15 = stu.Y15
  ,
                                          M16 = stu.M16
  ,
                                          Y16 = stu.Y16
  ,
                                          M21 = stu.M21
  ,
                                          Y21 = stu.Y21
  ,
                                          M22 = stu.M22
  ,
                                          Y22 = stu.Y22
  ,
                                          M23 = stu.M23
  ,
                                          Y23 = stu.Y23
  ,
                                          M24 = stu.M24
  ,
                                          Y24 = stu.Y24
  ,
                                          M25 = stu.M25
  ,
                                          Y25 = stu.Y25
  ,
                                          M26 = stu.M26
  ,
                                          Y26 = stu.Y26
  ,
                                          M27 = stu.M27
  ,
                                          Y27 = stu.Y27
  ,
                                          M31 = stu.M31
  ,
                                          Y31 = stu.Y31
  ,
                                          M32 = stu.M32
  ,
                                          Y32 = stu.Y32
  ,
                                          M33 = stu.M33
  ,
                                          Y33 = stu.Y33
  ,
                                          M34 = stu.M34
  ,
                                          Y34 = stu.Y34
  ,
                                          M35 = stu.M35
  ,
                                          Y35 = stu.Y35
  ,
                                          M36 = stu.M36
  ,
                                          Y36 = stu.Y36
  ,
                                          M37 = stu.M37
  ,
                                          Y37 = stu.Y37
  ,
                                          YR1 = stu.YR1
  ,
                                          YR2 = stu.YR2
  ,
                                          YR3 = stu.YR3
                                      }).ToList();

                }
                else if (Course == "MPC")
                {
                    var result = KUPOContext.sp_GetSDLCConData_TRReport_MPC().AsQueryable();
                    studentdetails = (from stu in result
                                      select new SDLCEntityCON
                                      {

                                          Code = stu.CODE
  ,
                                          HTNO = stu.HTNO
  ,
                                          P11 = stu.P11
  ,
                                          P12 = stu.P12
  ,
                                          P13 = stu.P13
  ,
                                          P14 = stu.P14
  ,
                                          P15 = stu.P15
  ,
                                          P16 = stu.P16
  ,
                                          P17 = stu.P17
  ,
                                          P21 = stu.P21
  ,
                                          P22 = stu.P22
  ,
                                          P23 = stu.P23
  ,
                                          P24 = stu.P24
  ,
                                          P25 = stu.P25
  ,
                                          P26 = stu.P26
  ,
                                          P27 = stu.P27
  ,
                                          P31 = stu.P31
  ,
                                          P32 = stu.P32
  ,
                                          P33 = stu.P33
  ,
                                          P34 = stu.P34
  ,
                                          P35 = stu.P35
  ,
                                          P36 = stu.P36
  ,
                                          P37 = stu.P37
  ,
                                          M11 = stu.M11
  ,
                                          Y11 = stu.Y11
  ,
                                          PM11 = stu.PM11
  ,
                                          PA11 = stu.PA11
  ,
                                          M12 = stu.M12
  ,
                                          Y12 = stu.Y12
  ,
                                          PM12 = stu.PM12
  ,
                                          PA12 = stu.PA12
  ,
                                          M13 = stu.M13
  ,
                                          Y13 = stu.Y13
  ,
                                          M14 = stu.M14
  ,
                                          PM14 = stu.PM14
  ,
                                          Y14 = stu.Y14
  ,
                                          PA14 = stu.PA14
  ,
                                          M15 = stu.M15
  ,
                                          PM15 = stu.PM15
  ,
                                          Y15 = stu.Y15
  ,
                                          PA15 = stu.PA15
  ,
                                          M16 = stu.M16
  ,
                                          PM16 = stu.PM16
  ,
                                          Y16 = stu.Y16
  ,
                                          PA16 = stu.PA16
  ,
                                          M17 = stu.M17
  ,
                                          Y17 = stu.Y17
  ,
                                          M21 = stu.M21
  ,
                                          Y21 = stu.Y21
  ,
                                          PM21 = stu.PM21
  ,
                                          PA21 = stu.PA21
  ,
                                          M22 = stu.M22
  ,
                                          Y22 = stu.Y22
  ,
                                          M23 = stu.M23
  ,
                                          Y23 = stu.Y23
  ,
                                          PM22 = stu.PM22
  ,
                                          PA22 = stu.PA22
  ,
                                          M24 = stu.M24
  ,
                                          PM24 = stu.PM24
  ,
                                          Y24 = stu.Y24
  ,
                                          PA24 = stu.PA24
  ,
                                          M25 = stu.M25
  ,
                                          PM25 = stu.PM25
  ,
                                          Y25 = stu.Y25
  ,
                                          PA25 = stu.PA25
  ,
                                          M26 = stu.M26
  ,
                                          PM26 = stu.PM26
  ,
                                          Y26 = stu.Y26
  ,
                                          PA26 = stu.PA26
  ,
                                          M27 = stu.M27
  ,
                                          PM27 = stu.PM27
  ,
                                          Y27 = stu.Y27
  ,
                                          PA27 = stu.PA27
  ,
                                          M31 = stu.M31
  ,
                                          PM31 = stu.PM31
  ,
                                          Y31 = stu.Y31
  ,
                                          PA31 = stu.PA31
  ,
                                          M32 = stu.M32
  ,
                                          PM32 = stu.PM32
  ,
                                          Y32 = stu.Y32
  ,
                                          PA32 = stu.PA32
  ,
                                          M33 = stu.M33
  ,
                                          PM33 = stu.PM33
  ,
                                          Y33 = stu.Y33
  ,
                                          PA33 = stu.PA33
  ,
                                          M34 = stu.M34
  ,
                                          PM34 = stu.PM34
  ,
                                          Y34 = stu.Y34
  ,
                                          PA34 = stu.PA34
  ,
                                          M35 = stu.M35
  ,
                                          PM35 = stu.PM35
  ,
                                          Y35 = stu.Y35
  ,
                                          PA35 = stu.PA35
  ,
                                          M36 = stu.M36
  ,
                                          PM36 = stu.PM36
  ,
                                          Y36 = stu.Y36
  ,
                                          PA36 = stu.PA36
  ,
                                          M37 = stu.M37
  ,
                                          Y37 = stu.Y37
  ,
                                          YR1 = stu.YR1
  ,
                                          YR2 = stu.YR2
  ,
                                          YR3 = stu.YR3


                                      }).ToList();

                }
                else if (Course == "BCM")
                {
                    var result = KUPOContext.sp_GetSDLCConData_TRReport_BCM().AsQueryable();
                    studentdetails = (from stu in result
                                      select new SDLCEntityCON
                                      {
                                          Code = stu.CODE
      ,
                                          HTNO = stu.HTNO
      ,
                                          P11 = stu.P11
      ,
                                          P12 = stu.P12
      ,
                                          P13 = stu.P13
      ,
                                          P14 = stu.P14
      ,
                                          P15 = stu.P15
      ,
                                          P16 = stu.P16
      ,
                                          P17 = stu.P17
      ,
                                          P21 = stu.P21
      ,
                                          P22 = stu.P22
      ,
                                          P23 = stu.P23
      ,
                                          P24 = stu.P24
      ,
                                          P25 = stu.P25
      ,
                                          P26 = stu.P26
      ,
                                          P27 = stu.P27
      ,
                                          P28 = stu.P28
      ,
                                          P31 = stu.P31
      ,
                                          P32 = stu.P32
      ,
                                          P33 = stu.P33
      ,
                                          P34 = stu.P34
      ,
                                          P35 = stu.P35
      ,
                                          P36 = stu.P36
      ,
                                          P37 = stu.P37
      ,
                                          P38 = stu.P38
      ,
                                          M11 = stu.M11
      ,
                                          Y11 = stu.Y11
      ,
                                          M12 = stu.M12
      ,
                                          Y12 = stu.Y12
      ,
                                          M13 = stu.M13
      ,
                                          Y13 = stu.Y13
      ,
                                          M14 = stu.M14
      ,
                                          Y14 = stu.Y14
      ,
                                          PM14 = stu.PM14
      ,
                                          PA14 = stu.PA14
      ,
                                          M15 = stu.M15
      ,
                                          Y15 = stu.Y15
      ,
                                          M16 = stu.M16
      ,
                                          Y16 = stu.Y16
      ,
                                          M17 = stu.M17
      ,
                                          Y17 = stu.Y17
      ,
                                          M21 = stu.M21
      ,
                                          Y21 = stu.Y21
      ,
                                          M22 = stu.M22
      ,
                                          Y22 = stu.Y22
      ,
                                          M23 = stu.M23
      ,
                                          Y23 = stu.Y23
      ,
                                          M24 = stu.M24
      ,
                                          Y24 = stu.Y24
      ,
                                          PM24 = stu.PM24
      ,
                                          PA24 = stu.PA24
      ,
                                          M25 = stu.M25
      ,
                                          Y25 = stu.Y25
      ,
                                          M26 = stu.M26
      ,
                                          Y26 = stu.Y26
      ,
                                          M27 = stu.M27
      ,
                                          Y27 = stu.Y27
      ,
                                          PM27 = stu.PM27
      ,
                                          PA27 = stu.PA27
      ,
                                          M28 = stu.M28
      ,
                                          Y28 = stu.Y28
      ,
                                          M31 = stu.M31
      ,
                                          Y31 = stu.Y31
      ,
                                          M32 = stu.M32
      ,
                                          Y32 = stu.Y32
      ,
                                          M33 = stu.M33
      ,
                                          Y33 = stu.Y33
      ,
                                          M34 = stu.M34
      ,
                                          Y34 = stu.Y34
      ,
                                          M35 = stu.M35
      ,
                                          Y35 = stu.Y35
      ,
                                          PM35 = stu.PM35
      ,
                                          PA35 = stu.PA35
      ,
                                          M36 = stu.M36
      ,
                                          Y36 = stu.Y36
      ,
                                          PM36 = stu.PM36
      ,
                                          PA36 = stu.PA36
      ,
                                          M37 = stu.M37
      ,
                                          Y37 = stu.Y37
      ,
                                          M38 = stu.M38
      ,
                                          Y38 = stu.Y38
      ,
                                          YR1 = stu.YR1
      ,
                                          YR2 = stu.YR2
      ,
                                          YR3 = stu.YR3


                                      }).ToList();

                }
                else if (Course == "BBM")
                {
                    var result = KUPOContext.sp_GetSDLCConData_TRReport_BBM().AsQueryable();
                    studentdetails = (from stu in result
                                      select new SDLCEntityCON
                                      {
                                          Code = stu.CODE
      ,
                                          HTNO = stu.HTNO
      ,
                                          P11 = stu.P11
      ,
                                          P12 = stu.P12
      ,
                                          P13 = stu.P13
      ,
                                          P14 = stu.P14
      ,
                                          P15 = stu.P15
      ,
                                          P16 = stu.P16
      ,
                                          P17 = stu.P17
      ,
                                          P18 = stu.P18
      ,
                                          P21 = stu.P21
      ,
                                          P22 = stu.P22
      ,
                                          P23 = stu.P23
      ,
                                          P24 = stu.P24
      ,
                                          P25 = stu.P25
      ,
                                          P26 = stu.P26
      ,
                                          P27 = stu.P27
      ,
                                          P28 = stu.P28
      ,
                                          P29 = stu.P29
      ,
                                          P31 = stu.P31
      ,
                                          P32 = stu.P32
      ,
                                          P33 = stu.P33
      ,
                                          P34 = stu.P34
      ,
                                          P35 = stu.P35
      ,
                                          P36 = stu.P36
      ,
                                          P37 = stu.P37
      ,
                                          P38 = stu.P38
      ,
                                          P39 = stu.P39
      ,
                                          P310 = stu.P310
      ,
                                          P311 = stu.P311
      ,
                                          M11 = stu.M11
      ,
                                          Y11 = stu.Y11
      ,
                                          M12 = stu.M12
      ,
                                          Y12 = stu.Y12
      ,
                                          M13 = stu.M13
      ,
                                          Y13 = stu.Y13
      ,
                                          M14 = stu.M14
      ,
                                          Y14 = stu.Y14
      ,
                                          M15 = stu.M15
      ,
                                          Y15 = stu.Y15
      ,
                                          M16 = stu.M16
      ,
                                          Y16 = stu.Y16
      ,
                                          M17 = stu.M17
      ,
                                          Y17 = stu.Y17
      ,
                                          M18 = stu.M18
      ,
                                          Y18 = stu.Y18
      ,
                                          M21 = stu.M21
      ,
                                          Y21 = stu.Y21
      ,
                                          M22 = stu.M22
      ,
                                          Y22 = stu.Y22
      ,
                                          M23 = stu.M23
      ,
                                          Y23 = stu.Y23
      ,
                                          M24 = stu.M24
      ,
                                          Y24 = stu.Y24
      ,
                                          M25 = stu.M25
      ,
                                          Y25 = stu.Y25
      ,
                                          M26 = stu.M26
      ,
                                          Y26 = stu.Y26
      ,
                                          M27 = stu.M27
      ,
                                          Y27 = stu.Y27
      ,
                                          M28 = stu.M28
      ,
                                          Y28 = stu.Y28
      ,
                                          PM28 = stu.PM28
      ,
                                          PA28 = stu.PA28
      ,
                                          M29 = stu.M29
      ,
                                          Y29 = stu.Y29
      ,
                                          M31 = stu.M31
      ,
                                          Y31 = stu.Y31
      ,
                                          PM31 = stu.PM31
      ,
                                          PA31 = stu.PA31
      ,
                                          M32 = stu.M32
      ,
                                          Y32 = stu.Y32
      ,
                                          M33 = stu.M33
      ,
                                          Y33 = stu.Y33
      ,
                                          M34 = stu.M34
      ,
                                          Y34 = stu.Y34
      ,
                                          M35 = stu.M35
      ,
                                          Y35 = stu.Y35
      ,
                                          M36 = stu.M36
      ,
                                          Y36 = stu.Y36
      ,
                                          M37 = stu.M37
      ,
                                          Y37 = stu.Y37
      ,
                                          M38 = stu.M38
      ,
                                          Y38 = stu.Y38
      ,
                                          M39 = stu.M39
      ,
                                          Y39 = stu.Y39
      ,
                                          M310 = stu.M310
      ,
                                          Y310 = stu.Y310
      ,
                                          M311 = stu.M311
      ,
                                          Y311 = stu.Y311
      ,
                                          YR1 = stu.YR1
      ,
                                          YR2 = stu.YR2
      ,
                                          YR3 = stu.YR3

                                      }).ToList();

                }

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
                MemoPrintDBDataContext KUPOContext = new MemoPrintDBDataContext();
                KUPOContext.CommandTimeout = 260;

                var result = KUPOContext.sp_GetSDLCResultData_TRReport(Course);

                var studentdetails = (from stu in result
                                      select new SDLCEntityPRES
                                      {
                                          HTNO = stu.HTNO,
                                          FullName = stu.FULLNAME,
                                          FName = stu.FNAME,
                                          EI = stu.EI,
                                          Year = stu.FK_YEAR,
                                          ColCode = stu.FK_CLGCODE,

                                          CLM = stu.CLM,
                                          SubjectCode = stu.SUB,
                                          order = Convert.ToInt32(stu.CLM.Substring(stu.CLM.Length - 1)),
                                          Course = stu.CRS,


                                          Marks = stu.MARKS,
                                          TotalMarks = stu.TOTAL_MARKS,

                                          Result = stu.RESULT,
                                          Part1 = stu.C_PRT1,
                                          Part2 = stu.C_PRT2

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
