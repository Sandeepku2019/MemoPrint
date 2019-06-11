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

                    Filepath = GenerateTabular(course);
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



                        if (HallTIcket == "6001135008")
                        {

                        }



                        var lstPREStuns = lstPREdata.Where(x => x.HTNO == HallTIcket).ToList<SDLCEntityPRES>();
                        var lstCONStuns = lstConData.Where(x => x.HTNO == HallTIcket).ToList<SDLCEntityCON>();

                        string Part1Marks = lstPREStuns[0].Part1;
                        string Part2Marks = lstPREStuns[0].Part2;
                        string FResult = string.Empty;
                        string yr1R = Convert.ToString(lstConData[0].YR1);
                        string yr2R = Convert.ToString(lstConData[0].YR2);
                        string yr3R = Convert.ToString(lstConData[0].YR3);

                        if (yr1R == "F" || yr2R == "F" || yr3R == "F")
                        {
                            FResult = "FAILED";
                        }


                        if (yr1R == "C" && yr2R == "C" && yr3R == "C")
                        {
                            FResult = "COMPLETED";
                        }

                        #region Formatting CON Data
                        foreach (var con in lstCONStuns)
                        {
                            //1year
                            int order1 = 1;
                            if (con.P11 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M11, con.Y11, con.HTNO, con.P11, "", "I", order1++); }
                            if (con.P12 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M12, con.Y12, con.HTNO, con.P12, "", "I", order1++); }
                            if (con.P13 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M13, con.Y13, con.HTNO, con.P13, "", "I", order1++); }
                            if (con.P14 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M14, con.Y14, con.HTNO, con.P14, "", "I", order1++); }
                            if (con.P15 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M15, con.Y15, con.HTNO, con.P15, "", "I", order1++); }
                            if (con.P16 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M16, con.Y16, con.HTNO, con.P16, "", "I", order1++); }
                            if (con.P17 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M17, con.Y17, con.HTNO, con.P17, "", "I", order1++); }
                            if (con.P18 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M18, con.Y18, con.HTNO, con.P18, "", "I", order1++); }
                            if (con.P19 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M19, con.Y19, con.HTNO, con.P19, "", "I", order1++); }

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
                            if (con.P21 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M21, con.Y21, con.HTNO, con.P21, "", "II", order2++); }
                            if (con.P22 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M22, con.Y22, con.HTNO, con.P22, "", "II", order2++); }
                            if (con.P23 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M23, con.Y23, con.HTNO, con.P23, "", "II", order2++); }
                            if (con.P24 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M24, con.Y24, con.HTNO, con.P24, "", "II", order2++); }
                            if (con.P25 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M25, con.Y25, con.HTNO, con.P25, "", "II", order2++); }
                            if (con.P26 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M26, con.Y26, con.HTNO, con.P26, "", "II", order2++); }
                            if (con.P27 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M27, con.Y27, con.HTNO, con.P27, "", "II", order2++); }
                            if (con.P28 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M28, con.Y28, con.HTNO, con.P28, "", "II", order2++); }
                            if (con.P29 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M29, con.Y29, con.HTNO, con.P29, "", "II", order2++); }

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

                            if (con.P31 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M31, con.Y31, con.HTNO, con.P31, "", "III", order3++); }
                            if (con.P32 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M32, con.Y32, con.HTNO, con.P32, "", "III", order3++); }
                            if (con.P33 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M33, con.Y33, con.HTNO, con.P33, "", "III", order3++); }
                            if (con.P34 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M34, con.Y34, con.HTNO, con.P34, "", "III", order3++); }
                            if (con.P35 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M35, con.Y35, con.HTNO, con.P35, "", "III", order3++); }
                            if (con.P36 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M36, con.Y36, con.HTNO, con.P36, "", "III", order3++); }
                            if (con.P37 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M37, con.Y37, con.HTNO, con.P37, "", "III", order3++); }
                            if (con.P38 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M38, con.Y38, con.HTNO, con.P38, "", "III", order3++); }
                            if (con.P39 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M39, con.Y39, con.HTNO, con.P39, "", "III", order3++); }
                            if (con.P310 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M310, con.Y310, con.HTNO, con.P310, "", "III", order3++); }
                            if (con.P311 != null) { lstEntity = CnrEntityVertical(lstEntity, con.M311, con.Y311, con.HTNO, con.P311, "", "III", order3++); }

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

                        #endregion



                        lstEntity = CnrEntityVertical(lstEntity, lstPREStuns);


                        string FinalResult = string.Empty;

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

                        string Part1Div = lstPREStuns[0].Part1Div;
                        string Part2Div = lstPREStuns[0].Part2Div;


                        string[] yrs = new string[] { "I", "II", "III" };
                        string[] Types = new string[] { "CONS.", "PRES." };

                        #region Page break and Col break set up
                        //int allpres = 0;
                        //foreach (string y in yrs)
                        //{

                        //    foreach (string ty in Types)
                        //    {
                        //        if (lstEntity.Where(x => x.Year == y && x.Type == ty && x.subjectCode != null).ToList().Count > 0)
                        //        {
                        //            rowcount = rowcount + 2;


                        //        }
                        //        if (ty == "PRES.")
                        //        {
                        //            if (lstEntity.Where(x => x.Year == y && x.Type == "PRES." && x.subjectCode != null).ToList().Count > 0)
                        //            {
                        //                allpres++;
                        //            }
                        //        }
                        //    }

                        //    if (lstEntity.Where(x => x.Year == y && x.Type == "CONS.").ToList().Count > 0 || lstEntity.Where(x => x.Year == y && x.Type == "PRES.").ToList().Count > 0)
                        //    {
                        //        rowcount = rowcount + 1;
                        //    }
                        //}
                        //if (allpres == 3)
                        //{
                        //    rowcount = rowcount + 1;
                        //}
                        //if (lstEntity.Where(x => x.Type == "CONS." && x.subjectCode != null).ToList().Count == 0)
                        //{
                        //    rowcount = rowcount + 1;
                        //}
                        //rowcount = rowcount + 2;

                        //if (allpres == 1 && lstEntity.Where(x => x.Type == "PRES." && x.Year == "III" && x.subjectCode != null).ToList().Count > 0)
                        //{
                        // rowcount = rowcount - 1;
                        //}




                        //if ((PageBraker + rowcount) > 72)
                        //{
                        //    int differ = 72 - PageBraker;

                        //    for (int h = 0; h < differ; h++)
                        //    {
                        //        sw.WriteLine(" ");
                        //    }
                        //    //rowcount = 1;

                        //    PageBraker = addHeaderFooter(sw, 72, course);
                        //}

                        ////if (PrevColcode != "" && PrevColcode.Trim() != colcode.Trim() && ColPagebrk == true)
                        ////{
                        ////    ColPagebrk = false;
                        ////    int differ = 73 - PageBraker;

                        ////    for (int h = 0; h < differ; h++)
                        ////    {
                        ////        sw.WriteLine(" ");
                        ////    }


                        ////    PageBraker = addHeaderFooter(sw, 72, course);

                        ////}


                        ////if (PrevColcode == "")
                        ////{
                        ////    PrevColcode = colcode;
                        ////}
                        #endregion

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
                                }
                                if(lstpopulaateData.Where(x => x.Type == "PRES.").ToList().Count() == 1)
                                {
                                    subcount1 = true;
                                }
                                bool isprac = false;
                                int PSOrder = 0;
                                var pracStratOrder = lstpopulaateData.Where(x =>  x.subjectCode.Contains("(P)")).OrderBy(z => z.Order).ToList();
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
                                        umakspace = GetSpaces((ordr - subord) *11);
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

                                    if (flag == true  )
                                    {
                                        spaces = spaces + GetSpaces(4);
                                        
                                        umakspace = umakspace + GetSpaces(4);
                                        umakspace = umakspace + GetSpaces(1);
                                        flag = false;

                                        if (abflag == true)
                                        {

                                            umakspace = umakspace + GetSpaces(1);
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
                                if (lstpopulaateData.Count() > 0)
                                {



                                    if (ty == "PRES.")
                                    {

                                        sw.WriteLine(subjectstring + GetSpaces(100 - subjectstring.Length) + result + GetSpaces(13 - result.Length));
                                        ////rowcount = (rowcount + 1) >= 72 ? addHeaderFooter(sw, 72, course, false, rowcount) : (rowcount + 1);
                                        sw.WriteLine(subjectsMarksPRE_U + GetSpaces(113 - subjectsMarksPRE_U.Length));
                                        // rowcount = (rowcount + 1) >= 72 ? addHeaderFooter(sw, 72, course, false, rowcount) : (rowcount + 1);
                                        sw.WriteLine("          ..........................................................................................................................");
                                        rowcount = (rowcount + 3) >= 68 ? addHeaderFooter(sw, 72, course, false, rowcount + 2) : (rowcount + 3);
                                    }
                                    else
                                    {

                                        sw.WriteLine(subjectstring + GetSpaces(100 - subjectstring.Length) + result + GetSpaces(13 - result.Length));
                                        //rowcount = (rowcount + 1) >= 72 ? addHeaderFooter(sw, 72, course, false, rowcount) : (rowcount + 1);
                                        sw.WriteLine(subjectsMarksPRE_U + GetSpaces(113 - subjectsMarksPRE_U.Length));
                                        rowcount = (rowcount + 2) >= 68 ? addHeaderFooter(sw, 72, course, false, rowcount + 2) : (rowcount + 2);
                                    }
                                }
                            }

                            //.WriteLine("          ..........................................................................................................................");
                        }
                        sw.WriteLine("            *******PART 1: " + Part1Marks + GetSpaces(5) + Part1Div + GetSpaces(25) + "********* Part 2: " + Part2Marks + GetSpaces(5) + Part1Div + GetSpaces(15));
                        // rowcount = (rowcount + 1) >= 72 ? addHeaderFooter(sw, 72, course, false, rowcount) : (rowcount + 1);
                        sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------");
                        rowcount = (rowcount + 2) >= 68 ? addHeaderFooter(sw, 72, course, false, rowcount + 2) : (rowcount + 2);


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
                        orse = lstEntity.Where(x => x.Year == YS && x.Type == "CONS.").ToList().Count;
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
                sw.WriteLine(("                                  TABULATION LIST OF " + course + " EXAMINATIONS HELD IN Oct/Nov., " + (DateTime.Now.Year - 1).ToString() + "                           ").Truncate(132));
                sw.WriteLine("SLNO." + GetSpaces(5) + "HTNO" + GetSpaces(15 - "HTNO".Length) + "NAME" + GetSpaces(43 - "NAME".Length) + "FATHER'S NAME" + GetSpaces(44 - "FATHER'S NAME".Length) + "             ");
                sw.WriteLine(("PAGE NO:" + PageNumber + "-------------------------------------------------------------------------------------------------------Date:" + DateTime.Now.ToString("dd-MMM-yyyy") + "===").Truncate(132));

                PageBraker = PageBraker + 8;

            }
            else if (rowcount == 72)
            {
                for (int i = 0; i < (rowcount - rows); i++)
                {
                    sw.WriteLine(" ");
                }

                rowcount = 1;
                PageNumber++;
                //sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(" ");

                sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(" ");
                // sw.WriteLine(("                                  TABULATION LIST OF " + course + " " + year + " YEAR " + sem + " SEM                              ").Truncate(132));
                sw.WriteLine(("                                  TABULATION LIST OF " + course + " EXAMINATIONS HELD IN Oct/Nov., " + (DateTime.Now.Year - 1).ToString() + "                           ").Truncate(132));
                sw.WriteLine("SLNO." + GetSpaces(5) + "HTNO" + GetSpaces(15 - "HTNO".Length) + "NAME" + GetSpaces(45 - "NAME".Length) + "FATHER'S NAME" + GetSpaces(45 - "FATHER'S NAME".Length) + "               ");
                sw.WriteLine(("PAGE NO:" + PageNumber + "-------------------------------------------------------------------------------------------------------Date:" + DateTime.Now.ToString("dd-MMM-yyyy") + "===").Truncate(132));

                PageBraker = 8;
                //addHeaderFooter(sw, rowcount, course, year, sem);


            }
            else { PageBraker++; }
            return PageBraker;
        }
    }
}