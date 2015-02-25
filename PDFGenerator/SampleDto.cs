using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFGenerator
{
    class SampleDto//dto to represent a row in a database table. 
    {
        public int getSUN { get; set; }

        public String getFirstName { get; set; }

        public String getSecondName { get; set; }

        public int getFirstYearGrade { get; set; }

        public int getSecondYearGrade { get; set; }

        public String getUsername { get; set; }

        public String getEmail { get; set; }

        public int getProgressCheck { get; set; }

        public String getFileName { get; set; }
    }
}
