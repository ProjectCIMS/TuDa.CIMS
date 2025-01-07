﻿using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Web.Components.Pages.WorkingGroup;

public partial class WorkingGroupPurchaseList : ComponentBase
{
    [Parameter] public List<Purchase> Purchases { get; set; } = [];

    public void NavigateToPurchase()
    {
        // do something
    }

}

