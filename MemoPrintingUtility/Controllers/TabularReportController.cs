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


                    int TotalSubjectAttemt = 0;
                    int TotalSubjectpass = 0;
                    string PrevColcode = "";
                    bool ColPagebrk = false;

                    List<string> lstColCodes = lstStudents.OrderBy(x => x.collegecode).Select(y => y.collegecode).Distinct().ToList<string>();


                    foreach (var colcode in lstColCodes)
                    {
                        List<string> HallticketNumbers = lstStudents.Where(y => y.collegecode == colcode).OrderBy(x => x.HallTicketNumber).Select(x => x.HallTicketNumber).Distinct().ToList<string>();
                        bool isExstudent = false;

                        ColPagebrk = true;
                        #region Htnonumbers
                        for (int i = 0; i < HallticketNumbers.Count; i++)
                        {

                            int totalSUbjects = 0;
                            int passedSubject = 0;
                            int rowcount = 0;
                            var lstStuns = lstStudents.Where(x => x.HallTicketNumber == HallticketNumbers[i]).ToList<StudentInformation>();
                            var StudentConsInformatio = LstConStudents.Where(x => x.HTNO == HallticketNumbers[i]).ToList<ConsDataEntity>();

                            string HallTicket = lstStuns[0].HallTicketNumber;
                            if (HallTicket == "006191656")
                            {


                            }

                            string FN = lstStuns[0].FatherName == null ? "" : lstStuns[0].FatherName;
                            string SN = lstStuns[0].StudentName == null ? "" : lstStuns[0].StudentName;
                            string CC = lstStuns[0].collegecode == null ? "" : lstStuns[0].collegecode;
                            string EI = lstStuns[0].Ei == null ? "" : lstStuns[0].Ei;
                            string CollegeCode = lstStuns[0].collegecode == null ? "" : lstStuns[0].collegecode;
                            string nameformat = i + 1 + GetSpaces(2) + CollegeCode + GetSpaces(5 - CollegeCode.Length) + HallTicket + GetSpaces(15 - HallTicket.Length) + SN + GetSpaces(45 - SN.Length) + FN + GetSpaces(45 - FN.Length);
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
                                    rowcount = rowcount + 3;
                                }

                                if (!isExstudent)
                                {
                                    rowcount = rowcount + 5;
                                }
                                else
                                {
                                    rowcount = rowcount + 4;

                                }


                            }
                            if ((PageBraker + rowcount) > 62)
                            {
                                int differ = 62 - PageBraker;

                                for (int h = 0; h < differ; h++)
                                {
                                    sw.WriteLine(" ");
                                }

                                PageBraker = addHeaderFooter(sw, 62, course, year.ToString(), sem.ToString());
                            }

                            if (PrevColcode != "" && PrevColcode.Trim() != colcode.Trim() && ColPagebrk == true)
                            {
                                ColPagebrk = false;
                                int differ = 62 - PageBraker;

                                for (int h = 0; h < differ; h++)
                                {
                                    sw.WriteLine(" ");
                                }

                                PageBraker = addHeaderFooter(sw, 62, course, year.ToString(), sem.ToString());

                            }


                            if (PrevColcode == "")
                            {
                                PrevColcode = colcode;
                            }

                            if (i == 0)
                            {
                                PageBraker = addHeaderFooter(sw, PageBraker, course, year.ToString(), sem.ToString());
                                sw.WriteLine(nameformat.Truncate(113) + EI + GetSpaces(7 - EI.Length) + HallTicket + GetSpaces(10 - HallTicket.Length));

                            }
                            else
                            {
                                sw.WriteLine(nameformat.Truncate(113) + EI + GetSpaces(7 - EI.Length) + HallTicket + GetSpaces(10 - HallTicket.Length));

                            }

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
                                SubjectCONSMarks_U = "    U" + GetSpaces(5 - "CONS.".Length);
                                SubjectCONSMarks_S = "    S" + GetSpaces(5 - "CONS.".Length) + GetSpaces(7);

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

                                subjectstringCONS = "CONS." + GetSpaces(5 - "CONS.".Length) + GetSpaces(7) + subjectCONSformt;
                                sw.WriteLine(subjectstringCONS + GetSpaces(113 - subjectstringCONS.Length) + ConsDetails.RES + "   " + ConsDetails.SGPA);


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
                                SubjectCONSMarks_U = SubjectCONSMarks_U + GetSpaces(7) + SubjectExtCons;
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
                                subjectstringPRE = "PRES." + GetSpaces(5 - "PRES.".Length) + GetSpaces(7);
                                subjectsMarksPRE_U = "    U" + GetSpaces(5 - "PRES.".Length) + GetSpaces(7);
                                if (!isExstudent)
                                {
                                    subjectsMarksPRE_S = "    S" + GetSpaces(5 - "PRES.".Length) + GetSpaces(7);
                                }
                                else
                                {
                                    subjectsMarksPRE_S = GetSpaces(5) + GetSpaces(5 - "PRES.".Length) + GetSpaces(7);
                                }
                                int subord = 1;

                                int count = 0;

                                //totalSUbjects = totalSUbjects + lstStuns.Count;
                                //passedSubject = passedSubject + lstStuns.Where(x => x.Status == "P").ToList<StudentInformation>().Count();


                                List<TotalsubjectRecord> lstts = new List<TotalsubjectRecord>();
                                foreach (StudentInformation studentM in lstStuns.OrderBy(X => X.Order))
                                {

                                    #region total subs and paassed subs

                                    TotalsubjectRecord obj = new TotalsubjectRecord() { subname = studentM.SubjectCode, status = studentM.Status };
                                    if (lstts != null && lstts.Count > 0)
                                    {
                                        if (lstts.Contains(obj))
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


                                    if (studentM.HallTicketNumber == "005181013")
                                    {
                                        string m = "";

                                    }

                                    string spaces = "";
                                    string umakspace = "";
                                    string intmarkspae = "";
                                    int ordr = studentM.Order;
                                    spaces = GetSpaces((ordr - subord) * 10);
                                    umakspace = GetSpaces((ordr - subord) * 10);
                                    intmarkspae = GetSpaces((ordr - subord) * 10);
                                    string intmark = string.Empty;
                                    if (isExstudent == false)
                                    {
                                        intmark = Convert.ToString(studentM.InternalMarks == null ? "" : studentM.InternalMarks) + GetSpaces(3 - Convert.ToString(studentM.InternalMarks == null ? "" : studentM.InternalMarks).Length) + " " + studentM.LeterGrade;
                                    }
                                    else
                                    {
                                        intmark = GetSpaces(1);

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
                                            umakspace = GetSpaces(umakspace.Length - (studentM.ExernalMarks.ToString() + " " + studentM.LeterGrade).Length);

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
                                        SubjectMarks_U = umakspace + studentM.ExernalMarks.ToString().Truncate(3) + " " + studentM.LeterGrade;

                                    }

                                    subjectsMarksPRE_U = subjectsMarksPRE_U + SubjectMarks_U;


                                    if (isExstudent == false)
                                    {
                                        intmark = intmarkspae + intmark;// + GetSpaces(7 - intmark.Length); ;
                                    }
                                    SubjectMarks_S = intmark;
                                    subjectsMarksPRE_S = subjectsMarksPRE_S + SubjectMarks_S;

                                    count++;
                                    subord = ordr;                                }

                                int external = lstStuns.Sum(x => x.ExernalMarks.ChangeINT());
                                int TotalMark = external + lstStuns.Sum(x => x.InternalMarks.ChangeINT());
                                sw.WriteLine(subjectstringPRE + GetSpaces(113 - subjectsMarksPRE_U.Length) + "<" + lstts.Where(x => x.status != "F").ToList().Count + "/" + lstts.Count + ">");
                                sw.WriteLine(subjectsMarksPRE_U + GetSpaces(113 - subjectsMarksPRE_U.Length) + lstStuns[0].FinalResult + "    " + lstStuns[0].SGPA + "  "+ lstStuns[0].Flotation);
                                if (isExstudent == false)
                                {
                                    sw.WriteLine(subjectsMarksPRE_S + GetSpaces(111 - subjectsMarksPRE_U.Length) + TotalMark);
                                }
                                else
                                {
                                    sw.WriteLine(GetSpaces(112) + external);

                                }
                                sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------");
                                //addHeaderFooter(sw, rowcount, course, year.ToString(), sem.ToString());

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
                string sub = subject + " " + yearPass.CheckNumeric(2);
                appenedstring = appenedstring + sub + GetSpaces(10 - sub.Length);
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
                appenedstring = appenedstring + sub + GetSpaces(10 - sub.Length);
                return appenedstring;
            }
            else
            {
                return appenedstring;
            }

        }

        private int addHeaderFooter(StreamWriter sw, int rowcount, string course, string year, string sem)
        {
            if (rowcount == 0)
            {
                sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(("                                  TABULATION lIST OF " + course + " " + year + " YEAR " + sem + " SEM                              ").Truncate(132));
                sw.WriteLine("SLNO." + GetSpaces(5) + "HTNO" + GetSpaces(15 - "HTNO".Length) + "NAME" + GetSpaces(45 - "NAME".Length) + "FATHER'S NAME" + GetSpaces(45 - "FATHER'S NAME".Length) + "RES/TOT. SGPA/CGPA");
                sw.WriteLine(("PAGE NO:" + PageNumber + "-------------------------------------------------------------------------------------------------------Date:" + DateTime.Now.ToString("dd-MMM-yyyy") + "===").Truncate(132));

                PageBraker = PageBraker + 3;

            }
            else if (rowcount == 62)
            {
                sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(" ");
                rowcount = 0;
                PageNumber++;
                sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(("                                  TABULATION LIST OF " + course + " " + year + " YEAR " + sem + " SEM                              ").Truncate(132));
                sw.WriteLine("SLNO." + GetSpaces(5) + "HTNO" + GetSpaces(15 - "HTNO".Length) + "NAME" + GetSpaces(45 - "NAME".Length) + "FATHER'S NAME" + GetSpaces(45 - "FATHER'S NAME".Length) + "RES/TOT. SGPA/CGPA");
                sw.WriteLine(("PAGE NO:" + PageNumber + "-------------------------------------------------------------------------------------------------------Date:" + DateTime.Now.ToString("dd-MMM-yyyy") + "===").Truncate(132));

                PageBraker = 3;
                //addHeaderFooter(sw, rowcount, course, year, sem);


            }
            else { PageBraker++; }
            return PageBraker;
        }


    }
}