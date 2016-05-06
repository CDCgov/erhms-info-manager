﻿using Epi;
using Epi.Fields;
using ERHMS.EpiInfo.DataAccess;
using ERHMS.EpiInfo.Domain;
using ERHMS.EpiInfo.Enter;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace ERHMS.Presentation.ViewModels
{
    public class RecordListViewModel : ListViewModelBase<ViewEntity>
    {
        public View View { get; private set; }
        public ViewEntityRepository<ViewEntity> Entities { get; private set; }

        private ICollection<DataGridColumn> columns;
        public ICollection<DataGridColumn> Columns
        {
            get { return columns; }
            set { Set(() => Columns, ref columns, value); }
        }

        public RelayCommand CreateCommand { get; private set; }
        public RelayCommand EditCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand UndeleteCommand { get; private set; }
        public RelayCommand RefreshCommand { get; private set; }

        public RecordListViewModel(View view)
        {
            Title = view.Name;
            View = view;
            Entities = new ViewEntityRepository<ViewEntity>(App.Current.DataContext.Driver, view);
            List<int> fieldIds = DataContext.Project.GetSortedFieldIds(view.Id).ToList();
            Columns = view.Fields.TableColumnFields
                .Cast<Field>()
                .OrderBy(field => fieldIds.IndexOf(field.Id))
                .Select(field => (DataGridColumn)new DataGridTextColumn
                {
                    Header = field.Name,
                    Binding = new Binding(field.Name)
                })
                .ToList();
            Refresh();
            Selecting += (sender, e) =>
            {
                EditCommand.RaiseCanExecuteChanged();
                DeleteCommand.RaiseCanExecuteChanged();
                UndeleteCommand.RaiseCanExecuteChanged();
            };
            CreateCommand = new RelayCommand(Create);
            EditCommand = new RelayCommand(Edit, HasSelectedItem);
            DeleteCommand = new RelayCommand(Delete, HasDeletableSelectedItem);
            UndeleteCommand = new RelayCommand(Undelete, HasUndeletableSelectedItem);
            RefreshCommand = new RelayCommand(Refresh);
            // TODO: Handle service messages
        }

        public bool HasDeletableSelectedItem()
        {
            return HasSelectedItem() && !SelectedItem.Deleted;
        }

        public bool HasUndeletableSelectedItem()
        {
            return HasSelectedItem() && SelectedItem.Deleted;
        }

        protected override ICollectionView GetItems()
        {
            return CollectionViewSource.GetDefaultView(Entities.Select());
        }

        protected override IEnumerable<string> GetFilteredValues(ViewEntity item)
        {
            yield break;
        }

        public void Create()
        {
            Enter.OpenView(View);
        }

        public void Edit()
        {
            Enter.OpenRecord(View, SelectedItem.UniqueKey.Value);
        }

        public void Delete()
        {
            Entities.Delete(SelectedItem);
        }

        public void Undelete()
        {
            Entities.Undelete(SelectedItem);
        }
    }
}