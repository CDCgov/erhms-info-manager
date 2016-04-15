﻿using ERHMS.EpiInfo;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.ComponentModel;
using System.IO;
using System.Windows.Data;
using System.Windows.Input;

namespace ERHMS.WPF.ViewModel
{
    public class TemplateListViewModel : ViewModelBase
    {
        private Template selectedTemplate;
        public Template SelectedTemplate
        {
            get { return selectedTemplate; }
            set { Set(() => SelectedTemplate, ref selectedTemplate, value); }
        }

        private CollectionViewSource templateList;
        public ICollectionView TemplateList
        {
            get { return templateList != null ? templateList.View : null; }
        }

        private string filter;
        public string Filter
        {
            get { return filter; }
            set
            {
                Set(() => Filter, ref filter, value);
                TemplateList.Filter = ListFilterFunc;
            }
        }

        private bool ListFilterFunc(object item)
        {
            Template t = item as Template;

            return Filter == null ||
                Filter.Equals("") ||
                (t.File.Name != null && t.File.Name.ToLower().Contains(Filter.ToLower()));
        }
        

        public TemplateListViewModel()
        {
            RefreshTemplateData();
        }

        private void RefreshTemplateData()
        {
            templateList = new CollectionViewSource();
            templateList.Source = App.GetDataContext().GetTemplates();
            TemplateList.Refresh();
            RaisePropertyChanged("TemplateList");
            SelectedTemplate = null;
        }

        private bool HasSelectedTemplate()
        {
            return SelectedTemplate != null;
        }
    }
}