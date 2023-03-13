using Menza.Shared;

namespace Menza.Client;

public class StartupService
{
    private readonly AuthService _auth;
    private readonly IRepository _repository;

    public StartupService(IRepository repository, AuthService auth)
    {
        _repository = repository;
        _auth = auth;
    }

    public List<MenuAndRating>? Menus { get; private set; }

    public async Task Initialize()
    {
        if (_auth.IsAuthenticated)
            Menus = await _repository.GetAll();
    }
}