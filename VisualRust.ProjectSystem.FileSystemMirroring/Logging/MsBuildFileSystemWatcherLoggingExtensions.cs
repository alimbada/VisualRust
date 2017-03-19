﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;
using VisualRust.Core;
using VisualRust.ProjectSystem.FileSystemMirroring.IO;
using static System.FormattableString;

namespace VisualRust.ProjectSystem.FileSystemMirroring.Logging {
    internal static class MsBuildFileSystemWatcherLoggingExtensions {
        public static void WatcherStarting(this IActionLog log) {
            log.Trace("MsBuildFileSystemWatcher starting");
        }

        public static void WatcherStarted(this IActionLog log) {
            log.Trace("MsBuildFileSystemWatcher started");
        }

        public static void WatcherConsumeChangesScheduled(this IActionLog log) {
            log.Trace("Consume file system changes scheduled");
        }

        public static void WatcherConsumeChangesStarted(this IActionLog log) {
            log.Trace("File system changes consumer started");
        }

        public static void WatcherConsumeChangesFinished(this IActionLog log) {
            log.Trace("File system changes consumer finished");
        }

        public static void WatcherApplyChange(this IActionLog log, string change) {
            log.Trace(Invariant($"Apply change: {change}"));
        }

        public static void WatcherApplyChangeFailed(this IActionLog log, string change, Exception exception) {
            log.Error(Invariant($"Failed to apply change '{change}':{exception}"));
        }

        public static void WatcherApplyRecoveryChange(this IActionLog log, string change) {
            log.Trace(Invariant($"Apply recovery change: {change}"));
        }

        public static void WatcherApplyRecoveryChangeFailed(this IActionLog log, string change, Exception exception) {
            log.Error(Invariant($"Failed to apply recovery change '{change}', closing watcher:{exception}"));
        }

        public static void WatcherChangesetSent(this IActionLog log, MsBuildFileSystemWatcher.Changeset changeset) {
            var sb = new StringBuilder();
            sb.AppendLine("MsBuildFileSystemWatcher changeset sent.")
                .AppendWatcherChangesetPart(changeset.AddedFiles, "Added Files:")
                .AppendWatcherChangesetPart(changeset.RenamedFiles, "Renamed Files:")
                .AppendWatcherChangesetPart(changeset.RemovedFiles, "Removed Files:")
                .AppendWatcherChangesetPart(changeset.AddedDirectories, "Added Directories:")
                .AppendWatcherChangesetPart(changeset.RenamedDirectories, "Renamed Directories:")
                .AppendWatcherChangesetPart(changeset.RemovedDirectories, "Removed Directories:");

            log.Trace(sb.ToString());
        }

        public static void ErrorInFileSystemWatcher(this IActionLog log, string watcherName, Exception e) {
            log.Error(Invariant($"{watcherName} failed with exception:{e}"));
        }

        private static StringBuilder AppendWatcherChangesetPart(this StringBuilder sb, ISet<string> changesetPart, string name) {
            if (changesetPart.Count > 0) {
                sb.AppendLine(name);
                foreach (var item in changesetPart) {
                    sb.Append(' ', 4);
                    sb.AppendLine(item);
                }
            }

            return sb;
        }

        private static StringBuilder AppendWatcherChangesetPart(this StringBuilder sb, IDictionary<string, string> changesetPart, string name) {
            if (changesetPart.Count > 0) {
                sb.AppendLine(name);
                foreach (var item in changesetPart) {
                    sb.AppendLine(Invariant($"{item.Key} -> {item.Value}"));
                }
            }

            return sb;
        }
    }
}