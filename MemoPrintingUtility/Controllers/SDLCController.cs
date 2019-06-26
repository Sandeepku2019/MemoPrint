using MemoPrintingUtility.BO;
using MemoPrintingUtility.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MemoPrintingUtility.Controllers
{
    public class SDLCController : Controller
    {
        // GET: SDLC

        int PageBraker = 0;
        int PageNumber = 1;
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GenerateTabularSemReport(string course, string ReportType)
        {
            try
            {
                string Filepath = "";
                if (ReportType == "Tab")
                {
                    Filepath = GenerateTabular(course);

                }
                else
                {

                    var s = GenrateShortMemo(course);
                }


                return Json(Filepath, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        private string GenerateTabular(string course)
        {

            try
            {
                string fileNamedirectory = @"D:\TabularReport\";
                string filename = course + DateTime.Now.ToString("ddMMyyyy") + "_SDLC.txt";

                int rownum = 0;

                // check for Directory
                if (!Directory.Exists(fileNamedirectory))  // if it doesn't exist, create
                {
                    Directory.CreateDirectory(fileNamedirectory);
                }


                // file information 
                FileInfo fi = new FileInfo(fileNamedirectory + filename);
                // Check if file already exists. If yes, delete it.     
                if (System.IO.File.Exists(fileNamedirectory + filename))
                {
                    System.IO.File.Delete(fileNamedirectory + filename);
                }

                MemoPrintService BoMemoService = new MemoPrintService();


                var lstPREdata = BoMemoService.GetSDLCInstance().GetPRESDataforSDLC(course);
                var lstConData = BoMemoService.GetSDLCInstance().GetConDataforSDLC(course);
                var lstSubData = BoMemoService.GetSDLCInstance().GetSUBJECTforSDLC(course);

                var lstColCodes = lstPREdata.OrderBy(x => x.ColCode.Trim()).Select(y => y.ColCode.Trim()).Distinct().ToList<string>();
                using (StreamWriter sw = fi.CreateText())
                {

                    string PrevColcode = "";
                    //bool ColPagebrk = false;

                    int series = 0;
                    //foreach (var colcode in lstColCodes)
                    //{

                    List<string> HallticketNumbers = lstPREdata.OrderBy(x => x.HTNO).Select(x => x.HTNO).Distinct().ToList<string>();
                    bool isExstudent = false;
                    //ColPagebrk = true;
                    int rowcount = 0;
                    #region 
                    for (int i = 0; i < HallticketNumbers.Count; i++)
                    {

                        List<SDLC> lstEntity = new List<SDLC>();
                        string HallTIcket = HallticketNumbers[i];
                        series++;
                        int totalSUbjects = 0;
                        int passedSubject = 0;
                        bool detained = false;

                        if (i == 357 || i == 1286)
                        {


                        }

                        if (HallTIcket == "0600936851")
                        {



                        }


                        var lstPREStuns = lstPREdata.Where(x => x.HTNO == HallTIcket).ToList<SDLCEntityPRES>();
                        var lstCONStuns = lstConData.Where(x => x.HTNO == HallTIcket).ToList<SDLCEntityCON>();
                        var lstSDLCSUB = lstSubData.Where(x => x.CRS == course).ToList<SDLCEntitySUB>();

                        string Part1Marks = lstPREStuns[0].Part1;
                        string Part2Marks = lstPREStuns[0].Part2;
                        string NewC1 = lstPREStuns[0].NEW_C_PRT1;
                        string NewC2 = lstPREStuns[0].NEW_C_PRT2;

                        string NewC1R = lstPREStuns[0].NEW_Com_PRT1;
                        string NewC2R = lstPREStuns[0].NEW_Com_PRT2;

                        string Part1Div = lstPREStuns[0].Part1Div;
                        string Part2Div = lstPREStuns[0].Part2Div;


                        string FResult = string.Empty;
                        string indRes = string.Empty;
                        string indTotMarks = string.Empty;
                        string yr1R = Convert.ToString(lstConData[0].YR1);
                        string yr2R = Convert.ToString(lstConData[0].YR2);
                        string yr3R = Convert.ToString(lstConData[0].YR3);


                        string TBCom = string.Empty;

                        #region Formatting CON Data
                        foreach (var con in lstCONStuns)
                        {


                            //1year
                            int order1 = 1;



                            if (con.P11 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M11, con.Y11, con.HTNO, con.P11, "", "I", 1); order1++; }
                            if (con.P12 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M12, con.Y12, con.HTNO, con.P12, "", "I", 2); order1++; }
                            if (con.P13 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M13, con.Y13, con.HTNO, con.P13, "", "I", 3); order1++; }
                            if (con.P14 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M14, con.Y14, con.HTNO, con.P14, "", "I", 4); order1++; }
                            if (con.P15 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M15, con.Y15, con.HTNO, con.P15, "", "I", 5); order1++; }
                            if (con.P16 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M16, con.Y16, con.HTNO, con.P16, "", "I", 6); order1++; }
                            if (con.P17 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M17, con.Y17, con.HTNO, con.P17, "", "I", 7); order1++; }
                            if (con.P18 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M18, con.Y18, con.HTNO, con.P18, "", "I", 8); order1++; }
                            if (con.P19 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M19, con.Y19, con.HTNO, con.P19, "", "I", 9); order1++; }

                            if (con.PM11 != null) { lstEntity = CnrEntityVertical(lstEntity, con.PM11, con.PA11, con.HTNO, con.P11 + "(P)", "", "I", order1++); }
                            if (con.PM12 != null) { lstEntity = CnrEntityVertical(lstEntity, con.PM12, con.PA12, con.HTNO, con.P12 + "(P)", "", "I", order1++); }
                            if (con.PM14 != null) { lstEntity = CnrEntityVertical(lstEntity, con.PM14, con.PA14, con.HTNO, con.P14 + "(P)", "", "I", order1++); }
                            if (con.PM15 != null) { lstEntity = CnrEntityVertical(lstEntity, con.PM15, con.PA15, con.HTNO, con.P15 + "(P)", "", "I", order1++); }
                            if (con.PM16 != null) { lstEntity = CnrEntityVertical(lstEntity, con.PM16, con.PA16, con.HTNO, con.P16 + "(P)", "", "I", order1++); }
                            if (con.PM17 != null) { lstEntity = CnrEntityVertical(lstEntity, con.PM17, con.PA17, con.HTNO, con.P17 + "(P)", "", "I", order1++); }
                            if (con.PM18 != null) { lstEntity = CnrEntityVertical(lstEntity, con.PM18, con.PA18, con.HTNO, con.P18 + "(P)", "", "I", order1++); }
                            if (con.PM19 != null) { lstEntity = CnrEntityVertical(lstEntity, con.PM19, con.PA19, con.HTNO, con.P19 + "(P)", "", "I", order1++); }


                            //2year

                            int order2 = 1;

                            if (con.P21 == null)
                            {

                            }
                            if (con.P21 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M21, con.Y21, con.HTNO, con.P21, "", "II", 1); order2++; }
                            if (con.P22 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M22, con.Y22, con.HTNO, con.P22, "", "II", 2); order2++; }
                            if (con.P23 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M23, con.Y23, con.HTNO, con.P23, "", "II", 3); order2++; }
                            if (con.P24 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M24, con.Y24, con.HTNO, con.P24, "", "II", 4); order2++; }
                            if (con.P25 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M25, con.Y25, con.HTNO, con.P25, "", "II", 5); order2++; }
                            if (con.P26 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M26, con.Y26, con.HTNO, con.P26, "", "II", 6); order2++; }
                            if (con.P27 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M27, con.Y27, con.HTNO, con.P27, "", "II", 7); order2++; }
                            if (con.P28 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M28, con.Y28, con.HTNO, con.P28, "", "II", 8); order2++; }
                            if (con.P29 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M29, con.Y29, con.HTNO, con.P29, "", "II", 9); order2++; }

                            if (con.PM21 != null) { lstEntity = CnrEntityVertical(lstEntity, con.PM21, con.PA21, con.HTNO, con.P21 + "(P)", "", "II", order2++); }
                            if (con.PM22 != null) { lstEntity = CnrEntityVertical(lstEntity, con.PM22, con.PA22, con.HTNO, con.P22 + "(P)", "", "II", order2++); }
                            if (con.PM23 != null) { lstEntity = CnrEntityVertical(lstEntity, con.PM23, con.PA23, con.HTNO, con.P23 + "(P)", "", "II", order2++); }
                            if (con.PM24 != null) { lstEntity = CnrEntityVertical(lstEntity, con.PM24, con.PA24, con.HTNO, con.P24 + "(P)", "", "II", order2++); }
                            if (con.PM25 != null) { lstEntity = CnrEntityVertical(lstEntity, con.PM25, con.PA25, con.HTNO, con.P25 + "(P)", "", "II", order2++); }
                            if (con.PM26 != null) { lstEntity = CnrEntityVertical(lstEntity, con.PM26, con.PA26, con.HTNO, con.P26 + "(P)", "", "II", order2++); }
                            if (con.PM27 != null) { lstEntity = CnrEntityVertical(lstEntity, con.PM27, con.PA27, con.HTNO, con.P27 + "(P)", "", "II", order2++); }
                            if (con.PM28 != null) { lstEntity = CnrEntityVertical(lstEntity, con.PM28, con.PA28, con.HTNO, con.P28 + "(P)", "", "II", order2++); }
                            if (con.PM29 != null) { lstEntity = CnrEntityVertical(lstEntity, con.PM29, con.PA29, con.HTNO, con.P29 + "(P)", "", "II", order2++); }
                            //3year
                            int order3 = 1;

                            if (con.P31 == null)
                            {

                            }

                            if (con.P31 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M31, con.Y31, con.HTNO, con.P31, "", "III", 1); order3++; }
                            if (con.P32 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M32, con.Y32, con.HTNO, con.P32, "", "III", 2); order3++; }
                            if (con.P33 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M33, con.Y33, con.HTNO, con.P33, "", "III", 3); order3++; }
                            if (con.P34 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M34, con.Y34, con.HTNO, con.P34, "", "III", 4); order3++; }
                            if (con.P35 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M35, con.Y35, con.HTNO, con.P35, "", "III", 5); order3++; }
                            if (con.P36 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M36, con.Y36, con.HTNO, con.P36, "", "III", 6); order3++; }
                            if (con.P37 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M37, con.Y37, con.HTNO, con.P37, "", "III", 7); order3++; }
                            if (con.P38 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M38, con.Y38, con.HTNO, con.P38, "", "III", 8); order3++; }
                            if (con.P39 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M39, con.Y39, con.HTNO, con.P39, "", "III", 9); order3++; }
                            if (con.P310 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M310, con.Y310, con.HTNO, con.P310, "", "III", 10); order3++; }
                            if (con.P311 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M311, con.Y311, con.HTNO, con.P311, "", "III", 11); order3++; }

                            if (con.PM31 != null) { lstEntity = CnrEntityVertical(lstEntity, con.PM31, con.PA31, con.HTNO, con.P31 + "(P)", "", "III", order3++); }
                            if (con.PM32 != null) { lstEntity = CnrEntityVertical(lstEntity, con.PM32, con.PA32, con.HTNO, con.P32 + "(P)", "", "III", order3++); }
                            if (con.PM33 != null) { lstEntity = CnrEntityVertical(lstEntity, con.PM33, con.PA33, con.HTNO, con.P33 + "(P)", "", "III", order3++); }
                            if (con.PM34 != null) { lstEntity = CnrEntityVertical(lstEntity, con.PM34, con.PA34, con.HTNO, con.P34 + "(P)", "", "III", order3++); }
                            if (con.PM35 != null) { lstEntity = CnrEntityVertical(lstEntity, con.PM35, con.PA35, con.HTNO, con.P35 + "(P)", "", "III", order3++); }
                            if (con.PM36 != null) { lstEntity = CnrEntityVertical(lstEntity, con.PM36, con.PA36, con.HTNO, con.P36 + "(P)", "", "III", order3++); }
                            if (con.PM37 != null) { lstEntity = CnrEntityVertical(lstEntity, con.PM37, con.PA37, con.HTNO, con.P37 + "(P)", "", "III", order3++); }
                            if (con.PM38 != null) { lstEntity = CnrEntityVertical(lstEntity, con.PM38, con.PA38, con.HTNO, con.P38 + "(P)", "", "III", order3++); }
                            if (con.PM39 != null) { lstEntity = CnrEntityVertical(lstEntity, con.PM39, con.PA39, con.HTNO, con.P39 + "(P)", "", "III", order3++); }
                            if (con.PM310 != null) { lstEntity = CnrEntityVertical(lstEntity, con.PM310, con.PA310, con.HTNO, con.P310 + "(P)", "", "III", order3++); }
                            if (con.PM311 != null) { lstEntity = CnrEntityVertical(lstEntity, con.PM311, con.PA311, con.HTNO, con.P311 + "(P)", "", "III", order3++); }

                        }


                        if (lstEntity.Where(x => x.Year == "III" && x.Type == "CONS.").ToList().Count < 6)
                        {
                            var test = lstEntity.Where(x => x.Year == "III" && x.Type == "CONS.").ToList();
                        }
                        #endregion
                        string[] yrs = new string[] { "I", "II", "III" };
                        string[] Types = new string[] { "CONS.", "PRES." };

                        // PRES DATA 
                        lstEntity = CnrEntityVertical(lstEntity, lstPREStuns);




                        int ry = 0;
                        int Part1Total = 0;
                        int Part2Total = 0;




                        foreach (string yr in yrs)
                        {

                            if (yr == "I")
                            { ry = 1; }

                            if (yr == "II")
                            { ry = 2; }

                            if (yr == "III")
                            { ry = 3; }
                            string result = string.Empty;



                            var lstSUB = lstSDLCSUB.Where(x => x.YR == ry.ToString()).ToList();

                            int CONcOUNT = 0;
                            int PRESCOUNT = 0;


                            var CnNR = lstEntity.Where(x => x.Type == "CONS." && x.Year == yr).ToList();
                            var PNR = lstEntity.Where(x => x.Type == "PRES." && x.Year == yr).ToList();





                            foreach (var cn in CnNR)
                            {


                                if (!cn.subjectCode.Contains("(P)"))
                                {
                                    var SUB = lstSUB.Where(x => x.SCODE == cn.subjectCode).ToList().FirstOrDefault();
                                    if (SUB != null && SUB.MNMR.ChangeINT() > cn.Marks.ChangeINT())
                                    {

                                        CONcOUNT++;
                                    }
                                }
                                else
                                {

                                    var SUB = lstSUB.Where(x => x.SCODE == cn.subjectCode.Replace("(P)", "")).ToList().FirstOrDefault();
                                    if (SUB != null && SUB.PMNMR.ChangeINT() > cn.Marks.ChangeINT())
                                    {

                                        CONcOUNT++;
                                    }
                                }

                            }


                            foreach (var cn in PNR)
                            {

                                if (!cn.subjectCode.Contains("(P)"))
                                {
                                    var SUB = lstSUB.Where(x => x.SCODE == cn.subjectCode).ToList().FirstOrDefault();
                                    if (SUB != null && SUB.MNMR.ChangeINT() <= cn.Marks.ChangeINT())
                                    {
                                        PRESCOUNT++;
                                    }
                                }
                                else
                                {
                                    var SUB = lstSUB.Where(x => x.SCODE == cn.subjectCode.Replace("(P)", "")).ToList().FirstOrDefault();
                                    if (SUB != null && SUB.PMNMR.ChangeINT() <= cn.Marks.ChangeINT())
                                    {

                                        PRESCOUNT++;
                                    }
                                }

                            }

                            if (PNR.Where(x => x.subjectCode.ToLower() == "tel" || x.subjectCode.ToLower() == "hin" || x.subjectCode.ToLower() == "urd" || x.subjectCode.ToLower() == "eng" || x.subjectCode.ToLower() == "san").ToList().Count() > 1)
                            {
                                if (Part1Div != "PASS")
                                {
                                    NewC1 = "COMP";
                                }
                            }

                            if (CONcOUNT != PRESCOUNT)
                            {
                                if (yr == "I")
                                { yr1R = "FAILED"; }

                                if (yr == "II")
                                { yr2R = "FAILED"; }

                                if (yr == "III")
                                { yr3R = "FAILED"; }
                            }
                            if (CONcOUNT == PRESCOUNT)
                            {
                                if (yr == "I" && CnNR.Count > 0)
                                { yr1R = "COMPLETED"; }

                                if (yr == "II" && CnNR.Count > 0)
                                { yr2R = "COMPLETED"; }

                                if (yr == "III" && CnNR.Count > 0)
                                { yr3R = "COMPLETED"; }
                            }


                        }
                        string FinalResult = string.Empty;
                        if (yr1R == "FAILED" || yr2R == "FAILED" || yr3R == "FAILED")
                        {
                            FResult = "FAILED";
                        }


                        if (yr1R == "COMPLETED" && yr2R == "COMPLETED" && yr3R == "COMPLETED")
                        {
                            FResult = "COMPLETED";
                        }




                        if (lstPREStuns[0].FResult != null)
                        {
                            if (lstPREStuns[0].FResult.ToLower() == "c")
                            {
                                FinalResult = "COMPLETED";
                            }

                            if (lstPREStuns[0].FResult.ToLower() == "f")
                            {
                                FinalResult = "FAILED";

                            }
                        }


                        string FN = lstPREStuns[0].FName == null ? "" : lstPREStuns[0].FName;
                        string SN = lstPREStuns[0].FullName == null ? "" : lstPREStuns[0].FullName;
                        string CC = lstPREStuns[0].ColCode == null ? "" : lstPREStuns[0].ColCode;
                        string EI = lstPREStuns[0].EI == null ? "" : lstPREStuns[0].EI;
                        string nameformat = series + GetSpaces(5 - series.ToString().Length) + GetSpaces(1) + CC + GetSpaces(5 - CC.Length) + HallTIcket + GetSpaces(12 - HallTIcket.Length) + SN + GetSpaces(45 - SN.Length) + FN + GetSpaces(45 - FN.Length) + EI + GetSpaces(7 - EI.Length) + HallTIcket + GetSpaces(10 - HallTIcket.Length);






                        if (i == 0)
                        {
                            rowcount = addHeaderFooter(sw, 72, course, false, 72);
                            //rowcount++;
                            sw.WriteLine(nameformat);
                            rowcount = (rowcount + 1) >= 68 ? addHeaderFooter(sw, 72, course, false, rowcount + 2) : (rowcount + 1);
                        }
                        else
                        {



                            sw.WriteLine(nameformat);
                            rowcount = (rowcount + 1) >= 68 ? addHeaderFooter(sw, 72, course, false, rowcount + 2) : (rowcount + 1);
                        }



                        foreach (string yr in yrs)
                        {

                            var WallChecCon = lstEntity.Where(x => x.Year == yr && x.Type == "CONS." && x.Marks == null).ToList();
                            var WallChecPres = lstEntity.Where(x => x.Year == yr && x.Type == "PRES." && x.Marks == null).ToList();
                            if (WallChecCon.Count > WallChecPres.Count)
                            {
                                if (yr == "I")
                                { yr1R = "WALL"; }

                                if (yr == "II")
                                { yr2R = "WALL"; }

                                if (yr == "III")
                                { yr3R = "WALL"; }
                            }
                            #region Printing 
                            string result = string.Empty;
                            foreach (string ty in Types)
                            {
                                int subord = 1;
                                int count = 0;
                                var lstpopulaateData = lstEntity.Where(x => x.Year == yr && x.Type == ty && x.subjectCode != null).OrderBy(y => y.Type).ThenBy(z => z.Order).ToList();

                                string subjectstring = string.Empty;
                                string SubjectMarks_U = string.Empty;
                                string subjectsMarksPRE_U = string.Empty;
                                string MarkformatPRE = GetSpaces(10);
                                string Aademicstatus = string.Empty;
                                subjectsMarksPRE_U = subjectsMarksPRE_U + MarkformatPRE;

                                string SubjectMarks_S = string.Empty;
                                string subjectsMarksPRE_S = string.Empty;
                                string subjectformt = "";

                                if (lstpopulaateData.Count() > 0)
                                {
                                    subjectstring = ty + GetSpaces(5 - ty.Length) + yr + GetSpaces(5 - yr.Length);
                                }

                                bool flag = false;
                                bool flag1 = false;
                                bool abflag = false;
                                bool subcount1 = false;
                                if (lstpopulaateData.Count() > 0)
                                {
                                    if (lstpopulaateData.OrderBy(z => z.Order).ToList()[0].Order > 1 && lstpopulaateData.ToList()[0].Type == "PRES.")
                                    {

                                        flag = true;

                                        if (lstpopulaateData.Where(z => z.Marks == "AB").ToList().Count > 0)
                                        {
                                            abflag = true;
                                        }
                                    }

                                    if (lstpopulaateData.OrderBy(z => z.Order).ToList()[0].Order > 1 && lstpopulaateData.ToList()[0].Type == "PRES." && lstpopulaateData.OrderBy(z => z.Order).ToList().Count > 1)
                                    {
                                        flag1 = true;
                                    }
                                }
                                if (lstpopulaateData.Where(x => x.Type == "PRES.").ToList().Count() == 1)
                                {
                                    subcount1 = true;
                                }
                                bool isprac = false;
                                int PSOrder = 0;
                                var pracStratOrder = lstpopulaateData.Where(x => x.subjectCode.Contains("(P)")).OrderBy(z => z.Order).ToList();
                                if (pracStratOrder != null && pracStratOrder.Count > 0)
                                {
                                    PSOrder = pracStratOrder[0].Order;
                                }

                                foreach (var sd in lstpopulaateData.OrderBy(z => z.Order))
                                {



                                    string spaces = "";
                                    string umakspace = "";
                                    string intmarkspae = "";
                                    int ordr = sd.Order;


                                    if (PSOrder > sd.Order && PSOrder != 0)
                                    {
                                        spaces = GetSpaces((ordr - subord) * 8);
                                        umakspace = GetSpaces((ordr - subord) * 8);
                                    }

                                    if (PSOrder < sd.Order && PSOrder != 0)
                                    {
                                        spaces = GetSpaces((ordr - subord) * 11);
                                        umakspace = GetSpaces((ordr - subord) * 11);
                                    }

                                    if (PSOrder == 0)
                                    {
                                        spaces = GetSpaces((ordr - subord) * 8);
                                        umakspace = GetSpaces((ordr - subord) * 8);
                                    }

                                    if (PSOrder == sd.Order && PSOrder != 0)
                                    {
                                        spaces = GetSpaces((ordr - subord) * 8);
                                        umakspace = GetSpaces((ordr - subord) * 8);
                                        spaces = spaces + GetSpaces(3);
                                    }

                                    if (flag == true)
                                    {
                                        spaces = spaces + GetSpaces(4);

                                        umakspace = umakspace + GetSpaces(4);
                                        umakspace = umakspace + GetSpaces(1);
                                        if (flag1 == true)
                                        {
                                            umakspace = umakspace + GetSpaces(1);
                                            flag1 = false;
                                        }
                                        flag = false;

                                        if (abflag == true)
                                        {

                                            umakspace = umakspace + GetSpaces(0);
                                            abflag = false;
                                        }
                                        if (subcount1 == true)
                                        {
                                            umakspace = umakspace + GetSpaces(1);
                                            subcount1 = false;
                                        }
                                    }

                                    string re = string.Empty;
                                    if (sd.Result != null)
                                    {
                                        re = sd.Result + " ";
                                    }

                                    string SDC = string.Empty;
                                    string SubCode = string.Empty;

                                    string Academic = string.Empty;
                                    if (sd.Academic != null)
                                    {
                                        Academic = sd.Academic;
                                    }

                                    if (sd.subjectCode != null)
                                    {

                                        SDC = sd.subjectCode;
                                        SubCode = SDC + " " + Academic;
                                        //isprac = false;


                                    }


                                    string Marks = string.Empty;
                                    if (sd.Marks != null)
                                    {
                                        if (sd.Marks.Length == 2)
                                        {
                                            Marks = " " + sd.Marks;
                                        }
                                        else
                                        {
                                            Marks = sd.Marks;
                                        }
                                    }





                                    subjectformt = GetSpaces(spaces.Length - SubCode.Length);
                                    subjectstring = subjectstring + subjectformt + SubCode;

                                    umakspace = GetSpaces(umakspace.Length - (Marks.Length + Convert.ToString(re).Length + 1));
                                    if (isprac == false)
                                    {
                                        SubjectMarks_U = umakspace + Marks.ToString().Truncate(3) + " " + re;
                                    }
                                    else
                                    {
                                        if (subord != ordr)
                                        {
                                            SubjectMarks_U = Marks.ToString().Truncate(3) + " " + re;
                                            isprac = false;
                                        }

                                    }

                                    subjectsMarksPRE_U = subjectsMarksPRE_U + SubjectMarks_U;
                                    subord = ordr;


                                }

                                if (yr == "I")
                                {
                                    indRes = yr1R;
                                    var INDMX = lstPREStuns.Where(x => x.Year == "1").ToList(); foreach (var indTM in INDMX) { indTotMarks = indTM.TotalMarks.ChangeINT().ToString(); }
                                }
                                if (yr == "II")
                                {
                                    indRes = yr2R;
                                    var INDMX = lstPREStuns.Where(x => x.Year == "2").ToList(); foreach (var indTM in INDMX) { indTotMarks = indTM.TotalMarks.ChangeINT().ToString(); }
                                }
                                if (yr == "III")
                                {
                                    indRes = yr3R;
                                    var INDMX = lstPREStuns.Where(x => x.Year == "3").ToList(); foreach (var indTM in INDMX) { indTotMarks = indTM.TotalMarks.ChangeINT().ToString(); }
                                }


                                if (yr1R == "FAILED" && yr2R == "COMPLETED" && yr3R == "COMPLETED")
                                {
                                    TBCom = "I YEAR TO COMPLETE";
                                }

                                else if (yr1R == "COMPLETED" && yr2R == "FAILED" && yr3R == "COMPLETED")
                                {
                                    TBCom = "II YEAR TO COMPLETE";
                                }
                                else if (yr1R == "FAILED" && yr2R == "FAILED" && yr3R == "COMPLETED")
                                {
                                    TBCom = "I & II YEARS TO COMPLETE";
                                }
                                else { TBCom = "C"; }



                                if (lstpopulaateData.Count() > 0)
                                {



                                    if (ty == "PRES.")
                                    {
                                        sw.WriteLine(subjectstring + GetSpaces(100 - subjectstring.Length) + result + GetSpaces(13 - result.Length));
                                        ////rowcount = (rowcount + 1) >= 72 ? addHeaderFooter(sw, 72, course, false, rowcount) : (rowcount + 1);
                                        sw.WriteLine(subjectsMarksPRE_U + GetSpaces(113 - subjectsMarksPRE_U.Length));
                                        // rowcount = (rowcount + 1) >= 72 ? addHeaderFooter(sw, 72, course, false, rowcount) : (rowcount + 1);
                                        sw.WriteLine("          ....................................................................................................  " + GetSpaces(3 - (indTotMarks.Length)) + indTotMarks + "     " + indRes);
                                        rowcount = (rowcount + 3) >= 68 ? addHeaderFooter(sw, 72, course, false, rowcount + 2) : (rowcount + 3);
                                    }
                                    else
                                    {

                                        sw.WriteLine("");
                                        sw.WriteLine(subjectstring + GetSpaces(100 - subjectstring.Length) + result + GetSpaces(13 - result.Length));
                                        //rowcount = (rowcount + 1) >= 72 ? addHeaderFooter(sw, 72, course, false, rowcount) : (rowcount + 1);
                                        sw.WriteLine(subjectsMarksPRE_U + GetSpaces(113 - subjectsMarksPRE_U.Length));
                                        //sw.WriteLine("          ..........................................................................................................................");
                                        rowcount = (rowcount + 3) >= 68 ? addHeaderFooter(sw, 72, course, false, rowcount + 2) : (rowcount + 3);
                                    }
                                }
                            }

                            #endregion
                            //sw.WriteLine("          ..........................................................................................................................");
                        }


                        #region Calc. part1 and Part2 calc
                        int p1subjectcout = 0;
                        int countP1f = 0;
                        foreach (string yr in yrs)
                        {

                            List<SDLC> CnNRTotal = new List<SDLC>();
                            CnNRTotal = lstEntity.Where(x => x.Type == "CONS." && x.Year == yr).ToList();

                            var PNR = lstEntity.Where(x => x.Type == "PRES." && x.Year == yr).ToList();

                            int p1total = 0;
                            int p2total = 0;


                            foreach (var PTot in PNR)
                            {
                                var ob = CnNRTotal.Where(x => x.subjectCode == PTot.subjectCode).ToList();
                                if (ob != null && ob.Count > 0)
                                {
                                    if (ob[0].Marks.ChangeINT() <= PTot.Marks.ChangeINT())
                                    {
                                        ob[0].Marks = PTot.Marks;
                                    }
                                }

                            }

                            var p1set = CnNRTotal.Where(x => x.subjectCode.ToLower() == "tel" || x.subjectCode.ToLower() == "hin" || x.subjectCode.ToLower() == "urd" || x.subjectCode.ToLower() == "eng" || x.subjectCode.ToLower() == "san").ToList();
                            p1total = p1set.Sum(y => y.Marks.ChangeINT());
                            p1subjectcout = p1subjectcout + p1set.Count();
                            countP1f = countP1f + p1set.Where(x => x.Marks.ChangeINT() < 35).ToList().Count;


                            p2total = CnNRTotal.Where(x => x.subjectCode.ToLower() != "tel" && x.subjectCode.ToLower() != "hin" && x.subjectCode.ToLower() != "urd" && x.subjectCode.ToLower() != "eng" && x.subjectCode.ToLower() != "san" && x.subjectCode.ToLower() != "ihc" && x.subjectCode.ToLower() != "scn" && x.subjectCode.ToLower() != "est").Sum(y => y.Marks.ChangeINT());


                            Part1Total = Part1Total + p1total;
                            Part2Total = Part2Total + p2total;
                        }

                        #endregion



                        string StrPartRes1 = string.Empty;
                        string StrPartRes2 = string.Empty;
                        string NewCom1 = string.Empty;
                        string NewCom2 = string.Empty;
                        //if (NewC1 == null) { NewCom1 = string.Empty; } else { NewCom1 = NewC1 + "/"; }
                        if (NewC2 == null) { NewCom2 = string.Empty; } else { NewCom2 = NewC2 + "/"; }

                        if (yr1R == "FAILED" || yr2R == "FAILED" || yr3R == "FAILED")
                        {
                            Part1Div = null;
                            Part2Div = null;
                        }

                        if (yr1R == "WALL" || yr2R == "WALL" || yr3R == "WALL")
                        {
                            FResult = "WALL";
                        }

                        if (countP1f == 0 && p1subjectcout == 4)
                        {
                            if (Part1Total >= 140 && Part1Total < 191)
                            {
                                Part1Div = "PASS";
                            }

                            if (Part1Total >= 192 && Part1Total <= 239)
                            {
                                Part1Div = "SECOND";
                            }

                            if (Part1Total >= 240 && Part1Total <= 279)
                            {
                                Part1Div = "FIRST";
                            }

                            if (Part1Total >= 280 && Part1Total <= 400)
                            {
                                Part1Div = "DISTN.";
                            }
                        }



                        if (Part1Div != null)
                        {
                            if (Part1Div == "PASS")
                            {
                                StrPartRes1 = NewCom1 + Part1Div + GetSpaces(15);
                            }
                            else
                            {
                                if (NewC1 == "COMP" || NewC1R == "C" || NewC2R == "C")
                                {
                                    NewCom1 = "COMPLETED  /  ";
                                }
                                StrPartRes1 = NewCom1 + Part1Div + GetSpaces(15);
                            }
                        }
                        else { StrPartRes1 = "- -" + GetSpaces(25); }
                        if (TBCom != "C")
                        {
                            StrPartRes2 = TBCom;
                        }
                        else
                        {
                            if (Part2Div != null)
                            {
                                StrPartRes2 = NewCom2 + Part2Div + GetSpaces(15);
                            }
                            else { StrPartRes2 = "- -" + GetSpaces(15); }

                        }

                        if (yr1R == "WALL" || yr2R == "WALL" || yr3R == "WALL")
                        {
                            FResult = "WALL";
                            StrPartRes1 = "---";
                            StrPartRes2 = "---";
                        }

                        sw.WriteLine("            *******PART 1: " + Part1Total + GetSpaces(5) + StrPartRes1 + "********* Part 2: " + Part2Total + GetSpaces(5) + StrPartRes2);
                        sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------");
                        rowcount = (rowcount + 2) >= 68 ? addHeaderFooter(sw, 72, course, false, rowcount + 1) : (rowcount + 2);


                    }
                    #endregion
                    //}
                }
                return fileNamedirectory + filename;
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }


        private string GetSpaces(int totalLength)
        {
            string result = string.Empty;
            for (int i = 0; i < totalLength; i++)
            {
                result += " ";
            }
            return result;
        }

        private List<SDLCSUB> CnrEntitySUB(List<SDLCSUB> lstSUBEntity, string YR, string SCODE, string OCODE, string MXMR, string MNMR, string PMXMR, string PMNMR, string CRS)
        {
            lstSUBEntity.Add(new SDLCSUB() { YR = YR, SCODE = SCODE, MXMR = MXMR, MNMR = MNMR, PMXMR = PMXMR, PMNMR = PMNMR, CRS = CRS });
            return lstSUBEntity;
        }

        private List<SDLC> CnrEntityVertical(List<SDLC> lstEntity, string Marks, string Academic, string HTNO, string SubjectCode, string EI, string year, int order)
        {
            lstEntity.Add(new SDLC() { HTNO = HTNO, Marks = Marks, Academic = Academic, subjectCode = SubjectCode, Order = order, Year = year, Type = "CONS." });
            return lstEntity;
        }


        private List<SDLC> CnrEntityVertical(List<SDLC> lstEntity, List<SDLCEntityPRES> lstPRES)
        {
            foreach (var pre in lstPRES)
            {
                string YearS = string.Empty;
                bool flag = false;
                if (pre.CLM.Contains("PM"))
                {
                    YearS = pre.CLM.Substring(2, 1);
                    flag = true;
                }
                else
                {
                    YearS = pre.CLM.Substring(1, 1);
                }

                string YS = string.Empty;

                if (YearS == "1")
                {
                    YS = "I";
                }


                if (YearS == "2")
                {
                    YS = "II";
                }


                if (YearS == "3")
                {
                    YS = "III";
                }
                int orse = 0;
                string subcode = string.Empty;
                if (flag == true)
                {
                    var a = lstEntity.Where(x => x.subjectCode == pre.SubjectCode + "(P)" && x.Year == YS && x.Type == "CONS.").ToList();
                    if (a != null && a.Count > 0)
                    {
                        orse = a[0].Order;
                    }
                    else
                    {
                        if (lstEntity.Where(x => x.Year == YS && x.Type == "PRES.").ToList().Count > 0)
                        {
                            orse = lstEntity.Where(x => x.Year == YS && x.Type == "PRES.").Max(s => s.Order) + 1;
                        }
                        else
                        {
                            orse = lstEntity.Where(x => x.Year == YS && x.Type == "CONS.").ToList().Count + 1;
                        }
                    }

                    subcode = pre.SubjectCode + "(P)";
                }
                else
                {
                    int a = pre.CLM.Length - pre.CLM.Substring(0, 2).Length;
                    orse = Convert.ToInt32(pre.CLM.Substring(2, a));
                    subcode = pre.SubjectCode;
                }
                lstEntity.Add(new SDLC()
                {
                    HTNO = pre.HTNO,
                    Marks = pre.Marks,
                    Academic = null,
                    subjectCode = subcode,
                    Order = Convert.ToInt32(orse),
                    Year = YS,
                    Type = "PRES.",
                    EI = pre.EI,
                    Result = pre.Result

                });
            }
            return lstEntity;
        }


        private JsonResult GenrateShortMemo(string course)
        {


            string hn = "";
            try
            {

                ///// Creting Notepad 
                //string fileNamedirectory = Server.MapPath("/Tab");
                string fileNamedirectory = @"D:\ShortMemo\";
                string filename = course + DateTime.Now.ToString("ddMMyyyy") + "_SDLC.txt";

                // check for Directory
                if (!Directory.Exists(fileNamedirectory))  // if it doesn't exist, create
                {
                    Directory.CreateDirectory(fileNamedirectory);
                }


                // file information 
                FileInfo fi = new FileInfo(fileNamedirectory + filename);
                // Check if file already exists. If yes, delete it.     
                if (System.IO.File.Exists(fileNamedirectory + filename))
                {
                    System.IO.File.Delete(fileNamedirectory + filename);
                }

                MemoPrintService BoMemoService = new MemoPrintService();

                string Syear = string.Empty;
                var lstStudentsPre = BoMemoService.GetSDLCInstance().GetPRESDataforSDLC(course);
                var lstSubData = BoMemoService.GetSDLCInstance().GetSUBJECTforSDLC(course);



                // Create a new file     
                using (StreamWriter sw = fi.CreateText())
                {
                    for (int yr = 1; yr <= 3; yr++)
                    {


                        if (yr == 1)
                        {
                            Syear = "I";
                        }


                        if (yr == 2)
                        {
                            Syear = "II";
                        }


                        if (yr == 3)
                        {
                            Syear = "III";
                        }

                        var lstYrStudent = lstStudentsPre.Where(y => y.Year == yr.ToString()).ToList<SDLCEntityPRES>();
                        List<string> HallticketNumbers = lstYrStudent.OrderBy(x => x.HTNO).Select(x => x.HTNO).Distinct().ToList<string>();


                        // Making even number for  side by side print 
                        if (HallticketNumbers.Count % 2 == 0)
                        {

                        }
                        else
                        {
                            HallticketNumbers.Add("0");
                        }

                        for (int i = 0; i < HallticketNumbers.Count; i++)
                        {

                            hn = HallticketNumbers[i];
                            // var lstStudents = lstYrStudent.Where(x => x.HTNO == hn).ToList();




                            if (i == 0)
                            {
                                sw.WriteLine("");
                                sw.WriteLine("");
                                sw.WriteLine("");
                            }
                            var lstStunsfirst = lstYrStudent.Where(x => x.HTNO == HallticketNumbers[i]).OrderBy(c => c.CLM).ToList<SDLCEntityPRES>();
                            var lstStubScon = lstYrStudent.Where(x => x.HTNO == HallticketNumbers[i + 1]).OrderBy(c => c.CLM).ToList<SDLCEntityPRES>();


                            if (lstStunsfirst == null || lstStunsfirst.Count == 0)
                            { lstStunsfirst = new List<SDLCEntityPRES>(); lstStunsfirst.Add(new SDLCEntityPRES() { SubjectName = "", Marks = "", Result = "", SubjectCode = "" }); }
                            if (lstStubScon == null || lstStubScon.Count == 0)
                            {
                                lstStubScon = new List<SDLCEntityPRES>();
                                lstStubScon.Add(new SDLCEntityPRES() { SubjectName = "", Marks = "", Result = "", SubjectCode = "" });
                            }


                            string gap = GetSpaces(5);
                            string HallTicket_1 = lstStunsfirst[0].HTNO == null ? "" : lstStunsfirst[0].HTNO;
                            string FN_1 = lstStunsfirst[0].FName == null ? "" : lstStunsfirst[0].FName;
                            string SN_1 = lstStunsfirst[0].FullName == null ? "" : lstStunsfirst[0].FullName;
                            string CC_1 = lstStunsfirst[0].ColCode == null ? "" : lstStunsfirst[0].ColCode;

                            string CourseDetails = GetSpaces(10) + course + " " + Syear + " YR. SUPPLY." + (DateTime.Now.Year - 1).ToString();
                            CourseDetails = CourseDetails + GetSpaces(55 - CourseDetails.Length) + DateTime.Now.ToString("dd-MM-yyyy");


                            string HallTicket_2 = lstStubScon[0].HTNO == null ? "" : lstStubScon[0].HTNO;
                            string FN_2 = lstStubScon[0].FName == null ? "" : lstStubScon[0].FName;
                            string SN_2 = lstStubScon[0].FullName == null ? "" : lstStubScon[0].FullName;
                            string CC_2 = lstStubScon[0].ColCode == null ? "" : lstStubScon[0].ColCode;

                            string CondidateRow = GetSpaces(10) + SN_1 + GetSpaces(55 - SN_1.Length) + gap + GetSpaces(10) + SN_2 + GetSpaces(55 - SN_2.Length);
                            string FatherRow = GetSpaces(10) + FN_1 + GetSpaces(45 - FN_1.Length) + HallTicket_1 + GetSpaces(10 - HallTicket_1.Length) + gap;
                            FatherRow = FatherRow + GetSpaces(10) + FN_2 + GetSpaces(45 - FN_2.Length) + HallTicket_2 + GetSpaces(10 - HallTicket_2.Length);

                            string ColCodeRow = CC_1 + GetSpaces(65 - CC_1.Length) + gap + CC_2 + GetSpaces(65 - CC_2.Length);

                            if (HallTicket_1 == "060006585" || HallTicket_2 == "060006585")
                            {

                            }


                            sw.WriteLine(ColCodeRow);
                            string CourseRows = CourseDetails + gap + CourseDetails;
                            sw.WriteLine(CourseRows);
                            sw.WriteLine(CondidateRow);
                            sw.WriteLine(FatherRow);


                            sw.WriteLine("");
                            sw.WriteLine("");

                            int rowCount = 0;
                            if (lstStunsfirst.Count > lstStubScon.Count)
                            {
                                rowCount = lstStunsfirst.Count;
                            }
                            else
                            {
                                rowCount = lstStubScon.Count;
                            }

                            int memorows = 0;
                            for (int j = 0; j < 19; j++)
                            {
                                if (memorows < 17)
                                {
                                    if (lstStunsfirst.Count - 1 < j)
                                    {
                                        lstStunsfirst.Add(new SDLCEntityPRES() { SubjectCode = "", SubjectName = "", Marks = "", Result = "", CLM = "" });
                                    }
                                    if (lstStubScon.Count - 1 < j)
                                    {
                                        lstStubScon.Add(new SDLCEntityPRES() { SubjectCode = "", SubjectName = "", Marks = "", Result = "", CLM = "" }); memorows++;
                                    }




                                    var sub1 = lstSubData.Where(x => x.SCODE == lstStunsfirst[j].SubjectCode && x.YR == lstStunsfirst[j].Year && x.CRS == course).ToList();
                                    var sub2 = lstSubData.Where(x => x.SCODE == lstStubScon[j].SubjectCode && x.YR == lstStubScon[j].Year && x.CRS == course).ToList();

                                    if (sub1 != null && sub1.Count > 0)
                                    {
                                        lstStunsfirst[j].SubjectName = sub1[0].OCODE;
                                    }
                                    else if (sub1.Count == 0 && course == "BCM")
                                    {
                                        sub1 = lstSubData.Where(x => x.SCODE == lstStunsfirst[j].SubjectCode && x.YR == lstStunsfirst[j].Year && x.CRS == "COM").ToList();
                                        if (sub1 != null && sub1.Count > 0)
                                        {
                                            lstStunsfirst[j].SubjectName = sub1[0].OCODE;
                                        }
                                    }
                                    else
                                    {
                                        lstStunsfirst[j].SubjectName = "";
                                    }







                                    if (sub2 != null && sub2.Count > 0)
                                    {
                                        lstStubScon[j].SubjectName = sub2[0].OCODE;  
                                    }
                                    else if (sub2.Count == 0 && course == "BCM")
                                    {
                                        sub2 = lstSubData.Where(x => x.SCODE == lstStubScon[j].SubjectCode && x.YR == lstStubScon[j].Year && x.CRS == "COM").ToList();
                                        if (sub2 != null && sub2.Count > 0)
                                        {
                                            lstStubScon[j].SubjectName = sub2[0].OCODE;
                                        }
                                    }
                                    else
                                    {
                                        lstStubScon[j].SubjectName = "";
                                    }

                                    if (lstStunsfirst[j].SubjectCode.Length < 55 && lstStubScon[j].SubjectCode.Length < 55)
                                    {
                                        SingleLingSubject(sw, j, lstStunsfirst, lstStubScon, lstSubData, yr.ToString(), course);
                                    }
                                    else
                                    {
                                        //Twolinesubject(sw, j, lstStunsfirst, lstStubScon);
                                        //memorows++;
                                        //memorows++;

                                    }
                                }

                            }

                            string SGPARow = string.Empty;
                            //first record
                            string FirstFinalResult = string.Empty;
                            if (lstStunsfirst.Where(x => x.Result == "FAIL" || x.Result == "AB").ToList().Count > 0)
                            {
                                FirstFinalResult = "FAILED";
                            }
                            else
                            {
                                FirstFinalResult = "PASSED";
                            }


                            if (FirstFinalResult == "PASSED" || FirstFinalResult == "COMPLETED" || FirstFinalResult == "PROMOTED")
                            {
                                string sg = lstStunsfirst.Sum(z => z.Marks.ChangeINT()).ToString();
                                SGPARow = GetSpaces(10) + sg + GetSpaces(55 - sg.Length);
                            }
                            else
                            {
                                string sg = lstStunsfirst.Sum(z => z.Marks.ChangeINT()).ToString();
                                SGPARow = GetSpaces(10) + sg + GetSpaces(55 - sg.Length);
                            }
                            SGPARow = SGPARow + gap;


                            //Second recod
                            string SecondFinalResult = string.Empty;
                            if (lstStubScon.Where(x => x.Result == "FAIL" || x.Result == "AB").ToList().Count > 0)
                            {
                                SecondFinalResult = "FAILED";
                            }
                            else
                            {
                                SecondFinalResult = "PASSED";
                            }

                            if (SecondFinalResult == "PASSED" || SecondFinalResult == "COMPLETED" || SecondFinalResult == "PROMOTED")
                            {
                                string sg1 = lstStubScon.Sum(y => y.Marks.ChangeINT()).ToString();
                                SGPARow = SGPARow + GetSpaces(10) + sg1 + GetSpaces(55 - sg1.Length);
                            }
                            else
                            {
                                //SGPARow = SGPARow + GetSpaces(10) + "***" + GetSpaces(55 - "***".Length);
                                string sg1 = lstStubScon.Sum(y => y.Marks.ChangeINT()).ToString();
                                SGPARow = SGPARow + GetSpaces(10) + sg1 + GetSpaces(55 - sg1.Length);
                            }

                            sw.WriteLine(SGPARow);



                            string statusrow = GetSpaces(10) + FirstFinalResult + GetSpaces(55 - FirstFinalResult.Length) + gap;
                            statusrow = statusrow + GetSpaces(10) + SecondFinalResult + GetSpaces(55 - SecondFinalResult.Length);

                            sw.WriteLine(statusrow);
                            sw.WriteLine("");
                            sw.WriteLine("");
                            sw.WriteLine("");
                            sw.WriteLine("");


                            sw.WriteLine("");
                            sw.WriteLine("");
                            sw.WriteLine("");
                            sw.WriteLine("");
                            i++;

                        }
                    }
                }

                // Write file contents on console.     
                using (StreamReader sr = System.IO.File.OpenText(fileNamedirectory + filename))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }

                return Json(fileNamedirectory + filename, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                if (hn != "0")
                {
                }
                return Json(Ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        private void SingleLingSubject(StreamWriter sw, int i, List<SDLCEntityPRES> lstStunsfirst, List<SDLCEntityPRES> lstStubScon, List<SDLCEntitySUB> lstSubs, string yr, string CRS)
        {
            bool flag1 = false;
            bool flag2 = false;
            string space = " ";
            string dotedrow = "- -  - -  - -  - -  - -  - -  - -";

            if (lstStunsfirst[i].SubjectName.Length < 55 && lstStubScon[i].SubjectName.Length < 55)
            {

                string SubjectAndGrades = string.Empty;
                if (i < lstStunsfirst.Count)
                {
                    var lstSub = lstSubs.Where(x => x.SCODE == lstStunsfirst[i].SubjectCode && x.YR == yr && x.CRS == CRS).ToList();

                    if (lstSub.Count == 0 && CRS == "BCM")
                    {
                        lstSub = lstSubs.Where(x => x.SCODE == lstStunsfirst[i].SubjectCode && x.YR == yr && x.CRS == "COM").ToList();
                    }


                    string SubjectName = string.Empty;
                    bool isPractical = false;
                    string SubjectTotal = "";
                    string subMinMr = "";
                    if (lstSub.Count > 0)
                    {
                        if (lstStunsfirst[i].CLM.Length > 2 && lstStunsfirst[i].CLM.Substring(0, 1).ToLower() == "p")
                        {
                            SubjectName = lstSub[0].OCODE + "-LAB";
                            isPractical = true;

                            SubjectTotal = lstSub[0].PMXMR;
                            subMinMr = lstSub[0].PMNMR;
                        }
                        else
                        {
                            SubjectName = lstSub[0].OCODE;
                            isPractical = false;
                            subMinMr = lstSub[0].MNMR;
                            SubjectTotal = lstSub[0].MXMR;
                        }
                    }


                    SubjectAndGrades = SubjectAndGrades + SubjectName + GetSpaces(43 - SubjectName.Length);
                    SubjectAndGrades = SubjectAndGrades + SubjectTotal + GetSpaces(6 - SubjectTotal.ToString().Length);
                    SubjectAndGrades = SubjectAndGrades + subMinMr + GetSpaces(6 - subMinMr.ToString().Length);
                    SubjectAndGrades = SubjectAndGrades + lstStunsfirst[i].Marks + GetSpaces(6 - lstStunsfirst[i].Marks.Length);

                    if (lstSub.Count > 0)
                    {
                        if (lstStunsfirst[i].Marks.Contains("AB") == false)
                        {
                            if (isPractical == false)
                            {
                                if (lstSub[0].MNMR.ChangeINT() > lstStunsfirst[i].Marks.ChangeINT())
                                {
                                    lstStunsfirst[i].Result = "FAIL";
                                }
                                else
                                {
                                    lstStunsfirst[i].Result = "PASS";
                                }
                            }
                            else
                            {

                                if (lstSub[0].PMNMR.ChangeINT() > lstStunsfirst[i].Marks.ChangeINT())
                                {
                                    lstStunsfirst[i].Result = "FAIL";
                                }
                                else
                                {
                                    lstStunsfirst[i].Result = "PASS";
                                }
                            }
                        }
                        else
                        {
                            lstStunsfirst[i].Result = "FAIL";
                        }
                    }
                    SubjectAndGrades = SubjectAndGrades + lstStunsfirst[i].Result + GetSpaces(4 - lstStunsfirst[i].Result.Length);
                }
                else
                {
                    if (flag1 == false)
                    {
                        SubjectAndGrades = SubjectAndGrades + dotedrow + GetSpaces(55 - dotedrow.Length);
                        SubjectAndGrades = SubjectAndGrades + space + GetSpaces(6 - space.Length);
                        SubjectAndGrades = SubjectAndGrades + space + GetSpaces(4 - space.Length);
                        flag1 = true;
                    }
                    else
                    {
                        SubjectAndGrades = GetSpaces(66);
                    }
                }
                SubjectAndGrades = SubjectAndGrades + GetSpaces(5);
                if (i < lstStubScon.Count)
                {
                    var lstSub2 = lstSubs.Where(x => x.SCODE == lstStubScon[i].SubjectCode && x.YR == yr && x.CRS == CRS).ToList();
                    if (lstSub2.Count == 0 && CRS == "BCM")
                    {
                        lstSub2 = lstSubs.Where(x => x.SCODE == lstStubScon[i].SubjectCode && x.YR == yr && x.CRS == "COM").ToList();
                    }
                    string SubjectName2 = string.Empty;
                    bool isPractical2 = false;
                    string SubjectTotal2 = "";
                    string SUBMNMR2 = "";
                    if (lstSub2.Count > 0)
                    {
                        if (lstStubScon[i].CLM.Length > 2 && lstStubScon[i].CLM.Substring(0, 1).ToLower() == "p")
                        {
                            SubjectName2 = lstSub2[0].OCODE + " - LAB";
                            isPractical2 = true;
                            SubjectTotal2 = lstSub2[0].PMXMR;
                            SUBMNMR2 = lstSub2[0].PMNMR;
                        }
                        else
                        {
                            SubjectName2 = lstSub2[0].OCODE;
                            isPractical2 = false;
                            SubjectTotal2 = lstSub2[0].MXMR;
                            SUBMNMR2 = lstSub2[0].MNMR;
                        }
                    }
                    SubjectAndGrades = SubjectAndGrades + SubjectName2 + GetSpaces(43 - SubjectName2.Length);
                    SubjectAndGrades = SubjectAndGrades + SubjectTotal2 + GetSpaces(6 - SubjectTotal2.ToString().Length);
                    SubjectAndGrades = SubjectAndGrades + SUBMNMR2 + GetSpaces(6 - SUBMNMR2.ToString().Length);
                    SubjectAndGrades = SubjectAndGrades + lstStubScon[i].Marks + GetSpaces(4 - lstStubScon[i].Marks.Length);



                    if (lstSub2.Count > 0)
                    {
                        if (lstStubScon[i].Marks.Contains("AB") == false)
                        {
                            if (isPractical2 == false)
                            {
                                if (lstSub2[0].MNMR.ChangeINT() > lstStubScon[i].Marks.ChangeINT())
                                {
                                    lstStubScon[i].Result = "FAIL";
                                }
                                else
                                {
                                    lstStubScon[i].Result = "PASS";
                                }
                            }
                            else
                            {
                                if (lstSub2[0].PMNMR.ChangeINT() > lstStubScon[i].Marks.ChangeINT())
                                {
                                    lstStubScon[i].Result = "FAIL";
                                }
                                else
                                {
                                    lstStubScon[i].Result = "PASS";
                                }
                            }
                        }
                        else
                        {
                            lstStubScon[i].Result = "FAIL";
                        }


                    }
                    SubjectAndGrades = SubjectAndGrades + lstStubScon[i].Result + GetSpaces(4 - lstStubScon[i].Result.Length);
                }
                else
                {
                    if (flag2 == false)
                    {
                        SubjectAndGrades = SubjectAndGrades + dotedrow + GetSpaces(55 - dotedrow.Length);
                        SubjectAndGrades = SubjectAndGrades + space + GetSpaces(6 - space.Length);
                        SubjectAndGrades = SubjectAndGrades + space + GetSpaces(4 - space.Length);
                        flag2 = true;
                    }
                    else
                    {
                        SubjectAndGrades = GetSpaces(66);
                    }
                }
                sw.WriteLine(SubjectAndGrades);

            }

        }

        //private void Twolinesubject(StreamWriter sw, int i, List<SDLCEntityPRES> lstStunsfirst, List<SDLCEntityPRES> lstStubScon)
        //{

        //    string subject1 = "";
        //    subject1 = lstStunsfirst[i].SubjectName;
        //    string s1sp1 = "";
        //    string s1sp2 = "";

        //    if (subject1.Length > 55)
        //    {
        //        s1sp1 = subject1.Substring(0, 48);
        //        s1sp2 = subject1.Substring(48);
        //    }
        //    else
        //    {
        //        s1sp1 = subject1.Substring(0, 50);
        //        s1sp2 = "";
        //    }


        //    string subject2 = "";
        //    subject2 = lstStubScon[i].SubjectName;
        //    string s2sp1 = "";
        //    string s2sp2 = "";
        //    if (subject2.Length > 55)
        //    {
        //        s2sp1 = subject2.Substring(0, 48);
        //        s2sp2 = subject2.Substring(48);
        //    }
        //    else
        //    {
        //        s2sp1 = subject2.Substring(0, 48);
        //        s2sp2 = "";

        //    }

        //    ///scenario 1
        //    if (subject1.Length > 55 && subject2.Length > 55)
        //    {
        //        string s1 = s1sp1 + GetSpaces(55 - s1sp1.Length) + lstStunsfirst[i].Credits + GetSpaces(6 - lstStunsfirst[i].Credits.Length) + lstStunsfirst[i].LeterGrade + GetSpaces(4 - lstStunsfirst[i].LeterGrade.Length) + GetSpaces(5);
        //        s1 = s1 + s2sp1 + GetSpaces(55 - s2sp1.Length) + lstStubScon[i].Credits + GetSpaces(6 - lstStubScon[i].Credits.Length) + lstStubScon[i].LeterGrade + GetSpaces(4 - lstStubScon[i].LeterGrade.Length);
        //        sw.WriteLine(s1);

        //        string sbuid = s1sp2 + GetSpaces(65 - s1sp2.Length) + GetSpaces(5) + s2sp2 + GetSpaces(65 - s2sp2.Length);


        //        sw.WriteLine(sbuid);
        //    }

        //    //Scenario 2
        //    ///scenario 1
        //    if (subject1.Length > 55 && subject2.Length < 55)
        //    {
        //        string s1 = s1sp1 + GetSpaces(55 - s1sp1.Length) + lstStunsfirst[i].Credits + GetSpaces(6 - lstStunsfirst[i].Credits.Length) + lstStunsfirst[i].LeterGrade + GetSpaces(4 - lstStunsfirst[i].LeterGrade.Length) + GetSpaces(5);
        //        s1 = s1 + s2sp1 + GetSpaces(55 - s2sp1.Length) + lstStubScon[i].Credits + GetSpaces(6 - lstStubScon[i].Credits.Length) + lstStubScon[i].LeterGrade + GetSpaces(4 - lstStubScon[i].LeterGrade.Length);
        //        sw.WriteLine(s1);

        //        string sbuid = s1sp2 + GetSpaces(65 - s1sp2.Length) + GetSpaces(5) + s2sp2 + GetSpaces(65 - s2sp2.Length);


        //        sw.WriteLine(sbuid);
        //    }
        //    //scenario 3
        //    if (subject1.Length < 55 && subject2.Length > 55)
        //    {
        //        string s1 = s1sp1 + GetSpaces(55 - s1sp1.Length) + lstStunsfirst[i].Credits + GetSpaces(6 - lstStunsfirst[i].Credits.Length) + lstStunsfirst[i].LeterGrade + GetSpaces(4 - lstStunsfirst[i].LeterGrade.Length) + GetSpaces(5);
        //        s1 = s1 + s2sp1 + GetSpaces(55 - s2sp1.Length) + lstStubScon[i].Credits + GetSpaces(6 - lstStubScon[i].Credits.Length) + lstStubScon[i].LeterGrade + GetSpaces(4 - lstStubScon[i].LeterGrade.Length);
        //        sw.WriteLine(s1);

        //        string sbuid = s1sp2 + GetSpaces(65 - s1sp2.Length) + GetSpaces(5) + s2sp2 + GetSpaces(65 - s2sp2.Length);


        //        sw.WriteLine(sbuid);
        //    }

        //}



        private int addHeaderFooter(StreamWriter sw, int rowcount, string course, bool cchange = false, int rows = 0)
        {
            if (rowcount == 0)
            {

                //sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(" ");

                sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(("                               TABULATION LIST OF " + course + "(SDLCE) I, II, III YEAR SUPPLY. 2018 EXAMINATIONS HELD IN JAN., " + (DateTime.Now.Year).ToString()).Truncate(132));
                sw.WriteLine("SLNO." + GetSpaces(5) + "HTNO" + GetSpaces(15 - "HTNO".Length) + "NAME" + GetSpaces(43 - "NAME".Length) + "FATHER'S NAME" + GetSpaces(44 - "FATHER'S NAME".Length) + "             ");
                sw.WriteLine(("PAGE NO:" + PageNumber + "-------------------------------------------------------------------------------------------------------Date:" + DateTime.Now.ToString("dd-MMM-yyyy") + "===").Truncate(132));

                PageBraker = PageBraker + 9;

            }
            else if (rowcount == 72)
            {
                for (int i = 0; i < (rowcount - rows); i++)
                {
                    sw.WriteLine(" ");
                }

                rowcount = 0;

                //sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(" ");

                sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(" ");
                // sw.WriteLine(("                                  TABULATION LIST OF " + course + " " + year + " YEAR " + sem + " SEM                              ").Truncate(132));
                sw.WriteLine(("                               TABULATION LIST OF " + course + "(SDLCE) I, II, III YEAR SUPPLY. 2018 EXAMINATIONS HELD IN JAN., " + (DateTime.Now.Year).ToString()).Truncate(132));
                sw.WriteLine("SLNO." + GetSpaces(5) + "HTNO" + GetSpaces(15 - "HTNO".Length) + "NAME" + GetSpaces(45 - "NAME".Length) + "FATHER'S NAME" + GetSpaces(45 - "FATHER'S NAME".Length) + "               ");
                sw.WriteLine(("PAGE NO:" + PageNumber + "-------------------------------------------------------------------------------------------------------Date:" + DateTime.Now.ToString("dd-MMM-yyyy") + "===").Truncate(132));
                PageNumber++;
                PageBraker = 9;
                //addHeaderFooter(sw, rowcount, course, year, sem);


            }
            else { PageBraker++; }
            return PageBraker;
        }



    }
}