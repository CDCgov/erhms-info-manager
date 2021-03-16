﻿using Epi;
using ERHMS.EpiInfo;

namespace ERHMS.Console.Utilities
{
    public class DeleteView : IUtility
    {
        public string ProjectPath { get; }
        public string ViewName { get; }
        public bool IncludeDescendants { get; }

        public DeleteView(string projectPath, string viewName)
            : this(projectPath, viewName, true) { }

        public DeleteView(string projectPath, string viewName, bool includeDescendants)
        {
            ProjectPath = projectPath;
            ViewName = viewName;
            IncludeDescendants = includeDescendants;
        }

        public void Run()
        {
            Project project = ProjectExtensions.Open(ProjectPath);
            View view = project.Views[ViewName];
            if (IncludeDescendants)
            {
                project.DeleteTree(view);
            }
            else
            {
                project.Delete(view);
            }
        }
    }
}
