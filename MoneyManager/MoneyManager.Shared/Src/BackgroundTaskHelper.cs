﻿using System;
using System.Linq;
using Windows.ApplicationModel.Background;
using Windows.UI.Popups;

namespace MoneyManager.Src
{
    public class BackgroundTaskHelper
    {
        private const string name = "RecurringTransactionTask";

        public void RegisterBackgroundTask()
        {
            if (IsTaskExisting()) return;

            var builder = new BackgroundTaskBuilder();
            //Task soll alle 12 Stunden laufen
            var trigger = new TimeTrigger(720, false);

            builder.Name = name;
            builder.TaskEntryPoint = typeof(Tasks.TransactionsWp.TransactionTask).FullName;
            builder.SetTrigger(trigger);
            var registration = builder.Register();
            registration.Completed += RegistrationOnCompleted;
        }

        private async void RegistrationOnCompleted(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
            var result = await BackgroundExecutionManager.RequestAccessAsync();
            if (result == BackgroundAccessStatus.Denied)
            {
                var dialog = new MessageDialog("mööp mööp");
                await dialog.ShowAsync();
            }
        }

        private bool IsTaskExisting()
        {
            return BackgroundTaskRegistration.AllTasks.Any(task => task.Value.Name == name);
        }
    }
}