﻿using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Core.Manager;

namespace MoneyManager.Core.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly TransactionManager transactionManager;
        private readonly AccountManager accountManager;

        /// <summary>
        ///     Creates an MainViewModel object.
        /// </summary>
        /// <param name="transactionManager">Instance of <see cref="TransactionManager"/></param>
        /// <param name="accountManager">Instance of <see cref="AccountManager"/></param>
        public MainViewModel(TransactionManager transactionManager, AccountManager accountManager)
        {
            this.accountManager = accountManager;
            this.transactionManager = transactionManager;

            GoToAddTransactionCommand = new MvxCommand<string>(GoToAddTransaction);
            GoToAddAccountCommand = new MvxCommand(GoToAddAccount);
            GoToAboutCommand = new MvxCommand(GoToAbout);
        }

        /// <summary>
        ///     Prepare everything and navigate to AddTransaction view
        /// </summary>
        public MvxCommand<string> GoToAddTransactionCommand { get; private set; }

        /// <summary>
        ///     Navigates to the About view
        /// </summary>
        public MvxCommand GoToAboutCommand { get; private set; }

        /// <summary>
        ///     Prepare everything and navigate to AddAccount view
        /// </summary>
        public MvxCommand GoToAddAccountCommand { get; private set; }

        private void GoToAddTransaction(string type)
        {
            transactionManager.PrepareCreation(type);
            ShowViewModel<ModifyTransactionViewModel>();
        }

        private void GoToAddAccount()
        {
            accountManager.PrepareCreation();
            ShowViewModel<ModifyAccountViewModel>();
        }

        private void GoToAbout()
        {
            ShowViewModel<AboutViewModel>();
        }
    }
}