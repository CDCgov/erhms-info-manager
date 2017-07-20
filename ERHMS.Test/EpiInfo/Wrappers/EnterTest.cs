﻿using Epi.Collections;
using Epi.Fields;
using ERHMS.EpiInfo.Wrappers;
using NUnit.Framework;
using System;
using System.Windows.Forms;
using View = Epi.View;

namespace ERHMS.Test.EpiInfo.Wrappers
{
#if IGNORE_LONG_TESTS
    [TestFixture(Ignore = "IGNORE_LONG_TESTS")]
#endif
    public partial class EnterTest : WrapperTestBase
    {
        [Test]
        public void OpenRecordTest()
        {
            // Invoke wrapper
            wrapper = Enter.OpenRecord.Create(project.FilePath, "Surveillance", 1);
            wrapper.Invoke();
            MainFormScreen mainForm = new MainFormScreen();
            mainForm.WaitForReady();

            Assert.AreEqual("100", mainForm.GetValue("CaseID"));
            Assert.AreEqual("John", mainForm.GetValue("FirstName"));
            Assert.AreEqual("Smith", mainForm.GetValue("LastName"));
            Assert.AreEqual("5/20/1968", mainForm.GetValue("BirthDate"));

            // Change record
            mainForm.SetValue("LastName", "Doe");
            mainForm.SetValue("BirthDate", "1/1/1980");

            // Attempt to close window
            mainForm.Window.Close();

            // Save record
            mainForm.GetCloseDialogScreen().Dialog.Close(DialogResult.Yes);

            wrapper.Exited.WaitOne();
            View view = project.Views["Surveillance"];
            view.LoadRecord(1);
            NamedObjectCollection<IInputField> fields = view.Fields.InputFields;
            Assert.AreEqual("Doe", fields["LastName"].CurrentRecordValueObject);
            Assert.AreEqual(new DateTime(1980, 1, 1), fields["BirthDate"].CurrentRecordValueObject);
        }

        [Test]
        public void OpenNewRecordTest()
        {
            // Invoke wrapper
            dynamic record = new
            {
                CaseID = "2100",
                FirstName = "John",
                LastName = "Doe",
                BirthDate = new DateTime(1980, 1, 1)
            };
            wrapper = Enter.OpenNewRecord.Create(project.FilePath, "Surveillance", record);
            wrapper.Invoke();
            MainFormScreen mainForm = new MainFormScreen();
            mainForm.WaitForReady();

            // Attempt to close window
            mainForm.Window.Close();

            // Save record
            mainForm.GetCloseDialogScreen().Dialog.Close(DialogResult.Yes);

            wrapper.Exited.WaitOne();
            View view = project.Views["Surveillance"];
            Assert.AreEqual(21, view.GetRecordCount());
            view.LoadRecord(21);
            NamedObjectCollection<IInputField> fields = view.Fields.InputFields;
            Assert.AreEqual(record.CaseID, fields["CaseID"].CurrentRecordValueObject);
            Assert.AreEqual(record.FirstName, fields["FirstName"].CurrentRecordValueObject);
            Assert.AreEqual(record.LastName, fields["LastName"].CurrentRecordValueObject);
            Assert.AreEqual(record.BirthDate, fields["BirthDate"].CurrentRecordValueObject);
        }
    }
}
