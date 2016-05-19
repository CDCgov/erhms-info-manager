﻿using Epi;
using Epi.Windows.MakeView;
using Epi.Windows.MakeView.PresentationLogic;
using ERHMS.Utility;
using System;
using System.Reflection;

namespace ERHMS.EpiInfo.MakeView
{
    internal class Template
    {
        object @base;

        public Template(GuiMediator mediator)
        {
            Type type = typeof(MakeViewWindowsModule).Assembly.GetType("Epi.Windows.MakeView.Template");
            ConstructorInfo constructor = type.GetConstructor(new Type[] { typeof(GuiMediator) });
            @base = constructor.Invoke(new object[] { mediator });
        }

        public void InstantiateTemplate(string path)
        {
            Log.Current.DebugFormat("Instantiating template: {0}", path);
            ReflectionExtensions.Invoke(@base, "CreateFromTemplate", new Type[] { typeof(string) }, new object[] { path });
        }

        public void InstantiateTemplate(EpiInfo.Template template)
        {
            InstantiateTemplate(template.File.FullName);
        }

        public void CreateTemplate(View view, string templateName)
        {
            Log.Current.DebugFormat("Creating template: {0}, {1}", view.Name, templateName);
            ReflectionExtensions.Invoke(@base, "CreateViewTemplate", new Type[] { typeof(string), typeof(View) }, new object[] { templateName, view });
        }

        public string CreateWebTemplate()
        {
            Log.Current.DebugFormat("Creating web template");
            return (string)ReflectionExtensions.Invoke(@base, "CreateWebSurveyTemplate", Type.EmptyTypes, null);
        }
    }
}
