using MudBlazor;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Components.Dashboard;
using TuDa.CIMS.Web.Components.Dashboard.Dialogs;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.Pages;

public partial class AssetListPage
{
    private AssetList _assetList = null!;

    private readonly IDialogService _dialogService;
    private readonly IAssetItemApi _assetItemApi;
    private readonly ISnackbar _snackbar;
    private readonly ILogger<AssetListPage> _logger;

    public AssetListPage(
        IAssetItemApi assetItemApi,
        IDialogService dialogService,
        ISnackbar snackbar,
        ILogger<AssetListPage> logger
    )
    {
        _assetItemApi = assetItemApi;
        _dialogService = dialogService;
        _snackbar = snackbar;
        _logger = logger;
    }

    private async Task ShowAndHandleCreateDialog()
    {
        var options = new DialogOptions { CloseOnEscapeKey = true };
        var dialog = await _dialogService.ShowAsync<AssetItemDialog>("Create Item", options);

        var createDto = await dialog.GetReturnValueAsync<CreateAssetItemDto>();

        if (createDto is not null)
            await CreateItemAsync(createDto);
    }

    private async Task ShowAndHandleEditDialog(AssetItem assetItem)
    {
        var parameters = new DialogParameters<AssetItemEditDialog> { { d => d.Item, assetItem } };

        var options = new DialogOptions { CloseOnEscapeKey = true };

        var dialog = await _dialogService.ShowAsync<AssetItemEditDialog>(
            "Edit Item",
            parameters,
            options
        );

        var result = await dialog.GetReturnValueAsync<object>();

        switch (result)
        {
            case bool delete:
                if (delete)
                    await RemoveItemAsync(assetItem.Id);
                break;
            case UpdateAssetItemDto updateDto:
                await UpdateItemAsync(assetItem.Id, updateDto);
                break;
        }
    }

    private async Task CreateItemAsync(CreateAssetItemDto createDto)
    {
        var errorOrSuccess = await _assetItemApi.CreateAsync(createDto);
        if (errorOrSuccess.IsError)
        {
            _logger.LogError(
                "Something went wrong with the creation of an assetItem: {Errors}",
                errorOrSuccess.Errors
            );
            SomethingWentWrong();
            return;
        }

        _logger.LogInformation("AssetItem created successfully");
        _snackbar.Add("Erfolgreich erstellt", Severity.Success);
        await _assetList.ReloadData();
    }

    private async Task UpdateItemAsync(Guid assetItemId, UpdateAssetItemDto updateDto)
    {
        var errorOrUpdated = await _assetItemApi.UpdateAsync(assetItemId, updateDto);

        if (errorOrUpdated.IsError)
        {
            _logger.LogError(
                "Something went wrong when updating the assetItem {Id} with {UpdateDto}. Errors: {Errors}",
                assetItemId,
                updateDto,
                errorOrUpdated.Errors
            );
            SomethingWentWrong();
        }
        else
        {
            _logger.LogInformation(
                "Updating assetItem {Id} with {UpdateDto} successfully",
                assetItemId,
                updateDto
            );
            _snackbar.Add("Erfolgreich aktualisiert", Severity.Success);
            await _assetList.ReloadData();
        }
    }

    private async Task RemoveItemAsync(Guid assetItemId)
    {
        var errorOrDeleted = await _assetItemApi.RemoveAsync(assetItemId);

        if (errorOrDeleted.IsError)
        {
            _logger.LogError(
                "Something went wrong when deleting the assetItem {Id}. Errors: {Errors}",
                assetItemId,
                errorOrDeleted.Errors
            );
            SomethingWentWrong();
        }
        else
        {
            _logger.LogInformation("Deleted assetItem {Id} successfully", assetItemId);
            _snackbar.Add("Erfolgreich gelöscht", Severity.Success);
            await _assetList.ReloadData();
        }
    }

    private void SomethingWentWrong() => _snackbar.Add("Etwas ist schiefgelaufen", Severity.Error);
}
