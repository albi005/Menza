using Blazored.LocalStorage;

namespace Menza.Client;

public class CardService
{
    private readonly ILocalStorageService _localStorageService;

    public CardService(ILocalStorageService localStorageService) => _localStorageService = localStorageService;

    public ulong? CardNumber { get; private set; }
    
    public async Task Initialize()
    {
        CardNumber = await _localStorageService.GetItemAsync<ulong?>(nameof(CardNumber));
    }
 
    public async Task SetCardNumber(ulong? cardNumber)
    {
        CardNumber = cardNumber;
        await _localStorageService.SetItemAsync(nameof(CardNumber), cardNumber);
    }
}