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


        [HttpPost]
        public JsonResult GenerateTabularSemReport(int Psem, string course)
        {
            try
            {

                ///// Creting Notepad 
                string fileNamedirectory = Server.MapPath("/TabularReport/");
                //string fileNamedirectory = @"D:\TabularReport\";
                string filename = "BA" + DateTime.Now.ToString("ddMMyyyy") + "_" + Psem + ".txt";

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
                    int rowcount = 0;


                    List<string> HallticketNumbers = lstStudents.OrderBy(x => x.HallTicketNumber).Select(x => x.HallTicketNumber).Distinct().ToList<string>();

                    for (int i = 0; i < HallticketNumbers.Count; i++)
                    {

                        var lstStuns = lstStudents.Where(x => x.HallTicketNumber == HallticketNumbers[i]).ToList<StudentInformation>();
                        var StudentConsInformatio = LstConStudents.Where(x => x.HTNO == HallticketNumbers[i]).ToList<ConsDataEntity>();




                        string HallTicket = lstStuns[0].HallTicketNumber;
                        string FN = lstStuns[0].FatherName == null ? "" : lstStuns[0].FatherName;
                        string SN = lstStuns[0].StudentName == null ? "" : lstStuns[0].StudentName;
                        string CC = lstStuns[0].collegecode == null ? "" : lstStuns[0].collegecode;
                        string EI = lstStuns[0].Ei == null ? "" : lstStuns[0].Ei;
                        string nameformat = i + 1 + GetSpaces(7) + HallTicket + GetSpaces(15 - HallTicket.Length) + SN + GetSpaces(45 - SN.Length) + FN + GetSpaces(45 - FN.Length);


                        if (i == 0)
                        {
                            rowcount = addHeaderFooter(sw, rowcount, course, year.ToString(), sem.ToString());
                            sw.WriteLine(nameformat.Truncate(113) + EI + GetSpaces(7 - EI.Length) + HallTicket + GetSpaces(10 - HallTicket.Length));

                        }
                        else
                        {
                            sw.WriteLine(nameformat.Truncate(113) + EI + GetSpaces(7 - EI.Length) + HallTicket + GetSpaces(10 - HallTicket.Length));

                            rowcount = addHeaderFooter(sw, rowcount, course, year.ToString(), sem.ToString());
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
                            rowcount = addHeaderFooter(sw, rowcount, course, year.ToString(), sem.ToString());
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
                            rowcount = addHeaderFooter(sw, rowcount, course, year.ToString(), sem.ToString());
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
                            rowcount = addHeaderFooter(sw, rowcount, course, year.ToString(), sem.ToString());
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
                            subjectsMarksPRE_S = "    S" + GetSpaces(5 - "PRES.".Length) + GetSpaces(7);
                            int subord = 1;
                            
                            int count = 0;
                           
                            foreach (StudentInformation studentM in lstStuns.OrderBy(X => X.Order))
                            {
                                string spaces = "";
                                int ordr = studentM.Order;
                                spaces = GetSpaces((ordr- subord)*10);
                                
                                if (count > 0)
                                {
                                    spaces = "";
                                    
                                    spaces = GetSpaces(7);
                                }

                                subjectformt = spaces + studentM.SubjectCode;
                                subjectstringPRE = subjectstringPRE + subjectformt ;

                                SubjectMarks_U = spaces + studentM.ExernalMarks.ToString()  ;
                                subjectsMarksPRE_U = subjectsMarksPRE_U + SubjectMarks_U;

                                string intmark = Convert.ToString(studentM.InternalMarks == null ? "" : studentM.InternalMarks) + GetSpaces(3 - Convert.ToString(studentM.InternalMarks == null ? "" : studentM.InternalMarks).Length) + " " + studentM.Status;
                                intmark = spaces + intmark;// + GetSpaces(7 - intmark.Length); ;
                                SubjectMarks_S = intmark;
                                subjectsMarksPRE_S = subjectsMarksPRE_S + SubjectMarks_S;
                                count++;
                            }

                            int TotalMark = lstStuns.Sum(x => x.ExernalMarks.ChangeINT());
                            TotalMark = TotalMark + lstStuns.Sum(x => x.InternalMarks.ChangeINT());
                            sw.WriteLine(subjectstringPRE);
                            rowcount = addHeaderFooter(sw, rowcount, course, year.ToString(), sem.ToString());


                            sw.WriteLine(subjectsMarksPRE_U + GetSpaces(113 - subjectsMarksPRE_U.Length) + lstStuns[0].FinalResult + "    " + TotalMark);
                            rowcount = addHeaderFooter(sw, rowcount, course, year.ToString(), sem.ToString());

                            sw.WriteLine(subjectsMarksPRE_S);
                            rowcount = addHeaderFooter(sw, rowcount, course, year.ToString(), sem.ToString());
                            sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------");
                            rowcount = addHeaderFooter(sw, rowcount, course, year.ToString(), sem.ToString());
                           
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
                sw.WriteLine(("                                  TABULATION lIST OF " + course + " " + year + " YEAR " + sem + " SEM                              ").Truncate(132));
                sw.WriteLine("SLNO." + GetSpaces(5) + "HTNO" + GetSpaces(15 - "HTNO".Length) + "NAME" + GetSpaces(45 - "NAME".Length) + "FATHER'S NAME" + GetSpaces(45 - "FATHER'S NAME".Length) + "RES/TOT. SGPA/CGPA");
                sw.WriteLine(("PAGE NO:" + PageNumber + "-------------------------------------------------------------------------------------------------------Date:" + DateTime.Now.ToString("dd-MMM-yyyy") + "===").Truncate(132));
                rowcount = rowcount + 7;


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
                sw.WriteLine(("                                  TABULATION lIST OF " + course + " " + year + " YEAR " + sem + " SEM                              ").Truncate(132));
                sw.WriteLine("SLNO." + GetSpaces(5) + "HTNO" + GetSpaces(15 - "HTNO".Length) + "NAME" + GetSpaces(45 - "NAME".Length) + "FATHER'S NAME" + GetSpaces(45 - "FATHER'S NAME".Length) + "RES/TOT. SGPA/CGPA");
                sw.WriteLine(("PAGE NO:" + PageNumber + "-------------------------------------------------------------------------------------------------------Date:" + DateTime.Now.ToString("dd-MMM-yyyy") + "===").Truncate(132));
                rowcount = rowcount + 7;
                //addHeaderFooter(sw, rowcount, course, year, sem);


            }
            else { rowcount++; }
            return rowcount;
        }


    }
}