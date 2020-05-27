using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Models
{
    public class SelectTestModel
    {
        public bool IsText { get; set; }
        public bool IsTextArea { get; set; }
        public bool IsTextAreaFull { get; set; }
        public bool IsOnlyNumber { get; set; }
        public bool IsOnlyNormalChar { get; set; }
        public bool IsOnlySpecialChar { get; set; }
        public bool IsAllChar { get; set; }
        public bool IsOpenExpander { get; set; }
        public bool IsOpenTab { get; set; }
        public bool IsEdit { get; set; }
        public bool IsCreateNew { get; set; }
    }
}
