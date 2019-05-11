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
    public class TabularReportController : Controller
    {
        int PageNumber = 1;
        // GET: TabularReport
        public ActionResult Index()
        {
            return View();
        }
        int PageBraker = 0;

        [HttpPost]
        public JsonResult GenerateTabularSemReport(int Psem, string course)
        {
            try
            {

                ///// Creting Notepad 
                //string fileNamedirectory = Server.MapPath("/TabularReport/");
                string fileNamedirectory = @"D:\TabularReport\";
                string filename = course + DateTime.Now.ToString("ddMMyyyy") + "_" + Psem + ".txt";

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

                int year = 0;
                int sem = 0;
                if (Psem == 11)
                {
                    year = 1;
                    sem = 1;

                }
                if (Psem == 21)
                {
                    year = 2;
                    sem = 1;

                }

                if (Psem == 31)
                {
                    year = 3;
                    sem = 1;

                }

                // Create a new file     
                using (StreamWriter sw = fi.CreateText())
                {
                    //
                    var lstStudents = BoMemoService.getTabularReportInstance().GetStudentDetail(course, Psem, sem, year);
                    var LstConStudents = BoMemoService.getTabularReportInstance().GetStudentsConsDetails(course, Psem);
                    var lsttotalpassed = BoMemoService.getTabularReportInstance().getTotalandPassed(course, year);

                    int TotalSubjectAttemt = 0;
                    int TotalSubjectpass = 0;
                    string PrevColcode = "";
                    bool ColPagebrk = false;

                    List<string> lstColCodes = lstStudents.OrderBy(x => x.collegecode.Trim()).Select(y => y.collegecode.Trim()).Distinct().ToList<string>();


                    int series = 0;
                    foreach (var colcode in lstColCodes)
                    {
                        List<string> HallticketNumbers = lstStudents.Where(y => y.collegecode.Trim() == colcode.Trim()).OrderBy(x => x.HallTicketNumber).Select(x => x.HallTicketNumber).Distinct().ToList<string>();
                        bool isExstudent = false;
                       
                        ColPagebrk = true;
                        #region Htnonumbers
                        for (int i = 0; i < HallticketNumbers.Count; i++)
                        {
                           

                                series++;
                            int totalSUbjects = 0;
                            int passedSubject = 0;
                            bool detained = false;
                            int rowcount = 0;
                            var lstStuns = lstStudents.Where(x => x.HallTicketNumber == HallticketNumbers[i]).ToList<StudentInformation>();
                            var StudentConsInformatio = LstConStudents.Where(x => x.HTNO == HallticketNumbers[i]).ToList<ConsDataEntity>();
                            
                            string HallTicket = lstStuns[0].HallTicketNumber;
                            var objtotalaspassed = lsttotalpassed.Where(x => x.Htno.Trim() == HallTicket.Trim()).ToList();

                            if (HallTicket == "131191022")
                            {

                            }

                            string FN = lstStuns[0].FatherName == null ? "" : lstStuns[0].FatherName;
                            string SN = lstStuns[0].StudentName == null ? "" : lstStuns[0].StudentName;
                            string CC = lstStuns[0].collegecode == null ? "" : lstStuns[0].collegecode;
                            string EI = lstStuns[0].Ei == null ? "" : lstStuns[0].Ei;
                            string CollegeCode = lstStuns[0].collegecode == null ? "" : lstStuns[0].collegecode;
                            string nameformat = series+ GetSpaces(5-series.ToString().Length) + GetSpaces(1) + CollegeCode + GetSpaces(5 - CollegeCode.Length) + HallTicket + GetSpaces(12 - HallTicket.Length) + SN + GetSpaces(4 - SN.Length) + FN + GetSpaces(45 - FN.Length);
                            if (EI == "E")
                            {
                                isExstudent = true;
                            }
                            else
                            {
                                isExstudent = false;
                            }




                            if (lstStudents != null && lstStudents.Count > 0)
                            {
                                if (StudentConsInformatio != null && StudentConsInformatio.Count > 0)
                                {
                                    rowcount = rowcount + 2;
                                }
                                rowcount = rowcount + 6;
                             
                            }
                            if ((PageBraker + rowcount) > 72)
                            {
                                int differ = 72- PageBraker;

                                for (int h = 0; h < differ; h++)
                                {
                                    sw.WriteLine(" ");
                                }
                               

                                    PageBraker = addHeaderFooter(sw, 72, course, year.ToString(), sem.ToString());
                            }

                            if (PrevColcode != "" && PrevColcode.Trim() != colcode.Trim() && ColPagebrk == true)
                            {
                                ColPagebrk = false;
                                int differ = 73- PageBraker;

                                for (int h = 0; h < differ; h++)
                                {
                                    sw.WriteLine(" ");
                                }
                               

                                PageBraker = addHeaderFooter(sw, 72, course, year.ToString(), sem.ToString());

                            }


                            if (PrevColcode == "")
                            {
                                PrevColcode = colcode;
                            }

                            if (i == 0)
                            {
                                PageBraker = addHeaderFooter(sw, PageBraker, course, year.ToString(), sem.ToString());
                                sw.WriteLine("");
                                sw.WriteLine(nameformat.Truncate(113) + EI + GetSpaces(7 - EI.Length) + HallTicket + GetSpaces(10 - HallTicket.Length));

                            }
                            else
                            {
                                sw.WriteLine("");
                                sw.WriteLine(nameformat.Truncate(113) + EI + GetSpaces(7 - EI.Length) + HallTicket + GetSpaces(10 - HallTicket.Length));

                            }
                            List<TotalsubjectRecord> lstts = new List<TotalsubjectRecord>();

                            #region Cons
                            if (StudentConsInformatio != null & StudentConsInformatio.Count > 0)
                            {

                                var ConsDetails = StudentConsInformatio.FirstOrDefault();
                                //string Attemps = CalucateTotalAttemps(ConsDetails);

                                string subjectstringCONS = string.Empty;
                                string SubjectCONSMarks_U = string.Empty;
                                string subjectsMarksCONS_U = string.Empty;
                                string MarkformatCONS = GetSpaces(10);

                                string SubjectCONSMarks_S = string.Empty;
                                string subjectsMarksCONS_S = string.Empty;

                                string subjectCONSformt = "";

                                subjectstringCONS = "CONS." + GetSpaces(5 - "CONS.".Length);
                                SubjectCONSMarks_U = "   U" + GetSpaces(5 - "CONS.".Length);
                                SubjectCONSMarks_S = "   S" + GetSpaces(5 - "CONS.".Length) + GetSpaces(2);

                                #region Subject title 


                                subjectCONSformt = ConcatenateSubject(subjectCONSformt, ConsDetails.P1, ConsDetails.A1);

                                subjectCONSformt = ConcatenateSubject(subjectCONSformt, ConsDetails.P2, ConsDetails.A2);
                                subjectCONSformt = ConcatenateSubject(subjectCONSformt, ConsDetails.P3, ConsDetails.A3);
                                subjectCONSformt = ConcatenateSubject(subjectCONSformt, ConsDetails.P4, ConsDetails.A4);
                                subjectCONSformt = ConcatenateSubject(subjectCONSformt, ConsDetails.P5, ConsDetails.A5);
                                subjectCONSformt = ConcatenateSubject(subjectCONSformt, ConsDetails.P6, ConsDetails.A6);
                                subjectCONSformt = ConcatenateSubject(subjectCONSformt, ConsDetails.P7, ConsDetails.A7);
                                subjectCONSformt = ConcatenateSubject(subjectCONSformt, ConsDetails.P8, ConsDetails.A8);
                                subjectCONSformt = ConcatenateSubject(subjectCONSformt, ConsDetails.P9, ConsDetails.A9);
                                subjectCONSformt = ConcatenateSubject(subjectCONSformt, ConsDetails.P10, ConsDetails.A10);

                                if(ConsDetails.P1!= null)lstts.Add(new TotalsubjectRecord() { subname = ConsDetails.P1, status = ConsDetails.R1 });
                                if(ConsDetails.P2!= null)lstts.Add(new TotalsubjectRecord() { subname = ConsDetails.P2, status = ConsDetails.R2 });
                                if(ConsDetails.P3!= null)lstts.Add(new TotalsubjectRecord() { subname = ConsDetails.P3, status = ConsDetails.R3 });
                                if(ConsDetails.P4!= null)lstts.Add(new TotalsubjectRecord() { subname = ConsDetails.P4, status = ConsDetails.R4 });
                                if(ConsDetails.P5!= null)lstts.Add(new TotalsubjectRecord() { subname = ConsDetails.P5, status = ConsDetails.R5 });
                                if(ConsDetails.P6!= null)lstts.Add(new TotalsubjectRecord() { subname = ConsDetails.P6, status = ConsDetails.R6 });
                                if(ConsDetails.P7!= null)lstts.Add(new TotalsubjectRecord() { subname = ConsDetails.P7, status = ConsDetails.R7 });
                                if(ConsDetails.P8!= null)lstts.Add(new TotalsubjectRecord() { subname = ConsDetails.P8, status = ConsDetails.R8 });
                                if(ConsDetails.P9!= null)lstts.Add(new TotalsubjectRecord() { subname = ConsDetails.P9, status = ConsDetails.R9 });
                                if (ConsDetails.P10 != null) lstts.Add(new TotalsubjectRecord() { subname = ConsDetails.P10, status = ConsDetails.R10 });


                                subjectstringCONS = "CONS." + GetSpaces(5 - "CONS.".Length) + GetSpaces(1) + subjectCONSformt;
                                sw.WriteLine(subjectstringCONS + GetSpaces(113 - subjectstringCONS.Length) + ConsDetails.RES + "   " + ConsDetails.SGPA);

                                if (ConsDetails.RES == "L")
                                {
                                    detained = true;
                                }
                                #endregion


                                #region Subject External Mark 
                                string SubjectExtCons = string.Empty;
                                SubjectExtCons = ConcatenateSubjectMarks(SubjectExtCons, ConsDetails.M1, string.Empty);
                                SubjectExtCons = ConcatenateSubjectMarks(SubjectExtCons, ConsDetails.M2, string.Empty);
                                SubjectExtCons = ConcatenateSubjectMarks(SubjectExtCons, ConsDetails.M3, string.Empty);
                                SubjectExtCons = ConcatenateSubjectMarks(SubjectExtCons, ConsDetails.M4, string.Empty);
                                SubjectExtCons = ConcatenateSubjectMarks(SubjectExtCons, ConsDetails.M5, string.Empty);
                                SubjectExtCons = ConcatenateSubjectMarks(SubjectExtCons, ConsDetails.M6, string.Empty);
                                SubjectExtCons = ConcatenateSubjectMarks(SubjectExtCons, ConsDetails.M7, string.Empty);
                                SubjectExtCons = ConcatenateSubjectMarks(SubjectExtCons, ConsDetails.M8, string.Empty);
                                SubjectExtCons = ConcatenateSubjectMarks(SubjectExtCons, ConsDetails.M9, string.Empty);
                                SubjectExtCons = ConcatenateSubjectMarks(SubjectExtCons, ConsDetails.M10, string.Empty);
                                SubjectCONSMarks_U = SubjectCONSMarks_U + GetSpaces(2) + SubjectExtCons;
                                sw.WriteLine(SubjectCONSMarks_U);

                                #endregion


                                #region Subject Internal Mark 
                                string SubjectintCons = string.Empty;

                                SubjectintCons = ConcatenateSubjectMarks(SubjectintCons, ConsDetails.S1, ConsDetails.R1);

                                SubjectintCons = ConcatenateSubjectMarks(SubjectintCons, ConsDetails.S2, ConsDetails.R2);


                                SubjectintCons = ConcatenateSubjectMarks(SubjectintCons, ConsDetails.S3, ConsDetails.R3);

                                SubjectintCons = ConcatenateSubjectMarks(SubjectintCons, ConsDetails.S4, ConsDetails.R4);

                                SubjectintCons = ConcatenateSubjectMarks(SubjectintCons, ConsDetails.S5, ConsDetails.R5);

                                SubjectintCons = ConcatenateSubjectMarks(SubjectintCons, ConsDetails.S6, ConsDetails.R6);

                                SubjectintCons = ConcatenateSubjectMarks(SubjectintCons, ConsDetails.S7, ConsDetails.R7);

                                SubjectintCons = ConcatenateSubjectMarks(SubjectintCons, ConsDetails.S8, ConsDetails.R8);


                                SubjectintCons = ConcatenateSubjectMarks(SubjectintCons, ConsDetails.S9, ConsDetails.R9);


                                SubjectintCons = ConcatenateSubjectMarks(SubjectintCons, ConsDetails.S10, ConsDetails.R10);

                                SubjectCONSMarks_S = SubjectCONSMarks_S + SubjectintCons;

                                sw.WriteLine(SubjectCONSMarks_S);

                                #endregion

                            }
                            #endregion


                            #region  Pre
                            string subjectstringPRE = string.Empty;
                            string SubjectMarks_U = string.Empty;
                            string subjectsMarksPRE_U = string.Empty;
                            string MarkformatPRE = GetSpaces(10);
                            string Aademicstatus = string.Empty;



                            string SubjectMarks_S = string.Empty;
                            string subjectsMarksPRE_S = string.Empty;


                            if (lstStuns.Count > 0)
                            {
                                string subjectformt = "";
                                subjectstringPRE = "PRES." + GetSpaces(5 - "PRES.".Length) + GetSpaces(1);
                                subjectsMarksPRE_U = "   U" + GetSpaces(5 - "PRES.".Length) + GetSpaces(2);
                                if (!isExstudent)
                                {
                                    subjectsMarksPRE_S = "   S" + GetSpaces(5 - "PRES.".Length) + GetSpaces(2);
                                }
                                else
                                {
                                    subjectsMarksPRE_S = GetSpaces(5) + GetSpaces(5 - "PRES.".Length) + GetSpaces(2);
                                }
                                int subord = 1;

                                int count = 0;

                                //totalSUbjects = totalSUbjects + lstStuns.Count;
                                //passedSubject = passedSubject + lstStuns.Where(x => x.Status == "P").ToList<StudentInformation>().Count();



                                foreach (StudentInformation studentM in lstStuns.OrderBy(X => X.Order))
                                {

                                    #region total subs and paassed subs

                                    TotalsubjectRecord obj = new TotalsubjectRecord() { subname = studentM.SubjectCode, status = studentM.Status };
                                    if (lstts != null && lstts.Count > 0)
                                    {
                                        if (lstts.Where(x=>x.subname.ToLower() == studentM.SubjectCode.ToLower()).ToList().Count>0)
                                        {
                                            foreach (var t in lstts)
                                            {
                                                if (t.subname == obj.subname)
                                                {
                                                    t.status = studentM.Status;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            lstts.Add(obj);
                                        }
                                    }
                                    else
                                    {
                                        lstts.Add(obj);

                                    }
                                    #endregion
                                    string spaces = "";
                                    string umakspace = "";
                                    string intmarkspae = "";
                                    int ordr = studentM.Order;
                                    spaces = GetSpaces((ordr - subord) * 8);
                                    umakspace = GetSpaces((ordr - subord) *8);
                                    intmarkspae = GetSpaces((ordr - subord) * 8);
                                    string subPReF = studentM.LeterGrade;
                                    if (studentM.LeterGrade == "AB")
                                    {
                                        subPReF = "F";
                                    }
                                    string intmark = string.Empty;
                                    if (isExstudent == false)
                                    {
                                        intmark = Convert.ToString(studentM.InternalMarks == null ? "" : studentM.InternalMarks) + GetSpaces(3 - Convert.ToString(studentM.InternalMarks == null ? "" : studentM.InternalMarks).Length) + " " + subPReF;
                                    }
                                   
                                    if (count > 0)
                                    {




                                        spaces = GetSpaces(spaces.Length - studentM.SubjectCode.Length);


                                        if (isExstudent == false)
                                        {
                                            umakspace = GetSpaces(umakspace.Length - studentM.ExernalMarks.ToString().Length);

                                            intmarkspae = GetSpaces(intmarkspae.Length - intmark.Length);
                                        }
                                        else
                                        {
                                            if (studentM.ExernalMarks.ToString() == "AB.")
                                            {
                                                umakspace = GetSpaces(umakspace.Length + 1 - (studentM.ExernalMarks.ToString() + " " + studentM.LeterGrade).Length);
                                            }
                                            else
                                            {
                                                umakspace = GetSpaces(umakspace.Length - (studentM.ExernalMarks.ToString().Truncate(3) + " " + studentM.LeterGrade).Length);
                                            }

                                        }
                                    }



                                    subjectformt = spaces + studentM.SubjectCode;
                                    subjectstringPRE = subjectstringPRE + subjectformt;

                                    if (isExstudent == false)
                                    {
                                        SubjectMarks_U = umakspace + studentM.ExernalMarks.ToString().Truncate(3);
                                    }
                                    else
                                    {
                                        SubjectMarks_U = umakspace + studentM.ExernalMarks.ToString().Truncate(4) + " " + subPReF;

                                    }

                                    subjectsMarksPRE_U = subjectsMarksPRE_U + SubjectMarks_U;


                                    if (isExstudent == false)
                                    {
                                        intmark = intmarkspae + intmark;// + GetSpaces(7 - intmark.Length); ;
                                    }
                                    SubjectMarks_S = intmark;
                                    subjectsMarksPRE_S = subjectsMarksPRE_S + SubjectMarks_S;

                                    count++;
                                    subord = ordr;
                                }

                                string subjectsPF = string.Empty;
                                if (objtotalaspassed == null || objtotalaspassed.Count == 0)
                                {
                                    objtotalaspassed = new List<TotalsubjectRecord>();
                                    objtotalaspassed.Add(new TotalsubjectRecord() { TotalSubs = 0, PassedSubs = 0 });
                                }
                                var aa = objtotalaspassed.Sum(x => x.PassedSubs) + lstts.Where(x => x.status != "F").ToList().Count;
                                var bb = objtotalaspassed.Sum(x => x.TotalSubs);

                                var Paa = lstts.Where(x => x.status != "F" && x.subname !=null).ToList().Count;
                                var Pbb = lstts.Where(x => x.subname != null).ToList().Count;

                                var expaa = lstStuns.Where(x => x.Status != "F").ToList().Count;

                                if (isExstudent == false)
                                {
                                    subjectsPF = "<" + Paa + "/" + Pbb + ">" + "<" + (aa).ToString().Truncate(2) + "/" + (bb + lstts.Count).ToString().Truncate(2) + ">";
                                }
                                else
                                {
                                    subjectsPF = "<" + Paa + "/" + Pbb + ">" + "<"+ (expaa + objtotalaspassed.Sum(x => x.PassedSubs)) + "/" + bb + ">";
                                }

                                int external = lstStuns.Sum(x => x.ExernalMarks.ChangeINT());
                                int TotalMark = external + lstStuns.Sum(x => x.InternalMarks.ChangeINT());
                                sw.WriteLine(subjectstringPRE + GetSpaces(113 - subjectsMarksPRE_U.Length) + "  " + subjectsPF + " " + TotalMark);
                                string finalR = lstStuns[0].FinalResult;
                                if (finalR != "MALL.PRACT")
                                {
                                    if (detained == true && TotalMark > 0)
                                    {
                                        finalR = "PROMOTED";

                                    }
                                    else
                                    {
                                        finalR = lstStuns[0].FinalResult;

                                    }
                                }
                                else
                                {
                                    finalR = "MP";

                                }

                                string Flotation = string.Empty;

                                //lstStuns.Where(x => x.Flotation.ToString() == "FL").ToList().Count > 0 ? "FL" : "";

                                

                                if (lstStuns.Where(x => x.GRACE_MARKS != null).ToList().Count > 0 && lstStuns.Where(x => x.GRACE_MARKS2 != null).ToList().Count > 0)
                                {
                                    Flotation = "FL AC";
                                }else
                                if (lstStuns.Where(x => x.GRACE_MARKS != null).ToList().Count > 0 && lstStuns.Where(x => x.GRACE_MARKS2 != null).ToList().Count == 0)
                                {
                                    Flotation = "FL";

                                }
                                else
                                if (lstStuns.Where(x => x.GRACE_MARKS2 != null).ToList().Count > 0 && lstStuns.Where(x => x.GRACE_MARKS != null).ToList().Count == 0)
                                {
                                    Flotation = "AC";

                                }

                                sw.WriteLine(subjectsMarksPRE_U + GetSpaces(113 - subjectsMarksPRE_U.Length) + "  "+finalR + "  " + lstStuns[0].SGPA + " " + Flotation);
                                if (isExstudent == false)
                                {
                                    sw.WriteLine(subjectsMarksPRE_S + GetSpaces(113 - subjectsMarksPRE_U.Length));

                                }

                                sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------");

                            }




                            #endregion

                            PageBraker = PageBraker + rowcount;

                        }

                        #endregion
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
                return Json(Ex.Message, JsonRequestBehavior.AllowGet);
            }


        }


        public void ShowFile(string filename)
        {
            //Clears all content output from Buffer Stream
            Response.ClearContent();
            //Clears all headers from Buffer Stream
            Response.ClearHeaders();
            //Adds an HTTP header to the output stream
            Response.AddHeader("Content-Disposition", "inline;filename=" + filename);
            //Gets or Sets the HTTP MIME type of the output stream
            Response.ContentType = "application/pdf";
            //Writes the content of the specified file directory to an HTTP response output stream as a file block
            Response.WriteFile(filename);
            //sends all currently buffered output to the client
            Response.Flush();
            //Clears all content output from Buffer Stream
            Response.Clear();
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


        [HttpPost]
        public JsonResult GetCourseDetials()
        {
            //get the course details
            MemoPrintService MemoPrintService = new MemoPrintService();
            var lstCourses = MemoPrintService.GetCourseInstance().GetCourseDetails();
            return Json(lstCourses, JsonRequestBehavior.AllowGet);

        }


        private string ConcatenateSubject(string appenedstring, string subject, string yearPass)
        {
            if (subject != null && subject.Length > 0)
            {
                string sub = subject + " " + yearPass;//.CheckNumeric(2);
                appenedstring = appenedstring + sub + GetSpaces(8 - sub.Length);
                return appenedstring;
            }
            else
            {
                return appenedstring;
            }
        }

        private string ConcatenateSubjectMarks(string appenedstring, string subjectMarks, string Result = "")
        {
            if (subjectMarks != null && subjectMarks.Length > 0)
            {
                string sub = subjectMarks + " " + Result;
                appenedstring = appenedstring + sub + GetSpaces(8 - sub.Length);
                return appenedstring;
            }
            else
            {
                return appenedstring;
            }

        }




        private int addHeaderFooter(StreamWriter sw, int rowcount, string course, string year, string sem, bool cchange = false)
        {
            if (rowcount == 0)
            {

                //sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(" ");

                sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(("                                  TABULATION LIST OF " + course + " " + year + " YEAR " + sem + " SEM.  EXAMINATIONS HELD IN Nov., " + (DateTime.Now.Year - 1).ToString() + "                           ").Truncate(132));
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
                sw.WriteLine(("                                  TABULATION LIST OF " + course + " " + year + " YEAR " + sem + " SEM.  EXAMINATIONS HELD IN Nov., " + (DateTime.Now.Year - 1).ToString() + "                           ").Truncate(132));
                sw.WriteLine("SLNO." + GetSpaces(5) + "HTNO" + GetSpaces(15 - "HTNO".Length) + "NAME" + GetSpaces(45 - "NAME".Length) + "FATHER'S NAME" + GetSpaces(45 - "FATHER'S NAME".Length) + "RES/TOT. SGPA/CGPA");
                sw.WriteLine(("PAGE NO:" + PageNumber + "-------------------------------------------------------------------------------------------------------Date:" + DateTime.Now.ToString("dd-MMM-yyyy") + "===").Truncate(132));

                PageBraker = 8;
                //addHeaderFooter(sw, rowcount, course, year, sem);


            }
            else { PageBraker++; }
            return PageBraker;
        }


        //private int addHeaderFooter(StreamWriter sw, int rowcount, string course, string year, string sem, bool cchange= false)
        //{
        //    if (rowcount == 0)
        //    {


        //        sw.WriteLine(" ");
        //        sw.WriteLine(" ");

        //        sw.WriteLine(" ");
        //        sw.WriteLine(" ");
        //        sw.WriteLine(" ");
        //        sw.WriteLine(("                                  TABULATION lIST OF " + course + " " + year + " YEAR " + sem + " SEM.  EXAMINATIONS HELD IN Nov., " + (DateTime.Now.Year - 1).ToString() + "                           ").Truncate(132));
        //        sw.WriteLine("SLNO." + GetSpaces(5) + "HTNO" + GetSpaces(15 - "HTNO".Length) + "NAME" + GetSpaces(43 - "NAME".Length) + "FATHER'S NAME" + GetSpaces(44 - "FATHER'S NAME".Length) + "RES/TOT. SGPA/CGPA");
        //        sw.WriteLine(("PAGE NO:" + PageNumber + "-------------------------------------------------------------------------------------------------------Date:" + DateTime.Now.ToString("dd-MMM-yyyy") + "===").Truncate(132));

        //        PageBraker = PageBraker + 9;

        //    }
        //    else if (rowcount == 72)
        //    {

        //        rowcount = 0;
        //        PageNumber++;

        //        sw.WriteLine(" ");
        //        sw.WriteLine(" ");

        //        sw.WriteLine(" ");
        //        sw.WriteLine(" ");
        //        sw.WriteLine(" ");
        //        // sw.WriteLine(("                                  TABULATION LIST OF " + course + " " + year + " YEAR " + sem + " SEM                              ").Truncate(132));
        //        sw.WriteLine(("                                  TABULATION lIST OF " + course + " " + year + " YEAR " + sem + " SEM.  EXAMINATIONS HELD IN Nov., " + (DateTime.Now.Year - 1).ToString() + "                           ").Truncate(132));
        //        sw.WriteLine("SLNO." + GetSpaces(5) + "HTNO" + GetSpaces(15 - "HTNO".Length) + "NAME" + GetSpaces(45 - "NAME".Length) + "FATHER'S NAME" + GetSpaces(45 - "FATHER'S NAME".Length) + "RES/TOT. SGPA/CGPA");
        //        sw.WriteLine(("PAGE NO:" + PageNumber + "-------------------------------------------------------------------------------------------------------Date:" + DateTime.Now.ToString("dd-MMM-yyyy") + "===").Truncate(132));

        //        PageBraker = 9;
        //        //addHeaderFooter(sw, rowcount, course, year, sem);


        //    }
        //    else { PageBraker++; }
        //    return PageBraker;
        //}


    }
}