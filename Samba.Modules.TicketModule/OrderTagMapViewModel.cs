﻿using System.Collections.Generic;
using System.Linq;
using Samba.Domain.Models.Tickets;
using Samba.Presentation.ViewModels;
using Samba.Services;

namespace Samba.Modules.TicketModule
{
    public class OrderTagMapViewModel : AbstractMapViewModel
    {
        protected internal OrderTagMap Model { get; set; }
        private const string NullLabel = "*";
        private readonly IMenuService _menuService;

        public OrderTagMapViewModel(OrderTagMap model, IMenuService menuService, IUserService userService, IDepartmentService departmentService, ISettingService settingService)
            : base(model, userService, departmentService, settingService)
        {
            _menuService = menuService;
            Model = model;
        }

        public string MenuItemGroupCodeLabel
        {
            get { return string.IsNullOrEmpty(MenuItemGroupCode) ? NullLabel : MenuItemGroupCode; }
        }

        public string MenuItemGroupCode
        {
            get { return Model.MenuItemGroupCode ?? NullLabel; }
            set
            {
                Model.MenuItemGroupCode = value;
                MenuItemId = 0;
                RaisePropertyChanged(() => MenuItemGroupCode);
                RaisePropertyChanged(() => MenuItemGroupCodeLabel);
                RaisePropertyChanged(() => MenuItems);
            }
        }

        public string MenuItemLabel { get { return MenuItemId > 0 ? AllMenuItems.Single(x => x.Id == MenuItemId).Name : NullLabel; } }

        public int MenuItemId
        {
            get { return Model.MenuItemId; }
            set
            {
                Model.MenuItemId = value;
                RaisePropertyChanged(() => MenuItemId);
                RaisePropertyChanged(() => MenuItemLabel);
            }
        }

        public IEnumerable<MenuItemData> MenuItems { get { return GetAllMenuItems(MenuItemGroupCode); } }
        public IEnumerable<string> MenuItemGroupCodes { get { return GetAllMenuItemGroupCodes(); } }

        private IEnumerable<MenuItemData> _allMenuItems;
        public IEnumerable<MenuItemData> AllMenuItems
        {
            get { return _allMenuItems ?? (_allMenuItems = _menuService.GetMenuItemData().OrderBy(x => x.Name)); }
        }

        private IEnumerable<string> GetAllMenuItemGroupCodes()
        {
            IList<string> result = new List<string>(_menuService.GetMenuItemGroupCodes().OrderBy(x => x));
            result.Insert(0, NullLabel);
            return result;
        }

        private IEnumerable<MenuItemData> GetAllMenuItems(string groupCode)
        {
            IList<MenuItemData> result = string.IsNullOrEmpty(groupCode) || groupCode == NullLabel
                                         ? AllMenuItems.ToList()
                                         : AllMenuItems.Where(x => x.GroupCode == groupCode).ToList();
            result.Insert(0, new MenuItemData { Name = NullLabel });
            return result;
        }

    }
}