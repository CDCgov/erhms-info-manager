﻿using ERHMS.Utility;
using System.Data.Common;
using System.IO;

namespace ERHMS.EpiInfo
{
    public class ProjectCreationInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DirectoryInfo Location { get; set; }
        public string Driver { get; set; }
        public DbConnectionStringBuilder Builder { get; set; }
        public string DatabaseName { get; set; }
        public bool Initialize { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}, {3}, {4}", Name, Location.FullName, Driver, Builder.GetCensoredConnectionString(), DatabaseName);
        }
    }
}