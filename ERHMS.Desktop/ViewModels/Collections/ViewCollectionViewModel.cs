﻿using Epi;
using ERHMS.Common;
using ERHMS.Common.ComponentModel;
using ERHMS.Desktop.Commands;
using ERHMS.Desktop.Data;
using ERHMS.Desktop.Dialogs;
using ERHMS.Desktop.Properties;
using ERHMS.Desktop.Services;
using ERHMS.Desktop.ViewModels.Wizards;
using ERHMS.EpiInfo;
using ERHMS.EpiInfo.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace ERHMS.Desktop.ViewModels.Collections
{
    public class ViewCollectionViewModel
    {
        public class ItemViewModel : ObservableObject
        {
            public View Value { get; }
            public int PageCount { get; private set; }
            public int FieldCount { get; private set; }
            public int RecordCount { get; private set; }

            private bool selected;
            public bool Selected
            {
                get { return selected; }
                set { SetProperty(ref selected, value); }
            }

            public ItemViewModel(View value)
            {
                Value = value;
            }

            public void Initialize()
            {
                PageCount = Value.Pages.Count;
                FieldCount = Value.Fields.InputFields.Count;
                if (Value.Project.CollectedData.TableExists(Value.TableName))
                {
                    using (RecordRepository repository = new RecordRepository(Value))
                    {
                        RecordCount = repository.CountByDeleted(false);
                    }
                }
                else
                {
                    RecordCount = 0;
                }
            }

            public override int GetHashCode()
            {
                return HashCodeCalculator.GetHashCode(Value.Project.Id, Value.Id);
            }

            public override bool Equals(object obj)
            {
                return obj is ItemViewModel item
                    && Value.Project.Id == item.Value.Project.Id
                    && Value.Id == item.Value.Id;
            }
        }

        public Project Project { get; }

        private readonly List<ItemViewModel> items;
        public ICollectionView Items { get; }

        public View CurrentValue => ((ItemViewModel)Items.CurrentItem)?.Value;

        public ICommand CreateCommand { get; }
        public ICommand OpenCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand DesignCommand { get; }
        public ICommand EnterCommand { get; }
        public ICommand RefreshCommand { get; }

        public ViewCollectionViewModel(Project project)
        {
            Project = project;
            items = new List<ItemViewModel>();
            Items = new ListCollectionView(items);
            CreateCommand = new AsyncCommand(CreateAsync);
            OpenCommand = new AsyncCommand(OpenAsync, Items.HasCurrent);
            DeleteCommand = new AsyncCommand(DeleteAsync, Items.HasCurrent);
            DesignCommand = new AsyncCommand(DesignAsync, Items.HasCurrent);
            EnterCommand = new AsyncCommand(EnterAsync, Items.HasCurrent);
            RefreshCommand = new AsyncCommand(RefreshAsync);
        }

        public async Task InitializeAsync()
        {
            IEnumerable<View> values = await Task.Run(() =>
            {
                Project.LoadViews();
                return Project.Views.Cast<View>().ToList();
            });
            items.Clear();
            items.AddRange(values.Select(value => new ItemViewModel(value)));
            await Task.Run(() =>
            {
                foreach (ItemViewModel item in items)
                {
                    item.Initialize();
                }
            });
            Items.Refresh();
        }

        public async Task CreateAsync()
        {
            CreateViewViewModel wizard = new CreateViewViewModel(Project);
            if (wizard.Show() == true)
            {
                await RefreshAsync();
            }
        }

        public async Task OpenAsync()
        {
            await MainViewModel.Instance.GoToViewAsync(() => Task.FromResult(CurrentValue));
        }

        public async Task DeleteAsync()
        {
            IDialogService dialog = ServiceLocator.Resolve<IDialogService>();
            dialog.Severity = DialogSeverity.Warning;
            dialog.Lead = Strings.Lead_ConfirmViewDeletion;
            dialog.Body = string.Format(Strings.Body_ConfirmViewDeletion, CurrentValue.Name);
            dialog.Buttons = DialogButtonCollection.ActionOrCancel(Strings.AccessText_Delete);
            if (dialog.Show() != true)
            {
                return;
            }
            IProgressService progress = ServiceLocator.Resolve<IProgressService>();
            progress.Title = Strings.Lead_DeletingView;
            await progress.Run(async () =>
            {
                await Task.Run(() =>
                {
                    Project.DeleteView(CurrentValue);
                });
                await InitializeAsync();
            });
        }

        public async Task DesignAsync()
        {
            await MainViewModel.Instance.StartEpiInfoAsync(
                Module.MakeView,
                $"/project:{Project.FilePath}",
                $"/view:{CurrentValue.Name}");
        }

        public async Task EnterAsync()
        {
            await MainViewModel.Instance.StartEpiInfoAsync(
                Module.Enter,
                $"/project:{Project.FilePath}",
                $"/view:{CurrentValue.Name}",
                "/record:*");
        }

        public async Task RefreshAsync()
        {
            IProgressService progress = ServiceLocator.Resolve<IProgressService>();
            progress.Title = Strings.Lead_RefreshingViews;
            await progress.Run(InitializeAsync);
        }
    }
}
