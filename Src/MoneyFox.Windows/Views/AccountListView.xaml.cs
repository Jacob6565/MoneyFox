﻿using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using MoneyFox.Business.ViewModels;
using MoneyFox.Business.ViewModels.DesignTime;
using System;
using Windows.UI.StartScreen;
using MvvmCross;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.Windows.Business.Tiles;

namespace MoneyFox.Windows.Views
{
    /// <summary>
    ///     View to display an list of accounts.
    /// </summary>
    public sealed partial class AccountListView
	{
		/// <summary>
		///     Initialize View.
		/// </summary>
		public AccountListView()
		{
			InitializeComponent();

			if (DesignMode.DesignModeEnabled)
			{
				ViewModel = new DesignTimeAccountListViewModel();
			}
		}

		private void AccountList_RightTapped(object sender, RightTappedRoutedEventArgs e)
		{
			var senderElement = sender as FrameworkElement;
			var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement) as MenuFlyout;

			flyoutBase?.ShowAt(senderElement, e.GetPosition(senderElement));
		}

		private void Edit_OnClick(object sender, RoutedEventArgs e)
		{
			var element = (FrameworkElement)sender;
			var account = element.DataContext as AccountViewModel;
			if (account == null)
			{
				return;
			}

			(DataContext as AccountListViewModel)?.EditAccountCommand.Execute(account);
		}

		private void Delete_OnClick(object sender, RoutedEventArgs e)
		{
			//this has to be called before the dialog service since otherwise the datacontext is reseted and the account will be null
			var element = (FrameworkElement)sender;
			var account = element.DataContext as AccountViewModel;
			if (account == null)
			{
				return;
			}

			(DataContext as AccountListViewModel)?.DeleteAccountCommand.Execute(account);
		}

		private async void AddToStartMenu_ClickAsync(object sender, RoutedEventArgs e)
		{
			var element = (FrameworkElement)sender;
			var account = element.DataContext as AccountViewModel;
			if (account == null) return;
            if (!Mvx.IoCProvider.CanResolve<IAccountService>()) return;

            var liveTileManager = new LiveTileManager(Mvx.IoCProvider.Resolve<IAccountService>());

			var name = account.Account;
			int id = name.Data.Id;
			bool isPinned = SecondaryTile.Exists(id.ToString());
			if (!isPinned)
			{

				SecondaryTile tile = new SecondaryTile(id.ToString(),"Money Fox","a",new Uri("ms-appx:///Assets/SmallTile.scale-150.png"), TileSize.Default);
				tile.VisualElements.ShowNameOnSquare150x150Logo = false;
				tile.VisualElements.ShowNameOnSquare310x310Logo = true;
				tile.VisualElements.ShowNameOnWide310x150Logo = false;
				tile.VisualElements.Square310x310Logo = new Uri("ms-appx:///Assets/Square310x310Logo.scale-100.png");
				tile.VisualElements.Square150x150Logo = new Uri("ms-appx:///Assets/Square150x150Logo.scale-100.png");
				tile.VisualElements.Wide310x150Logo = new Uri("ms-appx:///Assets/Wide310x150Logo.scale-100.png");
				tile.VisualElements.Square71x71Logo = new Uri("ms-appx:///Assets/Square71x71Logo.scale-100.png");
				bool ispinned = await tile.RequestCreateAsync();
				if (ispinned)
				{
					await liveTileManager.UpdateSecondaryLiveTiles();
				}
			}
			else
			{
				await liveTileManager.UpdateSecondaryLiveTiles();
				await liveTileManager.UpdatePrimaryLiveTile();
			}
		}
	}
}