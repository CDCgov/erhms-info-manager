﻿using log4net;
using System;
using System.IO;

namespace ERHMS.Common
{
    public static class Log
    {
        private class ProgressImpl : IProgress<string>
        {
            public void Report(string value)
            {
                Default.Debug(value);
            }
        }

        public static string DefaultDirectoryPath { get; } =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

        public static string DefaultFilePath { get; } =
            Path.Combine(DefaultDirectoryPath, $"{nameof(ERHMS)}.{DateTime.Today:yyyy-MM-dd}.txt");

        public static ILog Default => LogManager.GetLogger(nameof(ERHMS));
        public static IProgress<string> Progress { get; } = new ProgressImpl();
    }
}