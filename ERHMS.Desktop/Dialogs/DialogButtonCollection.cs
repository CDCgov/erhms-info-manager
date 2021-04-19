﻿using ERHMS.Desktop.Properties;
using System.Collections.Generic;

namespace ERHMS.Desktop.Dialogs
{
    public class DialogButtonCollection : List<DialogButton>
    {
        public static DialogButtonCollection Ok => new DialogButtonCollection
        {
            { ResXResources.AccessText_Ok, null, true, true }
        };

        public static DialogButtonCollection Close => new DialogButtonCollection
        {
            { ResXResources.AccessText_Close, null, true, true }
        };

        public void Add(object content, bool? result, bool isDefault, bool isCancel)
        {
            Add(new DialogButton(content, result, isDefault, isCancel));
        }
    }
}
