﻿using ERHMS.Dapper;

namespace ERHMS.EpiInfo.DataAccess
{
    public class DataContextBase : IDataContext
    {
        public IDatabase Database { get; private set; }
        public Project Project { get; private set; }

        public DataContextBase(Project project)
        {
            Database = project.GetDatabase();
            Project = project;
        }
    }
}
