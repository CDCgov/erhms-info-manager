﻿using ERHMS.Common;
using ERHMS.Common.Logging;
using System;
using System.Diagnostics;
using System.IO;

namespace ERHMS.EpiInfo
{
    public enum Module
    {
        Analysis,
        AnalysisDashboard,
        Enter,
        MakeView,
        Mapping,
        Menu
    }

    public static class ModuleExtensions
    {
        private static string ToFileName(this Module @this)
        {
            switch (@this)
            {
                case Module.Analysis:
                case Module.AnalysisDashboard:
                case Module.Enter:
                case Module.MakeView:
                case Module.Mapping:
                    return $"{@this}.exe";
                case Module.Menu:
                    return "EpiInfo.exe";
                default:
                    throw new ArgumentOutOfRangeException(nameof(@this));
            }
        }

        public static Process Start(this Module @this, params string[] args)
        {
            Log.Instance.Debug($"Starting module: {@this}");
            string workingDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
            return Process.Start(new ProcessStartInfo
            {
                UseShellExecute = false,
                WorkingDirectory = workingDirectoryPath,
                FileName = Path.Combine(workingDirectoryPath, @this.ToFileName()),
                Arguments = CommandLine.Quote(args)
            });
        }
    }
}
