﻿using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MoneyFox.Windows.Views.Dialogs;

namespace MoneyFox.Windows.Services
{
    public class DialogService : IDialogService
    {
        private LoadingDialog loadingDialog;

        /// <summary>
        ///     Show a dialog with two buttons with customizable Texts. If no message is passed the dialog will have a Yes and No
        ///     Button
        /// </summary>
        /// <param name="title">Title to display.</param>
        /// <param name="message">Text to display.</param>
        /// <param name="positiveButtonText">Text for the yes button.</param>
        /// <param name="negativeButtonText">Text for the no button.</param>
        /// <param name="positivAction">Action who shall be executed on the positive button click.</param>
        /// <param name="negativAction">Action who shall be executed on the negative button click.</param>
        public async Task ShowConfirmMessage(string title, string message, Action positivAction,
            string positiveButtonText = null,
            string negativeButtonText = null, Action negativAction = null)
        {
            var isPositiveAnswer = await ShowConfirmMessage(title, message, positiveButtonText, negativeButtonText);

            if (isPositiveAnswer)
            {
                positivAction();
            }
            else
            {
                negativAction?.Invoke();
            }
        }

        /// <summary>
        ///     Show a dialog with two buttons with customizable Texts. Returns the answer.
        /// </summary>
        /// <param name="title">Title for the dialog.</param>
        /// <param name="message">Text for the dialog.</param>
        /// <param name="positiveButtonText">Text for the yes button.</param>
        /// <param name="negativeButtonText">Text for the no button.</param>
        public async Task<bool> ShowConfirmMessage(string title, string message, string positiveButtonText = null,
            string negativeButtonText = null)
        {
            var dialog = new MessageDialog(message, title);
            dialog.Commands.Add(new UICommand(positiveButtonText ?? Strings.YesLabel));
            dialog.Commands.Add(new UICommand(negativeButtonText ?? Strings.NoLabel));

            var result = await dialog.ShowAsync();

            return result.Label == (positiveButtonText ?? Strings.YesLabel);
        }

        /// <summary>
        ///     Shows a dialog with title and message. Contains only an OK button.
        /// </summary>
        /// <param name="title">Title to display.</param>
        /// <param name="message">Text to display.</param>
        public async Task ShowMessage(string title, string message)
        {
            var dialog = new MessageDialog(message, title);
            dialog.Commands.Add(new UICommand(Strings.OkLabel));

            await dialog.ShowAsync();
        }

        /// <summary>
        ///     Shows a loading Dialog.
        /// </summary>
        public async void ShowLoadingDialog(string message = null)
        {
            // Be sure no other dialog is open.
            HideLoadingDialog();

            loadingDialog = new LoadingDialog {Text = message ?? Strings.LoadingLabel};
            await loadingDialog.ShowAsync();
        }

        /// <summary>
        ///     Hides the previously opened Loading Dialog.
        /// </summary>
        public void HideLoadingDialog()
        {
            loadingDialog?.Hide();
        }
    }
}