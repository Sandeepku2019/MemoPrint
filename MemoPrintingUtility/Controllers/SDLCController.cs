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
                string filename = course + DateTime.Now.ToString("ddMMyyyy") + "_SDLC_" + course + ".txt";

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
                    bool ColPagebrk = false;

                    int series = 0;
                    foreach (var colcode in lstColCodes)
                    {

                        List<string> HallticketNumbers = lstPREdata.Where(y => y.ColCode.Trim() == colcode.Trim()).OrderBy(x => x.HTNO).Select(x => x.HTNO).Distinct().ToList<string>();
                        bool isExstudent = false;
                        ColPagebrk = true;

                        #region 
                        for (int i = 0; i < HallticketNumbers.Count; i++)
                        {
                            List<SDLC> lstEntity = new List<SDLC>();
                            string HallTIcket = HallticketNumbers[i];
                            series++;
                            int totalSUbjects = 0;
                            int passedSubject = 0;
                            bool detained = false;
                            int rowcount = 0;


                            var lstPREStuns = lstPREdata.Where(x => x.HTNO == HallticketNumbers[i]).ToList<SDLCEntityPRES>();
                            var lstCONStuns = lstConData.Where(x => x.HTNO == HallticketNumbers[i]).ToList<SDLCEntityCON>();

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

                                //2year
                                lstEntity = CnrEntityVertical(lstEntity, con.M21, con.Y21, con.HTNO, con.P21, "", "II", 1);
                                lstEntity = CnrEntityVertical(lstEntity, con.M22, con.Y22, con.HTNO, con.P22, "", "II", 2);
                                lstEntity = CnrEntityVertical(lstEntity, con.M23, con.Y23, con.HTNO, con.P23, "", "II", 3);
                                lstEntity = CnrEntityVertical(lstEntity, con.M24, con.Y24, con.HTNO, con.P24, "", "II", 4);
                                lstEntity = CnrEntityVertical(lstEntity, con.M25, con.Y25, con.HTNO, con.P25, "", "II", 5);
                                lstEntity = CnrEntityVertical(lstEntity, con.M26, con.Y26, con.HTNO, con.P26, "", "II", 6);
                                lstEntity = CnrEntityVertical(lstEntity, con.M27, con.Y27, con.HTNO, con.P27, "", "II", 7);

                                //3year
                                lstEntity = CnrEntityVertical(lstEntity, con.M31, con.Y31, con.HTNO, con.P31, "", "III", 1);
                                lstEntity = CnrEntityVertical(lstEntity, con.M32, con.Y32, con.HTNO, con.P32, "", "III", 2);
                                lstEntity = CnrEntityVertical(lstEntity, con.M33, con.Y33, con.HTNO, con.P33, "", "III", 3);
                                lstEntity = CnrEntityVertical(lstEntity, con.M34, con.Y34, con.HTNO, con.P34, "", "III", 4);
                                lstEntity = CnrEntityVertical(lstEntity, con.M35, con.Y35, con.HTNO, con.P35, "", "III", 5);
                                lstEntity = CnrEntityVertical(lstEntity, con.M36, con.Y36, con.HTNO, con.P36, "", "III", 6);
                                lstEntity = CnrEntityVertical(lstEntity, con.M37, con.Y37, con.HTNO, con.P37, "", "III", 7);

                            }

                            #endregion
                            lstEntity = CnrEntityVertical(lstEntity, lstPREStuns);


                            string FN = lstPREStuns[0].FName == null ? "" : lstPREStuns[0].FName;
                            string SN = lstPREStuns[0].FullName == null ? "" : lstPREStuns[0].FullName;
                            string CC = lstPREStuns[0].ColCode == null ? "" : lstPREStuns[0].ColCode;
                            string EI = lstPREStuns[0].EI == null ? "" : lstPREStuns[0].EI;
                            string nameformat = series + GetSpaces(5 - series.ToString().Length) + GetSpaces(1) + CC + GetSpaces(5 - CC.Length) + HallTIcket + GetSpaces(12 - HallTIcket.Length) + SN + GetSpaces(4 - SN.Length) + FN + GetSpaces(45 - FN.Length);


                            string[] yrs = new string[] { "I", "II", "III" };


                            #region Page break and Col break set up
                            foreach (string y in yrs)
                            {
                                if (lstEntity.Where(x => x.Year == y).ToList().Count > 0)
                                {
                                    rowcount = rowcount + 2;
                                }
                                rowcount = rowcount + 1;
                            }
                            rowcount = rowcount + 3;


                            if ((PageBraker + rowcount) > 72)
                            {
                                int differ = 72 - PageBraker;

                                for (int h = 0; h < differ; h++)
                                {
                                    sw.WriteLine(" ");
                                }


                                PageBraker = addHeaderFooter(sw, 72, course);
                            }

                            if (PrevColcode != "" && PrevColcode.Trim() != colcode.Trim() && ColPagebrk == true)
                            {
                                ColPagebrk = false;
                                int differ = 73 - PageBraker;

                                for (int h = 0; h < differ; h++)
                                {
                                    sw.WriteLine(" ");
                                }


                                PageBraker = addHeaderFooter(sw, 72, course);

                            }


                            if (PrevColcode == "")
                            {
                                PrevColcode = colcode;
                            }
                            #endregion

                            if (i == 0)
                            {
                                PageBraker = addHeaderFooter(sw, PageBraker, course);
                                sw.WriteLine(nameformat.Truncate(113) + GetSpaces(7 - EI.Length) + HallTIcket + GetSpaces(10 - HallTIcket.Length));
                            }
                            else
                            {
                                sw.WriteLine(nameformat.Truncate(113) + GetSpaces(7 - EI.Length) + HallTIcket + GetSpaces(10 - HallTIcket.Length));
                            }


                            string[] Types = new string[] { "CONS.", "PRES." };
                            foreach (string yr in yrs)
                            {
                                foreach (string ty in Types)
                                {
                                    int subord = 1;
                                    int count = 0;
                                    var lstpopulaateData = lstEntity.Where(x => x.Year == yr && x.Type == ty).ToList().OrderBy(y => y.Type);

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
                                        subjectstring = ty + GetSpaces(5 - ty.Length) + GetSpaces(1) + yr + GetSpaces(3);
                                    }
                                    foreach (var sd in lstpopulaateData)
                                    {


                                        string spaces = "";
                                        string umakspace = "";
                                        string intmarkspae = "";
                                        int ordr = sd.Order;
                                        spaces = GetSpaces((ordr - subord) * 10);
                                        umakspace = GetSpaces((ordr - subord) * 10);
                                        string re = string.Empty;
                                        if (sd.Result != null)
                                        {
                                            re = sd.Result;
                                        }

                                        string SDC= string.Empty;
                                        if (sd.subjectCode != null)
                                        {
                                            SDC = sd.subjectCode;
                                        }



                                        

                                        string SubCode = SDC + " " + re;

                                        subjectformt = GetSpaces (spaces.Length - SubCode.Length);
                                        subjectstring = subjectstring + subjectformt + SubCode;

                                        umakspace = GetSpaces(umakspace.Length - (sd.Marks.Length + Convert.ToString(re).Length));
                                        SubjectMarks_U = umakspace + sd.Marks.ToString().Truncate(4) + re;

                                        subjectsMarksPRE_U = subjectsMarksPRE_U + SubjectMarks_U;
                                        subord = ordr;

                                    }
                                    sw.WriteLine(subjectstring + GetSpaces(113 - subjectstring.Length));
                                    sw.WriteLine(subjectsMarksPRE_U + GetSpaces(113 - subjectsMarksPRE_U.Length));
                                }

                                sw.WriteLine("----------------------------------------------------------------------------------------------------------------------");

                                PageBraker = PageBraker + rowcount;
                            }
                        }
                        #endregion
                    }
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
                string YearS = pre.CLM.Substring(pre.CLM.Length - 2, 1);
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
                lstEntity.Add(new SDLC()
                {
                    HTNO = pre.HTNO,
                    Marks = pre.Marks,
                    Academic = "",
                    subjectCode = pre.SubjectCode,
                    Order = Convert.ToInt32(pre.CLM.Substring(pre.CLM.Length - 1)),
                    Year = YS,
                    Type = "PRES.",
                    EI = pre.EI,
                    Result = pre.Result

                });
            }
            return lstEntity;
        }


        private int addHeaderFooter(StreamWriter sw, int rowcount, string course, bool cchange = false)
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
                sw.WriteLine("SLNO." + GetSpaces(5) + "HTNO" + GetSpaces(15 - "HTNO".Length) + "NAME" + GetSpaces(43 - "NAME".Length) + "FATHER'S NAME" + GetSpaces(44 - "FATHER'S NAME".Length) + "RES/TOT. SGPA/CGPA");
                sw.WriteLine(("PAGE NO:" + PageNumber + "-------------------------------------------------------------------------------------------------------Date:" + DateTime.Now.ToString("dd-MMM-yyyy") + "===").Truncate(132));

                PageBraker = PageBraker + 8;

            }
            else if (rowcount == 72)
            {

                rowcount = 0;
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