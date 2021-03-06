﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoPrintingUtility.DA
{
    public interface IMemoPrintDAFactory
    {
        ICourseDAService GetCourseInstance();
        ITabularReportDAService GetTabularDAService();
        ISubjectRangeGenerateDA GetSubjectRangeService();

        ISDLTablarDA GetSDLCInstance();

        ILongMemoDAService GetLongMemoService();
    }
}
