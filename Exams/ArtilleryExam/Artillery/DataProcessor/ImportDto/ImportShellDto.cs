using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType(Constants.SHELL)]
    public class ImportShellDto
    {
        [XmlElement(Constants.SHELL_WEIGHT)]
        public double ShellWeight { get; set; }

        [XmlElement(Constants.CALIBER)]
        public string Caliber { get; set; }
    }
}
