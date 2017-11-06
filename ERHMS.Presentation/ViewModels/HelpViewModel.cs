﻿using ERHMS.Presentation.Messages;
using GalaSoft.MvvmLight.Command;
using System.Diagnostics;

namespace ERHMS.Presentation.ViewModels
{
    public class HelpViewModel : ViewModelBase
    {
        public RelayCommand ShowUserGuideCommand { get; private set; }
        public RelayCommand ShowRespondersCommand { get; private set; }
        public RelayCommand ShowIncidentsCommand { get; private set; }
        public RelayCommand ShowViewsCommand { get; private set; }
        public RelayCommand ShowTemplatesCommand { get; private set; }
        public RelayCommand ShowPgmsCommand { get; private set; }
        public RelayCommand ShowCanvasesCommand { get; private set; }

        public HelpViewModel(IServiceManager services)
            : base(services)
        {
            Title = "Help";
            ShowUserGuideCommand = new RelayCommand(ShowUserGuide);
            ShowRespondersCommand = new RelayCommand(ShowResponders);
            ShowIncidentsCommand = new RelayCommand(ShowIncidents);
            ShowViewsCommand = new RelayCommand(ShowViews);
            ShowTemplatesCommand = new RelayCommand(ShowTemplates);
            ShowPgmsCommand = new RelayCommand(ShowPgms);
            ShowCanvasesCommand = new RelayCommand(ShowCanvases);
        }

        private bool Validate()
        {
            if (Context == null)
            {
                AlertMessage msg = new AlertMessage
                {
                    Message = "Please open a data source."
                };
                msg.Dismissed += (sender, e) =>
                {
                    Documents.ShowDataSources();
                };
                MessengerInstance.Send(msg);
                return false;
            }
            return true;
        }

        public void ShowUserGuide()
        {
            Process.Start("https://www.cdc.gov/niosh/docs/2017-169/pdf/2017-169.pdf");
        }

        public void ShowResponders()
        {
            if (!Validate())
            {
                return;
            }
            Documents.ShowResponders();
        }

        public void ShowIncidents()
        {
            if (!Validate())
            {
                return;
            }
            Documents.ShowIncidents();
        }

        public void ShowViews()
        {
            if (!Validate())
            {
                return;
            }
            Documents.ShowViews();
        }

        public void ShowTemplates()
        {
            if (!Validate())
            {
                return;
            }
            Documents.ShowTemplates();
        }

        public void ShowPgms()
        {
            if (!Validate())
            {
                return;
            }
            Documents.ShowPgms();
        }

        public void ShowCanvases()
        {
            if (!Validate())
            {
                return;
            }
            Documents.ShowCanvases();
        }
    }
}
