﻿namespace ERHMS.EpiInfo.Data
{
    public enum RecordStatus : short
    {
        Deleted = 0,
        Undeleted = 1
    }

    public static class RecordStatusExtensions
    {
        public static bool ToDeleted(this RecordStatus @this)
        {
            return @this == RecordStatus.Deleted;
        }

        public static RecordStatus FromDeleted(bool deleted)
        {
            return deleted ? RecordStatus.Deleted : RecordStatus.Undeleted;
        }
    }
}
