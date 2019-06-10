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
                       


                        if (HallTIcket == "6421124014")
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
                            lstEntity = CnrEntityVertical(lstEntity, con.M11, con.Y11, con.HTNO, con.P11, "", "I", 1);
                            lstEntity = CnrEntityVertical(lstEntity, con.M12, con.Y12, con.HTNO, con.P12, "", "I", 2);
                            lstEntity = CnrEntityVertical(lstEntity, con.M13, con.Y13, con.HTNO, con.P13, "", "I", 3);
                            lstEntity = CnrEntityVertical(lstEntity, con.M14, con.Y14, con.HTNO, con.P14, "", "I", 4);
                            lstEntity = CnrEntityVertical(lstEntity, con.M15, con.Y15, con.HTNO, con.P15, "", "I", 5);
                            lstEntity = CnrEntityVertical(lstEntity, con.M16, con.Y16, con.HTNO, con.P16, "", "I", 6);
                            lstEntity = CnrEntityVertical(lstEntity, con.M17, con.Y17, con.HTNO, con.P17, "", "I", 7);
                            lstEntity = CnrEntityVertical(lstEntity, con.M18, con.Y18, con.HTNO, con.P18, "", "I", 8);
                            lstEntity = CnrEntityVertical(lstEntity, con.M19, con.Y19, con.HTNO, con.P19, "", "I", 9);

                            //2year
                            lstEntity = CnrEntityVertical(lstEntity, con.M21, con.Y21, con.HTNO, con.P21, "", "II", 1);
                            lstEntity = CnrEntityVertical(lstEntity, con.M22, con.Y22, con.HTNO, con.P22, "", "II", 2);
                            lstEntity = CnrEntityVertical(lstEntity, con.M23, con.Y23, con.HTNO, con.P23, "", "II", 3);
                            lstEntity = CnrEntityVertical(lstEntity, con.M24, con.Y24, con.HTNO, con.P24, "", "II", 4);
                            lstEntity = CnrEntityVertical(lstEntity, con.M25, con.Y25, con.HTNO, con.P25, "", "II", 5);
                            lstEntity = CnrEntityVertical(lstEntity, con.M26, con.Y26, con.HTNO, con.P26, "", "II", 6);
                            lstEntity = CnrEntityVertical(lstEntity, con.M27, con.Y27, con.HTNO, con.P27, "", "II", 7);
                            lstEntity = CnrEntityVertical(lstEntity, con.M28, con.Y28, con.HTNO, con.P28, "", "II", 8);
                            lstEntity = CnrEntityVertical(lstEntity, con.M29, con.Y29, con.HTNO, con.P29, "", "II", 9);


                            //3year
                            lstEntity = CnrEntityVertical(lstEntity, con.M31, con.Y31, con.HTNO, con.P31, "", "III", 1);
                            lstEntity = CnrEntityVertical(lstEntity, con.M32, con.Y32, con.HTNO, con.P32, "", "III", 2);
                            lstEntity = CnrEntityVertical(lstEntity, con.M33, con.Y33, con.HTNO, con.P33, "", "III", 3);
                            lstEntity = CnrEntityVertical(lstEntity, con.M34, con.Y34, con.HTNO, con.P34, "", "III", 4);
                            lstEntity = CnrEntityVertical(lstEntity, con.M35, con.Y35, con.HTNO, con.P35, "", "III", 5);
                            lstEntity = CnrEntityVertical(lstEntity, con.M36, con.Y36, con.HTNO, con.P36, "", "III", 6);
                            lstEntity = CnrEntityVertical(lstEntity, con.M37, con.Y37, con.HTNO, con.P37, "", "III", 7);
                            lstEntity = CnrEntityVertical(lstEntity, con.M38, con.Y38, con.HTNO, con.P38, "", "III", 8);
                            lstEntity = CnrEntityVertical(lstEntity, con.M39, con.Y39, con.HTNO, con.P39, "", "III", 9);
                            lstEntity = CnrEntityVertical(lstEntity, con.M310, con.Y310, con.HTNO, con.P310, "", "III", 10);

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

                        //#region Page break and Col break set up
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
                        //#endregion

                        if (i == 0)
                        {
                            rowcount = addHeaderFooter(sw, 72, course, false, 72);
                            //rowcount++;
                            sw.WriteLine(nameformat);
                            rowcount = (rowcount + 1) >= 68 ? addHeaderFooter(sw, 72, course, false, rowcount+2) : (rowcount + 1);
                        } 
                        else
                        {


                            
                            sw.WriteLine(nameformat);
                            rowcount = (rowcount + 1) >= 68 ? addHeaderFooter(sw, 72, course, false, rowcount + 2) : (rowcount + 1);
                        }



                        foreach (string yr in yrs)
                        {

                            string result = string.Empty;

                            //if (yr == "I")
                            //{
                            //    result = yr1R;
                            //}

                            //if (yr == "II")
                            //{
                            //    result = yr2R;
                            //}


                            //if (yr == "III")
                            //{
                            //    result = yr3R;
                            //}
                            foreach (string ty in Types)
                            {
                                int subord = 1;
                                int count = 0;
                                var lstpopulaateData = lstEntity.Where(x => x.Year == yr && x.Type == ty && x.subjectCode != null).ToList().OrderBy(y => y.Type);

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

                                if (lstpopulaateData.Count() > 0)
                                {
                                    if (lstpopulaateData.ToList()[0].Order > 1 && lstpopulaateData.ToList()[0].Type == "PRES.")
                                    {

                                        flag = true;

                                        if (lstpopulaateData.Where(z => z.Marks == "AB").ToList().Count > 0)
                                        {
                                            abflag = true;
                                        }
                                    }
                                }
                                bool isprac = false;
                                foreach (var sd in lstpopulaateData.OrderBy(z => z.Order))
                                {



                                    string spaces = "";
                                    string umakspace = "";
                                    string intmarkspae = "";
                                    int ordr = sd.Order;
                                    spaces = GetSpaces((ordr - subord) * 10);
                                    umakspace = GetSpaces((ordr - subord) * 10);


                                    if (flag == true)
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
                                        if (subjectstring.Contains(sd.subjectCode) == false)
                                        {
                                            SDC = sd.subjectCode;
                                            SubCode = SDC + " " + Academic;
                                            //isprac = false;
                                        }
                                        else
                                        {
                                            SDC = string.Empty;
                                            SubCode = "";
                                            isprac = true;
                                        }

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
                        rowcount = (rowcount + 2) >= 68 ? addHeaderFooter(sw, 72, course, false, rowcount+2) : (rowcount + 2);


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
                if (flag == true)
                {
                    int a = pre.CLM.Length - pre.CLM.Substring(0, 3).Length;
                    orse = Convert.ToInt32(pre.CLM.Substring(3, a));
                }
                else
                {
                    int a = pre.CLM.Length - pre.CLM.Substring(0, 2).Length;
                    orse = Convert.ToInt32(pre.CLM.Substring(2, a));
                }
                lstEntity.Add(new SDLC()
                {
                    HTNO = pre.HTNO,
                    Marks = pre.Marks,
                    Academic = "",
                    subjectCode = pre.SubjectCode,
                    Order = Convert.ToInt32(orse),
                    Year = YS,
                    Type = "PRES.",
                    EI = pre.EI,
                    Result = pre.Result

                });
            }
            return lstEntity;
        }


        private int addHeaderFooter(StreamWriter sw, int rowcount, string course, bool cchange = false, int rows =  0)
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