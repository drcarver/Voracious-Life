﻿using System;

using Voracious.Core.Enum;
using Voracious.Core.Interface;

namespace Voracious.Core.Model;

public partial class BookNavigationCore : IBookNavigationCore
{
    public int Id { get; set; }
    public ResourceCore? Resource { get; set; }
    public DateTime MostRecentNavigationDate { get; set; } = DateTime.Now;
    public int NumberOfCatalogViews { get; set; }
    public int NumberOfSwipeRight { get; set; }
    public int NumberOfSwipeLeft { get; set; }
    public int NumberReading { get; set; }
    public int NumberSpecificSelection { get; set; }
    public string CurrentSpot { get; set; } = "";
    public UserStatusEnum CurrentStatus { get; set; } = UserStatusEnum.NoStatus;
    public DateTime TimeMarkedDone { get; set; } = DateTime.MinValue;
    public DateTime FirstNavigationDate { get; set; } = DateTime.Now;
    public bool IsDone { get; set; } = false;
}
