﻿using Epi;
using System;
using System.Collections.Generic;
using System.Data;

namespace ERHMS.EpiInfo.Metadata
{
    public class GridColumnDataRow
    {
        private static readonly ISet<string> metadataNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ColumnNames.UNIQUE_ROW_ID,
            ColumnNames.REC_STATUS,
            ColumnNames.FOREIGN_KEY,
            ColumnNames.GLOBAL_RECORD_ID
        };

        public static implicit operator DataRow(GridColumnDataRow gridColumn)
        {
            return gridColumn.Row;
        }

        public DataRow Row { get; }
        public string Name => Row.Field<string>(ColumnNames.NAME);

        public string SourceTableName
        {
            get { return Row.Field<string>(ColumnNames.SOURCE_TABLE_NAME); }
            set { Row.SetField(ColumnNames.SOURCE_TABLE_NAME, value); }
        }

        public int FieldId
        {
            get { return Row.Field<int>(ColumnNames.FIELD_ID); }
            set { Row.SetField(ColumnNames.FIELD_ID, value); }
        }

        public int FieldTypeId => Row.Field<int>(ColumnNames.FIELD_TYPE_ID);
        public MetaFieldType FieldType => (MetaFieldType)FieldTypeId;

        public GridColumnDataRow(DataRow row)
        {
            Row = row;
        }

        public bool IsMetadata()
        {
            return metadataNames.Contains(Name);
        }
    }
}