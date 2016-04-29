﻿using ERHMS.Domain;
using ERHMS.Presentation.Messages;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ERHMS.Presentation.ViewModels
{
    public class IncidentNotesViewModel : ViewModelBase
    {
        public Incident Incident { get; private set; }

        private ICollection<IncidentNote> notes;
        public ICollection<IncidentNote> Notes
        {
            get { return notes; }
            set { Set(() => Notes, ref notes, value); }
        }

        private IncidentNote note;
        public IncidentNote Note
        {
            get { return note; }
            set { Set(() => Note, ref note, value); }
        }

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand RefreshCommand { get; private set; }

        public IncidentNotesViewModel(Incident incident)
        {
            Incident = incident;
            Refresh();
            Note = CreateNote();
            SaveCommand = new RelayCommand(Save, HasContent);
            RefreshCommand = new RelayCommand(Refresh);
            Messenger.Default.Register<RefreshMessage<IncidentNote>>(this, OnRefreshMessage);
        }

        public bool HasContent()
        {
            return !string.IsNullOrWhiteSpace(Note.Content);
        }

        private IncidentNote CreateNote()
        {
            IncidentNote note = DataContext.IncidentNotes.Create();
            note.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName != "Content")
                {
                    SaveCommand.RaiseCanExecuteChanged();
                }
            };
            return note;
        }

        public void Save()
        {
            Note.IncidentId = Incident.IncidentId;
            Note.Date = DateTime.Now;
            DataContext.IncidentNotes.Save(Note);
            Messenger.Default.Send(new RefreshMessage<IncidentNote>(Incident.IncidentId));
            Note = CreateNote();
        }

        public void Refresh()
        {
            Notes = DataContext.IncidentNotes.SelectByIncident(Incident.IncidentId)
                .OrderByDescending(note => note.Date)
                .ToList();
        }

        private void OnRefreshMessage(RefreshMessage<IncidentNote> msg)
        {
            if (msg.IncidentId == Incident.IncidentId)
            {
                Refresh();
            }
        }
    }
}