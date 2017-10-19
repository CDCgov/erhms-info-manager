﻿using ERHMS.Domain;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Linq;

namespace ERHMS.Presentation.ViewModels
{
    public class IncidentReportViewModel : ListViewModel<JobTicket>
    {
        public Incident Incident { get; private set; }

        public IEnumerable<Responder> SelectedResponders
        {
            get { return TypedSelectedItems.Select(jobTicket => jobTicket.Responder).Distinct(); }
        }

        public RelayCommand EditCommand { get; private set; }
        public RelayCommand EmailCommand { get; private set; }
        public RelayCommand AssignCommand { get; private set; }

        public IncidentReportViewModel(IServiceManager services, Incident incident)
            : base(services)
        {
            Title = "Report";
            Incident = incident;
            Refresh();
            EditCommand = new RelayCommand(Edit, HasSelectedItem);
            EmailCommand = new RelayCommand(Email, HasSelectedItem);
            AssignCommand = new RelayCommand(Assign, HasSelectedItem);
            SelectionChanged += (sender, e) =>
            {
                EmailCommand.RaiseCanExecuteChanged();
                EditCommand.RaiseCanExecuteChanged();
                AssignCommand.RaiseCanExecuteChanged();
            };
        }

        protected override IEnumerable<JobTicket> GetItems()
        {
            return Context.JobTickets.SelectUndeletedByIncidentId(Incident.IncidentId)
                .OrderBy(jobTicket => jobTicket.Responder.FullName)
                .ThenBy(jobTicket => jobTicket.Job.Name);
        }

        protected override IEnumerable<string> GetFilteredValues(JobTicket item)
        {
            yield return item.Responder.FullName;
            yield return item.Job.Name;
            yield return item.Team?.Name;
            yield return item.IncidentRole?.Name;
            yield return item.LocationNames;
        }

        public void Edit()
        {
            Documents.ShowJob(SelectedItem.Job);
        }

        public void Email()
        {
            Documents.Show(
                () => new EmailViewModel(Services, SelectedResponders),
                document => false);
        }

        public void Assign()
        {
            Dialogs.ShowAsync(new AssignViewModel(Services, Incident, SelectedResponders));
        }
    }
}